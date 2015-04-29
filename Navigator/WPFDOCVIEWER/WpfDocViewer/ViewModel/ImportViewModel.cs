using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.ComponentModel;
using System.Drawing;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using WpfDocViewer.Model;
using System.Windows.Threading;
using System.Windows;

namespace WpfDocViewer.ViewModel
{
    public class ImportViewModel : INotifyPropertyChanged
    {
        BitmapImage bitImage;
        string defaultPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\th.jpg";
        string setPreviewPath;
        private List<string> importList;
        private List<string> keepList;
       // private ObservableCollection<AttachmentClass> attachments;
       // private ObservableCollection<AttachmentClass> keepAttachments;
       // private AttachmentClass Attachments;
        Image boundImage;
        private string recentlyChanged = "yes"; // Recently changed on creation
        string filelist;
        
        #region INotifyPropertyChanged Member Details
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ImportViewModel()
        {
            this.RemoveRowCommand = new RelayCommand(OnRemove);
            ImportList = Model.AttachmentClass.Filelist;
        }

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
        public string SetPreviewPath
        {
            get{
                if(string.IsNullOrWhiteSpace(setPreviewPath))
                {
                    setPreviewPath = defaultPath;
                }
                return setPreviewPath;
            }
            set
            {
                setPreviewPath = value;
                //pdf does not work in Image
                if (setPreviewPath != null)
                {
                    if (!setPreviewPath.Contains(".pdf"))
                    {
                        //update image
                        bitImage = new BitmapImage();
                        bitImage.BeginInit();

                        bitImage.UriSource = new Uri(SetPreviewPath);
                        bitImage.EndInit();
                        PreviewSelectedItem = bitImage;
                    }
                }
                
                VerifyPropertyName("SetPreviewPath");
            }
        }
        public BitmapImage PreviewSelectedItem
        {
            
         
            get
            {   
                return bitImage;
            }
            set
            {
                bitImage = new BitmapImage();
                //can not handle pdf
                if (!SetPreviewPath.Contains(".pdf"))
                {
                    bitImage.BeginInit();

                    bitImage.UriSource = new Uri(SetPreviewPath);
                    bitImage.EndInit();
                }
                VerifyPropertyName("PreviewSelectedItem");
            }

        }

        public void AddToFileList(string[] addTo)
        {
            
            if (addTo != null)
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;


                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action)(() =>
                    {
                        foreach (string str in addTo)
                        {
                            if (keepList == null)
                            {
                                keepList = new List<string>();
                                //refill from existing to support back and forth from import controls
                                if (Model.AttachmentClass.Filelist != null)
                                {
                                    keepList = Model.AttachmentClass.Filelist;
                                }
                            }
                            keepList.Add(str);

                        }
                        Model.AttachmentClass.Filelist = keepList;
                        ImportList = Model.AttachmentClass.Filelist;
                        RecentlyChanged = "yes";
                    }));
                }
                else
                {
                    foreach (string str in addTo)
                    {
                        if (keepList == null)
                        {
                            keepList = new List<string>();
                            //refill from existing to support back and forth from import controls
                            if (Model.AttachmentClass.Filelist != null)
                            {
                                keepList = Model.AttachmentClass.Filelist;
                            }
                        }
                        keepList.Add(str);

                    }
                    Model.AttachmentClass.Filelist = keepList;
                    ImportList = Model.AttachmentClass.Filelist;
                    RecentlyChanged = "yes";
                }
               
                
            }

           
           
        }
        /// <summary>
        /// Item to remove from import list
        /// </summary>
        /// <param name="removeFrom"></param>
        public void RemoveFromFileList(string[] removeFrom)
        {
            
            if (removeFrom != null)
            {
                foreach (string str in removeFrom)
                {
                    if (keepList != null)
                    {
                        keepList.Remove(str);
                    }
                }
                Model.AttachmentClass.Filelist = keepList;
                ImportList = Model.AttachmentClass.Filelist;
                RecentlyChanged = "yes";
            }
           
        }
       
    
        /// <summary>
        /// Mark Lane use this for MVVM to have dispatcher from UI run thread
        /// </summary>
        private void SendingToTheDispatcherThread()
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;


            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke((Action)(() =>
                                                    {
                                                        // put code for the dispatched here
                                                    }));
            }
            else
            {
                // put code for the dispatched here
            }
        }
      
        /// <summary>
        /// List bound to items to import.
        /// needs get data from the Model.
        /// Model.Attachment class is shared amongst controls.
        /// </summary>
        public List<string> ImportList
        {
            get
            {
                return importList;
            }
            set
            {
                importList = value;
                OnPropertyChanged("ImportList");
               
            }
        }
        public string Filelist
        {
            get { return this.ToString(); }
            set { filelist = value; }
        } 
        public string RecentlyChanged
        {
            get { return recentlyChanged; }
            set
            {
                recentlyChanged = value;
                OnPropertyChanged("RecentlyChanged");
            }
        }
        public void OpenPictureViewer()
        {
            //View.DocumentViewer doc;
            //if (doc != null)
            //{
            //    doc.Visibility = System.Windows.Visibility.Visible;
            //    doc.Show();
            //    doc.DrawDocument(ifn, pageno);
            //}
            //else
            //{
            //    doc = new View.DocumentViewer("imageprod", Environment.UserName, "");
            //    doc.Visibility = System.Windows.Visibility.Visible;
            //    doc.Show();
            //    doc.DrawDocument(ifn, pageno);
            //}
        }
        /// <summary>
        /// command bound to the views Remove Selected Row button.
        /// </summary>
        public RelayCommand RemoveRowCommand
        {
            get;
            private set;

        }
        /// <summary>
        /// Mark Lane
        /// 
        /// Used to remove from the exportable files list.
        /// use the dispatcher to merge the threads since the UI has two different
        /// threads that use the ViewModel.
        /// </summary>
        private void OnRemove()
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;


            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke((Action)(() =>
                {
                    string[] newlist = new string[1];
                    newlist[0] = SetPreviewPath;
                    RemoveFromFileList(newlist);
                }));
            }
            else
            {
                string[] newlist = new string[1];
                newlist[0] = SetPreviewPath;
                RemoveFromFileList(newlist);
            }
           
            
        }
        #endregion
       

    }
}
