using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Rti.DataModel;

namespace Rti.DocumentManagerServer.DataAccess
{
    class PostDoc
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetPostDocExtServiceIdByTowIdInsertIfNull(string workstation, string userid, int towId,
            string serviceId)
        {
            Log.DebugFormat("Enter GetPostDocExtServiceIdByTowIdInsertIfNull() workstation:[{0}] user:[{1}]", workstation, userid);
            using (var db = new TowerModelContainer())
            {
                var query = (from a in db.POSTDOCEXTs
                    where
                        a.TOWID == towId
                    select new
                    {
                        a.TOWID,
                        a.FINANCIAL_CLASS,
                        a.SERVICEID
                    }).FirstOrDefault();

                if (query == null)
                {
                    var myPostDocExt = new POSTDOCEXT
                    {
	                    TOWID = towId,
	                    SERVICEID = serviceId
                    };
                    db.POSTDOCEXTs.Add(myPostDocExt);
                    var inserted = db.SaveChanges();
                    Debug.WriteLine("Rows inserted:[{0}]", inserted);
                    Log.Debug("Exit GetPostDocExtServiceIdByTowIdInsertIfNull()");
                    return serviceId;
                }
                else
                {
                    Log.Debug("Exit GetPostDocExtServiceIdByTowIdInsertIfNull()");
                    return query.SERVICEID.ToString();
                }
            }
        }

        public DataTable GetPostDocByParDocNo(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetPostDocByParDocNo() Workstation:[{0}], Username:[{1}], ParDocNo:[{2}]", 
                workstation, username, parDocNo);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("SCANDATE");
            dataTable.Columns.Add("ORIGDOC");

