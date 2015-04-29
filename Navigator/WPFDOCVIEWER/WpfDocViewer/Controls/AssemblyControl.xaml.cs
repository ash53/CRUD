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
    /// Interaction logic for AssemblyControl.xaml
    /// </summary>
    public partial class AssemblyControl : UserControl
    {
        ViewModel.AssemblyViewModel context;
     
        public AssemblyControl()
        {
            InitializeComponent();
            TreeRoot[] treeRoots = AssemblyDataAccessLayer.GetTreeRoots();
  
            context = new ViewModel.AssemblyViewModel(treeRoots);
            DataContext = context;
        }

        private void ChecksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void DrawChildDocument(string docno, string ifn, short pageno)
        {

        }

        private void CheckOrRemit_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            double redcolor = 200.00 + CheckOrRemit.Value;
            double greencolor = 300 + CheckOrRemit.Value;
            double bluecolor = 200 + CheckOrRemit.Value;
            if (CheckOrRemit.Value == 0) { redcolor = 105; greencolor = 150; bluecolor = 209; }
            if (CheckOrRemit.Value == 1) { redcolor = 178; greencolor = 209; bluecolor = 207; }
            Color color = Color.FromRgb((byte)redcolor, (byte)greencolor, (byte)bluecolor);
            this.Background = new SolidColorBrush(color);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
           //add the Remit to the List
            context.AddRemit(Button.ContentProperty.ToString());
        }
    }
}
