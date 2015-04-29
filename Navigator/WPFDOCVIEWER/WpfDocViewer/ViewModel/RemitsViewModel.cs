using WpfDocViewer.Model;

namespace WpfDocViewer.ViewModel
{
    public class RemitsViewModel : TreeViewItemViewModel
    {
        readonly RemitData _remitdata;

        public RemitsViewModel(RemitData remitdata, ChecksViewModel parentState)
            : base(parentState, false)
        {
            _remitdata = remitdata;
        }

    }
}