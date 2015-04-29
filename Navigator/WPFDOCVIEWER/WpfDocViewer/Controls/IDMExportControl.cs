using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfDocViewer.Controls
{
    public partial class IDMExportControl : UserControl
    {
        string IDMServer;
        string IDMUsername;
        string IDMPassword;

        public IDMExportControl()
        {
            InitializeComponent();
            
        }
        public void Login(string idmServer, string idmUsername, string idmPassword)
        {
            IDMServer = idmServer;
            IDMUsername = idmUsername;
            IDMPassword = idmPassword;
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
        public string ExportImage(string ifn, string exportedDocno)
        {
            string path = "";
            string userExportedFilename = @"C:\Users\" + Environment.UserName + @"\" + exportedDocno + ".tif";
            try
            {
                axTTDocOp1.IFN = ifn;
                axTTDocOp1.CurPage = 1;
                if(System.IO.File.Exists(userExportedFilename))
                {
                    System.IO.File.Delete(userExportedFilename);
                }
                axTTDocOp1.ExportEx(ifn, 1, axTTDocOp1.TowerPages, userExportedFilename, 1);
                path = axTTDocOp1.ExportedFileNames.Split(';')[0] + axTTDocOp1.ExportedFileNames.Split(';')[1];
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("being used"))
                {
                    MessageBox.Show("File Is In Use. " + userExportedFilename + " or you already exported this image.");
                }
                else
                {
                throw new Exception("ExportImage error: " + ex.Message.ToString());
                }
            }
            
            return path;

        }
    }
}
