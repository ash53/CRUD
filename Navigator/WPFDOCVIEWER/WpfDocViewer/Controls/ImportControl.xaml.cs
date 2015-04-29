using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfDocViewer.Model;

namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Interaction logic for ImportControl.xaml
    /// </summary>
    public partial class ImportControl : UserControl
    {
        ViewModel.ImportViewModel context;
        ExplorerControl content;
        SearchControl searchcontent;
        IDMExportControl exportcontent;
        string[] filesAdd;
        string[] filesRemove;
        

        public ImportControl()
        {
            
            InitializeComponent();
            try
            {
                //ML initialize your viewmodel
                context = new ViewModel.ImportViewModel();
                this.DataContext = context;
               
                exportcontent = new IDMExportControl();
                ExportControlHost.Child = exportcontent;
                exportcontent.Login(PermissionsModel.Permissions.IDMPermissions.IDMServer, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                ExportControlHost.Visibility = System.Windows.Visibility.Hidden;
                
                //allow the control to keep the attachments until user removes them or its imported then it will be cleared.
                ImportListView.ItemsSource = null;
                ImportListView.ItemsSource = context.ImportList;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error on ImportControl :" + ex.Message + " Inner: " + ex.InnerException);
            }
   
        }
        public void BrowseForFiles()
        {
            try
            {
                //initialize your windows browse control
                content = new ExplorerControl();
                WinFormHost.Child = content;
                WinFormHost.Visibility = System.Windows.Visibility.Visible;
                filesAdd = content.BrowseFiles();
                WinFormHost.Visibility = System.Windows.Visibility.Hidden;
                context.AddToFileList(filesAdd);
                ImportListView.ItemsSource = null;
                ImportListView.ItemsSource = context.ImportList;
                if (context.ImportList.Count >= 1)
                    LblError.Visibility = Visibility.Collapsed;
             }
            catch(Exception ex)
            {
                MessageBox.Show("Error on ImportControl :" + ex.Message + " Inner: " + ex.InnerException);
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

            
            filesAdd = content.BrowseFiles();
            context.AddToFileList(filesAdd);
            ImportListView.ItemsSource = null;
            ImportListView.ItemsSource = context.ImportList;

        }

        private void ImportSourceImported_Selected(object sender, RoutedEventArgs e)
        {

            searchcontent = new SearchControl();
            WpfContentControl.Content = searchcontent;

        }

        private void ImportListItem_Selected(object sender, RoutedEventArgs e)
        {
        
        }

        private void ImportListView_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ImportListView_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImportListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           this.ImagePreview.Source = context.PreviewSelectedItem;
        }
        /// <summary>
        /// Remove this and put in ViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportIFN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FindResource("ExportIFN").ToString().StartsWith("498") && searchcontent.IsVisible)
                {
                    //IDM Controls need a WindowsFormsHost to run so can not use ViewModel binding to run.
                    exportcontent.Login(Model.PermissionsModel.Permissions.IDMPermissions.IDMServer, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                    string[] newExport = new string[1];
                    newExport[0] = exportcontent.ExportImage(FindResource("ExportIFN").ToString(), FindResource("ExportDocno").ToString());
                    context.AddToFileList(newExport);
                    ImportListView.ItemsSource = null;
                    ImportListView.ItemsSource = context.ImportList;
                   
                }
            }
            catch
            {
                throw new Exception("Export IFN error");
            }
        }

        private void ImagePreview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// This method is not using binding. 
        /// Remove if the binding issue is resolved.
        /// Below code is not used. In the ViewModel now.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] removefile = new string[1];
                removefile[0] = ImportListView.SelectedItem.ToString();
                context.RemoveFromFileList(removefile);
                //ML wow even though binding is used after the ListView is changed the ItemsSource must be refreshed manually to reveal new data.
                this.ImportListView.ItemsSource = null;
                this.ImportListView.ItemsSource = context.ImportList;
            }
            catch
            {
               //no error handler for now
            }
            

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
           // SplashScreen splash = new SplashScreen(
           // splash.Show();
        }

        private void LocalImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //initialize your windows browse control
                //content = new ExplorerControl();
                //WinFormHost.Child = content;
                //WinFormHost.Visibility = System.Windows.Visibility.Visible;
                BrowseForFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on ImportControl :" + ex.Message + " Inner: " + ex.InnerException);
            }
        }

        private void ScannedImport_Click(object sender, RoutedEventArgs e)
        {
            searchcontent = new SearchControl();
            WpfContentControl.Content = searchcontent;
        }

       
        
    }
}