            using (var db = new TowerModelContainer())
            {
                var query = (from a in db.POSTDOCs
                    join b in db.POSTDOCs on a.PARDOCNO equals b.PARDOCNO
                    where
                        b.DOCNO == parDocNo
                    orderby
                        a.DOCTYPE,
                        a.DOCNO,
                        a.SCANDATE descending
                    select new
                    {
                        a.IFNDS,
                        a.IFNID,
                        a.NPAGES,
                        a.DOCGROUP,
                        a.DOCTYPE,
                        a.TOWID,
                        a.DOCNO,
                        a.SCANDATE,
                        a.ORIGDOC
                    }).Distinct();

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(formattedIFN, row.NPAGES, row.DOCGROUP, row.DOCTYPE,
                        row.TOWID, row.DOCNO, row.SCANDATE, row.ORIGDOC);
                }
            }

            Log.DebugFormat("Exit GetPostDocByParDocNo() Rows:[{0}]", dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetImportedPostDocByTowId(string workstation, string username, int towId)
        {
            Log.DebugFormat("Enter GetImportedPostDocByTowId() Workstation:[{0}], Username:[{1}], TowId:[{2}]",
                workstation, username, towId);

            DataTable dataTable = new DataTable("postdoc");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");
            dataTable.Columns.Add("SLEVEL");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("ORIGDOC");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("RLFLAG");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("SCANDATE");
            dataTable.Columns.Add("SCANTIME");
            dataTable.Columns.Add("SCANUID");
            dataTable.Columns.Add("SOURCEID");
            dataTable.Columns.Add("BOXNUM");
            dataTable.Columns.Add("EXPSTAT");
            dataTable.Columns.Add("SPARE1");
            dataTable.Columns.Add("SPARE2");
            dataTable.Columns.Add("SPARE3");
            dataTable.Columns.Add("TOWID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCs in db.POSTDOCs
                    where
                        POSTDOCs.TOWID == towId
                    select new
                    {
                        POSTDOCs.NPAGES,
                        POSTDOCs.IFNDS,
                        POSTDOCs.IFNID,
                        POSTDOCs.SLEVEL,
                        POSTDOCs.DOCNO,
                        POSTDOCs.PARDOCNO,
                        POSTDOCs.ORIGDOC,
                        POSTDOCs.PRACTICE,
                        POSTDOCs.DIVISION,
                        POSTDOCs.ACCTNUM,
                        POSTDOCs.EMBACCT,
                        POSTDOCs.CHECKNUM,
                        POSTDOCs.CHECKAMT,
                        POSTDOCs.DEPDT,
                        POSTDOCs.EXTPAYOR,
                        POSTDOCs.DEPCODE,
                        POSTDOCs.DOCGROUP,
                        POSTDOCs.DOCTYPE,
                        POSTDOCs.DOCDET,
                        POSTDOCs.STATUS,
                        POSTDOCs.RLFLAG,
                        POSTDOCs.RECDATE,
                        POSTDOCs.SCANDATE,
                        POSTDOCs.SCANTIME,
                        POSTDOCs.SCANUID,
                        POSTDOCs.SOURCEID,
                        POSTDOCs.BOXNUM,
                        POSTDOCs.EXPSTAT,
                        POSTDOCs.SPARE1,
                        POSTDOCs.SPARE2,
                        POSTDOCs.SPARE3,
                        POSTDOCs.TOWID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.NPAGES
                        , row.IFNDS
                        , row.IFNID
                        , row.SLEVEL
                        , row.DOCNO
                        , row.PARDOCNO
                        , row.ORIGDOC
                        , row.PRACTICE
                        , row.DIVISION
                        , row.ACCTNUM
                        , row.EMBACCT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.DEPDT
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.DOCGROUP
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.STATUS
                        , row.RLFLAG
                        , row.RECDATE
                        , row.SCANDATE
                        , row.SCANTIME
                        , row.SCANUID
                        , row.SOURCEID
                        , row.BOXNUM
                        , row.EXPSTAT
                        , row.SPARE1
                        , row.SPARE2
                        , row.SPARE3
                        , row.TOWID
                        );
                }
            }
            Log.DebugFormat("Exit GetImportedPostDocByTowId() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetImportedPostDocByDocNo(string workstation, string username, string docNo)
        {
            Log.DebugFormat("Enter GetImportedPostDocByDocNo() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdoc");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");
            dataTable.Columns.Add("SLEVEL");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("ORIGDOC");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("RLFLAG");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("SCANDATE");
            dataTable.Columns.Add("SCANTIME");
            dataTable.Columns.Add("SCANUID");
            dataTable.Columns.Add("SOURCEID");
            dataTable.Columns.Add("BOXNUM");
            dataTable.Columns.Add("EXPSTAT");
            dataTable.Columns.Add("SPARE1");
            dataTable.Columns.Add("SPARE2");
            dataTable.Columns.Add("SPARE3");
            dataTable.Columns.Add("TOWID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCs in db.POSTDOCs
                    where
                        POSTDOCs.DOCNO == docNo
                    select new
                    {
                        POSTDOCs.NPAGES,
                        POSTDOCs.IFNDS,
                        POSTDOCs.IFNID,
                        POSTDOCs.SLEVEL,
                        POSTDOCs.DOCNO,
                        POSTDOCs.PARDOCNO,
                        POSTDOCs.ORIGDOC,
                        POSTDOCs.PRACTICE,
                        POSTDOCs.DIVISION,
                        POSTDOCs.ACCTNUM,
                        POSTDOCs.EMBACCT,
                        POSTDOCs.CHECKNUM,
                        POSTDOCs.CHECKAMT,
                        POSTDOCs.DEPDT,
                        POSTDOCs.EXTPAYOR,
                        POSTDOCs.DEPCODE,
                        POSTDOCs.DOCGROUP,
                        POSTDOCs.DOCTYPE,
                        POSTDOCs.DOCDET,
                        POSTDOCs.STATUS,
                        POSTDOCs.RLFLAG,
                        POSTDOCs.RECDATE,
                        POSTDOCs.SCANDATE,
                        POSTDOCs.SCANTIME,
                        POSTDOCs.SCANUID,
                        POSTDOCs.SOURCEID,
                        POSTDOCs.BOXNUM,
                        POSTDOCs.EXPSTAT,
                        POSTDOCs.SPARE1,
                        POSTDOCs.SPARE2,
                        POSTDOCs.SPARE3,
                        POSTDOCs.TOWID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.NPAGES
                        , row.IFNDS
                        , row.IFNID
                        , row.SLEVEL
                        , row.DOCNO
                        , row.PARDOCNO
                        , row.ORIGDOC
                        , row.PRACTICE
                        , row.DIVISION
                        , row.ACCTNUM
                        , row.EMBACCT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.DEPDT
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.DOCGROUP
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.STATUS
                        , row.RLFLAG
                        , row.RECDATE
                        , row.SCANDATE
                        , row.SCANTIME
                        , row.SCANUID
                        , row.SOURCEID
                        , row.BOXNUM
                        , row.EXPSTAT
                        , row.SPARE1
                        , row.SPARE2
                        , row.SPARE3
                        , row.TOWID
                        );
                }
            }
            Log.DebugFormat("Exit GetImportedPostDocByDocNo() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetImportedPostDocByParDocNo(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetImportedPostDocByParDocNo() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdoc");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");
            dataTable.Columns.Add("SLEVEL");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("ORIGDOC");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("RLFLAG");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("SCANDATE");
            dataTable.Columns.Add("SCANTIME");
            dataTable.Columns.Add("SCANUID");
            dataTable.Columns.Add("SOURCEID");
            dataTable.Columns.Add("BOXNUM");
            dataTable.Columns.Add("EXPSTAT");
            dataTable.Columns.Add("SPARE1");
            dataTable.Columns.Add("SPARE2");
            dataTable.Columns.Add("SPARE3");
            dataTable.Columns.Add("TOWID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCs in db.POSTDOCs
                    where
                        POSTDOCs.PARDOCNO == parDocNo
                    select new
                    {
                        POSTDOCs.NPAGES,
                        POSTDOCs.IFNDS,
                        POSTDOCs.IFNID,
                        POSTDOCs.SLEVEL,
                        POSTDOCs.DOCNO,
                        POSTDOCs.PARDOCNO,
                        POSTDOCs.ORIGDOC,
                        POSTDOCs.PRACTICE,
                        POSTDOCs.DIVISION,
                        POSTDOCs.ACCTNUM,
                        POSTDOCs.EMBACCT,
                        POSTDOCs.CHECKNUM,
                        POSTDOCs.CHECKAMT,
                        POSTDOCs.DEPDT,
                        POSTDOCs.EXTPAYOR,
                        POSTDOCs.DEPCODE,
                        POSTDOCs.DOCGROUP,
                        POSTDOCs.DOCTYPE,
                        POSTDOCs.DOCDET,
                        POSTDOCs.STATUS,
                        POSTDOCs.RLFLAG,
                        POSTDOCs.RECDATE,
                        POSTDOCs.SCANDATE,
                        POSTDOCs.SCANTIME,
                        POSTDOCs.SCANUID,
                        POSTDOCs.SOURCEID,
                        POSTDOCs.BOXNUM,
                        POSTDOCs.EXPSTAT,
                        POSTDOCs.SPARE1,
                        POSTDOCs.SPARE2,
                        POSTDOCs.SPARE3,
                        POSTDOCs.TOWID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.NPAGES
                        , row.IFNDS
                        , row.IFNID
                        , row.SLEVEL
                        , row.DOCNO
                        , row.PARDOCNO
                        , row.ORIGDOC
                        , row.PRACTICE
                        , row.DIVISION
                        , row.ACCTNUM
                        , row.EMBACCT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.DEPDT
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.DOCGROUP
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.STATUS
                        , row.RLFLAG
                        , row.RECDATE
                        , row.SCANDATE
                        , row.SCANTIME
                        , row.SCANUID
                        , row.SOURCEID
                        , row.BOXNUM
                        , row.EXPSTAT
                        , row.SPARE1
                        , row.SPARE2
                        , row.SPARE3
                        , row.TOWID
                        );
                }
            }
            Log.DebugFormat("Exit GetImportedPostDocByParDocNo() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetImportedPostDocByPracticeAndAccount(string workstation, string username
            ,string practice, string account)
        {
            Log.DebugFormat("Enter GetImportedPostDocByPracticeAndAccount() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdoc");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");
            dataTable.Columns.Add("SLEVEL");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("ORIGDOC");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("RLFLAG");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("SCANDATE");
            dataTable.Columns.Add("SCANTIME");
            dataTable.Columns.Add("SCANUID");
            dataTable.Columns.Add("SOURCEID");
            dataTable.Columns.Add("BOXNUM");
            dataTable.Columns.Add("EXPSTAT");
            dataTable.Columns.Add("SPARE1");
            dataTable.Columns.Add("SPARE2");
            dataTable.Columns.Add("SPARE3");
            dataTable.Columns.Add("TOWID");

            using (var db = new TowerModelContainer())
            {
                var query =
                    db.POSTDOCs.SqlQuery("select * from postdoc where postdoc.practice = '" + practice + "' and postdoc.embacct = '" + account + "'");

                if (query.Any())
                {
                    foreach (var row in query)
                    {
                        dataTable.Rows.Add(
                            row.NPAGES
                            , row.IFNDS
                            , row.IFNID
                            , row.SLEVEL
                            , row.DOCNO
                            , row.PARDOCNO
                            , row.ORIGDOC
                            , row.PRACTICE
                            , row.DIVISION
                            , row.ACCTNUM
                            , row.EMBACCT
                            , row.CHECKNUM
                            , row.CHECKAMT
                            , row.DEPDT
                            , row.EXTPAYOR
                            , row.DEPCODE
                            , row.DOCGROUP
                            , row.DOCTYPE
                            , row.DOCDET
                            , row.STATUS
                            , row.RLFLAG
                            , row.RECDATE
                            , row.SCANDATE
                            , row.SCANTIME
                            , row.SCANUID
                            , row.SOURCEID
                            , row.BOXNUM
                            , row.EXPSTAT
                            , row.SPARE1
                            , row.SPARE2
                            , row.SPARE3
                            , row.TOWID
                            );
                    }
                }
            }

            Log.DebugFormat("Exit GetImportedPostDocByPracticeAndAccount() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocStatusLookup(string workstation, string username)
        {
            Log.DebugFormat("Enter GetPostDocStatusLookup() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdoc_status_lookup");
            dataTable.Columns.Add("TYPECODE");
            dataTable.Columns.Add("DESCRIPTION");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOC_STATUS_LOOKUPs in db.POSTDOC_STATUS_LOOKUP
                    select new
                    {
                        POSTDOC_STATUS_LOOKUPs.TYPECODE,
                        POSTDOC_STATUS_LOOKUPs.DESCRIPTION,
                        POSTDOC_STATUS_LOOKUPs.MAP_NAME,
                        POSTDOC_STATUS_LOOKUPs.ACT_NAME
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.TYPECODE
                        , row.DESCRIPTION
                        , row.MAP_NAME
                        , row.ACT_NAME
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocStatusLookup() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocSearchByParDocNo(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetPostDocSearchByParDocNo() Workstation:[{0}], Username:[{1}]", 
                workstation, username);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("SOURCEID");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("SERVICEID");
            dataTable.Columns.Add("FINANCIAL_CLASS");

            using (var db = new TowerModelContainer())
            {
                var query = (from a in db.POSTDOCs
                    from p in db.POSTDOCEXTs
                    where
                      a.TOWID == p.TOWID &&
                      a.PARDOCNO.StartsWith( parDocNo )
                    select new {
                      a.DOCNO,
                      a.DOCTYPE,
                      a.DOCDET,
                      a.DEPDT,
                      a.CHECKNUM,
                      a.CHECKAMT,
                      a.PRACTICE,
                      a.DIVISION,
                      a.EMBACCT,
                      a.PARDOCNO,
                      a.TOWID,
                      a.NPAGES,
                      a.IFNDS,
                      a.IFNID,
                      a.DOCGROUP,
                      a.EXTPAYOR,
                      a.SOURCEID,
                      a.ACCTNUM,
                      a.DEPCODE,
                      p.SERVICEID,
                      p.FINANCIAL_CLASS
                    }).Take(Constants.MAX_RESULTS_LIMIT);

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIfn = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(
                        row.DOCNO
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.PARDOCNO
                        , row.TOWID
                        , row.NPAGES
                        , formattedIfn
                        , row.DOCGROUP
                        , row.EXTPAYOR
                        , row.SOURCEID
                        , row.ACCTNUM
                        , row.DEPCODE
                        , row.SERVICEID
                        , row.FINANCIAL_CLASS
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocSearchByParDocNo() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocSearchByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            Log.DebugFormat("Enter GetPostDocSearchByPPracticeAndAccount() Workstation:[{0}], Username:[{1}] [{2}] [{3}]", 
                workstation, username, practice, account);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DOCGROUP");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCSs in db.POSTDOCs
                    where POSTDOCSs.PRACTICE == practice &&
                          POSTDOCSs.ACCTNUM == account
                    select new
                    {
                        POSTDOCSs.DOCNO
                        ,POSTDOCSs.DOCTYPE
                        ,POSTDOCSs.DOCDET
                        ,POSTDOCSs.DEPDT
                        ,POSTDOCSs.CHECKNUM
                        ,POSTDOCSs.CHECKAMT
                        ,POSTDOCSs.PRACTICE
                        ,POSTDOCSs.DIVISION
                        ,POSTDOCSs.EMBACCT
                        ,POSTDOCSs.PARDOCNO
                        ,POSTDOCSs.TOWID
                        ,POSTDOCSs.NPAGES
                        ,POSTDOCSs.IFNDS
                        ,POSTDOCSs.IFNID
                        ,POSTDOCSs.DOCGROUP
                    });

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(
                        row.DOCNO
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.PARDOCNO
                        , row.TOWID
                        , row.NPAGES
                        , formattedIFN
                        , row.DOCGROUP
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocSearchByPracticeAndAccount() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        /// <summary>
        /// Currently only returns one row.
        /// </summary>
        /// <param name="workstation"></param>
        /// <param name="username"></param>
        /// <param name="docNo"></param>
        /// <returns></returns>
        public DataTable GetPostDocSearchByDocNo(string workstation, string username, string docNo)
        {
            Log.DebugFormat("Enter GetPostDocSearchByDocNo() Workstation:[{0}], Username:[{1}], docNo:[{2}]", 
                workstation, username, docNo);

            DataTable dataTable = new DataTable("a");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DOCGROUP");

            Debug.WriteLine(String.Format("GetPostDocSearchByDocNo() Preparing query for docNo:[{0}]", docNo));
            using (var db = new TowerModelContainer())
            {
                var query = db.POSTDOCs.SqlQuery(@"SELECT * FROM postdoc a WHERE a.docno = '" + docNo + "'");

                if (query.Any())
                {
                    foreach (var row in query)
                    {
                        // Format IFN
                        // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                        // I.e. 498/1660-329774
                        var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                        dataTable.Rows.Add(
                            row.DOCNO, row.DOCTYPE, row.DOCDET, row.DEPDT, row.CHECKNUM, row.CHECKAMT
                            , row.PRACTICE, row.DIVISION, row.EMBACCT, row.PARDOCNO, row.TOWID
                            , row.NPAGES, formattedIFN, row.DOCGROUP);
                    }
                }
            }

            Log.Debug(String.Format("Exit GetPostDocSearchByDocNo() Rows:[{0}]", 
                dataTable.Rows.Count));
            return dataTable;
        }

        public DataTable GetPostDocSearchByCheckNum(string workstation, string username, string checkNum)
        {
            Log.DebugFormat("Enter GetPostDocSearchByCheckNum() Workstation:[{0}], Username:[{1}] checkNum:[{2}]", 
                workstation, username, checkNum);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DOCGROUP");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCSs in db.POSTDOCs
                    where POSTDOCSs.CHECKNUM == checkNum
                    select new
                    {
                        POSTDOCSs.DOCNO
                        ,POSTDOCSs.DOCTYPE
                        ,POSTDOCSs.DOCDET
                        ,POSTDOCSs.DEPDT
                        ,POSTDOCSs.CHECKNUM
                        ,POSTDOCSs.CHECKAMT
                        ,POSTDOCSs.PRACTICE
                        ,POSTDOCSs.DIVISION
                        ,POSTDOCSs.EMBACCT
                        ,POSTDOCSs.PARDOCNO
                        ,POSTDOCSs.TOWID
                        ,POSTDOCSs.NPAGES
                        ,POSTDOCSs.IFNDS
                        ,POSTDOCSs.IFNID
                        ,POSTDOCSs.DOCGROUP
                    });

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(
                        row.DOCNO
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.PARDOCNO
                        , row.TOWID
                        , row.NPAGES
                        , formattedIFN
                        , row.DOCGROUP
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocSearchByCheckNum() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocSearchBy(string workstation, string username, DateTime depDt)
        {
            Log.DebugFormat("Enter GetPostDocSearchBy() Workstation:[{0}], Username:[{1}]", 
                workstation, username);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DOCGROUP");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCSs in db.POSTDOCs
                    where POSTDOCSs.DEPDT == depDt.Date
                    select new
                    {                        
                        POSTDOCSs.DOCNO
                        ,POSTDOCSs.DOCTYPE
                        ,POSTDOCSs.DOCDET
                        ,POSTDOCSs.DEPDT
                        ,POSTDOCSs.CHECKNUM
                        ,POSTDOCSs.CHECKAMT
                        ,POSTDOCSs.PRACTICE
                        ,POSTDOCSs.DIVISION
                        ,POSTDOCSs.EMBACCT
                        ,POSTDOCSs.PARDOCNO
                        ,POSTDOCSs.TOWID
                        ,POSTDOCSs.NPAGES
                        ,POSTDOCSs.IFNDS
                        ,POSTDOCSs.IFNID
                        ,POSTDOCSs.DOCGROUP
                    });

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(
                        row.DOCNO
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.PARDOCNO
                        , row.TOWID
                        , row.NPAGES
                        , formattedIFN
                        , row.DOCGROUP
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocSearchByDeptDt() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocSearchByPracticeAndDeptDt(string workstation, string username, 
            string practice, DateTime depDt)
        {
            Log.DebugFormat("Enter GetPostDocSearchByPracticeAndDeptDt() Workstation:[{0}], Username:[{1}] practice:[{2}]", 
                workstation, username, practice);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("DOCGROUP");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCSs in db.POSTDOCs
                    where POSTDOCSs.DEPDT == depDt.Date &&
                          POSTDOCSs.PRACTICE == practice
                    select new
                    {
                        POSTDOCSs.DOCNO
                        ,POSTDOCSs.DOCTYPE
                        ,POSTDOCSs.DOCDET
                        ,POSTDOCSs.DEPDT
                        ,POSTDOCSs.CHECKNUM
                        ,POSTDOCSs.CHECKAMT
                        ,POSTDOCSs.PRACTICE
                        ,POSTDOCSs.DIVISION
                        ,POSTDOCSs.EMBACCT
                        ,POSTDOCSs.PARDOCNO
                        ,POSTDOCSs.TOWID
                        ,POSTDOCSs.NPAGES
                        ,POSTDOCSs.IFNDS
                        ,POSTDOCSs.IFNID
                        ,POSTDOCSs.DOCGROUP
                    });

                foreach (var row in query)
                {
                    // Format IFN
                    // select '498/' ||  Mod (a.ifnds, 65536) || '-' || a.ifnid as IFN
                    // I.e. 498/1660-329774
                    var formattedIFN = String.Format("{0}{1}-{2}", "498/", row.IFNDS%65536, row.IFNID);

                    dataTable.Rows.Add(
                        row.DOCNO
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.DEPDT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.PRACTICE
                        , row.DIVISION
                        , row.EMBACCT
                        , row.PARDOCNO
                        , row.TOWID
                        , row.NPAGES
                        , formattedIFN
                        , row.DOCGROUP
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocSearchByPracticeAndDeptDt() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDocByDateAndUser(string workstation, string username, DateTime date, string scanuid)
        {
            Log.DebugFormat("Enter GetPostDocByDateAndUser() Workstation:[{0}], Username:[{1}], scanuid:[{2}]", 
                workstation, username, scanuid);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("NPAGES");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");
            dataTable.Columns.Add("SLEVEL");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PARDOCNO");
            dataTable.Columns.Add("ORIGDOC");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("ACCTNUM");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("EXTPAYOR");
            dataTable.Columns.Add("DEPCODE");
            dataTable.Columns.Add("DOCGROUP");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("RLFLAG");
            dataTable.Columns.Add("RECDATE");
            dataTable.Columns.Add("SCANDATE");
            dataTable.Columns.Add("SCANTIME");
            dataTable.Columns.Add("SCANUID");
            dataTable.Columns.Add("SOURCEID");
            dataTable.Columns.Add("BOXNUM");
            dataTable.Columns.Add("EXPSTAT");
            dataTable.Columns.Add("SPARE1");
            dataTable.Columns.Add("SPARE2");
            dataTable.Columns.Add("SPARE3");
            dataTable.Columns.Add("TOWID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDOCSs in db.POSTDOCs
                    where POSTDOCSs.SCANDATE == date.Date &&
                          POSTDOCSs.SCANUID == scanuid
                    select new
                    {
                        POSTDOCSs.NPAGES
                        ,POSTDOCSs.IFNDS
                        ,POSTDOCSs.IFNID
                        ,POSTDOCSs.SLEVEL
                        ,POSTDOCSs.DOCNO
                        ,POSTDOCSs.PARDOCNO
                        ,POSTDOCSs.ORIGDOC
                        ,POSTDOCSs.PRACTICE
                        ,POSTDOCSs.DIVISION
                        ,POSTDOCSs.ACCTNUM
                        ,POSTDOCSs.EMBACCT
                        ,POSTDOCSs.CHECKNUM
                        ,POSTDOCSs.CHECKAMT
                        ,POSTDOCSs.DEPDT
                        ,POSTDOCSs.EXTPAYOR
                        ,POSTDOCSs.DEPCODE
                        ,POSTDOCSs.DOCGROUP
                        ,POSTDOCSs.DOCTYPE
                        ,POSTDOCSs.DOCDET
                        ,POSTDOCSs.STATUS
                        ,POSTDOCSs.RLFLAG
                        ,POSTDOCSs.RECDATE
                        ,POSTDOCSs.SCANDATE
                        ,POSTDOCSs.SCANTIME
                        ,POSTDOCSs.SCANUID
                        ,POSTDOCSs.SOURCEID
                        ,POSTDOCSs.BOXNUM
                        ,POSTDOCSs.EXPSTAT
                        ,POSTDOCSs.SPARE1
                        ,POSTDOCSs.SPARE2
                        ,POSTDOCSs.SPARE3
                        ,POSTDOCSs.TOWID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.NPAGES
                        , row.IFNDS
                        , row.IFNID
                        , row.SLEVEL
                        , row.DOCNO
                        , row.PARDOCNO
                        , row.ORIGDOC
                        , row.PRACTICE
                        , row.DIVISION
                        , row.ACCTNUM
                        , row.EMBACCT
                        , row.CHECKNUM
                        , row.CHECKAMT
                        , row.DEPDT
                        , row.EXTPAYOR
                        , row.DEPCODE
                        , row.DOCGROUP
                        , row.DOCTYPE
                        , row.DOCDET
                        , row.STATUS
                        , row.RLFLAG
                        , row.RECDATE
                        , row.SCANDATE
                        , row.SCANTIME
                        , row.SCANUID
                        , row.SOURCEID
                        , row.BOXNUM
                        , row.EXPSTAT
                        , row.SPARE1
                        , row.SPARE2
                        , row.SPARE3
                        , row.TOWID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDocByDateAndUser() Rows:[{0}]", 
                dataTable.Rows.Count);
            return dataTable;
        }

        public bool UpdatePostDocNPagesByTowId(string workstation, string username, int nPages, int towId)
        {
            Log.DebugFormat("Enter UpdatePostDocNPagesByTowId() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            bool status;

            using (TowerModelContainer context = new TowerModelContainer())
            {
                var queryPOSTDOCs =
                    from POSTDOCs in context.POSTDOCs
                    where
                      POSTDOCs.TOWID == towId
                    select POSTDOCs;
                foreach (var POSTDOCs in queryPOSTDOCs)
                {
                    POSTDOCs.NPAGES = nPages;
                }
                status = context.SaveChanges() > 0 ? true : false;
            }

            Log.DebugFormat("Exit InsertIntoPostDetail() Result:[{0}]",
                status);
            return status;
        }

        public bool InsertIntoPostDoc(string workstation, string username, POSTDOC postDoc)
        {
            Log.DebugFormat("Enter InsertIntoPostDoc() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            bool status;

            using (TowerModelContainer context = new TowerModelContainer())
            {
                context.POSTDOCs.Add(postDoc);
                status = context.SaveChanges() > 0;
            }

            Log.DebugFormat("Exit InsertIntoPostDoc() Result:[{0}]", status);
            return status;
        }

        public DataTable GetDocNoFromPostDocsByParDocNo(string workstation, string username, string parDocNo)
        {
            Log.DebugFormat("Enter GetDocNoFromPostDocsByParDocNo() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdocs");
            dataTable.Columns.Add("DOCNO");

            using (var db = new TowerModelContainer())
            {
                var query = db.POSTDOCs.SqlQuery("select * from postdoc where postdoc.pardocno = '" + parDocNo + "'");

                if (query.Any())
                {
                    foreach (var row in query)
                    {
                        dataTable.Rows.Add(row.DOCNO);
                    }
                }
            }

            Log.DebugFormat("Exit GetDocNoFromPostDocsByParDocNo() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

    }
}
