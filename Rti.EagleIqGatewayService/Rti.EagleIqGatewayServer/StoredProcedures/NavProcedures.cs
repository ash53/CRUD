using System;
using System.Data;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Rti.DataModel;

namespace Rti.EagleIqGatewayServer.StoredProcedures
{
    public class NavProcedures
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetPostBd(string workstation, string username,
            string mapName, string actName, string practice, string division,
            string depDtStart, string depDtEnd, string embAcct, string reason, string checkNum,
            string checkAmountMin, string checkAmountMax, string paidAmountMin, string paidAmountMax,
            string docType, string docDet, string who, string docNo, string parDocNo)
        {
            Log.Debug(string.Format("Enter GetPostBd() Workstation:[{0}], Username:[{1}]",
                workstation, username));

            checkAmountMin = checkAmountMin == "" ? "0" : checkAmountMin;
            checkAmountMax = checkAmountMax == "" ? "0" : checkAmountMax;
            paidAmountMin = paidAmountMin == "" ? "0" : paidAmountMin;
            paidAmountMax = paidAmountMax == "" ? "0" : paidAmountMax;

            var dataTable = new DataTable("postbd");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");
            dataTable.Columns.Add("ASSIGNED_TO");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PAIDAMT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("POSTDETAIL_ID");
            dataTable.Columns.Add("SERVICEID");
            dataTable.Columns.Add("CORRECTION_FOLDER");
            dataTable.Columns.Add("ESCALATION_REASON");
            dataTable.Columns.Add("USERNAME");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("COURIER_INST_ID");

            using (var db = new TowerModelContainer())
            {
                var param1 = new OracleParameter("p_MAP_NAME", OracleDbType.Varchar2, mapName,
                    ParameterDirection.Input);
                var param2 = new OracleParameter("p_ACT_NAME", OracleDbType.Varchar2, actName,
                    ParameterDirection.Input);
                var param3 = new OracleParameter("p_practice", OracleDbType.Varchar2, practice,
                    ParameterDirection.Input);
                var param4 = new OracleParameter("p_division", OracleDbType.Varchar2, division,
                    ParameterDirection.Input);
                var param5 = new OracleParameter("p_depdt_start", OracleDbType.Varchar2, depDtStart,
                    ParameterDirection.Input);
                var param6 = new OracleParameter("p_depdt_end", OracleDbType.Varchar2, depDtEnd,
                    ParameterDirection.Input);
                var param7 = new OracleParameter("p_embacct", OracleDbType.Varchar2, embAcct,
                    ParameterDirection.Input);
                var param8 = new OracleParameter("p_reason", OracleDbType.Varchar2, reason,
                    ParameterDirection.Input);
                var param9 = new OracleParameter("p_checknum", OracleDbType.Varchar2, checkNum,
                    ParameterDirection.Input);
                var param10 = new OracleParameter("p_checkamt_min", OracleDbType.Varchar2, checkAmountMin,
                    ParameterDirection.Input);
                var param11 = new OracleParameter("p_checkamt_max", OracleDbType.Varchar2, checkAmountMax,
                    ParameterDirection.Input);
                var param12 = new OracleParameter("p_paidamt_min", OracleDbType.Varchar2, paidAmountMin,
                    ParameterDirection.Input);
                var param13 = new OracleParameter("p_paidamt_max", OracleDbType.Varchar2, paidAmountMax,
                    ParameterDirection.Input);
                var param14 = new OracleParameter("p_doctype", OracleDbType.Varchar2, docType,
                    ParameterDirection.Input);
                var param15 = new OracleParameter("p_docdet", OracleDbType.Varchar2, docDet,
                    ParameterDirection.Input);
                var param16 = new OracleParameter("p_who", OracleDbType.Varchar2, who,
                    ParameterDirection.Input);
                var param17 = new OracleParameter("p_docno", OracleDbType.Varchar2, docNo,
                    ParameterDirection.Input);
                var param18 = new OracleParameter("p_pardocno", OracleDbType.Varchar2, parDocNo,
                    ParameterDirection.Input);
                var param19 = new OracleParameter("cursor1", OracleDbType.RefCursor,
                    ParameterDirection.Output);

                try
                {
                    var query =
                        db.Database.SqlQuery<PostBdDisplay>(
                            "BEGIN EMBILLZNAVPKG.GET_POSTBD(:p_MAP_NAME,:p_ACT_NAME,:p_practice,:p_division,:p_depdt_start,:p_depdt_end,:p_embacct,:p_reason,:p_checknum,:p_checkamt_min,:p_checkamt_max,:p_paidamt_min,:p_paidamt_max,:p_doctype,:p_docdet,:p_who,:p_docno,:p_pardocno,:cursor1); end;",
                            param1, param2, param3, param4, param5, param6, param7, param8, param9, param10,
                            param11, param12, param13, param14, param15, param16, param17, param18, param19).ToList();

                    foreach (var row in query)
                    {
                        dataTable.Rows.Add(row.MAP_NAME, row.ACT_NAME, row.WHO, row.DOCNO, row.PRACTICE, row.DIVISION,
                            row.EMBACCT
                            , row.REASON, row.DOCTYPE, row.DOCDET, row.DEPTDT, row.CHECKNUM, row.CHECKAMT, row.PAIDAMT
                            , row.PARDOCNO, row.RECDATE, row.EXTPAYOR, row.DEPCODE, row.POSTDETAIL_ID, row.SERVICEID
                            , row.CORRECTION_FOLDER, row.ESCALATION_REASON, row.USERNAME, row.TOWID, row.IFN
                            , row.NPAGES, row.DOCGROUP, row.STATUS, row.COURIER_INST_ID);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }

            Log.Debug(String.Format("Exit GetPostBd() Rows:[{0}]", dataTable.Rows.Count));
            return dataTable;
        }

        /// <summary>
        /// EMBILLZNAVPKG.GET_POSTBD() Stored Procedure Return Type
        /// </summary>
        private class PostBdDisplay
        {
            public string MAP_NAME { set; get; }
            public string ACT_NAME { set; get; }
            public string WHO { set; get; }
            public string DOCNO { set; get; }
            public string PRACTICE { set; get; }
            public string DIVISION { set; get; }
            public string EMBACCT { set; get; }
            public string REASON { set; get; }
            public string DOCTYPE { set; get; }
            public string DOCDET { set; get; }
            public string DEPTDT { set; get; }
            public string CHECKNUM { set; get; }
            public string CHECKAMT { set; get; }
            public string PAIDAMT { set; get; }
            public string PARDOCNO { set; get; }
            public string RECDATE { set; get; }
            public string EXTPAYOR { set; get; }
            public string DEPCODE { set; get; }
            public int? POSTDETAIL_ID { set; get; }
            public string SERVICEID { set; get; }
            public string CORRECTION_FOLDER { set; get; }
            public string ESCALATION_REASON { set; get; }
            public string USERNAME { set; get; }
            public int? TOWID { set; get; }
            public string PAIDAMT2 { set; get; }
            public string IFN { set; get; }
            public int? NPAGES { set; get; }
            public string DOCGROUP { set; get; }
            public string STATUS { set; get; }
            public int? COURIER_INST_ID { set; get; }
        }
    }
}
