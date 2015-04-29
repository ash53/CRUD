using System.Collections.Generic;

namespace WpfDocViewer.Model
{
    public class TreeRoot
    {
        public TreeRoot(string treeRootName)
        {
            this.TreeRootName = treeRootName;
        }

        public string TreeRootName { get; private set; }

        readonly List<CheckData> _checkDatas = new List<CheckData>();
        public List<CheckData> CheckDatas
        {
            get { return _checkDatas; }
        }
    }
}