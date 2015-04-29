using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Rti.DataModel;
using Rti.Imaging;
using Rti.Imaging.Settings;

namespace Rti.DocumentManagerServer.DataAccess
{
    class DocUtils
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string CreateDocNo(string workstation, string username)
        {
            Log.DebugFormat("Enter CreateDocNo() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            string nextDocNo;
            
            // Year(last 2) + JulianDate(3) + sourceid(2) + document_number_seq.nextval(8)
            var calendar = new JulianCalendar();
            var julianDay = calendar.GetDayOfYear(DateTime.Now);
            var year = DateTime.Now.Year % 100;

            try
            {
                var db = new TowerModelContainer();
                var query = db.Database.SqlQuery<int>(@"SELECT document_number_seq.nextval FROM DUAL");
                nextDocNo = query.FirstOrDefault().ToString("D8");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return "";
            }

            var formattedDocNo = year.ToString("D2") + julianDay.ToString("D3") + Constants.SOURCEID + nextDocNo;

            Log.DebugFormat("Exit CreateDocNo:[{0}]", formattedDocNo);
            return formattedDocNo;
        }

        public DataTable ListRoutingInfo(string workstation, string username)
        {
            Log.DebugFormat("Enter ListRoutingInfio() Workstation:[{0}], Username:[{1}]", workstation, username);

            DataTable dataTable = new DataTable("routinginfo");
            dataTable.Columns.Add("StartruleLookupId");
            dataTable.Columns.Add("DocType");
            dataTable.Columns.Add("DocDet");
            dataTable.Columns.Add("Reason"); 
            dataTable.Columns.Add("SecurityGroup");
            dataTable.Columns.Add("SourceType");
            dataTable.Columns.Add("StatusCode");
            dataTable.Columns.Add("MapName");
            dataTable.Columns.Add("ActName");
            dataTable.Columns.Add("Description");

            using (var db = new TowerModelContainer())
            {
                var results = (from a in db.STARTRULE_LOOKUP
                    join b in db.SOURCETYPE_LOOKUP on a.SOURCETYPE_LOOKUP_ID equals b.SOURCETYPE_LOOKUP_ID into b_join
                    from b in b_join.DefaultIfEmpty()
                    join c in db.POSTDETAIL_STATUS_LOOKUP on a.POSTDETAIL_STATUS_LOOKUP_ID equals
                        c.POSTDETAIL_STATUS_LOOKUP_ID into c_join
                    from c in c_join.DefaultIfEmpty()
                    orderby
                        a.REASON,
                        a.DOCTYPE,
                        a.DOCDET,
                        a.SECURITY_GROUP
                    select new
                    {
                        a.STARTRULE_LOOKUP_ID,
                        a.DOCTYPE,
                        a.DOCDET,
                        a.REASON,
                        a.SECURITY_GROUP,
                        sourcetype = b.TYPECODE,
                        statuscode = c.TYPECODE, 
                        c.MAP_NAME, 
                        c.ACT_NAME,
                        c.DESCRIPTION
                    });

                foreach (var row in results)
                {
                    dataTable.Rows.Add(row.STARTRULE_LOOKUP_ID, row.DOCTYPE, row.DOCDET, row.REASON,
                        row.SECURITY_GROUP, row.sourcetype, row.statuscode, row.MAP_NAME, row.ACT_NAME,
                        row.DESCRIPTION);
                }
            }

            Log.DebugFormat("Exit ListRoutingInfio() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable ListStartRuleLookups(string workstation, string username)
        {
            Log.DebugFormat("Enter ListStartRuleLookups() Workstation:[{0}], Username:[{1}]", workstation, username);

            DataTable dataTable = new DataTable("startrulelookup");
            dataTable.Columns.Add("STARTRULE_LOOKUP_ID");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("SECURITY_GROUP");
            dataTable.Columns.Add("SOURCETYPE_LOOKUP_ID");
            dataTable.Columns.Add("POSTDETAIL_STATUS_LOOKUP_ID");

            using (var db = new TowerModelContainer())
            {
                var query = (from a in db.STARTRULE_LOOKUP
                    select new
                    {
                        a.STARTRULE_LOOKUP_ID,
                        a.DOCTYPE,
                        a.DOCDET,
                        a.REASON,
                        a.SECURITY_GROUP,
                        a.SOURCETYPE_LOOKUP_ID,
                        a.POSTDETAIL_STATUS_LOOKUP_ID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(row.STARTRULE_LOOKUP_ID, row.DOCTYPE, row.DOCDET, row.REASON,
                        row.SECURITY_GROUP, row.SOURCETYPE_LOOKUP_ID, row.POSTDETAIL_STATUS_LOOKUP_ID);
                }
            }

            Log.DebugFormat("Exit ListStartRuleLookups() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable ListPractices(string workstation, string username)
        {
            Log.DebugFormat("Enter ListPractices() Workstation:[{0}], Username:[{1}]", workstation, username);

            DateTime saveNow = DateTime.Now;

            DataTable dataTable = new DataTable("practices");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMPRAC");
            dataTable.Columns.Add("CODETYPE");
            dataTable.Columns.Add("RLOGGRP");
            dataTable.Columns.Add("CODEGRP");
            dataTable.Columns.Add("VERIFYGRP");
            dataTable.Columns.Add("DATATYPE");
            dataTable.Columns.Add("EFFDATE");
            dataTable.Columns.Add("ENDDATE");
            dataTable.Columns.Add("HOSPITALIST");
            dataTable.Columns.Add("IQSERVER_NAME");
            dataTable.Columns.Add("CITY");
            dataTable.Columns.Add("STATE");
            dataTable.Columns.Add("REGION_ABBR");

            using (var db = new TowerModelContainer())
            {
                var query = (from rticlient in db.RTICLIENTs
                    where
                        rticlient.POST_EFFDATE == null ||
                        rticlient.POST_EFFDATE <= saveNow
                    orderby
                        rticlient.PRACTICE
                    select new
                    {
                        rticlient.PRACTICE,
                        rticlient.DIVISION,
                        rticlient.EMPRAC,
                        rticlient.CODETYPE,
                        rticlient.RLOGGRP,
                        rticlient.CODEGRP,
                        rticlient.VERIFYGRP,
                        rticlient.DATATYPE,
                        rticlient.EFFDATE,
                        rticlient.ENDDATE,
                        rticlient.HOSPITALIST,
                        rticlient.IQSERVER_NAME,
                        rticlient.CITY,
                        rticlient.STATE,
                        rticlient.REGION_ABBR
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(row.PRACTICE,
                        CommonFunctions.ToEmptyStringIfDbNull(row.DIVISION),
                        row.EMPRAC,
                        row.CODETYPE, row.RLOGGRP, row.CODEGRP, row.VERIFYGRP,
                        row.DATATYPE, row.EFFDATE, row.ENDDATE, row.HOSPITALIST,
                        row.IQSERVER_NAME, row.CITY, row.STATE, row.REGION_ABBR);
                }
            }

            Log.DebugFormat("Exit ListPractices() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable ListDocTypeLookups(string workstation, string username)
        {
            Log.DebugFormat("Enter ListDocTypeLookups()Workstation:[{0}], Username:[{1}]", 
                workstation, username);

            DataTable dataTable = new DataTable("doctype_lookup");
            dataTable.Columns.Add("DOCTYPE_LOOKUP_ID");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DESCRIPTION");

            using (var db = new TowerModelContainer())
            {
                var query = (from doctypeLookup in db.DOCTYPE_LOOKUP
                    select new
                    {
                        doctypeLookup.DOCTYPE_LOOKUP_ID
                        ,doctypeLookup.DOCTYPE
                        ,doctypeLookup.DESCRIPTION
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.DOCTYPE_LOOKUP_ID
                        ,row.DOCTYPE
                        ,row.DESCRIPTION
                        );
                }
            }
            Log.DebugFormat("Exit ListDocTypeLookups() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public int GetTowerSequenceNextValue(string workstation, string username)
        {
            Log.DebugFormat("Enter GetTowerSequenceNextValue() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            using (TowerModelContainer context = new TowerModelContainer())
            {
                var b = context.Database.SqlQuery<int>("SELECT tower_seq.nextval FROM DUAL");
                Log.Debug("Exit GetTowerSequenceNextValue()");
                return b.FirstOrDefault();
            }
        }

        public DataTable GetBillDocByDocNo(string workstation, string username, string docNo)
        {
            Log.DebugFormat("Enter GetBillDocByDocNo() Workstation:[{0}], Username:[{1}], docNo;[{2}]",
                workstation, username, docNo);

            DataTable dataTable = new DataTable("billdoc");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOS");
            dataTable.Columns.Add("TNIFNDS");
            dataTable.Columns.Add("TNIFNID");
            dataTable.Columns.Add("TNSTAT");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("ASDOCNO");
            dataTable.Columns.Add("RECDATE");

            using (var db = new TowerModelContainer())
            {
                var query = (from BILLDOCs in db.BILLDOCs
                    where BILLDOCs.DOCNO == docNo
                    select new
                    {
                        BILLDOCs.IFNDS,
                        BILLDOCs.IFNID,
                        BILLDOCs.NPAGES,
                        BILLDOCs.DOCTYPE,
                        BILLDOCs.DOS,
                        BILLDOCs.TNIFNDS,
                        BILLDOCs.TNIFNID,
                        BILLDOCs.TNSTAT,
                        BILLDOCs.TOWID,
                        BILLDOCs.PRACTICE,
                        BILLDOCs.DIVISION,
                        BILLDOCs.ACCTNUM,
                        BILLDOCs.ASDOCNO,
                        BILLDOCs.RECDATE
                    });

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(
                        formattedIFN
                        , row.NPAGES
                        , row.DOCTYPE
                        , row.DOS
                        , row.TNIFNDS
                        , row.TNIFNID
                        , row.TNSTAT
                        , row.TOWID
                        , row.PRACTICE
                        , row.DIVISION
                        , row.ACCTNUM
                        , row.ASDOCNO
                        , row.RECDATE
                        );
                }
            }

            Log.DebugFormat("Exit GetBillDocByDocNo() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetRtiBDByAccountName(string workstation, string username, string accountName)
        {
            Log.DebugFormat("Enter GetRtiBDByAccountName() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("rtibd");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("DOS");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("CODETYPE");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DATATYPE");
            dataTable.Columns.Add("ASSIGNED_TO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("ESCALATION_REASON");
            dataTable.Columns.Add("FINANCIAL_CLASS");
            dataTable.Columns.Add("STARTSTEP");
            dataTable.Columns.Add("COURIER_INST_ID");
            dataTable.Columns.Add("PLX_COURIER_ID");
            dataTable.Columns.Add("NUMOFCHARTS");
            dataTable.Columns.Add("CORRECTION_FOLDER");
            dataTable.Columns.Add("RTIACCTNUM");

            using (var db = new TowerModelContainer())
            {
                var query = (from b in db.RTIBDs
                    where
                        b.ACT_NAME == accountName
                    orderby
                        b.PRACTICE,
                        b.DIVISION,
                        b.DOS,
                        b.RECDATE
                    select new
                    {
                        b.PRACTICE,
                        b.DIVISION,
                        b.RECDATE,
                        b.DOS,
                        b.DOCNO,
                        b.CODETYPE,
                        b.ACCTNUM,
                        b.IFN,
                        b.DATATYPE,
                        b.WHO,
                        b.DOCTYPE,
                        b.REASON,
                        b.ESCALATION_REASON,
                        b.FINANCIAL_CLASS,
                        b.STARTSTEP,
                        b.COURIER_INST_ID,
                        b.PLX_COURIER_ID,
                        b.NUMOFCHARTS,
                        b.CORRECTION_FOLDER,
                        b.RTIACCTNUM
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.PRACTICE
                        , row.DIVISION
                        , row.RECDATE
                        , row.DOS
                        , row.DOCNO
                        , row.CODETYPE
                        , row.ACCTNUM
                        , row.IFN
                        , row.DATATYPE
                        , row.WHO
                        , row.DOCTYPE
                        , row.REASON
                        , row.ESCALATION_REASON
                        , row.FINANCIAL_CLASS
                        , row.STARTSTEP
                        , row.COURIER_INST_ID
                        , row.PLX_COURIER_ID
                        , row.NUMOFCHARTS
                        , row.CORRECTION_FOLDER
                        , row.RTIACCTNUM
                        );
                }
            }

            Log.DebugFormat("Exit GetRtiBDByAccountName() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetStartActivityJoinedPostDetail(string workstation, string username)
        {
            Log.DebugFormat("Enter GetStartActivityJoinedPostDetail() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("startrule_lookup");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("SECURITY_GROUP");
            dataTable.Columns.Add("TYPECODE");
            dataTable.Columns.Add("DESCRIPTION");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");

            using (var db = new TowerModelContainer())
            {
                var query = (from a in db.STARTRULE_LOOKUP
                    from b in db.POSTDETAIL_STATUS_LOOKUP
                    where
                        a.POSTDETAIL_STATUS_LOOKUP_ID == b.POSTDETAIL_STATUS_LOOKUP_ID
                    select new
                    {
                        a.DOCTYPE,
                        a.DOCDET,
                        a.REASON,
                        a.SECURITY_GROUP,
                        b.TYPECODE,
                        b.DESCRIPTION,
                        b.MAP_NAME,
                        b.ACT_NAME
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.DOCTYPE
                        , row.DOCDET
                        , row.REASON
                        , row.SECURITY_GROUP
                        , row.TYPECODE
                        , row.DESCRIPTION
                        , row.MAP_NAME
                        , row.ACT_NAME
                        );
                }
            }

            Log.DebugFormat("Exit GetStartActivityJoinedPostDetail() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetSourceTypeLookupByTypeCode(string workstation, string username)
        {
            Log.DebugFormat("Enter GetSourceTypeLookupByTypeCode() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("sourcetype_lookup");
            dataTable.Columns.Add("SOURCETYPE_LOOKUP_ID");
            dataTable.Columns.Add("TYPECODE");
            dataTable.Columns.Add("DESCRIPTION");

            using (var db = new TowerModelContainer())
            {
                var query = (from SOURCETYPE_LOOKUP in db.SOURCETYPE_LOOKUP
                    where SOURCETYPE_LOOKUP.TYPECODE.StartsWith("2")
                    select new
                    {
                        SOURCETYPE_LOOKUP.SOURCETYPE_LOOKUP_ID,
                        SOURCETYPE_LOOKUP.TYPECODE,
                        SOURCETYPE_LOOKUP.DESCRIPTION
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.SOURCETYPE_LOOKUP_ID
                        , row.TYPECODE
                        , row.DESCRIPTION
                        );
                }
            }

            Log.DebugFormat("Exit GetSourceTypeLookupByTypeCode() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetDocTypes(string workstation, string username)
        {
            Log.DebugFormat("Enter GetDocTypes() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("doctype_lookup");
            dataTable.Columns.Add("TYPE_KEY");
            dataTable.Columns.Add("TYPE_DESCRIPTION");
            dataTable.Columns.Add("DETAIL_KEY");
            dataTable.Columns.Add("DETAIL_DESCRIPTION");

            using (var db = new TowerModelContainer())
            {
                var query = (from DOCTYPE_LOOKUP in db.DOCTYPE_LOOKUP
                    join DOCDETAIL_LOOKUP in db.DOCDETAIL_LOOKUP
                        on new {DOCTYPE_LOOKUP_ID = (decimal) DOCTYPE_LOOKUP.DOCTYPE_LOOKUP_ID}
                        equals new {DOCTYPE_LOOKUP_ID = DOCDETAIL_LOOKUP.DOCTYPE_LOOKUP_ID} into DOCDETAIL_LOOKUP_join
                    from DOCDETAIL_LOOKUP in DOCDETAIL_LOOKUP_join.DefaultIfEmpty()
                    orderby
                        DOCTYPE_LOOKUP.DOCTYPE,
                        DOCDETAIL_LOOKUP.DOCDET
                    select new
                    {
                        TYPE_KEY = DOCTYPE_LOOKUP.DOCTYPE,
                        TYPE_DESCRIPTION = DOCTYPE_LOOKUP.DESCRIPTION,
                        DETAIL_KEY = DOCDETAIL_LOOKUP.DOCDET,
                        DETAIL_DESCRIPTION = DOCDETAIL_LOOKUP.DESCRIPTION
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.TYPE_KEY
                        , row.TYPE_DESCRIPTION
                        , row.DETAIL_KEY
                        , row.DETAIL_DESCRIPTION
                        );
                }
            }

            Log.DebugFormat("Exit GetDocTypes() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        private class CourierUser
        {
            public int COURIER_INST_ID { set; get; }
            public string USER_NAME { set; get; }
            public string RETURN_STATE { set; get; }
            public string MAP_NAME { set; get; }
            public string ACT_NAME { set; get; }
            public DateTime MYRETURN { set; get; }
            public int USER_ID { set; get; }
        }

        public DataTable GetLastTenCouriers(string workstation, string username, string loginName)
        {
            Log.DebugFormat("Enter GetLastTenCouriers() Workstation:[{0}], Username:[{1}] loginName:[{2}]",
                workstation, username, loginName);

            DataTable dataTable = new DataTable("listlastten");
            dataTable.Columns.Add("COURIRER_INST_ID");
            dataTable.Columns.Add("USER_NAME");
            dataTable.Columns.Add("RETURN_STATE");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");
            dataTable.Columns.Add("MYRETURN");
            dataTable.Columns.Add("USER_ID");

            using (IfamModelContainer context = new IfamModelContainer())
            {
                OracleParameter newparam = new OracleParameter();
                newparam.Value = loginName;
                newparam.Direction = ParameterDirection.Input;
                newparam.DbType = DbType.String;
                newparam.ParameterName = "param2";

                var b = context.Database.SqlQuery<CourierUser>(@"
                    SELECT 
                        a.courier_inst_id, 
                        a.user_name, 
                        a.return_state, 
                        a.map_name, 
                        a.act_name, 
                        New_time(TO_DATE('19700101', 'YYYYMMDD') + 1 / 24 / 60 / 60 * a.return_date, 'GMT', 'EDT') AS myReturn, 
                        b.user_id
                    FROM 
                        ifam.courier_log a, ifam.users b 
                    WHERE b.user_id = a.user_id 
                        AND ROWNUM < 11 
                        AND New_time(TO_DATE('19700101', 'YYYYMMDD') + 1 / 24 / 60 / 60 * a.return_date, 'GMT', 'EDT') >= SYSDATE - 1
                        and trim(b.login_name) = :param2
                    ORDER BY 
                        a.return_date DESC", newparam);

                foreach (var row in b)
                {
                    dataTable.Rows.Add(row.COURIER_INST_ID, row.USER_NAME, row.RETURN_STATE, row.MAP_NAME, row.ACT_NAME, row.MYRETURN, row.USER_ID);
                }
            }

            Log.Debug("Exit GetLastTenCouriers()");
            return dataTable;
        }

        public DataTable GetRtiBDByMapAndActivity(string workstation, string username, 
            string map, string activity)
        {
            Log.DebugFormat("Enter GetRtiBDByMapAndActivity() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("rtibd");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("DOS");
            dataTable.Columns.Add("ASSIGNED_TO");

            using (var db = new TowerModelContainer())
            {
                var query = (from b in db.RTIBDs
                    where
                        b.MAP_NAME == map &&
                        b.ACT_NAME == activity
                    group b by new
                    {
                        b.PRACTICE,
                        b.DIVISION,
                        b.RECDATE,
                        b.DOS,
                        b.WHO
                    }
                    into g
                    orderby
                        g.Key.PRACTICE,
                        g.Key.DIVISION,
                        g.Key.DOS,
                        g.Key.RECDATE,
                        g.Key.WHO
                    select new
                    {
                        g.Key.PRACTICE,
                        g.Key.DIVISION,
                        g.Key.RECDATE,
                        g.Key.DOS,
                        g.Key.WHO,
                        CourierCount = g.Count()
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.PRACTICE
                        , row.DIVISION
                        , row.RECDATE
                        , row.DOS
                        , row.WHO
                        , row.CourierCount
                        );
                }
            }

            Log.DebugFormat("Exit GetRtiBDByMapAndActivity() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public bool ConvertPDFtoTif(string workstation, string username, string pdfFileName, out string message)
        { 
            Log.DebugFormat("Enter ConvertPDFtoTif() Workstation:[{0}], Username:[{1}], PDF File:[{2}]", 
                workstation, username, pdfFileName);

            int intPageCount = 0;
            bool status = false;
            string outFile = pdfFileName.Replace(".pdf", ".tif");
            message = "";

            var convertWith = ConfigurationManager.AppSettings[Constants.ConvertPdfToTiffWith];

            if (convertWith == Constants.Atalasoft)
            {
                Log.Debug("ConvertPDFtoTif() Converting with Atalasoft");
                var imageProc = new ImageProcessor();
                string returnMessage = "";

                try
                {
                    imageProc.convertPDF2TIF(pdfFileName,
                        outFile,
                        1,
                        ref intPageCount,
                        ref returnMessage);

                    message = returnMessage;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    throw;
                }

                if (message != null && message == "")
                {
                    status = true;
                }
                else
                {
                    Log.Error(message);
                    
                    Log.Debug("ConvertPDFtoTif() Converting with Atalasoft failed, attempting with GhostScript...");
                
                    OpenImageProcessor.convertPDF2TIF(pdfFileName, outFile, 1, ref intPageCount, ref message);
//                    OpenImageProcessor.GenerateOutput(pdfFileName, outFile, settings);
                    
                    if (System.IO.File.Exists(outFile))
                    {
                        status = true;
                    }
                    else
                    {
                        message = "GhostScript failed to create TIF file, file not found";
                    }
                }
            }
            else
            {
                Log.Debug("ConvertPDFtoTif() Converting with GhostScript");
                
                var settings = new GhostscriptSettings
                {
                    Device = GhostscriptDevices.tiffg4,
                    Compression = true,
                    Page = new GhostscriptPages
                    {
                        AllPages = true
                    },
                    Resolution = new Size
                    {
                        // Render at dpi
                        Height = 200,
                        Width = 200
                    },
                    Size = new GhostscriptPageSize
                    {
                        // The dimentions of the incoming PDF must be
                        // specified. The example PDF is US Letter sized.
                        Native = GhostscriptPageSizes.letter
                    }
                };

                OpenImageProcessor.GenerateOutput(pdfFileName, outFile, settings);

                if (System.IO.File.Exists(outFile))
                {
                    status = true;
                }
                else
                {
                    message = "GhostScript failed to create TIF file, file not found";
                }
            }

            Log.DebugFormat("Exit ConvertPDFtoTif() Workstation:[{0}], Username:[{1}], Tif File:[{2}] Status:[{3}] Message:[{4}]", 
                workstation, username, outFile, status, message);

            return status;
        }

        public DataTable GetMapAndActivityByLoginName(string workstation, string username, string loginName)
        { 
            Log.DebugFormat("Enter GetMapAndActivityByLoginName() Workstation:[{0}], Username:[{1}]", 
                workstation, username);

            DataTable dataTable = new DataTable("mapandactivitybyloginname");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");

            using (var db = new IfamModelContainer())
            {
                var query = (from b in db.ACTIVITY_NAMES
                    from a in db.USERS
                    from c in db.PERMISSIONs
                    from d in db.MAP_NAMES
                    from e in db.MAP_ACT_INST
                    where
                        a.LOGIN_NAME.Trim() == loginName &&
                        a.USER_STATUS == 0 &&
                        a.USER_ID == c.USER_ID &&
                        c.ACT_NAME_ID == b.ACT_NAME_ID &&
                        b.ACT_NAME_ID == e.ACT_NAME_ID &&
                        e.MAP_NAME_ID == d.MAP_NAME_ID
                    select new
                    {
                        d.MAP_NAME,
                        b.ACT_NAME
                    }).Distinct().OrderBy(a => new {a.MAP_NAME, a.ACT_NAME});

                foreach (var row in query)
                {
                    dataTable.Rows.Add(row.MAP_NAME, row.ACT_NAME);
                }
            }

            Log.DebugFormat("Exit GetMapAndActivityByLoginName() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetNavC2PByParDocNoJoinPostDoc(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetNavC2PByParDocNoJoinPostDoc() Workstation:[{0}], Username:[{1}], parDocNo:[{2}]", 
                workstation, username, parDocNo);

            DataTable dataTableProgress = GetNavC2PByParDocNo(workstation, username, parDocNo);
            DataTable dataTablePostDoc = GetPostDocByDocNo(workstation, username, parDocNo);

            DataTable dataTable = new DataTable("navc2p");
            dataTable.Columns.Add("CHECKDT");
            dataTable.Columns.Add("CSTATUS");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("DEPOSITSEQ");
            dataTable.Columns.Add("EXTPAYCD");
            dataTable.Columns.Add("FILEDT");
			dataTable.Columns.Add("MATCHID");
			dataTable.Columns.Add("MATCHSTAT");
			dataTable.Columns.Add("MATCHUID");
			dataTable.Columns.Add("NOTESKEY");
			dataTable.Columns.Add("ORIGCD");
			dataTable.Columns.Add("ORIGNO");
			dataTable.Columns.Add("SUSPENDNO");
			dataTable.Columns.Add("DOCNO");
			dataTable.Columns.Add("PARDOCNO");
			dataTable.Columns.Add("COMINGLESTAT");
			dataTable.Columns.Add("CHECKAMT");
			dataTable.Columns.Add("CHECKNUM");
			dataTable.Columns.Add("DIV");
			dataTable.Columns.Add("DOCDET");
			dataTable.Columns.Add("DOCTYPE");
			dataTable.Columns.Add("KEY");
			dataTable.Columns.Add("PRACTICE");
			dataTable.Columns.Add("SERVICEID");
			dataTable.Columns.Add("STAT");
			dataTable.Columns.Add("IFN");
			dataTable.Columns.Add("NPAGES");
			dataTable.Columns.Add("DOCGROUP");
			dataTable.Columns.Add("TOWID");
			dataTable.Columns.Add("SCANDATE");

            // Remove all but the first row for PostDoc, don't need them
            for (int i = dataTablePostDoc.Rows.Count - 1; i >= 1; i--)
            {
                DataRow row = dataTablePostDoc.Rows[i];
                row.Delete();
            }

            var result = (from dataRows1 in dataTableProgress.AsEnumerable()
                    join dataRows2 in dataTablePostDoc.AsEnumerable()
                    on dataRows1.Field<string>("DOCNO") equals dataRows2.Field<string>("DOCNO")
                          select new
                          {
                                CHECKDT = dataRows1.Field<string>("CHECKDT"),
                                CSTATUS = dataRows1.Field<string>("CSTATUS"),
                                DEPDT = dataRows1.Field<string>("DEPDT"),
                                DEPOSITSEQ = dataRows1.Field<string>("DEPOSITSEQ"),
                                EXTPAYCD = dataRows1.Field<string>("EXTPAYCD"),
                                FILEDT = dataRows1.Field<string>("FILEDT"),
                                MATCHID = dataRows1.Field<string>("MATCHID"),
                                MATCHSTAT = dataRows1.Field<string>("MATCHSTAT"),
                                MATCHUID = dataRows1.Field<string>("MATCHUID"),
                                NOTESKEY = dataRows1.Field<string>("NOTESKEY"),
                                ORIGCD = dataRows1.Field<string>("ORIGCD"),
                                ORIGNO = dataRows1.Field<string>("ORIGNO"),
                                SUSPENDNO = dataRows1.Field<string>("SUSPENDNO"),
                                DOCNO = dataRows1.Field<string>("DOCNO"),
                                PARDOCNO = dataRows1.Field<string>("PARDOCNO"),
                                COMINGLESTAT = dataRows1.Field<string>("COMINGLESTAT"),
                                CHECKAMT = dataRows1.Field<string>("CHECKAMT"),
                                CHECKNUM = dataRows1.Field<string>("CHECKNUM"),
                                DIV = dataRows1.Field<string>("DIV"),
                                DOCDET = dataRows1.Field<string>("DOCDET"),
                                DOCTYPE = dataRows1.Field<string>("DOCTYPE"),
                                KEY = dataRows1.Field<string>("KEY"),
                                PRACTICE  = dataRows1.Field<string>("PRACTICE"),
                                SERVICEID = dataRows1.Field<string>("SERVICEID"),
                                STAT = dataRows1.Field<string>("STAT"),
                                IFN = dataRows2.Field<string>("IFN"),
                                NPAGES = dataRows2.Field<string>("NPAGES"),
                                DOCGROUP = dataRows2.Field<string>("DOCGROUP"),
                                TOWID = dataRows2.Field<string>("TOWID"),
                                SCANDATE = dataRows2.Field<string>("SCANDATE")
                          });

            foreach (var item in result)
            {
                dataTable.Rows.Add(item.CHECKDT, item.CSTATUS, item.DEPDT, item.DEPOSITSEQ, item.EXTPAYCD,
                    item.FILEDT, item.MATCHID, item.MATCHSTAT, item.MATCHUID, item.NOTESKEY, item.ORIGCD,
                    item.ORIGNO, item.SUSPENDNO, item.DOCNO, item.PARDOCNO, item.COMINGLESTAT, item.CHECKAMT,
                    item.CHECKNUM, item.DIV, item.DOCDET, item.DOCTYPE, item.KEY, item.PRACTICE, item.SERVICEID,
                    item.STAT, item.IFN, item.NPAGES, item.DOCGROUP, item.TOWID, item.SCANDATE);
            }

            Log.DebugFormat("Exit GetNavC2PByParDocNoJoinPostDoc() Rows:[{0}]", dataTableProgress.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocByDocNo(string workstation, string username, string docNo)
        {
            Log.DebugFormat("Enter GetPostDocByParDocNo() Workstation:[{0}], Username:[{1}], docNo:[{2}]", 
                workstation, username, docNo);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("SCANDATE");

            using (var db = new TowerModelContainer())
            {
                try
                {
                    var results = db.POSTDOCs.Where(x => x.DOCNO.StartsWith(docNo));
                    if (results.Any())
                    {
                        foreach (var row in results)
                        {
                            // Format IFN
                            // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                            // I.e. 498/1660-329774
                            var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                            dataTable.Rows.Add(formattedIFN, row.NPAGES, row.DOCGROUP, row.DOCTYPE,
                                row.TOWID, row.DOCNO, row.SCANDATE);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    throw;
                }
            }

            Log.DebugFormat("Exit GetPostDocByDocNo() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetEiqNavC2PByDocTypeDocDetPractice(string workstation, string username, 
            string docType, string docDet, string practice)
        {
            Log.DebugFormat("Enter GetEiqNavC2PByDocTypeDocDetPractice() Workstation:[{0}], Username:[{1}], docType:[{2}], docDet:[{3}]", 
                workstation, username, docType, docDet);

            DataTable dataTable = new DataTable("navc2pbydoctypedocdetp");
            dataTable.Columns.Add("CHECKDT");
            dataTable.Columns.Add("CSTATUS");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("DEPOSITSEQ");
            dataTable.Columns.Add("EXTPAYCD");
            dataTable.Columns.Add("FILEDT");
			dataTable.Columns.Add("MATCHID");
			dataTable.Columns.Add("MATCHSTAT");
			dataTable.Columns.Add("MATCHUID");
			dataTable.Columns.Add("NOTESKEY");
			dataTable.Columns.Add("ORIGCD");
			dataTable.Columns.Add("ORIGNO");
			dataTable.Columns.Add("SUSPENDNO");
			dataTable.Columns.Add("DOCNO");
			dataTable.Columns.Add("PARDOCNO");
			dataTable.Columns.Add("COMINGLESTAT");
			dataTable.Columns.Add("CHECKAMT");
			dataTable.Columns.Add("CHECKNUM");
			dataTable.Columns.Add("DIV");
			dataTable.Columns.Add("DOCDET");
			dataTable.Columns.Add("DOCTYPE");
			dataTable.Columns.Add("KEY");
			dataTable.Columns.Add("PRACTICE");
			dataTable.Columns.Add("SERVICEID");
			dataTable.Columns.Add("STAT");

            using (var db = new OdbcConnection(Constants.DB_PROGRESS_ODBC_CONNECT_STRING))
            {
                db.Open();
                var dbCmd = db.CreateCommand();

                string cmd =
                    "select TOP 10000 checkdt,cstatus,depdt,\"deposit-seq\",extpaycd,filedt,matchid,matchstat,matchuid,noteskey,origcd,origno,suspendno,docno,pardocno,cominglestat,checkamt,checknum,div,docdet,doctype,\"key\",practice,serviceid,stat from pub.cash2post where ";
                cmd += String.Format("('{0}'='' or doctype='{0}') and ('{1}'='' or docdet='{1}') and ('{2}'='' or practice='{2}') and depdt > SYSDATE - 180", docType, docDet, practice);

                Debug.WriteLine(cmd);

                dbCmd.CommandText = cmd;

                var dbReader = dbCmd.ExecuteReader();

                while (dbReader.Read())
                {
                    CommonFunctions.DebugDumpDbReaderRow("cash2post", dbReader);

                    var matchstat = dbReader[7].ToString();
                    DateTime checkdt = Constants.RtiMinTime;
                    DateTime depdt = Constants.RtiMinTime;
                    DateTime filedt = Constants.RtiMinTime;

                    if (dbReader[0].ToString() != "")
                        checkdt = (DateTime) dbReader[0];
                    if (dbReader[2].ToString() != "")
                        depdt = (DateTime) dbReader[2];
                    if (dbReader[5].ToString() != "")
                        filedt = (DateTime) dbReader[5];

                    dataTable.Rows.Add(
                        checkdt,
                        dbReader[1],
                        depdt,
                        dbReader[3],
                        dbReader[4],
                        filedt,
                        dbReader[6],
                        matchstat,
                        dbReader[8],
                        dbReader[9],
                        dbReader[10],
                        dbReader[11],
                        dbReader[12],
                        dbReader[13],
                        dbReader[14],
                        dbReader[15],
                        dbReader[16],
                        dbReader[17] != DBNull.Value ? dbReader[17].ToString() : "",
                        dbReader[18],
                        dbReader[19],
                        dbReader[20],
                        dbReader[21],
                        dbReader[22],
                        dbReader[23],
                        dbReader[24]);
                }
                dbReader.Close();
                db.Close();
            }

            Log.DebugFormat("Exit GetEiqNavC2PByDocTypeDocDetPractice() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetEiqNavC2PByDocTypeDocDet(string workstation, string username, string docType, string docDet)
        {
            Log.DebugFormat("Enter GetEiqNavC2PByDocTypeDocDet() Workstation:[{0}], Username:[{1}], docType:[{2}], docDet:[{3}]", 
                workstation, username, docType, docDet);
            
            DataTable dataTable = new DataTable("navc2pbydoctypedocdet");
            dataTable.Columns.Add("CHECKDT");
            dataTable.Columns.Add("CSTATUS");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("DEPOSITSEQ");
            dataTable.Columns.Add("EXTPAYCD");
            dataTable.Columns.Add("FILEDT");
			dataTable.Columns.Add("MATCHID");
			dataTable.Columns.Add("MATCHSTAT");
			dataTable.Columns.Add("MATCHUID");
			dataTable.Columns.Add("NOTESKEY");
			dataTable.Columns.Add("ORIGCD");
			dataTable.Columns.Add("ORIGNO");
			dataTable.Columns.Add("SUSPENDNO");
			dataTable.Columns.Add("DOCNO");
			dataTable.Columns.Add("PARDOCNO");
			dataTable.Columns.Add("COMINGLESTAT");
			dataTable.Columns.Add("CHECKAMT");
			dataTable.Columns.Add("CHECKNUM");
			dataTable.Columns.Add("DIV");
			dataTable.Columns.Add("DOCDET");
			dataTable.Columns.Add("DOCTYPE");
			dataTable.Columns.Add("KEY");
			dataTable.Columns.Add("PRACTICE");
			dataTable.Columns.Add("SERVICEID");
			dataTable.Columns.Add("STAT");

            using (var db = new OdbcConnection(Constants.DB_PROGRESS_ODBC_CONNECT_STRING))
            {
                db.Open();
                var dbCmd = db.CreateCommand();

                string cmd =
                    "select TOP 10000 checkdt,cstatus,depdt,\"deposit-seq\",extpaycd,filedt,matchid,matchstat,matchuid,noteskey,origcd,origno,suspendno,docno,pardocno,cominglestat,checkamt,checknum,div,docdet,doctype,\"key\",practice,serviceid,stat from pub.cash2post where ";
                cmd += String.Format("('{0}'='' or doctype='{0}') and ('{1}'='' or docdet='{1}') and depdt > SYSDATE - 180", docType, docDet);

                Debug.WriteLine(cmd);

                dbCmd.CommandText = cmd;

                var dbReader = dbCmd.ExecuteReader();

                while (dbReader.Read())
                {
                    CommonFunctions.DebugDumpDbReaderRow("cash2post", dbReader);

                    var matchstat = dbReader[7].ToString();
                    DateTime checkdt = Constants.RtiMinTime;
                    DateTime depdt = Constants.RtiMinTime;
                    DateTime filedt = Constants.RtiMinTime;

                    if (dbReader[0].ToString() != "")
                        checkdt = (DateTime) dbReader[0];
                    if (dbReader[2].ToString() != "")
                        depdt = (DateTime) dbReader[2];
                    if (dbReader[5].ToString() != "")
                        filedt = (DateTime) dbReader[5];

                    dataTable.Rows.Add(
                        checkdt,
                        dbReader[1],
                        depdt,
                        dbReader[3],
                        dbReader[4],
                        filedt,
                        dbReader[6],
                        matchstat,
                        dbReader[8],
                        dbReader[9],
                        dbReader[10],
                        dbReader[11],
                        dbReader[12],
                        dbReader[13],
                        dbReader[14],
                        dbReader[15],
                        dbReader[16],
                        dbReader[17] != DBNull.Value ? dbReader[17].ToString() : "",
                        dbReader[18],
                        dbReader[19],
                        dbReader[20],
                        dbReader[21],
                        dbReader[22],
                        dbReader[23],
                        dbReader[24]);
                }
                dbReader.Close();
                db.Close();
            }

            Log.DebugFormat("Exit GetEiqNavC2PByDocTypeDocDet() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetNavC2PByParDocNo(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetNavC2PByParDocNo() Workstation:[{0}], Username:[{1}], parDocNo:[{2}]", 
                workstation, username, parDocNo);

            DataTable dataTable = new DataTable("navc2pbypardocno");
            dataTable.Columns.Add("CHECKDT");
            dataTable.Columns.Add("CSTATUS");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("DEPOSITSEQ");
            dataTable.Columns.Add("EXTPAYCD");
            dataTable.Columns.Add("FILEDT");
			dataTable.Columns.Add("MATCHID");
			dataTable.Columns.Add("MATCHSTAT");
			dataTable.Columns.Add("MATCHUID");
			dataTable.Columns.Add("NOTESKEY");
			dataTable.Columns.Add("ORIGCD");
			dataTable.Columns.Add("ORIGNO");
			dataTable.Columns.Add("SUSPENDNO");
			dataTable.Columns.Add("DOCNO");
			dataTable.Columns.Add("PARDOCNO");
			dataTable.Columns.Add("COMINGLESTAT");
			dataTable.Columns.Add("CHECKAMT");
			dataTable.Columns.Add("CHECKNUM");
			dataTable.Columns.Add("DIV");
			dataTable.Columns.Add("DOCDET");
			dataTable.Columns.Add("DOCTYPE");
			dataTable.Columns.Add("KEY");
			dataTable.Columns.Add("PRACTICE");
			dataTable.Columns.Add("SERVICEID");
			dataTable.Columns.Add("STAT");

            using (var db = new OdbcConnection(Constants.DB_PROGRESS_ODBC_CONNECT_STRING))
            {
                db.Open();
                var dbCmd = db.CreateCommand();

                string cmd = "select checkdt,cstatus,depdt,\"deposit-seq\",extpaycd,filedt,matchid,matchstat,matchuid,noteskey,origcd,origno,suspendno,docno,pardocno,cominglestat,checkamt,checknum,div,docdet,doctype,\"key\",practice,serviceid,stat from pub.cash2post where pardocno = ";
                cmd += String.Format("'{0}'", parDocNo);

                Debug.WriteLine(cmd);

                dbCmd.CommandText = cmd;

                var dbReader = dbCmd.ExecuteReader();

                while (dbReader.Read())
                {
                    CommonFunctions.DebugDumpDbReaderRow("cash2post", dbReader);

                    var matchstat = dbReader[7].ToString();
                    DateTime checkdt = Constants.RtiMinTime;
                    DateTime depdt = Constants.RtiMinTime;
                    DateTime filedt = Constants.RtiMinTime;

                    if (matchstat == "A")
                    {
                        if (dbReader[0].ToString() != "")
                            checkdt = (DateTime) dbReader[0];
                        if (dbReader[2].ToString() != "")
                            depdt = (DateTime) dbReader[2];
                        if (dbReader[5].ToString() != "")
                            filedt = (DateTime) dbReader[5];

                        dataTable.Rows.Add(
                            checkdt,
                            dbReader[1],
                            depdt,
                            dbReader[3],
                            dbReader[4],
                            filedt,
                            dbReader[6],
                            matchstat,
                            dbReader[8],
                            dbReader[9],
                            dbReader[10],
                            dbReader[11],
                            dbReader[12],
                            dbReader[13],
                            dbReader[14],
                            dbReader[15],
                            dbReader[16],
                            dbReader[17] != DBNull.Value ? dbReader[17].ToString() : "",
                            dbReader[18],
                            dbReader[19],
                            dbReader[20],
                            dbReader[21],
                            dbReader[22],
                            dbReader[23],
                            dbReader[24]);
                    }
                }
                dbReader.Close();
                db.Close();
            }

            Log.DebugFormat("Exit GetNavC2PByParDocNo() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

    }
}
