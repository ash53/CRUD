using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Author: Mark Lane
    /// Viewer's User Control to house all the IDM ActiveX controls
    /// </summary>
    public partial class DocViewerControl : UserControl
    {
        //Prepping and IDM based variables
        bool retainOrientation;
        int rotation = 0;
        string previousIFN = "";
        Int16 pages = 1;  //only zero on error. Default is "1" 

        public DocViewerControl(string server, string username, string password)
        { 
            InitializeComponent();
            IDMServer = server;
            IDMUsername = username;
            IDMPassword = password;
            //MUST LOGIN TO IDM!!! FOR ALL CONTROLS!!!
            LoginToIDM(IDMServer, IDMUsername, IDMPassword);
        }
        #region private variables for prepping and recovery
        
        private string IDMServer
        {
            get;
            set;
        }
        private string IDMUsername
        {
            get;
            set;
        }
        private string IDMIFN
        {
            get;
            set;
        }
        private short IDMPagenumber
        {
            get;
            set;
        }
        private string IDMPassword
        {
            get;
            set;
        }
        #endregion
        #region Imaging Functions
        public Int16 Pages { get { return pages; } set { pages = value; } }
        public void LoginToIDM(string server, string username, string password)
        {
            try
            {
                if (this.axTTLogin1.LoggedIn == false)
                {
                    this.axTTLogin1.LoginEx(IDMServer, IDMUsername, IDMPassword);
                }
                else
                {
                    this.axTTLogin1.Login(); //piggyback
                }

            }
            catch (Exception ex)
            {
                //capture the error message and number.
                this.axTTLogin1.Login();
            }
        }
        private void BuildAnnotationToolbar()
        {
            short annotations;
            //load once and store.
            ImageList imagelist = new ImageList();
            //load only if not present.
            for (annotations = 0; annotations < axTTDoc.Annotations.Tools.Count; annotations++)
            {
                Image nodoCtlAX = (Image)ImageConverter.IPictureToImage((axTTDoc.Annotations.Tools.Item[annotations].Picture));
                imagelist.Images.Add(nodoCtlAX);
                nodoCtlAX = null;
            }
            annotationToolStrip.ImageList = imagelist;
            annotations = 0;
            for (annotations = 0; annotations < axTTDoc.Annotations.Tools.Count; annotations++)
            {
                if (annotationToolStrip.Items.Count < axTTDoc.Annotations.Tools.Count)
                {
                    ToolStripButton toolstripbutton = new ToolStripButton();
                    annotationToolStrip.Items.Add(toolstripbutton);
                    annotationToolStrip.Items[annotations].ImageIndex = annotations; //must set the index
                }
            }
        }
        public void ResizeViewer()
        {
            try
            {
                axTTDoc.Fit(TTDoc.ttFitCodes.ttFitAll);
                axTTDoc.Refresh();
            }
            catch
            {
                //not loaded
            }
        }
        /// <summary>
        /// method to show the annotations or hide them based on the type. Should be callable from menu?
        /// if not from menu this should be made private
        /// </summary>
        /// <param name="visible"></param>
        public void ShowAnnotations(bool visible)
        {
                
                short annotations = 0;

                if (annotationToolStrip.Items.Count < 2)
                {
                    BuildAnnotationToolbar();
                }
                for(annotations = 0; annotations < axTTDoc.Annotations.Tools.Count; annotations++)
                {
                    annotationToolStrip.Items[annotations].Visible = visible;
                    
                }
        }
        /// <summary>
        /// Used to load the imaging info like IFN, pages, subpages, pardocno 
        /// for both pre and post billing.
        /// </summary>
        /// <param name="dt"></param>
       
        #endregion
        #region IDM control events to capture
        //occurs first after a ttdoc.Refresh and new IFN is loaded.
        private void axTTDoc_ImageChanged(object sender, EventArgs e)
        {
            //login and load annotations
            
        }
        void axTTDoc_KeyDownEvent(object sender, AxTTDoc._DTTDocEvents_KeyDownEvent e)
        {
            //throw new System.NotImplementedException();
        }

        public void axTTDoc_DisplayChanged(object sender, System.EventArgs e)
        {
            //login and load annotations
           
        }
        void axTTDoc_PositionChanged(object sender, System.EventArgs e)
        {
            //position changed
        }

        void axTTDoc_ToolStateChanged(object sender, System.EventArgs e)
        {
            Thread.Sleep(500);
            //login and load annotations
             for (short i = 1; i < axTTDoc.Annotations.Tools.Count - 1; i++)
             {
                   // annotationToolStrip.Items[i].Enabled = axTTDoc.Annotations.Tools[i].Enabled;
        
                  //  annotationToolStrip.Items[i].Enabled = axTTDoc.Annotations.Tools[i].Selected;
             }
        }

        public void axTTDoc_SelectionChanged(object sender, System.EventArgs e)
        {
           // throw new System.NotImplementedException();
            
        }

        private void axTTLogin1_LoggedInEvent(object sender, EventArgs e)
        {
            //
        }
        private void annotationToolStrip_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            try
            {
                
                if (e.ClickedItem.ImageIndex > -1)
                {
                    string sent = sender.ToString();
                    this.axTTDoc.Annotations.Tools[Convert.ToInt16(e.ClickedItem.ImageIndex)].Selected = true;
                    this.axTTDoc.Annotations.AutoReset = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Clicking Annotations : " + ex.Message);
                axTTDoc.Annotations.Reload();
                
            }
        }

    #endregion

        #region IDM ActiveX control public functions.
       /// <summary>
       /// Author: Mark Lane
       /// One function to handle the display of image and page
       /// Fills the ttdocop first for operations and to retain that required relationship.
       /// AutoRecovery needs to logout and in and reset pagenumber to fix lost miscells
       /// or Retriever errors.
       /// </summary>
       /// <param name="ifn"></param>
       /// <param name="pageno"></param>
        public void DrawDoc(string ifn, short pageno)
        {
            IDMIFN = ifn;
            IDMPagenumber = pageno;
           
            if (retainOrientation == true)
            {
                rotation = axTTDoc.Orientation;//stored publically for Coding and Prepping
            }
            else
            {
                rotation = 0;
            }

                try
                {
                    CheckTowerTemp();
                    //set ttdocop first to check as a trial.  Reverse this if eligibility issues occur in import.
                    axTTDocOp1.IFN = ifn;
                    axTTDocOp1.CurPage = pageno;
                    ImageSettings();
                    //doc op used to set proper options based on type.
                    axTTDoc.IFN = ifn;
                    axTTDoc.Page = pageno;
                    axTTDoc.Refresh();
                    //load
                    if (IDMIFN != previousIFN)
                    {
                        //set the IFN's max pages
                        Pages = axTTDoc.Pages;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("IDM"))
                    {
                        //do not auto recover for now.
                        MessageBox.Show("DrawDoc Error: " + ex.Message + " inner: " + ex.InnerException);
                        return;
                    }
                    if (pageno > axTTDoc.Pages || pageno == 0)
                    {
                        IDMPagenumber = 1;
                    }
                    AutoRecovery();
                    if (IDMIFN != null)
                    {
                        DrawDoc(IDMIFN, IDMPagenumber);
                    }
                }
            
        }
        private void AutoRecovery()
        {
            try
            {
                axTTLogin1.Logout();
                Thread.Sleep(2000);
                LoginToIDM(IDMServer, IDMUsername, IDMPassword);

            }
            catch (Exception ex)
            {
                axTTLogin1.Login();
            }
            finally
            {
                Thread.Sleep(500);
            }

        }
        private void ImageSettings()
        {
            int parcelType;

            parcelType = axTTDocOp1.ParcelType;
            
            switch (parcelType)
            {
                case 1:
                    //TIF
                    axTTDoc.Preview = true;
                    axTTDoc.Annotations.Mode = TTDoc.ttAnnotModes.ttAnnotShowEditable;
                    axTTDoc.ScaleToGrey = true;
                    axTTDoc.Negated = false;
                    ShowAnnotations(true);
                    break;
                case 22:
                    //XML
                    axTTDoc.Annotations.Mode = TTDoc.ttAnnotModes.ttAnnotHide;
                    ShowAnnotations(false);
                    break;
                case 7:
                    //TEXT
                    axTTDoc.Annotations.Mode = TTDoc.ttAnnotModes.ttAnnotHide;
                    ShowAnnotations(false);
                    break;
                case 17:
                    //JPG
                    axTTDoc.Preview = true;
                    
                   
                    axTTDoc.Annotations.Mode = TTDoc.ttAnnotModes.ttAnnotShowEditable;
                    
                    axTTDoc.ScaleToGrey = true; //must be called before refresh?
                    axTTDoc.Negated = false;
                    ShowAnnotations(true);
                    break;
                default:
                    //Unknown
                    axTTDoc.Preview = true;
                    axTTDoc.Annotations.Mode = TTDoc.ttAnnotModes.ttAnnotShowEditable;
                    axTTDoc.ScaleToGrey = true;
                    axTTDoc.Negated = false;
                    ShowAnnotations(true);
                    break;
            }
            //Check the settings and load the proper tools
        }
        private void CheckTowerTemp()
        {
            try
            {
            
            //for IDM bugs with XML documents.
                    if (Environment.MachineName.ToUpper().Contains("RTICTX"))
                    {
                        DriveInfo wdrive = new DriveInfo("W");
                        if(wdrive.IsReady)
                        {
                            DirectoryInfo towertemp = new DirectoryInfo(@"W:\TowerTEMP");
                            if (!towertemp.Exists)
                            {
                                Directory.CreateDirectory(@"W:\TowerTEMP");
                            }
                        }
                        else
                        {
                            MessageBox.Show(@"W:\Drive Not found.");
                        }
                    }
                    else
                    {
                            DirectoryInfo towertemp = new DirectoryInfo(@"C:\TowerTEMP");
                            if (!towertemp.Exists)
                            {
                                Directory.CreateDirectory(@"C:\TowerTEMP");
                            }
                    }
                
            }
            catch(Exception ex)
            {
                //what?
            }
        }
        /// <summary>
        /// Used to clear the annotations on a page before leaving current page
        /// when in PostBilling.
        /// Used to clear annotations from Menu option.
        /// </summary>
        public void DeleteAnnotations()
        {
            axTTDoc.Annotations.DeleteAll();
        }
        public void PageNavigation(string action)
        {
            switch (action.ToLower())
            {
                case "next":
                    //go
                    //use the current ifn.
                    DrawDoc(axTTDoc.IFN, Convert.ToInt16(axTTDoc.Page + 1));
                    break;
                case "previous":
                    //back
                    DrawDoc(axTTDoc.IFN, Convert.ToInt16(axTTDoc.Page - 1));
                    break;
                case "last":
                    //max
                    DrawDoc(axTTDoc.IFN, axTTDoc.Pages);
                    break;
                case "first":
                    //first
                    DrawDoc(axTTDoc.IFN, 1);
                    break;
                default:
                    break;
            }
        }
        public string ImportIFN(string path, int ifndspool, string newifn)
        {
            axTTDocOp1.ImportEx(path, ifndspool, newifn);
            return "success";
        }
        public void ImageNegate()
        {
            try
            {
                    this.axTTDoc.Negated = !this.axTTDoc.Negated;
                    this.axTTDoc.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
        public void ImageRestore()
        {
            try
            {
                this.axTTDoc.Restore();
                this.axTTDoc.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
        private void ImageRotate180()
        {
            try
            {
                this.axTTDoc.Orientation = Convert.ToInt16(this.axTTDoc.Orientation + 180);
                this.axTTDoc.Refresh();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void ImageRotateLeft()
        {
            try
            {
                this.axTTDoc.Orientation = Convert.ToInt16(this.axTTDoc.Orientation - 90);
                this.axTTDoc.Refresh();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void ImageRotateRight()
        {
            try
            {
                this.axTTDoc.Orientation = Convert.ToInt16(this.axTTDoc.Orientation + 90);
                this.axTTDoc.Refresh();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void ImageScaleToGrey()
        {
            try
            {
                this.axTTDoc.ScaleToGrey = !this.axTTDoc.ScaleToGrey;
                this.axTTDoc.Refresh();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void ImagePreviewMode()
        {
            try
            {
                this.axTTDoc.Preview = !this.axTTDoc.Preview;
                this.axTTDoc.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
        public void ImageSizeWidth()
        {
            try
            {
                this.axTTDoc.Fit(TTDoc.ttFitCodes.ttFitWidth);
                this.axTTDoc.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
        public void ImageSizeHeight()
        {
            try
            {
                this.axTTDoc.Fit(TTDoc.ttFitCodes.ttFitHeight);
                this.axTTDoc.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
        public void ImageSizeAll()
        {
            try
            {
                this.axTTDoc.Fit(TTDoc.ttFitCodes.ttFitAll);
                this.axTTDoc.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
        public void ImageZoomIn()
        {
            try
            {
                this.axTTDoc.ZoomIn();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void ImageZoomOut()
        {
            try
            {
                this.axTTDoc.ZoomOut();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void ExportImage()
        {
            this.axTTDoc.Export();
        }
        public void PrintSetup()
        {
            try
            {
                this.axTTDoc.PrintSetup();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void PrintPage()
        {
            try
            {
                this.axTTDoc.PrintCurrPage();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void PrintAllPages()
        {
            try
            {
                this.axTTDoc.PrintAllPages();
            }
            catch(Exception ex)
            {
            
            }
        }
        public void PrintDialog()
        {
            try
            {
                this.axTTDoc.PrintDialog();
            }
            catch(Exception ex)
            {
            
            }
        }

        #endregion


        
    }
}
