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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfDocViewer.Model;
using System.Windows.Media.Animation;
using System.Timers;
using System.Collections.ObjectModel;
using Rti;
using System.Windows.Controls.Primitives;


namespace WpfDocViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Author: Mark Lane
    /// Main starting point for this WPF MVVM project
    /// </summary>
    public partial class MainWindow : Window
    {
        View.DocumentViewer doc;
        ViewModel.SearchAccounts searchAccounts;
        ViewModel.MainWindowViewModel context;
   


        public MainWindow()
        {
            //Mark Lane use a Try Catch with the InitializeComponent in order to find the exact item in the xaml with the error.
            //A first chance exception of type 'System.Windows.Markup.XamlParseException' occurred in PresentationFramework.dll
            try
            {
                InitializeComponent();
                context = new ViewModel.MainWindowViewModel();
                this.DataContext = context;
                Controls.IDMLogin idmLogin = new Controls.IDMLogin();
                this.MainWindowsFormsHost1.Child = idmLogin;
                this.MainWindowsFormsHost1.Margin = new Thickness(0, 35, 0, 0);
                idmLogin.Login(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");

                //doc = new View.DocumentViewer(PermissionsModel.Permissions.IDMPermissions.IDMServer, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                //doc.Visibility = System.Windows.Visibility.Hidden;
               // Controls.SplashScreenControl loadcontrol = new Controls.SplashScreenControl();
               // contentControl1.Content = loadcontrol;
                //build the app.xaml's PracticeList
                context.LoadPracticeList();
                var version = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version;
            
                VersionInfo.Content = "Version : " + version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + "." + version.Revision.ToString();
               // ExpandCollapseToggle.IsChecked = false;  //history collapsed to start
               // processMangerExpander.IsExpanded = false; //hide history items
            
              
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error on InitializeComponent: " + ex.Message + " inner: " + ex.InnerException);
            }
           

        }     

        public void FindAndEnable(string source)
        {
            if (contentControl1.Content == null)
            {
               context.ContentControlOne = GetUserControl(source);
                
                this.Expander1.Visibility = System.Windows.Visibility.Visible;
                this.Expander1.IsExpanded = true;
                contentControl1.Content = context.ContentControlOne;
                this.headerText1.Text = source;
            }
            else if (contentControl2.Content == null)
            {
                context.ContentControlTwo = GetUserControl(source);
                this.Expander2.Visibility = System.Windows.Visibility.Visible;
                this.Expander1.IsExpanded = false;
                this.Expander2.IsExpanded = true;
                contentControl2.Content = context.ContentControlTwo;
                this.headerText2.Text = source;
            }
            else if (contentControl3.Content == null)
            {
                context.ContentControlThree = GetUserControl(source);
                this.Expander3.Visibility = System.Windows.Visibility.Visible;
                this.Expander1.IsExpanded = false;
                this.Expander2.IsExpanded = false;
                this.Expander3.IsExpanded = true;
                contentControl3.Content = context.ContentControlThree;
                this.headerText3.Text = source;
            }
            else if (contentControl4.Content == null)
            {
                context.ContentControlFour = GetUserControl(source);
                this.Expander4.Visibility = System.Windows.Visibility.Visible;
                this.Expander1.IsExpanded = false;
                this.Expander2.IsExpanded = false;
                this.Expander3.IsExpanded = false;
                this.Expander4.IsExpanded = true;
                this.headerText4.Text = source;
                contentControl4.Content = context.ContentControlFour;
            }
            else if (contentControl5.Content == null)
            {
                context.ContentControlFive = GetUserControl(source);
                this.Expander5.Visibility = System.Windows.Visibility.Visible;
                this.Expander1.IsExpanded = false;
                this.Expander2.IsExpanded = false;
                this.Expander3.IsExpanded = false;
                this.Expander4.IsExpanded = false;
                this.Expander5.IsExpanded = true;
                this.headerText5.Text = source;
                contentControl5.Content = context.ContentControlFive;
            }
            else if (contentControl6.Content == null)
            {
                context.ContentControlFive = GetUserControl(source);
                this.Expander6.Visibility = System.Windows.Visibility.Visible;
                this.Expander1.IsExpanded = false;
                this.Expander2.IsExpanded = false;
                this.Expander3.IsExpanded = false;
                this.Expander4.IsExpanded = false;
                this.Expander5.IsExpanded = false;
                this.Expander6.IsExpanded = true;
                this.headerText6.Text = source;
                contentControl6.Content = context.ContentControlSix;
            }
        }
        public UserControl GetUserControl(string source)
        {
            UserControl getUserControl;

            switch (source)
            {
                case "Find Docs":
                    getUserControl = new Controls.SearchControl();
                    return getUserControl;
                case "Find Work":
                    getUserControl = new Controls.FindWork();
                    return getUserControl;
                case "Match":
                    getUserControl = new Controls.MatchingControl();
                    return getUserControl;
                case "Match Cash":
                    getUserControl = new Controls.MatchingControl();
                    return getUserControl;
                case "Add Docs":
                    getUserControl = new Controls.MainImportControl();
                    return getUserControl;
                case "Assembly":
                    getUserControl = new Controls.AssemblyControl();
                    return getUserControl;
                case "Work":
                    getUserControl = new Controls.SplashScreenControl();
                    return getUserControl;
                case "Add Work":
                    getUserControl = new Controls.MainWorkControl();
                    return getUserControl;
                case "Settings":
                    getUserControl = new Controls.SettingsControl();
                    return getUserControl;
                default:
                    getUserControl = new Controls.AssemblyContentPresenter();
                    return getUserControl;
            }
        }
        /// <summary>
        /// Keep the MainWindow responsible for all open viewer events
        /// 
        /// </summary>
        public void OpenDocumentViewer()
        {
            string ifn = FindResource("SharedIFN").ToString();
            Int16 pageno = Convert.ToInt16(FindResource("SharedPage").ToString());
            if (doc != null)
            {
                doc = null;
                doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                          
                doc.Visibility = System.Windows.Visibility.Visible;
                doc.Show();
                doc.DrawDocument(ifn, pageno);
            }
            else
            {
                doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                doc.Visibility = System.Windows.Visibility.Visible;
                doc.Show();
                doc.DrawDocument(ifn, pageno);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ifn = FindResource("SharedIFN").ToString(); ;
                Int16 pageno = Convert.ToInt16(FindResource("SharedPage").ToString());
                //must login to IDM before any controls are loaded.
                if (doc != null)
                {
                    doc = null;
                    doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                          
                    doc.Visibility = System.Windows.Visibility.Visible;
                    doc.Show();
                    doc.DrawDocument(ifn, pageno);
                }
                else  
                {
                    doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                    doc.Visibility = System.Windows.Visibility.Visible;
                    doc.Show();
                    doc.DrawDocument(ifn, pageno);
                }
            }
            catch(Exception ex)
            {
                //display or handle?
            }
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        //Show the DataGrids for this, hide calendar and get data 
        private void MenuMatchChecks(object sender, MouseButtonEventArgs e)
        {
            //Controls.AssemblyControl loadcontrol = new Controls.AssemblyControl();
            //contentControl1.Content = loadcontrol;
            FindAndEnable("Match");
        }
        //show the calendar
        private void MenuHome(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuImport_Click(object sender, RoutedEventArgs e)
        {
            //Controls.ImportControl loadcontrol = new Controls.ImportControl();
            //contentControl1.Content = loadcontrol;
            //loadcontrol.BrowseForFiles();
            FindAndEnable("Add Docs");

        }

        private void MenuAppend_Click(object sender, RoutedEventArgs e)
        {
            //Controls.ImportControl loadcontrol = new Controls.ImportControl();
            //contentControl1.Content = loadcontrol;
            //loadcontrol.BrowseForFiles();
            FindAndEnable("Add Docs");
        }

        private void MenuSearchAmount_Click(object sender, RoutedEventArgs e)
        {
            //Controls.SearchControl loadcontrol = new Controls.SearchControl();
            //contentControl1.Content = loadcontrol;
            FindAndEnable("Find Docs");
        }

        private void MenuSearchDocno_Click(object sender, RoutedEventArgs e)
        {
            Controls.SearchControl loadcontrol = new Controls.SearchControl();
            contentControl1.Content = loadcontrol;
        }

        private void MenuSearchAccount_Click(object sender, RoutedEventArgs e)
        {
            Controls.SearchControl loadcontrol = new Controls.SearchControl();
            contentControl1.Content = loadcontrol;
        }
        /// <summary>
        /// Special quick search to load the control and execute the command from Main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuQuickSearch_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["SearchType"] = this.QuickSearchBy.Text;
            //DataContext = searchAccounts;

            Controls.SearchControl loadcontrol = new Controls.SearchControl();
            contentControl1.Content = loadcontrol;
           loadcontrol.QuickSearch_Click();
           
        }

        private void QuickSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (QuickSearchValue.Text != "*" || QuickSearchValue.Text != "")
            {
               ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                resources["SearchValue"] = QuickSearchValue.Text;
            }
       
        }

        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
        
        private void RunCheckFind_Click(object sender, RoutedEventArgs e)
        {
           // Controls.AssemblyContentPresenter loadcontrol = new Controls.AssemblyContentPresenter();
           // contentControl1.Content = loadcontrol;
            FindAndEnable("Assembly");
        }

        private void Work_Click(object sender, RoutedEventArgs e)
        {
           // Controls.MyWork loadcontrol = new Controls.MyWork();
           // contentControl1.Content = loadcontrol;
            FindAndEnable("Work");
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
           // Controls.SettingsControl loadcontrol = new Controls.SettingsControl();
           // contentControl1.Content = loadcontrol;
            FindAndEnable("Settings");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit += Current_Exit;
            Application.Current.Shutdown();
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
           //save your app settings friend.
            Application.Current.Shutdown();
            
        }

        private void MyHouse_Click(object sender, RoutedEventArgs e)
        {
           // Controls.MyWork loadcontrol = new Controls.MyWork();
           // contentControl1.Content = loadcontrol;
        }

        private void AssemblyScreen_Click(object sender, RoutedEventArgs e)
        {
           // Controls.MatchingControl loadcontrol = new Controls.MatchingControl();
            //contentControl1.Content = loadcontrol;
            FindAndEnable("Match");
        }

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void WamItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // Controls.MyFolder loadcontrol = new Controls.MyFolder();
            //contentControl1.Content = loadcontrol;
            FindAndEnable("Work");
        }

        private void CheckMatchItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // Controls.MatchingControl loadcontrol = new Controls.MatchingControl();
           // contentControl1.Content = loadcontrol;
            FindAndEnable("Match");
        }

        private void ImportItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Controls.ImportControl loadcontrol = new Controls.ImportControl();
            contentControl1.Content = loadcontrol;
            loadcontrol.BrowseForFiles();
        }

        private void SearchItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Controls.SearchControl loadcontrol = new Controls.SearchControl();
            contentControl1.Content = loadcontrol;
            ExpandExpander();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            //Controls.SearchControl loadcontrol = new Controls.SearchControl();
            //contentControl1.Content = loadcontrol;
            FindAndEnable("Find Docs");
            //FrameworkElement element = new FrameworkElement();
            
           // Animate()
        }
        /// <summary>
        /// Used to animate the controls moving
        /// </summary>
        /// <param name="e"></param>
        private void Animate(FrameworkElement e, UserControl _container)
        {
            DoubleAnimation ani = new DoubleAnimation()
            {
                From = _container.ActualWidth,
                To = 0.0,
                Duration = new Duration(new TimeSpan(0, 0, 10))
            };

            TranslateTransform trans = new TranslateTransform();
            e.RenderTransform = trans;

            trans.BeginAnimation(TranslateTransform.XProperty, ani, HandoffBehavior.Compose);
        }
        private void ExpandExpander()
        {
          //WidenObject(650, TimeSpan.FromSeconds(2));
        }

       

        private void ExpandButton1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               contentControl1.Content = null;
                this.Expander1.IsExpanded = false;
                this.Expander1.Visibility = System.Windows.Visibility.Hidden;
            }
            catch(Exception ex)
            {

            }
        }

        private void ExpandButton5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl5.Content = null;
                this.Expander5.IsExpanded = false;
                this.Expander5.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButton4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl4.Content = null;
                this.Expander4.IsExpanded = false;
                this.Expander4.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButton3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl3.Content = null;
                this.Expander3.IsExpanded = false;
                this.Expander3.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButton2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl2.Content = null;
                this.Expander2.IsExpanded = false;
                this.Expander2.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void FindWork_Click(object sender, RoutedEventArgs e)
        {
            FindAndEnable("Find Work");
        }

        private void ShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            FindAndEnable("Match Cash");
        }

        private void ExpandButtonMin_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView1 = new System.Windows.Window();

            MaxView1.WindowState = System.Windows.WindowState.Minimized;
            MaxView1.Content = context.ContentControlOne;

            MaxView1.Visibility = System.Windows.Visibility.Visible;
            MaxView1.Closing += MaxView_Closing;
            ExpandButtonMax.IsEnabled = false;
            ExpandButtonMin.IsEnabled = false;
            
        }

        private void ExpandButtonMax_Click(object sender, RoutedEventArgs e)
        {
            
            Window MaxView = new System.Windows.Window();
           
            //MaxView.Height = 1024;
            //MaxView.Width = 1280;
            
            MaxView.WindowState = System.Windows.WindowState.Maximized;
            MaxView.Content = context.ContentControlOne;
           
            MaxView.Visibility = System.Windows.Visibility.Visible;
            MaxView.Closing += MaxView_Closing;
            MaxView.SizeChanged += MaxView_SizeChanged;
            MaxView.Show();
            ExpandButtonMax.IsEnabled = false;
            ExpandButtonMin.IsEnabled = false;
            //MaxView.Closed += MaxView_Closed;
          
           // View.MaximizeView newView = new View.MaximizeView();
            //newView.MaximizeControl(contentControl1);
           // newView.Show();
        }

        void MaxView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          
        }

        void MaxView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //context.ContentControlOne = FindAndEnable(context.ContentControlOne.GetType().ToString());
            //this.contentControl1.Content = context.ContentControlOne;

            context.ContentControlOne = GetUserControl(this.headerText1.Text);

            this.Expander1.Visibility = System.Windows.Visibility.Visible;
            this.Expander1.IsExpanded = true;
            this.Expander2.IsExpanded = false;
            this.Expander3.IsExpanded = false;
            this.Expander4.IsExpanded = false;
            this.Expander5.IsExpanded = false;
            this.Expander6.IsExpanded = false;
            contentControl1.Content = context.ContentControlOne;
            this.contentControl1.Visibility = Visibility.Visible;
            ExpandButtonMax.IsEnabled = true;
            ExpandButtonMin.IsEnabled = true;
                     
        }

        void MaxView_ToolTipClosing(object sender, ToolTipEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void MaxView_Closed(object sender, RoutedEventArgs e)
        {
           //return control back to expander.
            //this.contentControl1 
        }

        private void ExpandButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var type = this.contentControl1.Content.GetType();
            if (type.Name.ToString().Equals("Controls.FindWork"))
            {
                this.contentControl1.Content.GetType().GetField("SearchValue", System.Reflection.BindingFlags.NonPublic);
              
            }
        }

        private void RestoreItem2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FindAndEnable(FindResource("History2").ToString());
        }

        private void RestoreItem1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FindAndEnable(FindResource("History1").ToString());
        }

        private void FindAndRestore(object p)
        {
           // throw new NotImplementedException();
        }

        private void RestoreItem3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FindAndEnable(FindResource("History3").ToString());
        }

        private void RestoreItem4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FindAndEnable(FindResource("History4").ToString());
        }

        private void ExpandButtonSave2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpandButtonMin2_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView2 = new System.Windows.Window();

            MaxView2.WindowState = System.Windows.WindowState.Minimized;
            MaxView2.Content = context.ContentControlTwo;

            MaxView2.Visibility = System.Windows.Visibility.Visible;
            MaxView2.Closing += MaxView2_Closing;
            ExpandButtonMax2.IsEnabled = false;
            ExpandButtonMin2.IsEnabled = false;
        }

        private void ExpandButtonMax2_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView2 = new System.Windows.Window();

            MaxView2.Height = 1024;
            MaxView2.Width = 1280;
            MaxView2.WindowState = System.Windows.WindowState.Maximized;
            MaxView2.Content = context.ContentControlTwo;

            MaxView2.Visibility = System.Windows.Visibility.Visible;
            MaxView2.Closing += MaxView2_Closing;
            MaxView2.Show();
            ExpandButtonMax2.IsEnabled = false;
            ExpandButtonMin2.IsEnabled = false;
        }

        void MaxView2_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.ContentControlTwo = GetUserControl(this.headerText2.Text);

            this.Expander2.Visibility = System.Windows.Visibility.Visible;
            this.Expander2.IsExpanded = true;
            this.Expander1.IsExpanded = false;
            this.Expander3.IsExpanded = false;
            this.Expander4.IsExpanded = false;
            this.Expander5.IsExpanded = false;
            this.Expander6.IsExpanded = false;
            contentControl2.Content = context.ContentControlTwo;
            this.contentControl2.Visibility = Visibility.Visible;
            ExpandButtonMax2.IsEnabled = true;
            ExpandButtonMin2.IsEnabled = true;
        }

        private void ExpandButtonClose2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl2.Content = null;
                this.Expander2.IsExpanded = false;
                this.Expander2.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButtonSave3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpandButtonMin3_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView3 = new System.Windows.Window();

            MaxView3.WindowState = System.Windows.WindowState.Minimized;
            MaxView3.Content = context.ContentControlThree;

            MaxView3.Visibility = System.Windows.Visibility.Visible;
            MaxView3.Closing += MaxView3_Closing;
            ExpandButtonMax3.IsEnabled = false;
            ExpandButtonMin3.IsEnabled = false;
        }

        private void ExpandButtonMax3_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView3 = new System.Windows.Window();

            
            MaxView3.WindowState = System.Windows.WindowState.Maximized;
            MaxView3.Content = context.ContentControlThree;

            MaxView3.Visibility = System.Windows.Visibility.Visible;
            MaxView3.Closing += MaxView3_Closing;
            MaxView3.Show();
            ExpandButtonMax3.IsEnabled = false;
            ExpandButtonMin3.IsEnabled = false;
        }

        void MaxView3_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.ContentControlThree = GetUserControl(this.headerText3.Text);

            this.Expander3.Visibility = System.Windows.Visibility.Visible;
            this.Expander3.IsExpanded = true;
            this.Expander1.IsExpanded = false;
            this.Expander2.IsExpanded = false;
            this.Expander4.IsExpanded = false;
            this.Expander5.IsExpanded = false;
            this.Expander6.IsExpanded = false;
            contentControl3.Content = context.ContentControlThree;
            this.contentControl3.Visibility = Visibility.Visible;
            ExpandButtonMax3.IsEnabled = true;
            ExpandButtonMin3.IsEnabled = true;
        }

        private void ExpandButtonClose3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl3.Content = null;
                this.Expander3.IsExpanded = false;
                this.Expander3.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButtonClose4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl4.Content = null;
                this.Expander4.IsExpanded = false;
                this.Expander4.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButtonSave4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpandButtonMin4_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView4 = new System.Windows.Window();

            MaxView4.WindowState = System.Windows.WindowState.Minimized;
            MaxView4.Content = context.ContentControlFour;

            MaxView4.Visibility = System.Windows.Visibility.Visible;
            MaxView4.Closing += MaxView4_Closing;
            ExpandButtonMax4.IsEnabled = false;
            ExpandButtonMin4.IsEnabled = false;
        }

        private void ExpandButtonMax4_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView4 = new System.Windows.Window();

            
            MaxView4.WindowState = System.Windows.WindowState.Maximized;
            MaxView4.Content = context.ContentControlFour;

            MaxView4.Visibility = System.Windows.Visibility.Visible;
            MaxView4.Closing += MaxView4_Closing;
            MaxView4.Show();
            ExpandButtonMax4.IsEnabled = false;
            ExpandButtonMin4.IsEnabled = false;
        }

        void MaxView4_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.ContentControlFour = GetUserControl(this.headerText4.Text);

            this.Expander4.Visibility = System.Windows.Visibility.Visible;
            this.Expander4.IsExpanded = true;
            this.Expander1.IsExpanded = false;
            this.Expander2.IsExpanded = false;
            this.Expander3.IsExpanded = false;
            this.Expander5.IsExpanded = false;
            this.Expander6.IsExpanded = false;
            contentControl4.Content = context.ContentControlFour;
            this.contentControl4.Visibility = Visibility.Visible;
            ExpandButtonMax4.IsEnabled = true;
            ExpandButtonMin4.IsEnabled = true;
        }

        private void ExpandButtonSave5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpandButtonMin5_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView5 = new System.Windows.Window();

            MaxView5.WindowState = System.Windows.WindowState.Minimized;
            MaxView5.Content = context.ContentControlFive;

            MaxView5.Visibility = System.Windows.Visibility.Visible;
            MaxView5.Closing += MaxView5_Closing;
            ExpandButtonMax5.IsEnabled = false;
            ExpandButtonMin5.IsEnabled = false;
        }

        private void ExpandButtonMax5_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView5 = new System.Windows.Window();

           
            MaxView5.WindowState = System.Windows.WindowState.Maximized;
            MaxView5.Content = context.ContentControlFive;

            MaxView5.Visibility = System.Windows.Visibility.Visible;
            MaxView5.Closing += MaxView5_Closing;
            MaxView5.Show();
            ExpandButtonMax5.IsEnabled = false;
            ExpandButtonMin5.IsEnabled = false;
        }

        void MaxView5_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.ContentControlFive = GetUserControl(this.headerText5.Text);

            this.Expander5.Visibility = System.Windows.Visibility.Visible;
            this.Expander5.IsExpanded = true;
            this.Expander1.IsExpanded = false;
            this.Expander2.IsExpanded = false;
            this.Expander3.IsExpanded = false;
            this.Expander4.IsExpanded = false;
            this.Expander6.IsExpanded = false;
            contentControl5.Content = context.ContentControlFive;
            this.contentControl5.Visibility = Visibility.Visible;
            ExpandButtonMax5.IsEnabled = true;
            ExpandButtonMin5.IsEnabled = true;
        }

        private void ExpandButtonClose5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contentControl5.Content = null;
                this.Expander5.IsExpanded = false;
                this.Expander5.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExpandButtonClose6_Click(object sender, RoutedEventArgs e)
        {
            contentControl6.Content = null;
            this.Expander6.IsExpanded = false;
            this.Expander6.Visibility = System.Windows.Visibility.Hidden;
         
        }

        private void ExpandButtonMax6_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView6 = new System.Windows.Window();

         
            MaxView6.WindowState = System.Windows.WindowState.Maximized;
            MaxView6.Content = context.ContentControlSix;

            MaxView6.Visibility = System.Windows.Visibility.Visible;
            MaxView6.Closing += MaxView6_Closing;
            MaxView6.Show();
            ExpandButtonMax6.IsEnabled = false;
            ExpandButtonMin6.IsEnabled = false;
        }

        void MaxView6_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.ContentControlSix = GetUserControl(this.headerText6.Text);

            this.Expander6.Visibility = System.Windows.Visibility.Visible;
            this.Expander6.IsExpanded = true;
            this.Expander1.IsExpanded = false;
            this.Expander2.IsExpanded = false;
            this.Expander3.IsExpanded = false;
            this.Expander4.IsExpanded = false;
            this.Expander5.IsExpanded = false;
            contentControl6.Content = context.ContentControlSix;
            this.contentControl6.Visibility = Visibility.Visible;
            ExpandButtonMax6.IsEnabled = true;
            ExpandButtonMin6.IsEnabled = true;
        }

        private void ExpandButtonMin6_Click(object sender, RoutedEventArgs e)
        {
            Window MaxView6 = new System.Windows.Window();

            MaxView6.WindowState = System.Windows.WindowState.Minimized;
            MaxView6.Content = context.ContentControlSix;

            MaxView6.Visibility = System.Windows.Visibility.Visible;
            MaxView6.Closing += MaxView6_Closing;
            ExpandButtonMax6.IsEnabled = false;
            ExpandButtonMin6.IsEnabled = false;
           
        }

        private void RestoreItem5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FindAndEnable(FindResource("History5").ToString());
        }

        private void RestoreItem6_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FindAndEnable(FindResource("History6").ToString());
        }

        private void AddWork_Click(object sender, RoutedEventArgs e)
        {
            FindAndEnable("Add Work");
        }

        private void RibbonButton_Click_2(object sender, RoutedEventArgs e)
        {
            View.HelpAboutView helpme = new View.HelpAboutView();
            helpme.Show();
        }

        private void HelpMe_Click(object sender, RoutedEventArgs e)
        {
            View.HelpAboutView helpme = new View.HelpAboutView();
            helpme.Show();
        }

        private void Close_Popup(object sender, RoutedEventArgs e)
        {
            popCartContents.IsOpen = false;
        }


      
    }
}
