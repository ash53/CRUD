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
    /// Interaction logic for ImportControl.xaml
    /// </summary>
    public partial class MyWork : UserControl
    {
        private ViewModel.MyWorkViewModel context;
        private GridLength _rememberWidth = GridLength.Auto;

        public MyWork()
        {
            InitializeComponent();
            context = new ViewModel.MyWorkViewModel();
            this.DataContext = context;
        }

        private void Grid_Expanded(object sender, RoutedEventArgs e)
        {
           // var grid = sender as Grid;
           // if (grid != null)
           // {
          //      grid.ColumnDefinitions[1].Width = _rememberWidth;
           // }
        }

        private void Grid_Collapsed(object sender, RoutedEventArgs e)
        {
           // var grid = sender as Grid;
           // if (grid != null)
           // {
          //      _rememberWidth = grid.ColumnDefinitions[1].Width;
          //      grid.ColumnDefinitions[1].Width = GridLength.Auto;
          //  }
        }
    }
}
