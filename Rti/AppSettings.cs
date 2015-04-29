using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Rti
{
    //
    // Provides facilities for manipulating key/value pairs in a standard .NET app config.
    // By default the app config is loaded according to the currently executing assembly,
    // but a different path can be specified in the constructor.
    // Most apps should use the Settings class instead. It is more streamlined for common
    // use cases.
    //
    public class AppSettings
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        //
        // The default AppSettings object located in the executable's directory, named the
        // same as the executable with ".config" appended. (e.g. "Program.exe.config")
        //
        public static AppSettings Default;

        private DateTime _lastReadTime;
        private XmlNodeList _nodeList;
        private Dictionary<string, string> _values;

        static AppSettings()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (assemblyPath != null)
            {
                var path = Path.Combine(assemblyPath, Assembly.GetEntryAssembly().GetName().Name + ".exe.config");
                Default = new AppSettings(path);
            }
        }

        public AppSettings(string filename)
        {
            // This try block just placates Designer, otherwise this shouldn't throw
            try
            {
                Filename = filename;
                LoadXml();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public string Filename { get; private set; }

        public string this[string key]
        {
            get { return GetStringSetting(key, null); }
            set { SetStringSetting(key, value); }
        }

        public string GetStringSetting(string settingName, string defaultValue)
        {
            var value = FindSetting(settingName);
            if (value == null)
                return defaultValue;
            return value;
        }

        public void SetStringSetting(string settingName, string value)
        {
            UpdateSetting(settingName, value);
        }

        public T GetEnumSetting<T>(string settingName, T defaultValue)
        {
            var settingValue = GetStringSetting(settingName, defaultValue.ToString());
            foreach (T val in Enum.GetValues(typeof(T)))
            {
                if (string.Compare(val.ToString(), settingValue, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return val;
                }
            }
            return defaultValue;
        }

        public void SetEnumSetting<T>(string settingName, T value)
        {
            UpdateSetting(settingName, value.ToString());
        }

        public bool GetBoolSetting(string settingName, bool defaultValue)
        {
            var str = FindSetting(settingName);
            if (str == null)
                return defaultValue;

            bool value;
            return bool.TryParse(str, out value) ? value : defaultValue;
        }

        public void SetBoolSetting(string settingName, bool value)
        {
            UpdateSetting(settingName, value.ToString());
        }

        public int GetIntSetting(string settingName, int defaultValue)
        {
            var str = FindSetting(settingName);
            if (str == null)
                return defaultValue;

            int value;
            return int.TryParse(str, out value) ? value : defaultValue;
        }

        public void SetIntSetting(string settingName, int value)
        {
            UpdateSetting(settingName, value.ToString(CultureInfo.InvariantCulture));
        }

        public double GetDoubleSetting(string settingName, double defaultValue)
        {
            var str = FindSetting(settingName);
            if (str == null)
                return defaultValue;

            double value;
            return double.TryParse(str, out value) ? value : defaultValue;
        }

        public void SetDoubleSetting(string settingName, double value)
        {
            UpdateSetting(settingName, value.ToString(CultureInfo.InvariantCulture));
        }

        private void UpdateSetting(string key, string value)
        {
            lock (this)
            {
                var doc = new XmlDocument();

                if (!File.Exists(Filename))
                {
                    var configurationNode = doc.CreateElement("configuration");
                    configurationNode.AppendChild(doc.CreateElement("appSettings"));
                    doc.AppendChild(configurationNode);
                }
                else
                {
                    doc.Load(Filename);
                }

                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (string.Compare(node.Name, "configuration", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        foreach (XmlNode appNode in node.ChildNodes)
                        {
                            if (string.Compare(appNode.Name, "appSettings", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                var found = false;
                                foreach (XmlNode itemNode in appNode.ChildNodes)
                                {
                                    if (itemNode.Attributes != null &&
                                        string.Compare(itemNode.Name, "add", StringComparison.OrdinalIgnoreCase) == 0 &&
                                        string.Compare(itemNode.Attributes.GetNamedItem("key").Value, key, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        itemNode.Attributes.GetNamedItem("value").Value = value;
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    var newNode = doc.CreateElement("add");
                                    var keyAttr = doc.CreateAttribute("key");
                                    keyAttr.Value = key;
                                    newNode.Attributes.SetNamedItem(keyAttr);

                                    var valAttr = doc.CreateAttribute("value");
                                    valAttr.Value = value;
                                    newNode.Attributes.SetNamedItem(valAttr);

                                    appNode.AppendChild(newNode);
                                }

                                doc.Save(Filename);
                                LoadXml();
                            }
                        }
                    }
                }
            }
        }

        private void LoadXml()
        {
            _nodeList = null;
            _values = new Dictionary<string, string>();
            var doc = new XmlDocument();
            try
            {
                doc.Load(Filename);
            }
            catch (IOException exc)
            {
                Log.Warn("Error loading XML document. " + exc.Message);
                return;
            }

            _lastReadTime = DateTime.UtcNow;
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (string.Compare(node.Name, "configuration", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    foreach (XmlNode appNode in node.ChildNodes)
                    {
                        if (string.Compare(appNode.Name, "appSettings", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            _nodeList = appNode.ChildNodes;

                            foreach (XmlNode itemNode in _nodeList)
                            {
                                if (string.Compare(itemNode.Name, "add", StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    try
                                    {
                                        _values.Add(itemNode.Attributes.GetNamedItem("key").Value,
                                                   itemNode.Attributes.GetNamedItem("value").Value);
                                    }
                                    catch (Exception exc)
                                    {
                                        Log.Warn("Malformed item node. Error: " + exc.Message);
                                    }
                                }
                            }

                            Log.Debug(Filename + " settings loaded");
                            return;
                        }
                    }
                }
            }
        }

        private string FindSetting(string settingName)
        {
            lock (this)
            {
                var fi = new FileInfo(Filename);
                if (fi.LastWriteTimeUtc > _lastReadTime)
                {
                    Log.Debug(Filename + " has been modified; reloading.");
                    LoadXml();
                }

                if (_values.ContainsKey(settingName))
                {
                    return _values[settingName];
                }
                return null;
            }
        }
    }
}