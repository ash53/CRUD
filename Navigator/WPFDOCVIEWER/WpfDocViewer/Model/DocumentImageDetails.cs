using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WpfChartMoverManager.Model
{
    /// <summary>
    /// Author: Mark Lane
    /// using ObservableCollection to contain the Image details and bind DocumentViewer XAML to this.
    /// </summary>
    public class DocumentImageDetails 
    {
        List<string> ListAddress;
        ObservableCollection<List<string>> ImageAddresses;
        public string DocumentNumber
        {
            get;
            set;
        }
        public short PageCount
        {
            get;
            set;
        }
       
        public string IFN
        {
            get;
            set;
        }
    }
    public class DocumentDetailsCollection : ObservableCollection<DocumentImageDetails> { }
  
}
