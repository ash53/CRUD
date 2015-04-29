using System;

namespace Rti
{
    //
    // Helper class (in lieu of AppSettings.Default)
    //
    public static class Settings
    {
        public static void Set<T>(string paramName, T value)
        {
            AppSettings.Default.SetStringSetting(paramName, value.ToString());
        }

        public static T Get<T>(string paramName, T defaultValue)
        {
            try
            {
                var setting = AppSettings.Default.GetStringSetting(paramName, null);
                if (string.IsNullOrEmpty(setting))
                {
                    return defaultValue;
                }

                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), setting);
                }

                return (T)Convert.ChangeType(setting, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}