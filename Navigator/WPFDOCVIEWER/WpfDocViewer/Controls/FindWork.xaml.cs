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
using GalaSoft.MvvmLight.Command;



namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class FindWork : UserControl
    {
        ViewModel.FindWorkViewModel context;
        public FindWork()
        {
            InitializeComponent();
            context = new ViewModel.FindWorkViewModel();
            this.DataContext = context;
          
        }
        /// <summary>
        /// Mark Lane
        /// The search criteria in FindWork should not update the global resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBy_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            //resources["SearchValue"] = SearchBy.Text;
        }

        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cboName.ItemsSource = (from p in persons where not string.IsNullOrEmpty(p.name) select p.Name Distinct)
            //ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            //resources["SearchType"] = SearchTypeComboBox.SelectedItem.ToString();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
       
           // ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
           // resources["SearchType"] = SearchTypeComboBox.Text;

        }
       
        /// <summary>
        /// Used for MainWindow Ribbon to execute a Quick Search as opposed to letting the UserControl and ViewModel handle this in traditional MVVM.
        /// </summary>
        public void QuickSearch_Click()
        {
            context.OnSearch();
        }
        /// <summary>
        /// Debating handling in the CodeBehind or in the ViewModel.  
        /// ViewModel first.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDMResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowareResults_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void FlowareResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        { 
            DependencyObject src = (DependencyObject)(e.OriginalSource);
            while (!(src is Control) && (VisualTreeHelper.GetParent(src) != null))
            {
                src = VisualTreeHelper.GetParent(src);
                if ((src.GetType().Name == "DataGridColumnHeader") || (src.GetType().Name == "Thumb") || (src.GetType().Name == "ScrollViewer") || (src.GetType().Name == "RepeatButton"))
                    return;
            }   
            context.OnView();
        }

       
        private void SaveColOrder_Click(object sender, RoutedEventArgs e)
        {
            string buildSaveOrder = "";
            int originalOrder = 0;
            var columns = FlowareResults.Columns.AsEnumerable().OrderBy(p => p.DisplayIndex);
            foreach (DataGridColumn cols in columns)
            {
                if (buildSaveOrder.Length > 0)
                {
                    buildSaveOrder = buildSaveOrder + ",";
                }
                buildSaveOrder = buildSaveOrder + cols.Header;
                originalOrder++;
            }
            Properties.Settings.Default["SaveColumnOrder"] = buildSaveOrder;
            Properties.Settings.Default.Save();
        }

      
      
        private void Close_Note(object sender, RoutedEventArgs e)
        {
            //Clsoe window
            pop1.IsOpen = false;

        }
        /// <summary>
        /// Mark Lane kick off the prefetch as user scrolls to a specific point
        /// half way grab first prefetch
        /// at the bottom grab more.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private double ScrollTo50PercentofNew500(double Height)
        {
            double returnScroll = context.SearchesRun * 500;
            double RightPercent = 0;
            double SetTo50PercentofNew = (context.SearchesRun - 0.5) * 500; //half the number of the next search
            //get the physical height divide that by the number of rows inside.
            RightPercent = (SetTo50PercentofNew) * (Height /returnScroll);

            return RightPercent;

           // returnScroll 
        }
        private void FlowareResults_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
            if (context.ShouldSearchContinue)
            {
                //if (FlowareResults.Items.Count.ToString().EndsWith("88"))
               // {
                    var scrollBar = FlowareResults.Descendents()
                          .OfType<ScrollViewer>().Where(sb => sb.Name == "DG_ScrollViewer")
                          .FirstOrDefault();
                    if (scrollBar != null) //System.Windows.Forms.ScrollBar
                    {
                        //var scrollvar = System.Reflection.TypeInfo.GetType(ScrollViewer).GetEvents(FlowareResults).ToDictionary<Name, Value>();
                        
                            double MidHeight = ScrollTo50PercentofNew500(scrollBar.ScrollableHeight);
                            if (MidHeight > 0)
                            {
                                if (scrollBar.VerticalOffset >= MidHeight)
                                {

                                    context.OnSearchMore();
                                }
                            }
                        
                    }
               // }
                
            }
        }

        private void ShoppingCart_Click(object sender, RoutedEventArgs e)
        {
           
        }

       

      
        

       


       

     
       
    }
}
