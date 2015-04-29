using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WpfDocViewer.Model
{
    class GetSearchData
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
                    OracleCommand cmd = new OracleCommand(sql, dbCxn) { CommandType = CommandType.Text };
                    //OracleParameter pr = new OracleParameter("param2", OracleType.VarChar);
                    //pr.Value = "llou";
                    //pr.Direction = ParameterDirection.Input;

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
        public DataTable SearchIDM(string searchBy, string searchValue)
        {
            DataTable getSearchDetails = new DataTable("IDM");
           

            //Table Valued Function
            string sql = string.Format(@"select a.docno, a.doctype, a.docdet, a.depdt, a.checknum, a.checkamt, a.practice, a.division, a.embacct, a.pardocno, a.towid,  a.npages, '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN, a.docgroup FROM tower.postdoc a WHERE " + searchBy + " = '" + searchValue + "'");

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
        public DataTable SearchFloware(string searchBy, string searchValue)
        {
            DataTable getSearchDetails = new DataTable("Floware");


            //Table Valued Function
            string sql = string.Format(@"SELECT a.MAP_NAME, a.ACT_NAME, a.assigned_To, a.docno, a.practice, a.division, a.embacct, a.reason, a.doctype, a.docdet, a.depdt, a.checknum, a.checkamt, 
                                             a.pardocno, 
                                            a.towid,  a.paidamt, a.ifn, 1 as npages, a.docgroup, a.courier_inst_id 
                                        FROM tower.postbd a 
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
        public List<string> GetELChecks(string searchBy, string searchValue)
        {
           List<string> getSearchDetails = new List<string>();


            //Table Valued Function
            string sql = string.Format(@"SELECT checknum, checkamt, depdt, docno, pardocno, docdet, doctype 
                                        FROM tower.postdoc a 
                                      WHERE doctype = 'CK' and docgroup = 'EL' and depdt >= SYSDATE - 7
                                      AND ROWNUM < 20");

            try
            {
                if (IsConnected())
                {
                    //MLane must use a SELECT statement and pass in the parameters like you are 
                    //calling a function in order to run a Table Valued Function into a Reader into a DataTable
                    OracleCommand cmd = new OracleCommand(sql, dbCxn) { CommandType = CommandType.Text };

                    using (OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        int rowcount = 0;
                        while (dr.Read())
                        {
                            //CheckData ck;
                            
                            //ck.CheckName = dataReader["checknum"]
                            //ck.CheckAmount
                            //ck.CheckDate
                            //ck.CheckDocno
                            string rowstring = "Chk(" + (rowcount+1) + ") Num:" + dr["checknum"].ToString() + " Amt: $" + dr["checkamt"].ToString() + " Date:" + dr["depdt"].ToString();
                            getSearchDetails.Add(rowstring);
                        }
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
        /// <summary>
        /// Author Mark Lane
        /// When user clicks a Row create an ObservableCollection of Image data
        /// using docno as the input.
        /// bind this collection to the DocumentViewer
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public void GetImageData(string docno)
        {
      
        }
        public DataTable SearchEmbillz(string searchBy, string searchValue)
        {
            DataTable getSearchDetails = new DataTable("Embillz");

                 /*
            //Table Valued Function
            string sql = string.Format(@"SELECT a.MAP_NAME, a.ACT_NAME, a.assigned_To, a.docno, a.practice, a.division, a.embacct, a.doctype, a.docdet, a.depdt, a.checknum, a.checkamt, 
                                             a.pardocno, 
                                            a.towid,  a.npages, '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN, a.docgroup
                                        FROM tower.postbd a 
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
             */
            return getSearchDetails;
        }
        public DataTable SearchAssembly(string searchBy, string searchValue)
        {
            DataTable getSearchDetails = new DataTable("Assembly");

            /*
            //Table Valued Function
            string sql = string.Format(@"SELECT a.MAP_NAME, a.ACT_NAME, a.assigned_To, a.docno, a.practice, a.division, a.embacct, a.doctype, a.docdet, a.depdt, a.checknum, a.checkamt, 
                                             a.pardocno, 
                                            a.towid,  a.npages, '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN, a.docgroup
                                        FROM tower.postbd a 
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
             */
            return getSearchDetails;
        }
    }
}
