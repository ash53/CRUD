using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Collections.Generic;


namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        ViewModel.SearchAccounts context;
        public SearchControl()
        {
            InitializeComponent();
            context = new ViewModel.SearchAccounts();
            this.DataContext = context;

        }

        // Clear default asterisk from text box on click
        private void SearchBy_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox sb = (TextBox) sender;
            if(sb.Text == "*")
           {
                    sb.Text = string.Empty;
           }
            sb.GotFocus -= SearchBy_GotFocus;
        }

        private void SearchBy_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["SearchValue"] = SearchBy.Text;
            if (context != null)
            {
                context.SearchValue = SearchBy.Text;
            }
        }

        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                resources["SearchType"] = SearchTypeComboBox.SelectedItem.ToString();
                if (context != null)
                {
                    context.SearchType = SearchTypeComboBox.SelectedItem.ToString();
                    SearchBy.ItemsSource = null;
                    SearchBy.ItemsSource = context.SearchList1;

                }
                if (SearchTypeComboBox.SelectedItem.ToString().Equals("Doctype"))
                {
                    this.SearchTypeComboBox2.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.SearchTypeComboBox2.Visibility = System.Windows.Visibility.Hidden;
                   // PracticeLookup.Visibility = System.Windows.Visibility.Hidden;
                    SearchBy2.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch(Exception ex)
            {
                //if its a null reference continue. object is not loaded yet.
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
       
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["SearchType"] = SearchTypeComboBox.Text;

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

        private void IDMResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject src = (DependencyObject)(e.OriginalSource);
            while (!(src is Control) && (VisualTreeHelper.GetParent(src) != null))
            {
                src = VisualTreeHelper.GetParent(src);
                if ((src.GetType().Name == "DataGridColumnHeader") || (src.GetType().Name == "Thumb") || (src.GetType().Name == "ScrollViewer") || (src.GetType().Name == "RepeatButton"))
                    return;
            }   
            context.OnIDMView();
           // context.OnEIQView();
        }

        private void SearchTypeComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (context != null)
            {
                context.SearchType2 = SearchTypeComboBox2.SelectedItem.ToString();
                SearchBy2.ItemsSource = null;
                SearchBy2.ItemsSource = context.SearchList2;
            }
            if(SearchTypeComboBox2.SelectedItem == "Docdet")
            {
               // PracticeLookup.Visibility = System.Windows.Visibility.Hidden;
                SearchBy2.Visibility = System.Windows.Visibility.Visible;
            }
            else if(SearchTypeComboBox2.SelectedItem == "Practice")
            {
               // PracticeLookup.Visibility = System.Windows.Visibility.Visible;
                SearchBy2.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
               // PracticeLookup.Visibility = System.Windows.Visibility.Hidden;
                SearchBy2.Visibility = System.Windows.Visibility.Hidden;
                if (context != null)
                {
                    context.SearchValue2 = "";
                }
            }
        }

        private void SearchBy2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (context != null)
            {
                context.SearchValue2 = SearchBy2.Text;
            }
        }

        private void SaveColOrder_Click(object sender, RoutedEventArgs e)
        {
            string buildSaveOrder = "";
            int originalOrder = 0;
           
            var columns = EmbillzResults.Columns.AsEnumerable().OrderBy(p => p.DisplayIndex);
            foreach (DataGridColumn cols in columns)
            {
                if (buildSaveOrder.Length > 0)
                {
                    buildSaveOrder = buildSaveOrder + ",";
                }
                buildSaveOrder = buildSaveOrder + cols.Header;
                originalOrder++;
            }
            var DataSource = EmbillzResults.Columns.AsEnumerable().Where(abc => abc.Header.ToString().Equals("client")).FirstOrDefault();
            if (DataSource == null)
            {
                Properties.Settings.Default["SaveEmbillzColumnOrder"] = buildSaveOrder;
            }
            else
            {
                Properties.Settings.Default["SaveNavC2PColumnOrder"] = buildSaveOrder;
            }
           
            Properties.Settings.Default.Save();
        }

        private void EmbillzResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject src = (DependencyObject)(e.OriginalSource);
            while (!(src is Control) && (VisualTreeHelper.GetParent(src) != null))
            {
                src = VisualTreeHelper.GetParent(src);
                if ((src.GetType().Name == "DataGridColumnHeader") || (src.GetType().Name == "Thumb") || (src.GetType().Name == "ScrollViewer") || (src.GetType().Name == "RepeatButton"))
                    return;
            }         
            context.OnEIQView();


        }

        private void IDMResults_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
        }

        private void EmbillzResults_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (context.ShouldSearchContinue)
            {
                if (EmbillzResults.SelectedIndex > 250)
                {
                    
                      //  context.OnSearchMore();

                    
                }
            }
        }

       

       




       
    }
}
