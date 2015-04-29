using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DocumentViewer;

namespace Messenger
{
    public partial class Messenger : Form, DocumentViewer.TcpListener
    {
       public Messenger()
        {
            InitializeComponent();
            BeginListen();
        }

        private void BeginListen()
        {
           
            
        }
        
    }

}
