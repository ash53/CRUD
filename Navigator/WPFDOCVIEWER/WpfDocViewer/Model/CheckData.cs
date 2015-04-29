using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
    public class CheckData
    {

        public CheckData(string checkNumber)
            {
                this.CheckNumber = checkNumber;
            }

            readonly List<RemitData> _remitDatas = new List<RemitData>();
            public List<RemitData> RemitDatas
            {
                get { return _remitDatas; }
            }

      
        public string CheckNumber { get; set; }
        public string CheckAmount { get; set; }
        public string CheckDate { get; set; }
        public string CheckDocno { get; set; }
        public string CheckDocdetail { get; set; }
    }
}
