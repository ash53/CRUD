using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rti.Imaging;

namespace RACServicesUnitTests
{
    [TestClass]
    public class RACTests7
    {
        [TestMethod]
        public void LocalConvertPDF2TIF()
        {
            string message = "";
            string sourcePath = @"C:\temp\pdf";
            string sourceFile = "testfile.pdf";
            string destFile = "testfile.pdf";
            string tifFile = "testfile.tif";
            int numofpages = 1;
            string source = System.IO.Path.Combine(sourcePath, sourceFile);
            string dest = System.IO.Path.Combine(sourcePath, destFile);
            string tif = System.IO.Path.Combine(sourcePath, tifFile);

            //System.IO.File.Delete(tif);
            //System.IO.File.Copy(source, dest, true);

            var imagingClient = new Rti.Imaging.ImageProcessor();
            DirectoryInfo dir = new DirectoryInfo(@"C:\temp\pdf");
            foreach (FileInfo fi in dir.GetFiles())
            {
                imagingClient.convertPDF2TIF(fi.FullName,
                    fi.FullName + "tif", 1, ref numofpages, ref message);
            }
            var result = message;
                Assert.IsNotNull(result);
                Debug.Print("Message:{0}", message);
                Debug.Print("result:{0}", result.ToString());
                
                Assert.IsTrue(System.IO.File.Exists(tif), "TIF file not created");
            
        }
    }
}
