using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml.Serialization;
using ADODB;


namespace Rti
{
    public static class CommonFunctions
    {
        // Returns the username, I.e. login id
        public static string GetUserName()
        {
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
            {
                var user = windowsIdentity.Name;
                if (user.Contains("\\"))
                {
                    var withoutDomain = user.Split(new[] { "\\" }, StringSplitOptions.None);
                    return withoutDomain[1].ToLower();
                }
                return user.ToLower();
            }
            return null;
        }

        // Gets the machine/host name
        public static string GetMachineName()
        {
            return Environment.MachineName;
        }

        // Returns a fully qualified domain name
        public static string GetFqdn()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();
            string fqdn;
            if (!hostName.Contains(domainName))
                fqdn = hostName + "." + domainName;
            else
                fqdn = hostName;

            return fqdn;
        }

        // Returns just the specific domain name
        public static string GetDomainName()
        {
            //return IPGlobalProperties.GetIPGlobalProperties().DomainName;

            // This one seems to be the more accurate for clients (I.e. EMSC)
            return Environment.UserDomainName;
        }

        // Converts datatable's to recordsets
        static public Recordset DataTableToRecordSet(DataTable dataTable)
        { 
            Recordset result = new Recordset();
            result.CursorLocation = CursorLocationEnum.adUseClient;

            Fields resultFields = result.Fields;
            DataColumnCollection inColumns = dataTable.Columns;

            foreach (DataColumn inColumn in inColumns)
            {
                resultFields.Append(inColumn.ColumnName
                    , TranslateType(inColumn.DataType)
                    , inColumn.MaxLength
                    , inColumn.AllowDBNull ? FieldAttributeEnum.adFldIsNullable : 
                                             FieldAttributeEnum.adFldUnspecified
                    , null);
            }

            result.Open(Missing.Value
                    , Missing.Value
                    , CursorTypeEnum.adOpenStatic
                    , LockTypeEnum.adLockOptimistic, 0);

            foreach (DataRow dr in dataTable.Rows)
            {
                result.AddNew(Missing.Value, 
                              Missing.Value);

                for (int columnIndex = 0; columnIndex < inColumns.Count; columnIndex++)
                {
                    resultFields[columnIndex].Value = dr[columnIndex];
                }
            }

            return result;
        }

        static DataTypeEnum TranslateType(Type columnType)
        {
            switch (columnType.UnderlyingSystemType.ToString())
            {
                case "System.Boolean":
                    return DataTypeEnum.adBoolean;

                case "System.Byte":
                    return DataTypeEnum.adUnsignedTinyInt;

                case "System.Char":
                    return DataTypeEnum.adChar;

                case "System.DateTime":
                    return DataTypeEnum.adDate;

                case "System.Decimal":
                    return DataTypeEnum.adCurrency;

                case "System.Double":
                    return DataTypeEnum.adDouble;

                case "System.Int16":
                    return DataTypeEnum.adSmallInt;

                case "System.Int32":
                    return DataTypeEnum.adInteger;

                case "System.Int64":
                    return DataTypeEnum.adBigInt;

                case "System.SByte":
                    return DataTypeEnum.adTinyInt;

                case "System.Single":
                    return DataTypeEnum.adSingle;

                case "System.UInt16":
                    return DataTypeEnum.adUnsignedSmallInt;

                case "System.UInt32":
                    return DataTypeEnum.adUnsignedInt;

                case "System.UInt64":
                    return DataTypeEnum.adUnsignedBigInt;

                case "System.String":
                default:
                    return DataTypeEnum.adVarChar;
            }
        }

        // Converts recordsets to datatable's
        static public DataTable RecordSetToDataTable(Recordset recordSet, string dataSetName, string tableName)
        {
            var dataAdapter = new OleDbDataAdapter();
            var dataSet = new DataSet(dataSetName);
            var dataTable = new DataTable();

            dataAdapter.Fill(dataSet, recordSet, tableName);

            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[tableName];
            }

