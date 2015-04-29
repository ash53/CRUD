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

namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Interaction logic for MainImportControl.xaml
    /// </summary>
    public partial class MainImportControl : UserControl
    {
        ImportControl child1 = new ImportControl();
     
       
        public MainImportControl()
        {
            InitializeComponent();        
            this.ImportContentControl.Content = child1;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (child1.ImportListView.Items.Count == 0)
            {
                child1.LblError.Visibility = Visibility.Visible;
            }
            else
            {
                Next.Visibility = Visibility.Collapsed;
                ImportDocumentDetails child2 = new ImportDocumentDetails();
                child2.ParentControl = this.ImportContentControl;
                this.ImportContentControl.Content = null;
                this.ImportContentControl.Content = child2;
            }
            

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Next.Visibility = Visibility.Visible;    
            this.ImportContentControl.Content = null;
            this.ImportContentControl.Content = child1;
        }
    }
}
