using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using System.Data;
using System.Collections.ObjectModel;

namespace WpfDocViewer.View
{
    /// <summary>
    /// Interaction logic for DocumentViewer.xaml
    /// Author: Mark Lane
    /// Viewer's User Control to house all the IDM ActiveX controls
    /// </summary>
    public partial class DocumentViewer : Window
    {
        Controls.DocViewerControl docviewer;
        WindowsFormsHost host = new WindowsFormsHost();
        DataGrid imageDataGrid;
        DataView imageDataView;
        private string strIFN;
        private short intPageno;
        private short currentPageno;
        private ObservableCollection<Model.DocumentModel> ImagesObservableCollection;

        ViewModel.DocumentViewModel context;

        public DocumentViewer(string servername, string username, string password)
        {
            InitializeComponent();

            docviewer = new Controls.DocViewerControl(servername, username, password);

            DocWindowsFormsHost1.Child = docviewer;
            
            //docviewer.Resize += new EventHandler(docviewer_Resize);
            DocWindowsFormsHost1.Margin = new Thickness(0, 0, 0, 0);
            DocWindowsFormsHost1.SizeChanged += new SizeChangedEventHandler(windowsFormsHost1_SizeChanged);
            //this.Grid1.Children.Add(windowsFormsHost1);
            //docviewer.Height = ViewModel.MathClass.Round( DocWindowsFormsHost1.Height)-5;
            //docviewer.Width = ViewModel.MathClass.Round(DocWindowsFormsHost1.Width) -5;
            //docviewer.ResizeViewer();
            context = new ViewModel.DocumentViewModel();
            DataContext = context;
            //this.docviewer.axTTDoc_SelectionChanged += new RoutedEventHandler(_GoButton_Click);
            //this._GoButton.Click += new RoutedEventHandler(_GoButton_Click);
            this.Closing += DocumentViewer_Closing;
            if (this.WindowState != System.Windows.WindowState.Minimized)
            {
                try
                {
                    this.Height = Convert.ToDouble(Properties.Settings.Default["DocHeight"]);
                    this.Width = Convert.ToDouble(Properties.Settings.Default["DocWidth"]);
                    this.Top = Convert.ToDouble(Properties.Settings.Default["DocTop"]);
                    this.Left = Convert.ToDouble(Properties.Settings.Default["DocLeft"]);
                }
                catch(Exception ex)
                {
                    //show error of not finding initial first settings or ignore?
                }
                
            }
           
        }

