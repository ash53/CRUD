using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
    public class DocumentModel
    {
        public DocumentModel(string ifn, short pageno, string docgroup, string doctype, string scandate )
        {
            this.IFN = ifn;
            this.Pagenos = pageno;
            this.DocGroup = docgroup;
            this.DocType = doctype;
            this.SCANDATE = scandate;
        }

        public string IFN { get; private set; }
        public short Pagenos { get; private set; }
        public string DocGroup { get; private set; }
        public string DocType { get; private set; }
        public string SCANDATE { get; private set; }
    }
}
