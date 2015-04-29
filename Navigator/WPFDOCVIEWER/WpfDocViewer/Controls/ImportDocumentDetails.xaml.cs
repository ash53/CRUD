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
using System.Windows.Threading;
using WpfDocViewer.Model;
using WpfDocViewer.Controls;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Threading;



namespace WpfDocViewer
{
    /// <summary>
    /// Interaction logic for ImportDocumentDetails.xaml
    /// </summary>
    public partial class ImportDocumentDetails : UserControl
    {
        ViewModel.ImportDataViewModel context;
        SubmissionStatus subStatus = new SubmissionStatus();
        MainImportControl ctr = new MainImportControl();
        public ContentControl ParentControl { get; set; }
        

        public ImportDocumentDetails()
        {
               InitializeComponent();           
             //Initialize viewmodel
             context = new ViewModel.ImportDataViewModel();
             this.DataContext = context;         
           
        }
        
       
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            context.Validate();
            if (context.validationErrors.Count == 0)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, args) =>
                {
                    context.OnSubmit();

                };
                worker.RunWorkerCompleted += (s, args) =>
                {
                    Button Back = (Button)ParentControl.FindName("Back");
                    Back.Visibility = Visibility.Collapsed;
                    subStatus.MyParentControl = this.MyImportContentControl;
                    this.MyImportContentControl.Content = null;
                    this.MyImportContentControl.Content = subStatus;
                    context.ImportList.Clear();

                };
                worker.RunWorkerAsync();
            }
            else
            {
                // MessageBox.Show("Please enter all the required fields.");
            }
        }
        
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CmbDocType.SelectedIndex = -1;
            CmbDocDetail.SelectedIndex = -1;
            CmbStartActivity.SelectedIndex = -1;          
            CmbPractice.SelectedIndex = -1;
            CmbDivision.SelectedIndex = -1;
            CmbServiceID.SelectedIndex = -1;           
            context.OnCancel();

        }

      

               
}

}
