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
using System.Windows.Shapes;
using Rti.EncryptionLib;

namespace WpfDocViewer.View
{
    /// <summary>
    /// Interaction logic for LoginPrompt.xaml
    /// </summary>
    public partial class LoginPrompt : Window
    {
        ViewModel.LoginViewModel context;
        public LoginPrompt()
        {
            InitializeComponent();
            context = new ViewModel.LoginViewModel();
            DataContext = context;
            txtUsername.Text = Environment.UserName;
            txtDomain.Text = Environment.UserDomainName;
            txtPassword.Focus();
            txtPassword.KeyUp += new KeyEventHandler(txtPassword_KeyUp);
        }

        void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginServer();
            }
        }
        /// <summary>
        /// Mark Lane
        /// For security reasons binding is disabled for passwords. Codebehind must be used to connect to RPC.
        /// </summary>
        /// Otherwise the path should be-> LoginView --> LoginViewViewModel --> LoginModel --> MainWindowViewModel --> MainWindowView
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //
            LoginServer();

        }
        private void LoginServer()
        {
            EncryptionFuncs encryptpass = new EncryptionFuncs();
            context.Username = txtUsername.Text;
            context.Password = txtPassword.Password;
            Model.PermissionsModel.Permissions.EncodedPassword = encryptpass.TripleDESEncode(context.Password);
            context.DomainName = Environment.UserDomainName;
            if (context.LoginApproved())
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Invalid Username or Password");
            }
            context.Password = "";
            txtPassword.Password = "";
        }
        private void txtDomain_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtDomain.Text = Environment.UserDomainName;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
