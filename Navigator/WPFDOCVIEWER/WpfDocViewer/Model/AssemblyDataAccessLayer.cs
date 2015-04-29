using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Collections.ObjectModel;


namespace WpfDocViewer.Model
{
    public class AssemblyDataAccessLayer
    {
        private OracleConnection dbCxn;
        private bool haveError;
        private int errorNumber;
        private string errorMessage;
        private bool isInitialized;
        private int errorBase = Int32.Parse("00F00000", System.Globalization.NumberStyles.HexNumber); //Decimal: 15728640




        bool IsConnected()
        {
            bool isConnected = false;
            if (dbCxn != null)
            {
                if (dbCxn.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        dbCxn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new DataException();
                    }
                }
                isConnected = (dbCxn.State != System.Data.ConnectionState.Closed);
            }
            else
            {
                try
                {
                    dbCxn = new OracleConnection(Properties.Settings.Default["DBFWCxn"].ToString());//Properties.Settings.DBCxnString);//getConnectionString(Properties.Settings.Default.Server, Properties.Settings.Default.Port, Properties.Settings.Default.Database, Properties.Settings.Default.userid, Properties.Settings.Default.password));//Properties.Settings.Default.DBCxnString);
                    dbCxn.Open();
                    isConnected = (dbCxn.State != System.Data.ConnectionState.Closed);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("WpfDocViewer", "CloseDBCxn: " + ex.Message, EventLogEntryType.Error);
                }
            }
            return isConnected;
        }
        public DataTable SearchChecks(string searchBy, string searchValue)
        {
            DataTable getSearchDetails = new DataTable("Floware");


            //Table Valued Function
            string sql = string.Format(@"SELECT Practice, Checknum, CheckAmt, Doctype, DepDt, DocDetail, docgroup
                                        FROM tower.postdoc  
                                      WHERE " + searchBy + " = '" + searchValue + "'");

            try
            {
                if (IsConnected())
                {
                    //MLane must use a SELECT statement and pass in the parameters like you are 
                    //calling a function in order to run a Table Valued Function into a Reader into a DataTable
                    OracleCommand cmd = new OracleCommand(sql, dbCxn) { CommandType = CommandType.Text };

                    using (OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        getSearchDetails.Load(dr);
                    }

                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("WpfDocViewer", "SearchIDM: " + ex.Message.ToString(), EventLogEntryType.Error);
            }
            finally
            {

            }
            return getSearchDetails;
        }

        #region GetRoot

        public static TreeRoot[] GetTreeRoots()
        {
            return new TreeRoot[]
            {
                new TreeRoot("ElectronicChecks"),
                new TreeRoot("PaperChecks")
            };
        }

        #endregion // GetRoot

        #region GetChecks
        /// <summary>
        /// Mark Lane: 
        /// Note-Used to get the check data first only as a list<string>
        /// into the check class. TODO
        /// once dal has right data decided the Check class needs all the data.
        /// <param name="region"></param>
        /// <returns></returns>
        public static CheckData[] GetChecks(TreeRoot region)
        {
            List<string> checklist = new List<string>();
            GetSearchData newSearch = new GetSearchData();
            
            checklist = newSearch.GetELChecks("", "");
            int count = checklist.Count;
            CheckData[] newCheckData = new CheckData[count];

            switch (region.TreeRootName)
            {
                case "ElectronicChecks":
                    //get a new list and fill
                   
                    int array = 0;
                    foreach (string str in checklist)
                    {
                       newCheckData[array++] = new CheckData(str);
                    }  
                    return newCheckData;

                case "PaperChecks":
                    return new CheckData[]
                    {
                        new CheckData("CheckNum: 12345 Amt: 45.00 DepDt: 10/5/2014"),
                        new CheckData("CheckNum: 5678 Amt: 3000.00 DepDt: 10/6/2014"),
                        new CheckData("CheckNum: 9101112 Amt: 299.00 DepDt: 10/7/2014")
                    };
            }

            return null;
        }

        #endregion // GetChecks

        #region GetRemits

        public static RemitData[] GetRemits(CheckData check)
        {
            switch (check.CheckNumber)
            {
                case "CheckNum: 12345 Amt: 45.00 DepDt: 10/5/2014":
                    return new RemitData[]
                    {
                        new RemitData("Remit: 20.00 DepDt: 10/5/2014"),
                        new RemitData("Remit: 10.00 DepDt: 10/5/2014"),
                        new RemitData("Remit: 15.00 DepDt: 10/5/2014")
                    };

                case "CheckNum: 5678 Amt: 3000.00 DepDt: 10/6/2014":
                    return new RemitData[]
                    {
                        new RemitData("Remit: 2000.00 DepDt: 10/6/2014"),
                        new RemitData("Remit: 500.00 DepDt: 10/6/2014"),
                        new RemitData("Remit: 500.00 DepDt: 10/7/2014")          
                    };

                case "CheckNum: 9101112 Amt: 299.00 DepDt: 10/7/2014":
                    return new RemitData[]
                    {
                        new RemitData("Remit: 80.00 DepDt: 10/7/2014"),
                        new RemitData("Remit: 50.00 DepDt: 10/7/2014"),
                        new RemitData("Remit: 220.00 DepDt: 10/7/2014"),
                        new RemitData("Remit: 50.00 DepDt: 10/7/2014")
                    };
            }

            return null;
        }

        #endregion // GetRemits

    }
}