            return dataTable;
        }

        // List to data table
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = 
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                     row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        // Data table to List
        public static List<T> ToList<T>(DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        // Attempts to dump any object of data to XML
        static public string ObjectToXml(object output)
        {
            string objectAsXmlString;

            var xs = new XmlSerializer(output.GetType());
            using (var sw = new StringWriter())
            {
                try
                {
                    xs.Serialize(sw, output);
                    objectAsXmlString = sw.ToString();
                }
                catch (Exception ex)
                {
                    objectAsXmlString = ex.ToString();
                }
            }

            return objectAsXmlString;
        }

        // Saves a form as an image, crops off form heading and form borders
        static public void FormToImage(Form activeForm, string pathAndFilename, ImageFormat imageFormat)
        {
            if (activeForm != null)
            {
                int formBorderWidth = SystemInformation.FrameBorderSize.Width;
                int formTitlebarHeight = SystemInformation.CaptionHeight;
                int formHeight = activeForm.Height;
                int formWidth = activeForm.Width;

                var bitmapOfForm = new Bitmap(formWidth, formHeight);
                activeForm.DrawToBitmap(bitmapOfForm, new Rectangle(Point.Empty, bitmapOfForm.Size));

                // Crop it
                var croppedBitmap = bitmapOfForm.Clone(
                    new Rectangle(formBorderWidth,
                        formTitlebarHeight + formBorderWidth,
                        formWidth - formBorderWidth * 2,
                        formHeight - (formTitlebarHeight + formBorderWidth * 2)
                        ),
                    bitmapOfForm.PixelFormat);

                croppedBitmap.Save(pathAndFilename, imageFormat);
            }
            else
            {
                throw new ArgumentException("Error: Must pass a valid form!");
            }
        }

        // Converts to/from
        // .gif, .jpeg, .bmp, .png, .tiff
        public static void ConvertImage(string pathAndFileName, ImageFormat format)
        {
            // File exists?
            if (File.Exists(pathAndFileName))
            {
                // converting to same type?
                if (pathAndFileName.EndsWith(format.ToString()))
                {
                    // can't convert n to n
                    MessageBox.Show("Error: Cannot convert target to same format!");
                }
                else
                {
                    var outFile = Path.GetFileNameWithoutExtension(pathAndFileName);
                    var inImage = Image.FromFile(pathAndFileName);
                    inImage.Save(outFile + "." + format, format);
                }
            }
            else
            {
                throw new FileNotFoundException("File:[" + pathAndFileName + "] not found");
            }
        }

        /// <summary>
        /// Fires a program from the command line, suppressing the dos window
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// The integer returned from the run program, use this to test status
        /// </returns>
        public static int RunViaCommandLine(string command)
        { 
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            return process.ExitCode;
        }

        /// <summary>
        /// Return datatable friendly empty strings instead of DbNull's
        /// </summary>
        /// <param name="checkString"></param>
        /// <returns></returns>
        public static string ToEmptyStringIfDbNull(object checkString)
        {
            if(checkString == DBNull.Value ||
                checkString == null)
            {
                return string.Empty;
            }
            else
            {
                return checkString.ToString();
            }
        }

        /// <summary>
        /// Pass in a List<> and the size to split it down to and get an array of lists back
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static List<T>[] PartitionList<T>(List<T> list, int size)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            
            if (size < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            int count = (int)Math.Ceiling(list.Count / (double)size);
            List<T>[] partitions = new List<T>[count];

            int k = 0;
            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>(size);
                for (int j = k; j < k + size; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += size;
            }

            return partitions;
        }

        // Simple prompt for input dialog
        //  provide defaultInput to pre-populate an answer
        //  password = true if asterisks should hide input
        public static string PromptForInputDialog(string title, string message, string defaultInput = "",
            bool password = false)
        {
            var size = new Size(300, 90);
            var inputBox = new Form();
            string userInput = defaultInput;

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            var label = new Label();
            label.Location = new Point(7, 7);
            label.Font = new Font("Consolas", 12);
            label.Text = message;
            label.AutoSize = true;
            inputBox.Controls.Add(label);

            var textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 33),
                Text = userInput
            };
            if (password)
            {
                textBox.PasswordChar = '*';
            }
            inputBox.Controls.Add(textBox);

            var okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80 - 80, 59)
            };
            inputBox.Controls.Add(okButton);

            var cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                Size = new Size(75, 23),
                Text = "&Cancel",
                Location = new Point(size.Width - 80, 59)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;
            
            inputBox.BringToFront();
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.TopMost = true;
            inputBox.ShowDialog();

            userInput = textBox.Text;

            return userInput;
        }

        // Helper type to add Add to tuple so you get {} initialazation ability
        public class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add( T1 item, T2 item2 )
            {
                Add( new Tuple<T1, T2>( item, item2 ) );
            }
        }

        // Takes a LINQ results list and converts it to a datatable
        public static DataTable LinqResultToDataTable<T>(IEnumerable<T> Linqlist, string dataTableName)
        {
            DataTable dt = new DataTable(dataTableName);
            PropertyInfo[] columns = null;
            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type IcolType = GetProperty.PropertyType;

                        if ((IcolType.IsGenericType) && (IcolType.GetGenericTypeDefinition()
                            == typeof(Nullable<>)))
                        {
                            IcolType = IcolType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, IcolType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo p in columns)
                {
                    dr[p.Name] = p.GetValue(Record, null) == null ? DBNull.Value : p.GetValue
                        (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Pass in a string that has a numeric value a string with a numeric min and a string 
        /// with a numeric max and it sorts out if the value is between them
        /// 
        /// If min max are undefined or blank it returns true as well
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool StringValueBetweenMinMax(string value, string min, string max)
        {
            decimal myValue = 0;
            decimal myMin = 0;

            // No min max specified
            bool status = string.IsNullOrEmpty(min)
                          && string.IsNullOrEmpty(max);

            // Specified a min/max, check it
            if (!status)
            {
                if (!string.IsNullOrEmpty(min))
                {
                    if (Decimal.TryParse(value, out myValue)
                        && Decimal.TryParse(min, out myMin))
                    {
                        if (myValue > myMin)
                        {
                            status = true;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(max))
                {
                    decimal myMax = 0;
                    if (Decimal.TryParse(value, out myValue)
                        && Decimal.TryParse(max, out myMax))
                    {
                        if (myMax > 0
                            && myValue < myMax
                            && myValue > myMin)
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }
            return status;
        }

        public static void DebugDumpDbReaderRow(string description, OdbcDataReader dataReader)
        {
            Debug.Write(description);
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Debug.Write(String.Format("[{0}]", dataReader[i]));
            }
            Debug.WriteLine("");
        }

        /// <summary>
        /// Returns DateTime of when assembly was built
        /// </summary>
        public static DateTime RetrieveBuildTimestamp()
        {
            string filePath = Assembly.GetCallingAssembly().Location;
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var buffer = new byte[2048];
            System.IO.Stream myStream = null;

            try
            {
                myStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                myStream.Read(buffer, 0, 2048);
            }
            finally
            {
                if (myStream != null)
                {
                    myStream.Close();
                }
            }

            int i = BitConverter.ToInt32(buffer, peHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(buffer, i + linkerTimestampOffset);
            DateTime retrieveLinkerTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            retrieveLinkerTimestamp = retrieveLinkerTimestamp.AddSeconds(secondsSince1970);
            retrieveLinkerTimestamp = retrieveLinkerTimestamp.ToLocalTime();
            return retrieveLinkerTimestamp;
        }

        /// <summary>
        /// File MD5 checksum generator, output is in readable format
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string Md5Hash(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
                }
            }
        }

    }
}
