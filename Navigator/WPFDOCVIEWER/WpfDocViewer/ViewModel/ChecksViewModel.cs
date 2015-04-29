using WpfDocViewer.Model;

namespace WpfDocViewer.ViewModel
{
    public class ChecksViewModel : TreeViewItemViewModel
    {
        readonly CheckData _checkData;

        public ChecksViewModel(CheckData checkdata, TreeRootViewModel parentTreeRoot)
            : base(parentTreeRoot, true)
        {
            _checkData = checkdata;
        }

        public string CheckNumber
        {
            get { return _checkData.CheckNumber; }
        }

        protected override void LoadChildren()
        {
            //show only when there is a found remit.
            try
            {
                foreach (RemitData remitdata in AssemblyDataAccessLayer.GetRemits(_checkData))
                    base.Children.Add(new RemitsViewModel(remitdata, this));
            }
            catch
            {
                //add a Dummy Node?
            }
        }
    }
}