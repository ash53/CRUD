using WpfDocViewer.Model;

namespace WpfDocViewer.ViewModel
{
    public class TreeRootViewModel : TreeViewItemViewModel
    {
        readonly TreeRoot _treeRoot;

        public TreeRootViewModel(Model.TreeRoot treeRoot) 
            : base(null, true)
        {
            _treeRoot = treeRoot;
        }

        public string TreeRootName
        {
            get { return _treeRoot.TreeRootName; }
        }

        protected override void LoadChildren()
        {
            foreach (CheckData check in AssemblyDataAccessLayer.GetChecks(_treeRoot))
                base.Children.Add(new ChecksViewModel(check, this));
        }
    }
}