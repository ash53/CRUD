using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DocumentViewer.Properties;

namespace DocumentViewer
{
    public partial class IDMViewer : Form
    {
        public string IDMServer;
        public string Username;
        public string Password;

        public IDMViewer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Author: Mark Lane
        /// main function for displaying images
        /// </summary>
        /// <param name="ifn"></param>
        /// <param name="pageno"></param>
        public void DrawDoc(string ifn, Int16 pageno)
        {
            try
            {
                axTTDoc1.IFN = ifn;
                axTTDoc1.Page = pageno;
                axTTDoc1.ScaleToGrey = true;
                axTTDoc1.Refresh();
                
            }
            catch (Exception)
            {
                Thread.Sleep(500);
                RecoverMiscell(ifn, pageno);
            }
        }

        private void SetViewerFeatures(string ifn, Int16 pageno)
        {
            
        }
        /// <summary>
        /// Author: Mark Lane
        /// used in Postbilling to rotate the document for banding.
        /// </summary>
        /// <param name="ifn"></param>
        /// <param name="pageno"></param>
        /// <param name="orientation"></param>
        public void DrawDoc(string ifn, Int16 pageno, Int16 orientation)
        {
            try
            {
                //to avoid
                axTTDoc1.IFN = ifn;
                axTTDoc1.Page = pageno;
                axTTDoc1.Orientation = orientation;
                axTTDoc1.Refresh();
                axTTDoc1.ScaleToGrey = true;
            }
            catch (Exception)
            {
                Thread.Sleep(500);
                RecoverMiscell(ifn, pageno);
            }
        }
        /// <summary>
        /// Author: Mark Lane
        /// recovers the ttdocop control and the miscell process.
        /// </summary>
        /// <param name="ifn"></param>
        /// <param name="pageno"></param>
        private void RecoverMiscell(string ifn, Int16 pageno)
        {
            try
            {
                axTTLogin1.Logout();
                Thread.Sleep(2000);
                axTTLogin1.LoginEx(IDMServer, Username, Password);
                Thread.Sleep(500);
            }
            catch (Exception)
            {
                axTTLogin1.Login();
            }
        }

        public void IdmLogin(string servername, string username, string password)
        {
            try
            {
                axTTLogin1.LoginEx(servername, username, password);
            }
            catch (Exception ex)
            { 
               axTTLogin1.Login();
            }
            finally
            {
                if (axTTLogin1.LoggedIn)
                {
                    Username = axTTLogin1.User.LongName;
                    Password = password;
                    IDMServer = servername;
                }
                else
                {
                    string errmsg = axTTLogin1.TowerErrorString;
                    MessageBox.Show("Error Logging In to IDM :" + servername + " username: " + username + " TowerError:" +
                                    errmsg);
                }
            }
        }
        /// <summary>
        /// Author: Mark Lane
        /// get the location of the last position saved and reuse it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDMViewer_Load(object sender, EventArgs e)
        {
            //anything special?
            if (!Settings.Default.StoreSize.IsEmpty)
            {
                Size = Settings.Default.StoreSize;
            }
            if (!Settings.Default.LastLocation.IsEmpty)
            {
                Location = Settings.Default.LastLocation;
            }
        }
    }
}
