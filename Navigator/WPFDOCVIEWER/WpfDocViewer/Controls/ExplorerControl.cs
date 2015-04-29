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
    public partial class ExplorerControl : UserControl
    {
        public ExplorerControl()
        {
            InitializeComponent();
            this.openFileDialog1.FileOk += openFileDialog1_FileOk;
        }

        public void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //select to save
        }


        public string[] BrowseFiles()
        {
            string[] filelist = new string[1];
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            openFileDialog1.FilterIndex = 2;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileNames;
            }
            else
            {
                return filelist;
            }
        }
        
        public object OpenFile { get; set; }
    }
}
