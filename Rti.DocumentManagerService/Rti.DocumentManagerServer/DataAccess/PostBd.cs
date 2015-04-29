using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using Rti.DataModel;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.DocumentManagerServer.DataAccess
{
    class PostBd
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Tuple<decimal?, decimal?> GetPostBdMapInstIdNodeId(string workstation, string userid, decimal? courierInstId)
        {
            Log.DebugFormat("Enter GetPostBdMapInstIdNodeId() workstation:[{0}] user:[{1}]", workstation, userid);
            using (var db = new TowerModelContainer())
            {
                var results = db.POSTBDs.FirstOrDefault(x => x.COURIER_INST_ID == courierInstId);
                if (results != null) return Tuple.Create(results.MAP_INST_ID,results.NODE_ID);
            }
            Log.Debug("Exit GetPostBdMapInstIdNodeId()");
            return Tuple.Create<decimal?,decimal?>(null, null);
        }

        public DataTable GetPostBdWithC2PR2PDataMerged(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "", 
            bool includeMatched = false, int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT)
        {
            Log.DebugFormat("Enter GetPostBdWithC2PR2PDataMerged() workstation:[{0}] user:[{1}]", workstation, username);
            
            // WCF sends through LINQ unfriendly null's, catch and correct
            if(string.IsNullOrEmpty(practice))
                practice = "";
            if (string.IsNullOrEmpty(division))
                division = "";
            if (string.IsNullOrEmpty(embAcct))
                embAcct = "";
            if (string.IsNullOrEmpty(mapName))
                mapName = "";
            if (string.IsNullOrEmpty(actName))
                actName = "";
            if (string.IsNullOrEmpty(docType))
                docType = "";
            if (string.IsNullOrEmpty(docDet))
                docDet = "";
            if (string.IsNullOrEmpty(who))
                who = "";
            if (string.IsNullOrEmpty(docNo))
                docNo = "";
            if (string.IsNullOrEmpty(parDocNo))
                parDocNo = "";
            if (string.IsNullOrEmpty(reason))
                reason = "";
            if (string.IsNullOrEmpty(checkNum))
                checkNum = "";
            if (string.IsNullOrEmpty(checkAmountMin))
                checkAmountMin = "";
            if (string.IsNullOrEmpty(checkAmountMax))
                checkAmountMax = "";
            if (string.IsNullOrEmpty(paidAmountMin))
                paidAmountMin = "";
            if (string.IsNullOrEmpty(paidAmountMax))
                paidAmountMax = "";
            if (string.IsNullOrEmpty(depDtStart))
                depDtStart = "";
            if (string.IsNullOrEmpty(depDtEnd))
                depDtEnd = "";

            Log.Debug(string.Format("Enter GetPostBdC2PR2PData() Workstation:[{0}], Username:[{1}]",
                workstation, username));

            DataSet postBdWithC2PR2PDataSet = new DataSet("PostBdWithC2PR2PDataMerged");

            postBdWithC2PR2PDataSet = GetPostBdWithC2PR2PData(workstation, username,
                mapName, actName, practice, division, depDtStart, depDtEnd, embAcct, reason, checkNum,
                checkAmountMin, checkAmountMax, paidAmountMin, paidAmountMax, docType, docDet, who,
                docNo, parDocNo, includeMatched, startRow, numRows);

            DataTable mergedDataTable = new DataTable("merged");
            mergedDataTable.Columns.Add("MatchId");
            mergedDataTable.Columns.Add("Client");
            mergedDataTable.Columns.Add("Div");
            mergedDataTable.Columns.Add("ExternalPayor");
            mergedDataTable.Columns.Add("Amount");
            mergedDataTable.Columns.Add("Dep/FileDate");
            mergedDataTable.Columns.Add("CheckDate");
            mergedDataTable.Columns.Add("CheckNumber");
            mergedDataTable.Columns.Add("SuspNo");
            mergedDataTable.Columns.Add("ProvID");
            mergedDataTable.Columns.Add("DocNo");
            mergedDataTable.Columns.Add("DocType");
            mergedDataTable.Columns.Add("DocDetail");
            mergedDataTable.Columns.Add("Status");
            mergedDataTable.Columns.Add("MatchStatus");
            mergedDataTable.Columns.Add("WorkFlowStatus");
            mergedDataTable.Columns.Add("OrigBank");
            mergedDataTable.Columns.Add("OrigBankAcct");
            mergedDataTable.Columns.Add("FileDt");
            mergedDataTable.Columns.Add("PmtMethod");
            mergedDataTable.Columns.Add("ExpectEPR");
            mergedDataTable.Columns.Add("RemitFileID");
            mergedDataTable.Columns.Add("Note");
            mergedDataTable.Columns.Add("MatchUid");
            mergedDataTable.Columns.Add("SvcType");
            mergedDataTable.Columns.Add("CourierInstId");
            mergedDataTable.Columns.Add("MapName");
            mergedDataTable.Columns.Add("ActName");
            mergedDataTable.Columns.Add("ASSIGNED_TO");
            mergedDataTable.Columns.Add("Reason");
            mergedDataTable.Columns.Add("EmbAcct");
            mergedDataTable.Columns.Add("Key");
            mergedDataTable.Columns.Add("DataSource");
            mergedDataTable.Columns.Add("MatchDate");
            mergedDataTable.Columns.Add("ComingleStat");
            mergedDataTable.Columns.Add("RowNum");
            mergedDataTable.Columns.Add("TotalRows");

            DataTable postBdDataTable = new DataTable("a");
            postBdDataTable = postBdWithC2PR2PDataSet.Tables[0];
            
            foreach (DataRow pbdRow in postBdDataTable.Rows)
            {
                var c2prows = postBdWithC2PR2PDataSet.Tables["c2p"].Select("DOCNO = '" + pbdRow["DOCNO"] + "'");
                foreach (DataRow c2pRow in c2prows)
                {
                    var hasNote = String.IsNullOrEmpty(c2pRow["NOTETEXT"].ToString()) ? "No" : "Yes";

                    mergedDataTable.Rows.Add(c2pRow["MATCHID"], pbdRow["PRACTICE"], pbdRow["DIVISION"], c2pRow["EXTPAYCD"],
                        c2pRow["CHECKAMT"], c2pRow["DEPDT"], c2pRow["CHECKDT"], c2pRow["CHECKNUM"], c2pRow["SUSPENDNO"],
                        "", pbdRow["DOCNO"], c2pRow["DOCTYPE"], c2pRow["DOCDET"], c2pRow["CSTATUS"],
                        c2pRow["MATCHSTAT"], "wf stat", c2pRow["ORIGCD"], c2pRow["ORIGNO"], c2pRow["FILEDT"],
                        "", "expectepr", "", hasNote, c2pRow["MATCHUID"], c2pRow["SERVICEID"],
                        pbdRow["COURIER_INST_ID"], pbdRow["MAP_NAME"], pbdRow["ACT_NAME"], pbdRow["ASSIGNED_TO"], 
                        pbdRow["REASON"], pbdRow["EMBACCT"], c2pRow["KEY"], "C2P",
                        DaysToDate(Constants.EmbillzPadStartDays, Convert.ToDouble(c2pRow["SECMODDT"])),
                        c2pRow["COMINGLESTAT"], pbdRow["ROWNUM"], pbdRow["TOTALROWS"]);
                    break;
                }

                if (!c2prows.Any())
                {
                    var r2prows = postBdWithC2PR2PDataSet.Tables["r2p"].Select("DOCNO = '" + pbdRow["DOCNO"] + "'");
                    foreach (DataRow r2pRow in r2prows)
                    {
                        var hasNote = String.IsNullOrEmpty(r2pRow["NOTETEXT"].ToString()) ? "No" : "Yes";

                        mergedDataTable.Rows.Add(r2pRow["MATCHID"], pbdRow["PRACTICE"], pbdRow["DIVISION"],
                            r2pRow["EXTPAYCD"],
                            r2pRow["CHECKAMT"], pbdRow["DEPDT"], "", r2pRow["CHECKNUM"], r2pRow["SUSPENDNO"],
                            r2pRow["PROVID"], pbdRow["DOCNO"], r2pRow["DOCTYPE"], r2pRow["DOCDET"], r2pRow["RSTATUS"],
                            r2pRow["MATCHSTAT"], "wf stat", r2pRow["ORIGCD"], r2pRow["ORIGNO"], r2pRow["FILEDT"],
                            r2pRow["TRANSTYPE"], "expectepr", r2pRow["REMITFILEID"], hasNote, r2pRow["MATCHUID"],
                            r2pRow["SERVICEID"], pbdRow["COURIER_INST_ID"], pbdRow["MAP_NAME"], pbdRow["ACT_NAME"],
                            pbdRow["ASSIGNED_TO"], pbdRow["REASON"],
                            pbdRow["EMBACCT"], r2pRow["KEY"], "R2P",
                            DaysToDate(Constants.EmbillzPadStartDays, Convert.ToDouble(r2pRow["SECMODDT"])), "",
                            pbdRow["ROWNUM"], pbdRow["TOTALROWS"]);
                        break;
                    }

                    if (!r2prows.Any() && !c2prows.Any())
                    {
                        mergedDataTable.Rows.Add("", pbdRow["PRACTICE"], pbdRow["DIVISION"], pbdRow["EXTPAYOR"],
                            pbdRow["CHECKAMT"], pbdRow["DEPDT"], "", pbdRow["CHECKNUM"], "",
                            "", pbdRow["DOCNO"], pbdRow["DOCTYPE"], pbdRow["DOCDET"], pbdRow["STATUS"],
                            "", "wf stat", "", "", "", "", "expectepr", "", " ", "", pbdRow["SERVICEID"],
                            pbdRow["COURIER_INST_ID"], pbdRow["MAP_NAME"], pbdRow["ACT_NAME"], pbdRow["ASSIGNED_TO"], 
                            pbdRow["REASON"], pbdRow["EMBACCT"], "", "PBD", "", "", pbdRow["ROWNUM"], pbdRow["TOTALROWS"]);
                    }
                }
            }

            Log.Debug("Exit GetPostBdWithC2PR2PDataMerged()");
            return mergedDataTable;
        }

        private DataSet GetPostBdWithC2PR2PData(string workstation, string username,
            string mapName, string actName, string practice, string division,
            string depDtStart, string depDtEnd, string embAcct, string reason, string checkNum,
            string checkAmountMin, string checkAmountMax, string paidAmountMin, string paidAmountMax,
            string docType, string docDet, string who, string docNo, string parDocNo, 
            bool includeMatched = false, int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT)
        {
            Log.DebugFormat("Enter GetPostBdWithC2PR2PData() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataSet postBdWithC2PR2PDataSet = new DataSet("PostBdWithC2PR2PData");
            DataTable postBdDataTable = new DataTable("postbd");
            DataTable c2pDataTable = new DataTable("c2p");
            DataTable r2pDataTable = new DataTable("r2p");

            postBdDataTable = SearchPostBd(workstation, username, mapName, actName, practice,
                division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin, checkAmountMax,
                paidAmountMin, paidAmountMax, docType, docDet, who, docNo, parDocNo, startRow, numRows);

            List<string> myDocNos = new List<string>();

            foreach (DataRow row in postBdDataTable.Rows)
            {
                myDocNos.Add(row["DOCNO"].ToString());
            }

            Debug.WriteLine("Fetching Progress data for [{0}] docnos", myDocNos.Count());

            c2pDataTable.Columns.Add("DOCNO");
            c2pDataTable.Columns.Add("PARDOCNO");
            c2pDataTable.Columns.Add("CHECKDT");
            c2pDataTable.Columns.Add("CSTATUS");
            c2pDataTable.Columns.Add("DEPDT");
            c2pDataTable.Columns.Add("DEPOSITSEQ");
            c2pDataTable.Columns.Add("EXTPAYCD");
            c2pDataTable.Columns.Add("FILEDT");
            c2pDataTable.Columns.Add("MATCHID");
            c2pDataTable.Columns.Add("MATCHSTAT");
            c2pDataTable.Columns.Add("MATCHUID");
            c2pDataTable.Columns.Add("NOTESKEY");
            c2pDataTable.Columns.Add("ORIGCD");
            c2pDataTable.Columns.Add("ORIGNO");
            c2pDataTable.Columns.Add("SUSPENDNO");
            c2pDataTable.Columns.Add("COMINGLESTAT");
            c2pDataTable.Columns.Add("CHECKAMT");
            c2pDataTable.Columns.Add("CHECKNUM");
            c2pDataTable.Columns.Add("DIV");
            c2pDataTable.Columns.Add("DOCDET");
            c2pDataTable.Columns.Add("DOCTYPE");
            c2pDataTable.Columns.Add("KEY");
            c2pDataTable.Columns.Add("PRACTICE");
            c2pDataTable.Columns.Add("SERVICEID");
            c2pDataTable.Columns.Add("STAT");
            c2pDataTable.Columns.Add("NOTETEXT");
            c2pDataTable.Columns.Add("SECMODDT");

            var c2ps = GetNavC2PByDocNoList(myDocNos, includeMatched);
            foreach (var c2p in c2ps)
            {
                c2pDataTable.Rows.Add(c2p.DOCNO, c2p.PARDOCNO, c2p.CHECKDT, c2p.CSTATUS,
                    c2p.DEPDT, c2p.DEPOSITSEQ, c2p.EXTPAYCD, c2p.FILEDT,
                    c2p.MATCHID, c2p.MATCHSTAT, c2p.MATCHUID, c2p.NOTESKEY, c2p.ORIGCD,
                    c2p.ORIGNO, c2p.SUSPENDNO, c2p.COMINGLESTAT, c2p.CHECKAMT, c2p.CHECKNUM,
                    c2p.DIV, c2p.DOCDET, c2p.DOCTYPE, c2p.KEY, c2p.PRACTICE,
                    c2p.SERVICEID, c2p.STAT, c2p.NOTETEXT, c2p.SECMODDT);
            }

            r2pDataTable.Columns.Add("DOCNO");
            r2pDataTable.Columns.Add("PARDOCNO");
            r2pDataTable.Columns.Add("EXTPAYCD");
            r2pDataTable.Columns.Add("FILEDT");
            r2pDataTable.Columns.Add("MATCHID");
            r2pDataTable.Columns.Add("MATCHSTAT");
            r2pDataTable.Columns.Add("MATCHUID");
            r2pDataTable.Columns.Add("ORIGCD");
            r2pDataTable.Columns.Add("ORIGNO");
            r2pDataTable.Columns.Add("PRCHKDT");
            r2pDataTable.Columns.Add("PROVID");
            r2pDataTable.Columns.Add("RSTATUS");
            r2pDataTable.Columns.Add("SUSPENDNO");
            r2pDataTable.Columns.Add("REMITFILEID");
            r2pDataTable.Columns.Add("CHECKAMT");
            r2pDataTable.Columns.Add("CHECKNUM");
            r2pDataTable.Columns.Add("DIV");
            r2pDataTable.Columns.Add("DOCDET");
            r2pDataTable.Columns.Add("DOCTYPE");
            r2pDataTable.Columns.Add("KEY");
            r2pDataTable.Columns.Add("PRACTICE");
            r2pDataTable.Columns.Add("SERVICEID");
            r2pDataTable.Columns.Add("STAT");
            r2pDataTable.Columns.Add("TRANSTYPE");
            r2pDataTable.Columns.Add("NOTETEXT");
            r2pDataTable.Columns.Add("SECMODDT");

            var r2ps = GetNavR2PByDocNoList(myDocNos, includeMatched);
            foreach (var r2p in r2ps)
            {
                r2pDataTable.Rows.Add(r2p.DOCNO, r2p.PARDOCNO, r2p.EXTPAYCD, r2p.FILEDT,
                    r2p.MATCHID, r2p.MATCHSTAT, r2p.MATCHUID, r2p.ORIGCD,
                    r2p.ORIGNO, r2p.PRCHKDT, r2p.PROVID, r2p.RSTATUS, r2p.SUSPENDNO,
                    r2p.REMITFILEID, r2p.CHECKAMT, r2p.CHECKNUM, r2p.DIV, r2p.DOCDET, 
                    r2p.DOCTYPE, r2p.KEY, r2p.PRACTICE, r2p.SERVICEID, r2p.STAT, 
                    r2p.TRANSTYPE, r2p.NOTETEXT, r2p.SECMODDT);
            }

            postBdWithC2PR2PDataSet.Tables.Add(postBdDataTable);
            postBdWithC2PR2PDataSet.Tables.Add(c2pDataTable);
            postBdWithC2PR2PDataSet.Tables.Add(r2pDataTable);

            Log.DebugFormat("Exit GetPostBdWithC2PR2PData() PostBd Rows:[{0}]", postBdDataTable.Rows.Count);
            return postBdWithC2PR2PDataSet;
        }

        /// <summary>
        /// Stored procedure access for searching PostBd
        /// </summary>
        /// <param name="workstation"></param>
        /// <param name="username"></param>
        /// <param name="mapName"></param>
        /// <param name="actName"></param>
        /// <param name="practice"></param>
        /// <param name="division"></param>
        /// <param name="depDtStart"></param>
        /// Date's need to be formatted YYYYMMDD
        /// <param name="depDtEnd"></param>
        /// Date's need to be formatted YYYYMMDD
        /// <param name="embAcct"></param>
        /// <param name="reason"></param>
        /// <param name="checkNum"></param>
        /// <param name="checkAmountMin"></param>
        /// <param name="checkAmountMax"></param>
        /// <param name="paidAmountMin"></param>
        /// <param name="paidAmountMax"></param>
        /// <param name="docType"></param>
        /// <param name="docDet"></param>
        /// <param name="who"></param>
        /// <param name="docNo"></param>
        /// <param name="parDocNo"></param>
        /// <param name="startRow"></param>
        /// <param name="numRows"></param>
        /// <returns></returns>
        public DataTable SearchPostBd(string workstation, string username, string mapName = "", string actName = "", 
            string practice = "", string division = "", string depDtStart = "", string depDtEnd = "", 
            string embAcct = "", string reason = "", string checkNum = "", string checkAmountMin = "", 
            string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "", string docType = "", 
            string docDet = "", string who = "", string docNo = "", string parDocNo = "", int startRow = 1,
            int numRows = Constants.MAX_RESULTS_LIMIT)
        {
            Log.DebugFormat("Enter SearchPostBd {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}", 
                workstation, username, mapName, actName, practice, division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin, 
                checkAmountMax, paidAmountMin, paidAmountMax, docType, docDet, who, docNo, parDocNo, startRow, numRows);

            // Formatting PaidAmount, Check Amount and Deposit Date
            // 4 scenario's
            // 1. Neither specified (do nothing)
            // 2. Only min specified (set max to MAX)
            // 3. Only max specified (set min to MIN)
            // 4. Min max specified (do nothing)

            if (paidAmountMin.Length > 0)
            {
                // 2. Only min specified
                if (paidAmountMax.Length == 0)
                    paidAmountMax = Constants.MAX_MONEY;
            }
            else
            {
                // 3. Only max specified
                if (paidAmountMax.Length > 0)
                    paidAmountMin = Constants.MIN_MONEY;
            }

            if (checkAmountMin.Length > 0)
            {
                // 2. Only min specified
                if (checkAmountMax.Length == 0)
                    checkAmountMax = Constants.MAX_MONEY;
            }
            else
            {
                // 3. Only max specified
                if (checkAmountMax.Length > 0)
                    checkAmountMin = Constants.MIN_MONEY;
            }

            // Format DepDate
            if (depDtStart.Length > 0)
            {
                depDtStart = Regex.Replace(depDtStart, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");

                // 2. Only min specified
                if (depDtEnd.Length == 0)
                    depDtEnd = Regex.Replace(Constants.MAX_DATE, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
                else
                    depDtEnd = Regex.Replace((int.Parse(depDtEnd) + 1).ToString(), @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
            }
            else
            {
                // 3. Only max specified
                if (depDtEnd.Length > 0)
                {
                    depDtEnd = Regex.Replace((int.Parse(depDtEnd) + 1).ToString(), @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
                    depDtStart = Regex.Replace(Constants.MIN_DATE, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
                }
            }

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
            dataTable.Columns.Add("ROWNUM");
            dataTable.Columns.Add("TOTALROWS");

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
                var param19 = new OracleParameter("p_startrow", OracleDbType.Int32, startRow,
                    ParameterDirection.Input);
                var param20 = new OracleParameter("p_numrows", OracleDbType.Int32, numRows,
                    ParameterDirection.Input);
                var param21 = new OracleParameter("cursor1", OracleDbType.RefCursor,
                    ParameterDirection.Output);

                try
                {
                    var query =
                        db.Database.SqlQuery<GetPostBdRowData>(
                            "BEGIN EMBILLZNAVPKG.GET_POSTBD_RANGE(:p_MAP_NAME,:p_ACT_NAME,:p_practice,:p_division,:p_depdt_start,:p_depdt_end,:p_embacct,:p_reason,:p_checknum,:p_checkamt_min,:p_checkamt_max,:p_paidamt_min,:p_paidamt_max,:p_doctype,:p_docdet,:p_who,:p_docno,:p_pardocno,:p_startrow,:p_numrows,:cursor1); end;",
                            param1, param2, param3, param4, param5, param6, param7, param8, param9, param10,
                            param11, param12, param13, param14, param15, param16, param17, param18, param19,
                            param20, param21).ToList();

                    foreach (var row in query)
                    {
                        dataTable.Rows.Add(row.MAP_NAME, row.ACT_NAME, row.WHO, row.DOCNO, row.PRACTICE, row.DIVISION
                            , row.EMBACCT, row.REASON, row.DOCTYPE, row.DOCDET, row.DEPDT, row.CHECKNUM, row.CHECKAMT
                            , row.PAIDAMT, row.PARDOCNO, row.RECDATE, row.EXTPAYOR, row.DEPCODE, row.POSTDETAIL_ID
                            , row.SERVICEID, row.CORRECTION_FOLDER, row.ESCALATION_REASON, row.USERNAME, row.TOWID
                            , row.IFN, row.NPAGES, row.DOCGROUP, row.STATUS, row.COURIER_INST_ID, row.RNUM, row.RESULT_COUNT);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            }

            Log.Debug("Exit SearchPostBd()");

            return dataTable;
        }

        /// <summary>
        /// For a given set of search criteria what would be the size of the result set
        /// </summary>
        /// <param name="workstation"></param>
        /// <param name="username"></param>
        /// <param name="mapName"></param>
        /// <param name="actName"></param>
        /// <param name="practice"></param>
        /// <param name="division"></param>
        /// <param name="depDtStart"></param>
        /// <param name="depDtEnd"></param>
        /// <param name="embAcct"></param>
        /// <param name="reason"></param>
        /// <param name="checkNum"></param>
        /// <param name="checkAmountMin"></param>
        /// <param name="checkAmountMax"></param>
        /// <param name="paidAmountMin"></param>
        /// <param name="paidAmountMax"></param>
        /// <param name="docType"></param>
        /// <param name="docDet"></param>
        /// <param name="who"></param>
        /// <param name="docNo"></param>
        /// <param name="parDocNo"></param>
        /// <returns></returns>
        public int SearchPostBdCount(string workstation, string username, string mapName = "", string actName = "", 
            string practice = "", string division = "", string depDtStart = "", string depDtEnd = "", 
            string embAcct = "", string reason = "", string checkNum = "", string checkAmountMin = "", 
            string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "", string docType = "", 
            string docDet = "", string who = "", string docNo = "", string parDocNo = "")
        {
            Log.DebugFormat("Enter SearchPostBdCount {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}", 
                workstation, username, mapName, actName, practice, division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin, 
                checkAmountMax, paidAmountMin, paidAmountMax, docType, docDet, who, docNo, parDocNo);

            int rowCount = 0;

            // Formatting PaidAmount, Check Amount and Deposit Date
            // 4 scenario's
            // 1. Neither specified (do nothing)
            // 2. Only min specified (set max to MAX)
            // 3. Only max specified (set min to MIN)
            // 4. Min max specified (do nothing)

            if (paidAmountMin.Length > 0)
            {
                // 2. Only min specified
                if (paidAmountMax.Length == 0)
                    paidAmountMax = Constants.MAX_MONEY;
            }
            else
            {
                // 3. Only max specified
                if (paidAmountMax.Length > 0)
                    paidAmountMin = Constants.MIN_MONEY;
            }

            if (checkAmountMin.Length > 0)
            {
                // 2. Only min specified
                if (checkAmountMax.Length == 0)
                    checkAmountMax = Constants.MAX_MONEY;
            }
            else
            {
                // 3. Only max specified
                if (checkAmountMax.Length > 0)
                    checkAmountMin = Constants.MIN_MONEY;
            }

            // Format DepDate
            if (depDtStart.Length > 0)
            {
                depDtStart = Regex.Replace(depDtStart, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");

                // 2. Only min specified
                if (depDtEnd.Length == 0)
                    depDtEnd = Regex.Replace(Constants.MAX_DATE, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
                else
                    depDtEnd = Regex.Replace((int.Parse(depDtEnd) + 1).ToString(), @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
            }
            else
            {
                // 3. Only max specified
                if (depDtEnd.Length > 0)
                {
                    depDtEnd = Regex.Replace((int.Parse(depDtEnd) + 1).ToString(), @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
                    depDtStart = Regex.Replace(Constants.MIN_DATE, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
                }
            }

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
                    rowCount =
                        db.Database.SqlQuery<int>(
                            "BEGIN EMBILLZNAVPKG.GET_POSTBD_COUNT(:p_MAP_NAME,:p_ACT_NAME,:p_practice,:p_division,:p_depdt_start,:p_depdt_end,:p_embacct,:p_reason,:p_checknum,:p_checkamt_min,:p_checkamt_max,:p_paidamt_min,:p_paidamt_max,:p_doctype,:p_docdet,:p_who,:p_docno,:p_pardocno,:cursor1); end;",
                            param1, param2, param3, param4, param5, param6, param7, param8, param9, param10,
                            param11, param12, param13, param14, param15, param16, param17, param18, param19).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            }

            Log.Debug("Exit SearchPostBdCount()");
            return rowCount;
        }

        public DataTable SearchPostBdLINQ(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "")
        {
            Log.DebugFormat("Enter SearchPostBd() workstation:[{0}] user:[{1}]", workstation, username);
            
            // WCF can send through LINQ unfriendly null's, catch and correct
            if(string.IsNullOrEmpty(practice))
                practice = "";
            if (string.IsNullOrEmpty(division))
                division = "";
            if (string.IsNullOrEmpty(embAcct))
                embAcct = "";
            if (string.IsNullOrEmpty(mapName))
                mapName = "";
            if (string.IsNullOrEmpty(actName))
                actName = "";
            if (string.IsNullOrEmpty(docType))
                docType = "";
            if (string.IsNullOrEmpty(docDet))
                docDet = "";
            if (string.IsNullOrEmpty(who))
                who = "";
            if (string.IsNullOrEmpty(docNo))
                docNo = "";
            if (string.IsNullOrEmpty(parDocNo))
                parDocNo = "";
            if (string.IsNullOrEmpty(reason))
                reason = "";
            if (string.IsNullOrEmpty(checkNum))
                checkNum = "";
            if (string.IsNullOrEmpty(checkAmountMin))
                checkAmountMin = "";
            if (string.IsNullOrEmpty(checkAmountMax))
                checkAmountMax = "";
            if (string.IsNullOrEmpty(paidAmountMin))
                paidAmountMin = "";
            if (string.IsNullOrEmpty(paidAmountMax))
                paidAmountMax = "";
            if (string.IsNullOrEmpty(depDtStart))
                depDtStart = "";
            if (string.IsNullOrEmpty(depDtEnd))
                depDtEnd = "";

            var dataTable = new DataTable("searchpostbd2");
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
                var query = (from a in db.POSTBDs
                    where (string.IsNullOrEmpty(practice) || a.PRACTICE == practice) &&
                          (string.IsNullOrEmpty(division) || a.DIVISION == division) &&
                          (string.IsNullOrEmpty(embAcct) || a.EMBACCT == embAcct) &&
                          (string.IsNullOrEmpty(mapName) || a.MAP_NAME.Contains(mapName)) &&
                          (string.IsNullOrEmpty(actName) || a.ACT_NAME.Contains(actName)) &&
                          (string.IsNullOrEmpty(docType) || a.DOCTYPE == docType) &&
                          (string.IsNullOrEmpty(docDet) || a.DOCDET == docDet) &&
                          (string.IsNullOrEmpty(who) || a.WHO == who) &&
                          (string.IsNullOrEmpty(docNo) || a.DOCNO == docNo) &&
                          (string.IsNullOrEmpty(parDocNo) || a.PARDOCNO == parDocNo) &&
                          (string.IsNullOrEmpty(reason) || a.REASON.Contains(reason)) &&
                          (string.IsNullOrEmpty(checkNum) || a.CHECKNUM == checkNum) &&
                          (string.IsNullOrEmpty(depDtStart) || String.Compare(a.DEPDT, depDtStart, StringComparison.Ordinal) >= 0) &&
                          (string.IsNullOrEmpty(depDtEnd) || String.Compare(a.DEPDT, depDtEnd, StringComparison.Ordinal) <= 0)
                    select new
                    {
                        MAP_NAME = a.MAP_NAME.Trim(),
                        ACT_NAME = a.ACT_NAME.Trim(),
                        a.WHO,
                        a.DOCNO,
                        a.PRACTICE,
                        a.DIVISION,
                        a.EMBACCT,
                        a.REASON,
                        a.DOCTYPE,
                        a.DOCDET,
                        a.DEPDT,
                        a.CHECKNUM,
                        a.CHECKAMT,
                        a.PAIDAMT,
                        a.PARDOCNO,
                        a.RECDATE,
                        a.EXTPAYOR,
                        a.DEPCODE,
                        a.POSTDETAIL_ID,
                        a.SERVICEID,
                        a.CORRECTION_FOLDER,
                        a.ESCALATION_REASON,
                        USERNAME = a.USERNAME.Trim(),
                        a.TOWID,
                        a.IFN,
                        npages = 1,
                        a.DOCGROUP,
                        a.STATUS,
                        a.COURIER_INST_ID
                    }).Take(Constants.MAX_RESULTS_LIMIT);

                foreach (var item in query)
                {
                    bool addRow = CommonFunctions.StringValueBetweenMinMax(item.PAIDAMT, paidAmountMin, paidAmountMax);

                    if (addRow)
                        addRow = CommonFunctions.StringValueBetweenMinMax(item.CHECKAMT, checkAmountMin, checkAmountMax);

                    if (addRow)
                    {
                        dataTable.Rows.Add(item.MAP_NAME, item.ACT_NAME, item.WHO, item.DOCNO, item.PRACTICE, item.DIVISION,item.EMBACCT, 
                            item.REASON, item.DOCTYPE, item.DOCDET, item.DEPDT, item.CHECKNUM,
                            item.CHECKAMT,item.PAIDAMT, item.PARDOCNO, item.RECDATE, item.EXTPAYOR, item.DEPCODE, item.POSTDETAIL_ID,
                            item.SERVICEID, item.CORRECTION_FOLDER, item.ESCALATION_REASON, item.USERNAME, item.TOWID,
                            item.IFN, item.npages, item.DOCGROUP, item.STATUS, item.COURIER_INST_ID);
                    }
                }
            }

            Log.DebugFormat("Exit SearchPostBd() Results:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        private List<NavC2P> GetNavC2PByDocNoList(List<string> docNos, bool includeMatched = false)
        {
            Log.Debug("Enter GetNavC2PByDocNoList()");

            List<NavC2P> myNavC2PList = new List<NavC2P>();

            using (var db = new OdbcConnection(Constants.DB_PROGRESS_ODBC_CONNECT_STRING))
            {
                db.Open();
                var dbCmd = db.CreateCommand();

                // Split the list into manageable segments
                var arrayOfLists = CommonFunctions.PartitionList(docNos, 50);

                foreach (var list in arrayOfLists)
                {
                    int count = 0;
                    string cmd =
                        @"select 
                            t1.checkdt,t1.cstatus,t1.depdt,t1.""deposit-seq"",t1.extpaycd,t1.filedt,t1.matchid,
                            t1.matchstat,t1.matchuid,t1.noteskey,t1.origcd,t1.origno,t1.suspendno,t1.docno,
                            {fn CONVERT(SUBSTRING(t1.""PARDOCNO"", 1, 30), SQL_VARCHAR)} PARDOCNO,t1.cominglestat,
                            t1.checkamt,t1.checknum,t1.div,t1.docdet,t1.doctype,t1.""key"",t1.practice,
                            t1.serviceid,t1.stat,t2.txt,t1.secmoddt 
                          from 
                            pub.cash2post t1 
                                left outer join pub.""s-Notes"" t2 on t1.""key""=t2.entitykey 
                                    and t2.entitytypecdkey=820 
                                    and t2.practice=t1.practice 
                          where t1.docno in (";

                    foreach (var docNo in list)
                    {
                        count += 1;
                        cmd += String.Format("{0}'{1}'", count > 1 ? "," : "", docNo);
                    }
                    cmd += @")";

                    Debug.WriteLine(cmd);

                    dbCmd.CommandText = cmd;

                    var dbReader = dbCmd.ExecuteReader();

                    while (dbReader.Read())
                    {
                        CommonFunctions.DebugDumpDbReaderRow("cash2post", dbReader);

                        var cstatus = dbReader[1].ToString();
                        var matchstat = dbReader[7].ToString();
                        bool addToList = false;

                        if (cstatus != "D" && !includeMatched && (matchstat == Constants.MatchStatus_Pending 
                                            || matchstat == Constants.MatchStatus_UnMatched 
                                            || matchstat == Constants.MatchStatus_Rejected))
                        {
                            addToList = true;
                        }
                        else if (cstatus != "D" && includeMatched 
                            && (matchstat == Constants.MatchStatus_Approved 
                                || matchstat == Constants.MatchStatus_Pending 
                                || matchstat == Constants.MatchStatus_UnMatched 
                                || matchstat == Constants.MatchStatus_Matched
                                || matchstat == Constants.MatchStatus_Rejected))
                        {
                            addToList = true;
                        }
                        else
                        {
                            //Discard
                        }

                        if (addToList)
                        {
                            var myNavC2P = new NavC2P();

                            if (dbReader[0].ToString() != "")
                                myNavC2P.CHECKDT = (DateTime) dbReader[0];
                            myNavC2P.CSTATUS = cstatus;
                            if (dbReader[2].ToString() != "")
                                myNavC2P.DEPDT = (DateTime) dbReader[2];
                            myNavC2P.DEPOSITSEQ = (int) dbReader[3];
                            myNavC2P.EXTPAYCD = dbReader[4].ToString();
                            if (dbReader[5].ToString() != "")
                                myNavC2P.FILEDT = (DateTime) dbReader[5];
                            myNavC2P.MATCHID = (int) dbReader[6];
                            myNavC2P.MATCHSTAT = matchstat;
                            myNavC2P.MATCHUID = dbReader[8].ToString();
                            myNavC2P.NOTESKEY = (int) dbReader[9];
                            myNavC2P.ORIGCD = dbReader[10].ToString();
                            myNavC2P.ORIGNO = dbReader[11].ToString();
                            myNavC2P.SUSPENDNO = (int) dbReader[12];
                            myNavC2P.DOCNO = dbReader[13].ToString();
                            myNavC2P.PARDOCNO = dbReader[14].ToString();
                            myNavC2P.COMINGLESTAT = dbReader[15].ToString();
                            myNavC2P.CHECKAMT = (decimal) dbReader[16];
                            myNavC2P.CHECKNUM = dbReader[17] != DBNull.Value ? dbReader[17].ToString() : "";
                            myNavC2P.DIV = dbReader[18].ToString();
                            myNavC2P.DOCDET = dbReader[19].ToString();
                            myNavC2P.DOCTYPE = dbReader[20].ToString();
                            myNavC2P.KEY = (int) dbReader[21];
                            myNavC2P.PRACTICE = dbReader[22].ToString();
                            myNavC2P.SERVICEID = dbReader[23].ToString();
                            myNavC2P.STAT = dbReader[24].ToString();
                            myNavC2P.NOTETEXT = dbReader[25].ToString();
                            myNavC2P.SECMODDT = dbReader[26] != DBNull.Value ? Convert.ToDouble(dbReader[26]) : 0.0;

                            myNavC2PList.Add(myNavC2P);
                        }
                    }
                    dbReader.Close();
                }
                db.Close();
            }

            Log.DebugFormat("Num C2P's:[{0}]", myNavC2PList.Count());
            return myNavC2PList;
        }

        private List<NavR2P> GetNavR2PByDocNoList(List<string> docNos, bool includeMatched = false)
        {
            Log.Debug("Enter GetNavR2PByDocNoList()");

            List<NavR2P> myNavR2PList = new List<NavR2P>();

            using (var db = new OdbcConnection( Constants.DB_PROGRESS_ODBC_CONNECT_STRING ))
            {
                db.Open();
                var dbCmd = db.CreateCommand();

                // Split the list into manageable segments
                var arrayOfLists = CommonFunctions.PartitionList(docNos, 50);

                foreach (var list in arrayOfLists)
                {
                    int count = 0;
                    string cmd = @"select 
                        t1.EXTPAYCD,t1.FILEDT,t1.MATCHID,t1.MATCHSTAT,t1.MATCHUID,t1.ORIGCD,t1.ORIGNO,t1.PRCHKDT,
                        {fn CONVERT(SUBSTRING(t1.""PROVID"", 1, 24), SQL_VARCHAR)} PROVID,
                        t1.RSTATUS,t1.SUSPENDNO,t1.DOCNO,
                        {fn CONVERT(SUBSTRING(t1.""PARDOCNO"", 1, 30), SQL_VARCHAR)} PARDOCNO,
                        t1.REMITFILEID, t1.CHECKAMT, t1.CHECKNUM, t1.DIV, t1.DOCDET, t1.DOCTYPE, t1.""KEY"", 
                        t1.PRACTICE, t1.SERVICEID, t1.STAT, t1.TRANSTYPE, t2.txt, t1.secmoddt 
                        from 
                          pub.remit2post t1 
                            left outer join pub.""s-Notes"" t2 on t1.""key""=t2.entitykey   
                              and t2.entitytypecdkey=823 
                              and t2.practice=t1.practice
                        where t1.docno in (";

                    foreach (var docNo in list)
                    {
                        count += 1;
                        cmd += String.Format("{0}'{1}'", count > 1 ? "," : "", docNo);
                    }
                    cmd += @") ";

                    Debug.WriteLine(cmd);

                    dbCmd.CommandText = cmd;

                    var dbReader = dbCmd.ExecuteReader();

                    while (dbReader.Read())
                    {
                        CommonFunctions.DebugDumpDbReaderRow("remit2post", dbReader);

                        var rstatus = dbReader[9].ToString();
                        var matchstat = dbReader[3].ToString();
                        bool addToList = false;

                        if (rstatus != "D" && !includeMatched && (matchstat == Constants.MatchStatus_Pending 
                                            || matchstat == Constants.MatchStatus_UnMatched 
                                            || matchstat == Constants.MatchStatus_Rejected))
                        {
                            addToList = true;
                        }
                        else if (rstatus != "D" && includeMatched 
                            && (matchstat == Constants.MatchStatus_Approved 
                                || matchstat == Constants.MatchStatus_Pending 
                                || matchstat == Constants.MatchStatus_UnMatched 
                                || matchstat == Constants.MatchStatus_Matched
                                || matchstat == Constants.MatchStatus_Rejected))
                        {
                            addToList = true;
                        }
                        else
                        {
                            //Discard
                        }

                        if (addToList)
                        {
                            var myNavR2P = new NavR2P();

                            myNavR2P.EXTPAYCD = dbReader[0].ToString();
                            if (dbReader[1].ToString() != "")
                                myNavR2P.FILEDT = (DateTime) dbReader[1];
                            myNavR2P.MATCHID = (int) dbReader[2];
                            myNavR2P.MATCHSTAT = matchstat;
                            myNavR2P.MATCHUID = dbReader[4].ToString();
                            myNavR2P.ORIGCD = dbReader[5].ToString();
                            myNavR2P.ORIGNO = dbReader[6].ToString();
                            if (dbReader[7].ToString() != "")
                                myNavR2P.PRCHKDT = (DateTime) dbReader[7];
                            myNavR2P.PROVID = dbReader[8].ToString();
                            myNavR2P.RSTATUS = rstatus;
                            myNavR2P.SUSPENDNO = (int) dbReader[10];
                            myNavR2P.DOCNO = dbReader[11].ToString();
                            myNavR2P.PARDOCNO = dbReader[12].ToString();
                            myNavR2P.REMITFILEID = dbReader[13].ToString();
                            myNavR2P.CHECKAMT = (decimal) dbReader[14];
                            myNavR2P.CHECKNUM = dbReader[15] != DBNull.Value ? dbReader[15].ToString() : "";
                            myNavR2P.DIV = dbReader[16].ToString();
                            myNavR2P.DOCDET = dbReader[17].ToString();
                            myNavR2P.DOCTYPE = dbReader[18].ToString();
                            myNavR2P.KEY = (int?) dbReader[19];
                            myNavR2P.PRACTICE = dbReader[20].ToString();
                            myNavR2P.SERVICEID = dbReader[21].ToString();
                            myNavR2P.STAT = dbReader[22].ToString();
                            myNavR2P.TRANSTYPE = dbReader[23].ToString();
                            myNavR2P.NOTETEXT = dbReader[24].ToString();
                            myNavR2P.SECMODDT = dbReader[25] != DBNull.Value ? Convert.ToDouble(dbReader[25]) : 0.0;

                            myNavR2PList.Add(myNavR2P);
                        }
                    }

                    if (dbReader != null)
                    {
                        dbReader.Close();
                    }
                }
                db.Close();
            }

            Log.DebugFormat("Num R2P's:[{0}]", myNavR2PList.Count());
            return myNavR2PList;
        }

        public DataTable GetPostBDByPracticeAndAccount(string workstation, string username,
            string practice, string embAccount)
        {
            Log.DebugFormat("Enter GetPostBDByPracticeAndAccount() Workstation:[{0}], Username:[{1}], Practice:[{2}], embAccount:[{3}]",
                workstation, username, practice, embAccount);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.PRACTICE == practice &&
                          POSTBDs.EMBACCT == embAccount
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }
            
            Log.DebugFormat("Exit GetPostBDByPracticeAndAccount() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByTowId(string workstation, string username, int towId)
        {
            Log.DebugFormat("Enter GetPostBDByTowId() Workstation:[{0}], Username:[{1}], towId:[{2}]",
                workstation, username, towId);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.TOWID == towId
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByTowId() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByPostDetailId(string workstation, string username, int postDetailId)
        {
            Log.DebugFormat("Enter GetPostBDByPostDetaiId() Workstation:[{0}], Username:[{1}], postDetailId:[{2}]",
                workstation, username, postDetailId);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.POSTDETAIL_ID == postDetailId
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByPostDetaiId() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByUsername(string workstation, string username, string searchUsername)
        {
            Log.DebugFormat("Enter GetPostBDByUsername() Workstation:[{0}], Username:[{1}], searchUserName:[{2}]",
                workstation, username, searchUsername);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.USERNAME == username
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByUsername() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByPracticeAndDepDate(string workstation, string username,
            string practice, string depDate)
        {
            Log.DebugFormat("Enter GetPostBDByPracticeAndDepDate() Workstation:[{0}], Username:[{1}] [{2}] [{3}]",
                workstation, username, practice, depDate);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.PRACTICE == practice &&
                          POSTBDs.DEPDT == depDate
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByPracticeAndDepDate() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByCheckNum(string workstation, string username, string checkNum)
        {
            Log.DebugFormat("Enter GetPostBDByCheckNum() Workstation:[{0}], Username:[{1}], checkNum:[{2}]",
                workstation, username, checkNum);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.CHECKNUM == checkNum
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByCheckNum() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByPracticeAndCheckAmount(string workstation, string username,
            string practice, string checkAmount)
        {
            Log.DebugFormat("Enter GetPostBDByPracticeAndCheckAmount() Workstation:[{0}], Username:[{1}], checkAmount:[{2}]",
                workstation, username, checkAmount);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.PRACTICE == practice &&
                          POSTBDs.CHECKAMT == checkAmount
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByPracticeAndCheckAmount() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByDocNo(string workstation, string username, string docNo)
        {
            Log.DebugFormat("Enter GetPostBDByDocNo() Workstation:[{0}], Username:[{1}], docNo:[{2}]",
                workstation, username, docNo);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.DOCNO == docNo
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByDocNo() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDByParDocNo(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetPostBDByParDocNo() Workstation:[{0}], Username:[{1}], parDocNo:[{2}]",
                workstation, username, parDocNo);

            DataTable dataTable = new DataTable("postbd");
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
                var query = (from POSTBDs in db.POSTBDs
                    where POSTBDs.PARDOCNO == parDocNo
                    select new
                    {
                        POSTBDs.MAP_NAME,
                        POSTBDs.ACT_NAME,
                        POSTBDs.WHO,
                        POSTBDs.DOCNO,
                        POSTBDs.PRACTICE,
                        POSTBDs.DIVISION,
                        POSTBDs.EMBACCT,
                        POSTBDs.REASON,
                        POSTBDs.DOCTYPE,
                        POSTBDs.DOCDET,
                        POSTBDs.DEPDT,
                        POSTBDs.CHECKNUM,
                        POSTBDs.CHECKAMT,
                        POSTBDs.PAIDAMT,
                        POSTBDs.PARDOCNO,
                        POSTBDs.RECDATE,
                        POSTBDs.EXTPAYOR,
                        POSTBDs.DEPCODE,
                        POSTBDs.POSTDETAIL_ID,
                        POSTBDs.SERVICEID,
                        POSTBDs.CORRECTION_FOLDER,
                        POSTBDs.ESCALATION_REASON,
                        POSTBDs.USERNAME,
                        POSTBDs.TOWID,
                        POSTBDs.IFN,
                        POSTBDs.DOCGROUP,
                        POSTBDs.STATUS,
                        POSTBDs.COURIER_INST_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.MAP_NAME
                        , row.ACT_NAME
                        , row.WHO
                        , row.DOCNO
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.REASON
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PAIDAMT
                        , row.PARDOCNO
                        , row.RECDATE
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.POSTDETAIL_ID
                        , row.SERVICEID
                        , row.CORRECTION_FOLDER
                        , row.ESCALATION_REASON
                        , row.USERNAME
                        , row.TOWID
                        , row.IFN
                        , 1
                        , row.DOCGROUP
                        , row.STATUS
                        , row.COURIER_INST_ID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDByParDocNo() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDOrderedByMapAndActivity(string workstation, string username,
            string map, string activity)
        {
            Log.DebugFormat("Enter GetPostBDOrderedByMapAndActivity() Workstation:[{0}], Username:[{1}] [{2}] [{3}]",
                workstation, username, map, activity);

            DataTable dataTable = new DataTable("postbd");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("ASSIGNED_TO");
            dataTable.Columns.Add("COURIERCOUNT");

            using (var db = new TowerModelContainer())
            {
                var query = (from b in db.POSTBDs
                    where
                        b.MAP_NAME == map &&
                        b.ACT_NAME == activity
                    group b by new
                    {
                        b.PRACTICE,
                        b.DIVISION,
                        b.DEPDT,
                        b.RECDATE,
                        b.WHO
                    }
                    into g
                    orderby
                        g.Key.PRACTICE,
                        g.Key.DIVISION,
                        g.Key.DEPDT,
                        g.Key.RECDATE,
                        g.Key.WHO
                    select new
                    {
                        g.Key.PRACTICE,
                        g.Key.DIVISION,
                        g.Key.RECDATE,
                        g.Key.DEPDT,
                        g.Key.WHO,
                        CourierCount = g.Count()
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.PRACTICE
                        , row.DIVISION
                        , row.RECDATE
                        , row.DEPDT
                        , row.WHO
                        , row.CourierCount
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDOrderedByMapAndActivity() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostBDOrderedByMapAndActivityDepDt(string workstation, string username,
            string map, string activity)
        {
            Log.DebugFormat("Enter GetPostBDOrderedByMapAndActivityDeptDt() Workstation:[{0}], Username:[{1}] [{2}] [{3}]",
                workstation, username, map, activity);

            DataTable dataTable = new DataTable("postbd");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("ASSIGNED_TO");
            dataTable.Columns.Add("COURIERCOUNT");

            using (var db = new TowerModelContainer())
            {

                var query = (from b in db.POSTBDs
                    where
                        b.MAP_NAME == map &&
                        b.ACT_NAME == activity
                    group b by new
                    {
                        b.PRACTICE,
                        b.DIVISION,
                        b.DEPDT,
                        b.RECDATE,
                        b.WHO
                    }
                    into g
                    orderby
                        g.Key.PRACTICE,
                        g.Key.DIVISION,
                        g.Key.DEPDT,
                        g.Key.RECDATE,
                        g.Key.WHO
                    select new
                    {
                        g.Key.PRACTICE,
                        g.Key.DIVISION,
                        g.Key.RECDATE,
                        g.Key.DEPDT,
                        g.Key.WHO,
                        CourierCount = g.Count()
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.PRACTICE
                        , row.DIVISION
                        , row.RECDATE
                        , row.DEPDT
                        , row.WHO
                        , row.CourierCount
                        );
                }
            }

            Log.DebugFormat("Exit GetPostBDOrderedByMapAndActivityDepDt() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        private class PostBdList
        {
            public int COURIER_INST_ID { set; get; }
            public string ACT_NAME { set; get; }
            public int CREATE_DATE { set; get; }
            public int IN_ACT_DATE { set; get; }
            public string LAST_NAME { set; get; }
            public string FIRST_NAME { set; get; }
        }

        public DataTable GetWorkItemsByIFN(string workstation, string username, 
            string ifn)
        {
            Log.DebugFormat("Enter GetWorkItemsByIFN() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("workitems");
            dataTable.Columns.Add("COURIER_INST_ID");
            dataTable.Columns.Add("ACT_NAME");
            dataTable.Columns.Add("CREATE_DATE");
            dataTable.Columns.Add("IN_ACT_DATE");
            dataTable.Columns.Add("LAST_NAME");
            dataTable.Columns.Add("FIRST_NAME");

            using (IfamModelContainer context = new IfamModelContainer())
            {
                string term = ifn;
                OracleParameter p = new OracleParameter("param1", term);
                object[] parameters = new object[] { p };

                var b = context.Database.SqlQuery<PostBdList>(@"SELECT a.courier_inst_id, a.act_name, b.create_date, b.in_act_date, c.last_name, 
                            c.first_name FROM tower.postbd a LEFT OUTER JOIN ifam.courier_instance b 
                            ON a.courier_inst_id = b.courier_inst_id LEFT OUTER JOIN ifam.users c on 
                            b.user_id = c.user_id WHERE a.ifn = :param1", parameters);

                foreach (var row in b)
                {
                    dataTable.Rows.Add(row.COURIER_INST_ID, row.ACT_NAME,
                        row.CREATE_DATE, row.IN_ACT_DATE, row.LAST_NAME, row.FIRST_NAME);
                }
            }

            Log.DebugFormat("Exit GetWorkItemsByIFN() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        private string DaysToDate(int padDays, double unixTime)
        {
            var unixDays = Math.Round(unixTime, 0) + padDays;
            var unixStartDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return String.Format("{0:MM/dd/yyyy}", unixStartDate.AddDays(unixDays));
        }

        private class GetPostBdRowData
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
            public string DEPDT { set; get; }
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
            public string IFN { set; get; }
            public int? NPAGES { set; get; }
            public string DOCGROUP { set; get; }
            public string STATUS { set; get; }
            public int? COURIER_INST_ID { set; get; }
            public int? RNUM { set; get; }
            public int? RESULT_COUNT { set; get; }
        }

    }
}
