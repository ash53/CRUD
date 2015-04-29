using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oracle.DataAccess.Client;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace WpfDocViewer.Model
{
    public class DataAccessMgr
    {

        private OracleConnection dbCxn;
        private bool haveError;
        private int errorNumber;
        private string errorMessage;
        private bool isInitialized;
        private int errorBase = Int32.Parse("00F00000", System.Globalization.NumberStyles.HexNumber); //Decimal: 15728640

        public bool Initialized
        {
            get { return isInitialized; }
        }
        public bool HaveError
        {
            get { return haveError; }
        }
        public int ErrorNumber
        {
            get { return errorNumber; }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        public void ClearError()
        {
            haveError = false;
            errorNumber = 0;
            errorMessage = "";
        }

        public DataAccessMgr()
        {
           
        }

        ~DataAccessMgr()
        {
        
        }


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

        public void CloseDBCxn()
        {

            if (dbCxn != null)
            {
                if (dbCxn.State != System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        dbCxn.Close();
                    }
                    catch (OracleException ex)
                    {
                        EventLog.WriteEntry("WpfDocViewer", "CloseDBCxn: " + ex.Message, EventLogEntryType.Error);
                    }
                }
            }
        }
        private static string getConnectionString(string databaseIP, int databasePort, string databaseSID, string databaseUN, string databasePW)
        {
            return string.Format(
                "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))(CONNECT_DATA =(SID = {2})));" +
                "Persist Security Info=True;User ID={3};Password={4}",
                databaseIP, databasePort, databaseSID, databaseUN, databasePW
            );
        }
        private void LogError(int errNum, string errMsg)
        {
            haveError = true;
            errorNumber = errNum;
            errorMessage = errMsg;
        }

       /// <summary>
       /// Author: Mark Lane
       /// Date: 6/4/2014
       /// Get the last unclaimed item, check it out and process it.
       /// </summary>
       /// <param name="customerID"></param>
       /// <returns>
       /// -1 = No queue items found
       /// </returns>
       public int GetNextQueueItem()
        {
            int result = 0;


            if (IsConnected())
            {
                OracleCommand comm = dbCxn.CreateCommand();
                comm.CommandText = "dbo.bronto_GetNextQueueItem";
                comm.CommandTimeout = 30;
                comm.CommandType = CommandType.StoredProcedure;
                OracleParameter param = new OracleParameter
                {
                    ParameterName = "@BrontoID",
                    Value = 0,
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Output
                };
                comm.Parameters.Add(param);
               
                try
                {
                    comm.ExecuteNonQuery();
                    result = Convert.ToInt32(comm.Parameters["@BrontoID"].Value.ToString());
                 
                }
                catch (Exception ex) 
                {
                    //it means there are no work items.
                    result = -1;
                    throw new DataException("GetNextQueueItem: No More Items to Process. " + ex.Message.ToString(), ex.InnerException);
                }
            }

            return result;
        }
        /// <summary>
        /// Author: Mark Lane
        /// Date: 6/9/2014
        /// check to see if there is work and how much
        /// </summary>
        /// <returns></returns>
       public int IsThereWork()
       {
           int result = 0;


           if (IsConnected())
           {
               OracleCommand comm = dbCxn.CreateCommand();
               comm.CommandText = "dbo.bronto_IsThereWork";
               comm.CommandTimeout = 30;
               comm.CommandType = CommandType.StoredProcedure;
            

               try
               {
                   comm.ExecuteScalar();
                   result = (int)comm.Parameters[0].Value;

               }
               catch (Exception ex)
               {
                   //it means there are no work items.
                   result = -1;
                   throw new DataException("GetNextQueueItem: No Work. " + ex.Message.ToString(), ex.InnerException);
               }
           }

           return result;
       }
       /// <summary>
       /// Author: Mark Lane
       /// Date: 6/27/2014
       /// get users floware groups on startup and save it.
       /// </summary>
       /// <param name="customerID"></param>
       /// <returns>
       /// -1 = No queue items found
       /// </returns>
       public DataTable GetDocno(string docno)
       {
      
            DataTable getTable = new DataTable("imagetable");
            string sql = string.Format(@"select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN, a.npages, a.docgroup, a.doctype, a.depdt from tower.postdoc a where a.pardocno = '" + docno + "' OR a.docno = '" + docno + "'");
           
           
               try
               {

                   if (IsConnected())
                   {
                       OracleCommand cmd = new OracleCommand(sql, dbCxn) { CommandType = CommandType.Text };

                       using (OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                       {
                           getTable.Load(dr);
                       }
                   }

               }
               catch (Exception ex)
               {
                   EventLog.WriteEntry("WpfDocViewer", "GetUsersDocno: " + docno + " error: " + ex.Message, EventLogEntryType.Error);
               }
               finally
               {
                  
               }

           return getTable;
       }
        /// <summary>
        /// Author: Mark Lane
        /// Date: 6/27/2014
       /// get the suite_group FROM appinfo.application_suite b where application_group = AppGroup
        /// </summary>
        /// <param name="templatetype"></param>
        /// <param name="identifier"></param>
        /// <param name="extendedProperties"></param>
        /// <returns></returns>
       internal DataTable GetUserApplicationGroup(string appGroup)
       {
           DataTable getQueueDetails = new DataTable("AppGroup");
           

           //Table Valued Function
           string sql = string.Format(@"SELECT suite_group FROM appinfo.application_suite b where application_group = " + appGroup);

           try
           {
               if (IsConnected())
               {
                   OracleCommand cmd = new OracleCommand(sql, dbCxn) {CommandType = CommandType.Text};

                   using (OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                   {
                       getQueueDetails.Load(dr);
                   }

               }

           }
           catch (Exception ex)
           {
               EventLog.WriteEntry("WpfDocViewer", "GetUserApplicationGroup: " + ex.Message.ToString(), EventLogEntryType.Error);
           }
           finally
           {

           }
           return getQueueDetails;
       }
       /// <summary>
       /// Author: Mark Lane
       /// Date: 6/4/2014
       /// Get Table Valued Function from emailqueue and process the detail.
       /// </summary>
       /// <param name="customerID"></param>
       /// <returns></returns>
       public DataTable GetApplicationData(int brontoQueueID)
       {
           DataTable getQueueDetails = new DataTable("AppInfoQueue");
           Object returnObj = null;

           //Table Valued Function
           string sql = string.Format(@"SELECT a.applicationname, a.version, a.minversion, a.idmversion,
                                           a.updatelocation, a.installationorder, a.restartreq,
                                           a.updatetype, a.minmemory, a.runextfile, a.email_enabled,
                                           a.sendemailto, a.processidname, a.updates_enabled,
                                           a.application_group
                                      FROM appinfo.appinfo a
                                      WHERE a.updates_enabled = 'True'
                                      ORDER BY a.installationorder ASC");
  
           try
           {
               if (IsConnected())
               {
                   //MLane must use a SELECT statement and pass in the parameters like you are 
                   //calling a function in order to run a Table Valued Function into a Reader into a DataTable
                   OracleCommand cmd = new OracleCommand(sql, dbCxn) {CommandType = CommandType.Text};

                   using (OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                   {
                       getQueueDetails.Load(dr);
                   }
                  
               }

           }
           catch (Exception ex)
           {
               EventLog.WriteEntry("WpfDocViewer", "GetApplicationData: " + ex.Message.ToString(), EventLogEntryType.Error);
           }
           finally
           {
               
           }
           return getQueueDetails;
       }
       
      
    }
}
