using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Rti.DataModel;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.DocumentManagerServer.DataAccess
{
    class PostDetail
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable ListPostDetailStatusLookups(string workstation, string username)
        {
            Log.DebugFormat("Enter ListPostDetailStatusLookups() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdetail_status_lookup");
            dataTable.Columns.Add("POSTDETAIL_STATUS_LOOKUP_ID");
            dataTable.Columns.Add("TYPECODE");
            dataTable.Columns.Add("DESCRIPTION");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDETAIL_STATUS_LOOKUPs in db.POSTDETAIL_STATUS_LOOKUP
                    select new
                    {
                        POSTDETAIL_STATUS_LOOKUPs.POSTDETAIL_STATUS_LOOKUP_ID
                        ,POSTDETAIL_STATUS_LOOKUPs.TYPECODE
                        ,POSTDETAIL_STATUS_LOOKUPs.DESCRIPTION
                        ,POSTDETAIL_STATUS_LOOKUPs.MAP_NAME
                        ,POSTDETAIL_STATUS_LOOKUPs.ACT_NAME
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.POSTDETAIL_STATUS_LOOKUP_ID
                        , row.TYPECODE
                        , row.DESCRIPTION
                        , row.MAP_NAME
                        , row.ACT_NAME
                        );
                }
            }
            Log.DebugFormat("Exit ListPostDetailStatusLookups() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDetailByTowerId(string workstation, string username, int towId)
        {
            Log.DebugFormat("Enter GetPostDetailByTowerId() Workstation:[{0}], Username:[{1}], towId:[{2}]",
                workstation, username, towId);

            DataTable dataTable = new DataTable("postdetail");
            dataTable.Columns.Add("POSTDETAIL_ID");
            dataTable.Columns.Add("TOWID");
            dataTable.Columns.Add("SVCDT");
            dataTable.Columns.Add("PAIDAMT");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("ROUTEINFO");
            dataTable.Columns.Add("STATUS");
            dataTable.Columns.Add("DOCPAGE");
            dataTable.Columns.Add("MODIFYUID");
            dataTable.Columns.Add("MODIFYDATE");
            dataTable.Columns.Add("CREATEUID");
            dataTable.Columns.Add("CREATEDATE");
            dataTable.Columns.Add("LEFT");
            dataTable.Columns.Add("TOP");
            dataTable.Columns.Add("HEIGHT");
            dataTable.Columns.Add("WIDTH");
            dataTable.Columns.Add("XSCALE");
            dataTable.Columns.Add("YSCALE");
            dataTable.Columns.Add("ORIENTATION");
            dataTable.Columns.Add("HSCROLL");
            dataTable.Columns.Add("VSCROLL");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("INFORMATIONAL");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDETAILs in db.POSTDETAILs
                    where
                        POSTDETAILs.TOWID == towId
                    select new
                    {
                        POSTDETAILs.POSTDETAIL_ID,
                        POSTDETAILs.TOWID,
                        POSTDETAILs.SVCDT,
                        POSTDETAILs.PAIDAMT,
                        POSTDETAILs.EMBACCT,
                        POSTDETAILs.ROUTEINFO,
                        POSTDETAILs.STATUS,
                        POSTDETAILs.DOCPAGE,
                        POSTDETAILs.MODIFYUID,
                        POSTDETAILs.MODIFYDATE,
                        POSTDETAILs.CREATEUID,
                        POSTDETAILs.CREATEDATE,
                        POSTDETAILs.LEFT,
                        POSTDETAILs.TOP,
                        POSTDETAILs.HEIGHT,
                        POSTDETAILs.WIDTH,
                        POSTDETAILs.XSCALE,
                        POSTDETAILs.YSCALE,
                        POSTDETAILs.ORIENTATION,
                        POSTDETAILs.HSCROLL,
                        POSTDETAILs.VSCROLL,
                        POSTDETAILs.IFN,
                        POSTDETAILs.REASON,
                        POSTDETAILs.INFORMATIONAL,
                        POSTDETAILs.PRACTICE,
                        POSTDETAILs.IFNDS,
                        POSTDETAILs.IFNID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.POSTDETAIL_ID
                        , row.TOWID
                        , row.SVCDT
                        , row.PAIDAMT
                        , row.EMBACCT
                        , row.ROUTEINFO
                        , row.STATUS
                        , row.DOCPAGE
                        , row.MODIFYUID
                        , row.MODIFYDATE
                        , row.CREATEUID
                        , row.CREATEDATE
                        , row.LEFT
                        , row.TOP
                        , row.HEIGHT
                        , row.WIDTH
                        , row.XSCALE
                        , row.YSCALE
                        , row.ORIENTATION
                        , row.HSCROLL
                        , row.VSCROLL
                        , row.IFN
                        , row.REASON
                        , row.INFORMATIONAL
                        , row.PRACTICE
                        , row.IFNDS
                        , row.IFNID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDetailByTowerId() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDetailByPracticeAndAccount(string workstation, string username, 
            string practice, string account)
        {
            Log.DebugFormat("Enter GetPostDetailByPracticeAndAccount() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdetail");
            dataTable.Columns.Add("POSTDETAIL_ID");
            dataTable.Columns.Add("TOWID, SVCDT");
            dataTable.Columns.Add("PAIDAMT");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("ROUTEINFO");
            dataTable.Columns.Add("STATUS, DOCPAGE");
            dataTable.Columns.Add("MODIFYUID");
            dataTable.Columns.Add("MODIFYDATE");
            dataTable.Columns.Add("CREATEUID");
            dataTable.Columns.Add("CREATEDATE");
            dataTable.Columns.Add("LEFT");
            dataTable.Columns.Add("TOP");
            dataTable.Columns.Add("HEIGHT");
            dataTable.Columns.Add("WIDTH");
            dataTable.Columns.Add("XSCALE");
            dataTable.Columns.Add("YSCALE");
            dataTable.Columns.Add("ORIENTATION");
            dataTable.Columns.Add("HSCROLL");
            dataTable.Columns.Add("VSCROLL");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("INFORMATIONAL");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDETAILs in db.POSTDETAILs
                    where
                        POSTDETAILs.PRACTICE == practice &&
                        POSTDETAILs.EMBACCT == account
                    select new
                    {
                        POSTDETAILs.POSTDETAIL_ID,
                        POSTDETAILs.TOWID,
                        POSTDETAILs.SVCDT,
                        POSTDETAILs.PAIDAMT,
                        POSTDETAILs.EMBACCT,
                        POSTDETAILs.ROUTEINFO,
                        POSTDETAILs.STATUS,
                        POSTDETAILs.DOCPAGE,
                        POSTDETAILs.MODIFYUID,
                        POSTDETAILs.MODIFYDATE,
                        POSTDETAILs.CREATEUID,
                        POSTDETAILs.CREATEDATE,
                        POSTDETAILs.LEFT,
                        POSTDETAILs.TOP,
                        POSTDETAILs.HEIGHT,
                        POSTDETAILs.WIDTH,
                        POSTDETAILs.XSCALE,
                        POSTDETAILs.YSCALE,
                        POSTDETAILs.ORIENTATION,
                        POSTDETAILs.HSCROLL,
                        POSTDETAILs.VSCROLL,
                        POSTDETAILs.IFN,
                        POSTDETAILs.REASON,
                        POSTDETAILs.INFORMATIONAL,
                        POSTDETAILs.PRACTICE,
                        POSTDETAILs.IFNDS,
                        POSTDETAILs.IFNID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.POSTDETAIL_ID
                        , row.TOWID
                        , row.SVCDT
                        , row.PAIDAMT
                        , row.EMBACCT
                        , row.ROUTEINFO
                        , row.STATUS
                        , row.DOCPAGE
                        , row.MODIFYUID
                        , row.MODIFYDATE
                        , row.CREATEUID
                        , row.CREATEDATE
                        , row.LEFT
                        , row.TOP
                        , row.HEIGHT
                        , row.WIDTH
                        , row.XSCALE
                        , row.YSCALE
                        , row.ORIENTATION
                        , row.HSCROLL
                        , row.VSCROLL
                        , row.IFN
                        , row.REASON
                        , row.INFORMATIONAL
                        , row.PRACTICE
                        , row.IFNDS
                        , row.IFNID
                        );
                }
            }

            Log.DebugFormat("Exit GetPostDetailByPracticeAndAccount() Rows:[{0}]",
                dataTable.Rows.Count);
            return dataTable;
        }

        public DataTable GetPostDetailByPostDetailId(string workstation, string username, int postDetailId)
        {
            Log.DebugFormat("Enter GetPostDetailByPostDetailId() Workstation:[{0}], Username:[{1}]",
                workstation, username);

            DataTable dataTable = new DataTable("postdetail");
            dataTable.Columns.Add("POSTDETAIL_ID");
            dataTable.Columns.Add("TOWID, SVCDT");
            dataTable.Columns.Add("PAIDAMT");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("ROUTEINFO");
            dataTable.Columns.Add("STATUS, DOCPAGE");
            dataTable.Columns.Add("MODIFYUID");
            dataTable.Columns.Add("MODIFYDATE");
            dataTable.Columns.Add("CREATEUID");
            dataTable.Columns.Add("CREATEDATE");
            dataTable.Columns.Add("LEFT");
            dataTable.Columns.Add("TOP");
            dataTable.Columns.Add("HEIGHT");
            dataTable.Columns.Add("WIDTH");
            dataTable.Columns.Add("XSCALE");
            dataTable.Columns.Add("YSCALE");
            dataTable.Columns.Add("ORIENTATION");
            dataTable.Columns.Add("HSCROLL");
            dataTable.Columns.Add("VSCROLL");
            dataTable.Columns.Add("IFN");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("INFORMATIONAL");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("IFNDS");
            dataTable.Columns.Add("IFNID");

            using (var db = new TowerModelContainer())
            {
                var query = (from POSTDETAILs in db.POSTDETAILs
                    where
                        POSTDETAILs.POSTDETAIL_ID == postDetailId
                    select new
                    {
                        POSTDETAILs.POSTDETAIL_ID,
                        POSTDETAILs.TOWID,
                        POSTDETAILs.SVCDT,
                        POSTDETAILs.PAIDAMT,
                        POSTDETAILs.EMBACCT,
                        POSTDETAILs.ROUTEINFO,
                        POSTDETAILs.STATUS,
                        POSTDETAILs.DOCPAGE,
                        POSTDETAILs.MODIFYUID,
                        POSTDETAILs.MODIFYDATE,
                        POSTDETAILs.CREATEUID,
                        POSTDETAILs.CREATEDATE,
                        POSTDETAILs.LEFT,
                        POSTDETAILs.TOP,
                        POSTDETAILs.HEIGHT,
                        POSTDETAILs.WIDTH,
                        POSTDETAILs.XSCALE,
                        POSTDETAILs.YSCALE,
                        POSTDETAILs.ORIENTATION,
                        POSTDETAILs.HSCROLL,
                        POSTDETAILs.VSCROLL,
                        POSTDETAILs.IFN,
                        POSTDETAILs.REASON,
                        POSTDETAILs.INFORMATIONAL,
                        POSTDETAILs.PRACTICE,
                        POSTDETAILs.IFNDS,
                        POSTDETAILs.IFNID
                    });

                foreach (var row in query)
                {
                    dataTable.Rows.Add(
                        row.POSTDETAIL_ID
                        , row.TOWID
                        , row.SVCDT
                        , row.PAIDAMT
                        , row.EMBACCT
                        , row.ROUTEINFO
                        , row.STATUS
                        , row.DOCPAGE
                        , row.MODIFYUID
                        , row.MODIFYDATE
                        , row.CREATEUID
                        , row.CREATEDATE
                        , row.LEFT
                        , row.TOP
                        , row.HEIGHT
                        , row.WIDTH
                        , row.XSCALE
                        , row.YSCALE
                        , row.ORIENTATION
                        , row.HSCROLL
                        , row.VSCROLL
                        , row.IFN
                        , row.REASON
                        , row.INFORMATIONAL
                        , row.PRACTICE
                        , row.IFNDS
                        , row.IFNID
                        );
                }
            }

            Log.Debug(String.Format("Exit GetPostDetailByPostDetailId() Rows:[{0}]",
                dataTable.Rows.Count));
            return dataTable;
        }

        public uint InsertIntoPostDetail(string workstation, string username, NavPostDetail postDetail)
        {
            Log.DebugFormat("Enter InsertIntoPostDetail() Workstation:[{0}], Username:[{1}], TowId:[{2}]",
                workstation, username, postDetail.TOWID);

            uint postDetailId = 0;

            var dbPostDetail = new POSTDETAIL(){
                POSTDETAIL_ID = 0,
                TOWID = postDetail.TOWID,
                ALT_TOWID = postDetail.ALT_TOWID == null ? 0M : (decimal)postDetail.ALT_TOWID,
                COMINGLED_STATUS = postDetail.COMINGLED_STATUS,
                CREATEDATE = postDetail.CREATEDATE,
                CREATEUID = postDetail.CREATEUID,
                DOCPAGE = postDetail.DOCPAGE,
                EMBACCT = postDetail.EMBACCT,
                HEIGHT = postDetail.HEIGHT,
                HSCROLL = postDetail.HSCROLL,
                IFN = postDetail.IFN,
                LEFT = postDetail.LEFT,
                MODIFYDATE = postDetail.MODIFYDATE,
                NEXT_ACT_NAME = postDetail.NEXT_ACT_NAME,
                NEXT_MAP_NAME = postDetail.NEXT_MAP_NAME,
                ORIENTATION = postDetail.ORIENTATION,
                ORIG_TOWID = postDetail.ORIG_TOWID,
                PAIDAMT = postDetail.PAIDAMT,
                PRACTICE = postDetail.PRACTICE,
                REASON = postDetail.REASON,
                ROUTEINFO = postDetail.ROUTEINFO,
                SERVICEID = postDetail.SERVICEID,
                STATUS = postDetail.STATUS,
                SVCDT = postDetail.SVCDT,
                TOP = postDetail.TOP,
                VSCROLL = postDetail.VSCROLL,
                WIDTH = postDetail.WIDTH,
                XSCALE = postDetail.XSCALE,
                YSCALE = postDetail.YSCALE,
                IFNDS = postDetail.IFNDS,
                IFNID = postDetail.IFNID
            };

            using (var context = new TowerModelContainer())
            {
                try
                {
                    context.Database.Connection.Open();
                    context.POSTDETAILs.Add(dbPostDetail);
                    context.Entry(dbPostDetail).State = EntityState.Added;
                    var status = context.SaveChanges() > 0 ? true : false;
                    Log.DebugFormat("InsertIntoPostDetail() status:[{0}]", status);
                    postDetailId = Convert.ToUInt32(dbPostDetail.POSTDETAIL_ID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Log.Debug(ex.Message);
                }
            }

            Log.DebugFormat("Exit InsertIntoPostDetail() PostDetailId:[{0}]",
                postDetailId.ToString());
            return postDetailId;
        }

    }
}