        void DocumentViewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.WindowState != System.Windows.WindowState.Minimized)
            {
                Properties.Settings.Default["DocHeight"] = this.Height.ToString();
                Properties.Settings.Default["DocWidth"] = this.Width.ToString();
                Properties.Settings.Default["DocTop"] = this.Top.ToString();
                Properties.Settings.Default["DocLeft"] = this.Left.ToString();
                Properties.Settings.Default.Save();
            }
            e.Cancel = true;
            this.Hide();
        }
        
        public void Close()
        {
            if (this.WindowState != System.Windows.WindowState.Minimized)
            {
                Properties.Settings.Default["DocHeight"] = this.Height.ToString();
                Properties.Settings.Default["DocWidth"] = this.Width.ToString();
                Properties.Settings.Default["DocTop"] = this.Top.ToString();
                Properties.Settings.Default["DocLeft"] = this.Left.ToString();
                Properties.Settings.Default.Save();
            }
            this.Closing -= DocumentViewer_Closing;
            //Add closing logic here.
            base.Close();
        }
        #region image functions
        public void DrawDocument(string ifn, short pageno)
        {
            docviewer.DrawDoc(ifn, pageno);
            currentPageno = pageno;
        }
        public void RotateDocument(int rotate)
        {

        }
        /// <summary>
        /// Fill the DataGrid on load. This will be used to navigate through the parent.
        /// </summary>
        /// <param name="docno"></param>
        public void GetParentDocnoData(string docno)
        {
           // ImageListCollection.DataContext = context.GetObservableDocnos(docno);
            dgImages.ItemsSource = null;
            dgImages.ItemsSource = context.GetObservableDocnos(docno);
            if (this.dgImages.SelectedValue != null)
            {
                int count = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).Pagenos;
            }
            PageCounts.Content = "Pg1/" + context.ImageCount;
            
            //ImageListCollection.ItemsSource =
        }
        /// <summary>
        /// Show the first page in Docno.
        /// </summary>
        /// <param name="docno"></param>
        public void ShowDocno()
        {
            //context.GetNextImage();
            //set the row to pull data from.
            this.dgImages.SelectedIndex = 0;
            strIFN = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).IFN;
            intPageno = 1;
            currentPageno = 1;
            DrawDocument(strIFN, currentPageno);
            PageCounts.Content = "Pg" + (this.dgImages.SelectedIndex + 1) + "/" + context.ImageCount;

        }
        #endregion
        #region image raised events
        void docviewer_Resize(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }
        /// <summary>
        /// wow don't touch the Windows Host object or Window Size Changed unless you know what your doing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void windowsFormsHost1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                

                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    docviewer.Height = Convert.ToInt32(Math.Round(e.NewSize.Height, 0));
                    docviewer.Width = Convert.ToInt32(Math.Round(e.NewSize.Width, 0));

                    docviewer.ResizeViewer();
                }
                else
                {
                    docviewer.Height = ViewModel.MathClass.Round(this.Height) - 95;
                    docviewer.Width = ViewModel.MathClass.Round(this.Width) - 20;
                    docviewer.ResizeViewer();
                }
                
            }
            catch
            {
                //if not instantiated height will be NaN
            }

            
        }


        void _GoButton_Click(object sender, RoutedEventArgs e)
        {

            

        }
        /// <summary>
        /// wow don't touch the Windows Host object or Window Size Changed unless you know what your doing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Height > 100 && this.Width > 100)
                {
                   
                    DocWindowsFormsHost1.Width = e.NewSize.Width - 10;
                    DocWindowsFormsHost1.Height = e.NewSize.Height - 95;
                   
                   // DocWindowsFormsHost1.Height = this.Height - 85;
                }
                
            }
            catch
            {
                //dont resize to anything negative
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            /*  ViewThumbnails.IsChecked = null;
              if ((PreviewMode.IsChecked == true) && (ScaleToGrey.IsChecked == true))
                  ViewThumbnails.IsChecked = true;
              if ((PreviewMode.IsChecked == false) && (ScaleToGrey.IsChecked == false))
                  ViewThumbnails.IsChecked = false;
             * */

        }
        #endregion

        private void RibbonWin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Object value = (this.ImageListCollection.Items[0] as DataRowView)[0];
                //context.SelectedImageIndex = this.ImageListCollection.SelectedIndex + 1;

                //context.GetNextImage();
                //set the row to pull data from.
                this.dgImages.SelectedIndex = this.dgImages.SelectedIndex + 1;
                //var row = ImageListCollection.ItemContainerGenerator.ContainerFromIndex(ImageListCollection.SelectedIndex) as DataGridRow;
                PageCounts.Content = "Pg" + (this.dgImages.SelectedIndex + 1) + "/" + context.ImageCount;

                strIFN = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).IFN;
                
                //Allow client control to override database count.
                intPageno = docviewer.Pages;
                //intPageno = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).Pagenos;
                if (intPageno > currentPageno)
                {
                    currentPageno = Convert.ToInt16(currentPageno + 1);
                    DrawDocument(strIFN, currentPageno);
                }
                else
                {
                    DrawDocument(strIFN, intPageno);
                }

                
            }
            catch(Exception ex)
            {
                //invalid array index
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //context.SelectedImageIndex = this.ImageListCollection.SelectedIndex - 1;

                //context.GetNextImage();
                //set the row to pull data from.
                if (this.dgImages.SelectedIndex > 0)
                {
                    this.dgImages.SelectedIndex = this.dgImages.SelectedIndex - 1;
                    PageCounts.Content = "Pg" + (this.dgImages.SelectedIndex + 1) + "/" + context.ImageCount;
                }
                //var row = ImageListCollection.ItemContainerGenerator.ContainerFromIndex(ImageListCollection.SelectedIndex) as DataGridRow;

                strIFN = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).IFN;
                //Allow client control to override database count.
                intPageno = docviewer.Pages;
                //intPageno = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).Pagenos;
                if (1 < currentPageno)
                {
                    currentPageno = Convert.ToInt16(currentPageno - 1);
                    DrawDocument(strIFN, currentPageno);
                }
                else
                {
                    DrawDocument(strIFN, intPageno);
                }
                
            }
            catch(Exception ex)
            {
                //invalid array index
            }
        }

        private void Helpme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ToolTip newtool = new ToolTip();
                newtool.Content = this.dgImages;
                
                newtool.HasDropShadow = true;
                Helpme.ToolTip = newtool;
         

                
               // this.ImageListCollection.BringIntoView();
            }
            catch(Exception ex)
            {
                //what?
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            //currently only prints the page you see.
            docviewer.PrintPage();
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ImageRotateLeft();
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ImageRotateRight();
        }

        private void ScaleToGrey_Checked(object sender, RoutedEventArgs e)
        {
            docviewer.ImageScaleToGrey();
        }

        private void ViewThumbnails_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PreviewMode_Checked(object sender, RoutedEventArgs e)
        {
            docviewer.ImagePreviewMode();
        }

        private void BestFit_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ImageSizeAll();
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ImageZoomIn();
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ImageZoomOut();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ExportImage();
        }

        private void Annotations_Click(object sender, RoutedEventArgs e)
        {
            docviewer.ShowAnnotations(true);
        }

        private void DrawBands_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearAnnotations_Click(object sender, RoutedEventArgs e)
        {
            docviewer.DeleteAnnotations();
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //context.SelectedImageIndex = this.ImageListCollection.SelectedIndex - 1;

                //context.GetNextImage();
                //set the row to pull data from.
                this.dgImages.SelectedIndex = this.dgImages.Items.Count - 1;


                

                //var row = ImageListCollection.ItemContainerGenerator.ContainerFromIndex(ImageListCollection.SelectedIndex) as DataGridRow;

                strIFN = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).IFN;
                //Allow client control to override database count.
                intPageno = docviewer.Pages;
                //intPageno = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).Pagenos;

                currentPageno = 1;
                DrawDocument(strIFN, currentPageno);
                DrawDocument(strIFN, docviewer.Pages);
                PageCounts.Content = "Pg" + context.ImageCount + "/" + context.ImageCount;

            }
            catch (Exception ex)
            {
                //invalid array index
            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //context.SelectedImageIndex = this.ImageListCollection.SelectedIndex - 1;

                //context.GetNextImage();
                //set the row to pull data from.
                this.dgImages.SelectedIndex = 0;
                
                   
                PageCounts.Content = "Pg1/" + context.ImageCount;
                
                //var row = ImageListCollection.ItemContainerGenerator.ContainerFromIndex(ImageListCollection.SelectedIndex) as DataGridRow;

                strIFN = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).IFN;
                //Allow client control to override database count.
                intPageno = docviewer.Pages;
                //intPageno = ((WpfDocViewer.Model.DocumentModel)(this.dgImages.SelectedValue)).Pagenos;
               
                currentPageno = 1;
                DrawDocument(strIFN, currentPageno);
                

            }
            catch (Exception ex)
            {
                //invalid array index
            }
        }
       
       
       

        
        
                       


    }
}
