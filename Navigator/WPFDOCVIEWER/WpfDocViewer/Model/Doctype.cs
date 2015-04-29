using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
    public static class Doctype
    {
        private static List<string> doctypeList = new List<string>();

        public static List<string> DocTypeList
        {
            get { return doctypeList; }
            set { doctypeList = value; }
        }
    }
}
