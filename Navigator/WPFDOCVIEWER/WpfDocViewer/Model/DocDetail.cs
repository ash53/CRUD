using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
   public static class DocDetail
    {
        private static List<string> docdetailList = new List<string>();

        public static List<string> DocDetailList
        {
            get { return docdetailList; }
            set { docdetailList = value; }
        }
    }
}
