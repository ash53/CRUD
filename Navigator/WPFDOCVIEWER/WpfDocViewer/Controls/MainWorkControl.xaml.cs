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
using System.ComponentModel;
using System.Threading;
using System.Data;
using WpfDocViewer.Model;




namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Interaction logic for MainWorkControl.xaml
    /// </summary>
    public partial class MainWorkControl : UserControl
    {
        ViewModel.MainWorkViewModel context;
        StartWorkSubmission workSub = new StartWorkSubmission();
      

        //public ContentControl ParentControl { get; set; }
        public MainWorkControl()
        {
            InitializeComponent();
            context = new ViewModel.MainWorkViewModel();
            this.DataContext = context;          
        }

        private void StartWork_Click(object sender, RoutedEventArgs e)
        {
            context.Validate();
            if (context.validationErrors.Count == 0)
            {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                context.OnStartWork();
            };
            worker.RunWorkerCompleted += (s, args) =>
            {
               
                workSub.MyParentControl = this.WorkContentControl;
                this.WorkContentControl.Content = null;
                this.WorkContentControl.Content = workSub;
            };

            worker.RunWorkerAsync();
            }
            else
            {
                // MessageBox.Show("Please enter all the required fields.");
            }
        }

     

        private void ShowDataByDocNo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject src = (DependencyObject)(e.OriginalSource);
            while (!(src is Control)  && (VisualTreeHelper.GetParent(src) != null))
            {               
                src = VisualTreeHelper.GetParent(src);              
                    if ((src.GetType().Name == "DataGridColumnHeader") || (src.GetType().Name == "Thumb") || (src.GetType().Name == "ScrollViewer") || (src.GetType().Name == "RepeatButton"))
                        return;
                
            }
            context.OnView();

            
               
        }



       

    }
    



}
