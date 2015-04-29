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


namespace WpfDocViewer.View
{
    /// <summary>
    /// Interaction logic for DocumentViewer.xaml
    /// Author: Mark Lane
    /// Viewer's User Control to house all the IDM ActiveX controls
    /// </summary>
    public partial class PictureViewer : Window
    {
        Controls.PictureControl docviewer;
        WindowsFormsHost host = new WindowsFormsHost();
        public PictureViewer()
        {
            InitializeComponent();



            docviewer = new Controls.PictureControl();

            DocWindowsFormsHost.Child = docviewer;
            docviewer.Resize += new EventHandler(docviewer_Resize);
            DocWindowsFormsHost.Margin = new Thickness(0, 0, 0, 0);
            DocWindowsFormsHost.SizeChanged += new SizeChangedEventHandler(windowsFormsHost1_SizeChanged);
            //this.Grid1.Children.Add(windowsFormsHost1);
           
 
            //this.docviewer.axTTDoc_SelectionChanged += new RoutedEventHandler(_GoButton_Click);
            //this._GoButton.Click += new RoutedEventHandler(_GoButton_Click);

        }

        void docviewer_Resize(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        void windowsFormsHost1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           // docviewer.Height = ViewModel.MathClass.Round(windowsFormsHost1.Height) - 1;
           // docviewer.Width = ViewModel.MathClass.Round(windowsFormsHost1.Width) - 1;
            
        }
       

        void _GoButton_Click(object sender, RoutedEventArgs e)
        {

           // docviewer.DrawDoc("", 1);

        }
        public void DrawPicture(string path)
        {
            //docviewer.
        }
        public void RotateDocument(int rotate)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Height > 100 && this.Width > 100)
                {
                    dockPanel1.Height = this.Height - 1;
                    dockPanel1.Width = this.Width - 50;
                    DocWindowsFormsHost.Width = this.Width - 5;
                    DocWindowsFormsHost.Height = this.Height - 1;
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
        
                       


    }
}
