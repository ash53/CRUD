using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
    public class WorkModel
    {
        public string DocumentNumber { get; set; }
        public Boolean NPrepping { get; set; }
        public bool OWriteDRoutingInfo { get; set; }
        public class StartActivity
        {
            public string typecode { get; set; }
            public string startdescription { get; set; }
            public string fullstartActivityDesc { get; set; }
            public string mapName { get; set; }
            public string activityName { get; set; }
        }
    }
}
