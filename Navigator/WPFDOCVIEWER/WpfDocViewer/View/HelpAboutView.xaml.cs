using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Rti;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceProxies;


namespace WpfDocViewer.View
{
    /// <summary>
    /// Interaction logic for HelpAboutView.xaml
    /// </summary>
    public partial class HelpAboutView : Window
    {
        public HelpAboutView()
        {
            InitializeComponent();
            var version = AssemblyName.GetAssemblyName("WpfDocViewer.exe").Version;
            txtVersion.Text = version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + "." + version.Revision.ToString();
            // FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("WpfDocViewer.exe");
           // txtVersion.Text = versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart + "." + versionInfo.FileBuildPart + "." + versionInfo.FilePrivatePart;
            //framework version
           // txtVersion.Text = App.ResourceAssembly.ImageRuntimeVersion;
           /* txtVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major + "."
                + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build + "."
                + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision ;
            
            **/
            
            ServerVersion();
        }

        public void ServerVersion()
        {   
            using (var administrationService = new AdministrationServiceClient(Constants.AdministrationServiceURL, Constants.AdministrationServiceENDPOINT_NAME))
            {
                var serviceInfo = administrationService.GetServiceInfo(Environment.MachineName, Environment.UserName);
                txtServerName.Text = serviceInfo.MachineName;
                txtServiceVersion.Text = serviceInfo.ServiceBuildTime.ToString();
                txtStartTime.Text = serviceInfo.ServiceStartTime.ToString();

            }

        }
    }
}
