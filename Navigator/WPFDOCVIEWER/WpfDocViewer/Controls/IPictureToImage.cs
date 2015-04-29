using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WpfDocViewer.Controls
{
    public class ImageConverter : System.Windows.Forms.AxHost
    {
        public ImageConverter()
            : base("59EE46BA-677D-4d20-BF10-8D8067CB8B33")
        {
        }

        public static stdole.IPictureDisp ImageToIpicture(System.Drawing.Image image)
        {
            return (stdole.IPictureDisp)ImageConverter.GetIPictureDispFromPicture(image);
        }

        public static System.Drawing.Image IPictureToImage(stdole.StdPicture picture)
        {
            return ImageConverter.GetPictureFromIPicture(picture);
        }
    } 
}
