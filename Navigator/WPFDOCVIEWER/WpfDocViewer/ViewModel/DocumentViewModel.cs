using System;
using System.ComponentModel;
using System.Data;
using System.Collections.ObjectModel;
using Rti;
using Rti.InternalInterfaces.ServiceProxies;

namespace WpfDocViewer.ViewModel
{
    /// <summary>
    /// Author: Mark Lane
    /// View Model for the imaging data
    /// </summary>
    public class DocumentViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        DataTable getDocnoAddresses;
        private ObservableCollection<Model.DocumentModel> getObservableCollectionImages = new ObservableCollection<Model.DocumentModel>();
        private int selectedImageIndex;
        private string showDocno = "Document Viewer";

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);

                handler(this, e);
            }
        }
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object. 
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

            }
        }
        
        public int SelectedImageIndex
        {
            get
            {
               return selectedImageIndex;
            }
            set
            {
                selectedImageIndex = value;
                OnPropertyChanged("SelectedImageIndex");
            }
        }
        public string ShowDocno
        {
            get
            {
                return showDocno;
            }
            set
            {
                showDocno = value;
                OnPropertyChanged("ShowDocno");
            }
        }
        public ObservableCollection<Model.DocumentModel> GetObservableDocnos(string docno)
        {
            ShowDocno = "Document Viewer : " + docno;
            Model.DocumentModel dm;
            ObservableCollection<Model.DocumentModel> obc = new ObservableCollection<Model.DocumentModel>();
            ImageCount = 0;
           // getDocnoAddresses = new DataTable("docnos");
            using (DocumentManagerServiceClient dataAccess =
                new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                //get the docno and pardocno records for imaging
                //order by doctype asc, docno asc, origno asc
               var getDocnoAddress = (from b in dataAccess.GetPostDocByParDocNo(Environment.MachineName, Environment.UserName, docno).AsEnumerable()
                                     orderby b.Field<string>("doctype"), b.Field<string>("docno"), b.Field<string>("origdoc")
                                     select b);
                foreach (DataRow row in getDocnoAddress.CopyToDataTable().Rows)
                {
                    dm = new Model.DocumentModel(row["IFN"].ToString(), Convert.ToInt16(row["npages"].ToString()), row["docgroup"].ToString(), row["doctype"].ToString(), row["SCANDATE"].ToString());
                    ImageCount = ImageCount + Convert.ToInt32(row["npages"].ToString());
                    obc.Add(dm);
                }
                GetObservableCollectionImages = obc;
            }
            return obc;
        }
        public int ImageCount { get; set; }

        public DataTable DocnoImages
        {
            get
            {
                return getDocnoAddresses;
            }
            set
            {
                getDocnoAddresses = value;
                OnPropertyChanged("DocnoImages");
            }
        }
        public ObservableCollection<Model.DocumentModel> GetObservableCollectionImages
        {
            get
            {
                return getObservableCollectionImages;
            }
            set
            {
                getObservableCollectionImages = value;
                OnPropertyChanged("GetObservableCollectionImages");
            }
        }
        public void GetNextImage()
        {
           
        }
        #endregion
    }
   
    
}
