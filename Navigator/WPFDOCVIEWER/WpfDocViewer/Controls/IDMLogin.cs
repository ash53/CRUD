using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WpfDocViewer.Controls
{
    /// <summary>
    /// Author: Mark Lane
    /// Viewer's User Control to house all the IDM ActiveX controls
    /// </summary>
    public partial class IDMLogin : UserControl
    {
        public IDMLogin()
        {
            InitializeComponent();
        }

        private void axTTLogin1_LoggedInEvent(object sender, EventArgs e)
        {

        }
        public void Login(string idmServer, string idmUsername, string idmPassword)
        {
            try
            {
                if (this.axTTLogin1.LoggedIn == false)
                {
                    axTTLogin1.LoginEx(idmServer, idmUsername, idmPassword);
                }
                else
                {
                    axTTLogin1.Login(); //piggyback
                }

            }
            catch (Exception ex)
            {
                //capture the error message and number.
                this.axTTLogin1.Login();
            }
        }
    }
}
