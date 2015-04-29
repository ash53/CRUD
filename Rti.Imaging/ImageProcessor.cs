using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Data;
using System.Drawing;
//using TiffDLL50Vic;
using Atalasoft.Imaging;
using Atalasoft.Imaging.Codec.Tiff;
using Atalasoft.Imaging.Codec;
//using Atalasoft.Annotate;
//using Atalasoft.Annotate.UI;
//using Atalasoft.Annotate.Icons;
using Atalasoft.Imaging.Codec.Pdf;
using Atalasoft.Imaging.ImageProcessing.Document;
using Atalasoft.Imaging.ImageProcessing;

namespace Rti.Imaging
{
    public class ImageProcessor
    {



        //[DllImport("Kernel32.dll")]
        //public static extern void Sleep(int ms);
        //private ClsTiffDLL50Class tiffHandle;
        //private TiffDLL100.TiffDLL tiffHandle;
        
        //private ImgAdminClass imageHandle;   

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageProcessor()
        {
            //tiffHandle = new ClsTiffDLL50Class();
            //tiffHandle = new TiffDLL100.TiffDLL();
         
        }

        /// <summary>
        /// Merging a list of files into the first file of the list.
        /// </summary>
        /// <param name="istrPath"></param>
        /// <param name="istrFileList"></param>
        /// <param name="ostrFile"></param>
        /// <param name="ostrRet"></param>
        //public void mergeTIFF(string istrPath, string istrFileList, ref string ostrFile, ref string ostrRet)
        //{
        //    string strTmpFile = "";
        //    string strParams = "";

        //    int intPageCnt = 0;
        //    int intRet = 0;

        //    try
        //    {
        //        ostrRet = "";
        //        if (istrFileList.Split(',').Length > 1)
        //        {
        //            ostrFile = istrPath + "\\" + istrFileList.Split(',')[0].ToString();
        //            if (File.Exists(istrPath + "\\" + ostrFile))
        //            {
        //                File.Delete(istrPath + "\\" + ostrFile);
        //            }

        //            for (int x = 0; x < istrFileList.Split(',').Length; x++)
        //            {
        //                strTmpFile = istrPath + "\\" + istrFileList.Split(',')[x].ToString();
        //                strParams = "in=" + strTmpFile + ";out=info_p";
        //                intPageCnt = tiffHandle.RunTiffDLL(strParams);
        //                for (int y = 1; y < intPageCnt + 1; y++)
        //                {
        //                    strParams = "in=" + strTmpFile + ";pages=" + y.ToString() +
        //                                ";out=" + istrPath + "\\" + ostrFile + ";save=1;format=tif/14;";
        //                    intRet = tiffHandle.RunTiffDLL(strParams);
        //                    if (intRet < 0)
        //                    {
        //                        ostrRet = "Error merging file " + strTmpFile + " (" + intRet.ToString() + ")";
        //                        return;
        //                    }
        //                }
        //                File.Delete(strTmpFile);
        //            }
        //        }
        //        else
        //        {
        //            ostrFile = istrFileList;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ostrRet = "Error merging files " + ex.Message;
        //    }
        //}
     
        /// Convert tiff file into Group 4 single strip format
        /// </summary>
        /// <param name="istrFile"></param>
        /// <param name="ostrRet"></param>
        //public void converttoG411(string istrFile, ref string ostrRet)
        //{
        //    //string strOutPutFile = "";
        //    string strParam = "";
        //    int intRet = 0;

        //    ostrRet = "";
        //    //strOutPutFile =  istrPath + "\\" + istrFile.

        //    strParam = "in=" + istrFile + ";out=" + istrFile + ";format=tif/14;";

        //    intRet = tiffHandle.RunTiffDLL(strParam);
        //    if (intRet < 0)
        //    {
        //        ostrRet = "The convertion for file " + istrFile + " (" + intRet.ToString() + ")";
        //    }
        //    //else
        //    //{
        //    //    ChartMoverDefaults.RtiEventLog( "Successfully converted the TIF file.");
        //    //}

        //}
        /// <summary>
        /// Get page count for a TIF file
        /// </summary>
        /// <param name="istrFile"></param>
        /// <returns></returns>
        //public int getTIFPageCount(string istrFile)
        //{
        //    string strParam = "";
        //    int intRet = 0;
        //    try
        //    {
        //        strParam = "in=" + istrFile + ";out=info_p";
        //        intRet = tiffHandle.RunTiffDLL(strParam);
        //        if (intRet < 0)
        //        {
        //            intRet = 0;
        //        }
        //        return intRet;
        //    }
        //    catch
        //    {
        //        return intRet;
        //    }
        //}
        /// <summary>
        /// Return number of page for a tiff file 
        /// </summary>
        /// <param name="istrSrcFile"></param>
        /// <returns></returns>
        public int getTIFFPageCount(string istrSrcFile)
        {
            int iPageCnt = -1;
            FileStream stm = null;
            TiffDocument tifdoc = null;

            try
            {
                using (stm = new FileStream(istrSrcFile, FileMode.Open, FileAccess.Read))
                {
                    tifdoc = new TiffDocument(stm);
                    iPageCnt = tifdoc.Pages.Count;
                }
                tifdoc = null;
                stm.Close();
                stm = null;

            }
            catch
            {
                iPageCnt = -1;



            }
            finally
            {
                if (tifdoc != null)
                {
                    tifdoc = null;
                }
                if (stm != null)
                {
                    stm.Dispose();
                    stm = null;
                }
            }
            return iPageCnt;





        }
        /// <summary>
        /// Use Atalasoft PDFRasterizer to convert PDF to TIF
        /// Mark Lane completely redesigned the Atalasoft recommended (Stack Overflow poor designed) approach
        /// and created my own architecture for this which gets the high quality Atalasoft conversion plus the 
        /// reliability of a system that can run on and on without crashing in the multithread environment.
        /// Date 12/12/2013
        /// </summary>
        /// <param name="istrFileIn"></param>
        /// <param name="ostrFileOut"></param>
        /// <param name="iintBits"></param>
        /// <param name="ostrRet"></param>
        public void convertPDF2TIF(string istrFileIn, string ostrFileOut, int iintBits, ref int ointNumPages, ref string ostrRet)
        {
            FileStream fsOpen = null;
            AtalaFileStream fsSave = null;
            PdfDecoder pdfdc = null;
            AtalaImage imageRGB = null;
            AtalaImage tmp = null;
            TiffEncoder tiffec = null;
            //ThresholdPixelChanger changer = null;
            ostrRet = "";
            //   ChartMoverDefaults.RtiEventLog("START convertPDF2TIF2: filein: " + istrFileIn + " fileout: " + ostrFileOut, (int)EventLogEntryType.Information);

            try
            {
                if (File.Exists(ostrFileOut))
                {
                    File.Delete(ostrFileOut);
                }
                //Mark Lane stack overflow error research. 10/24/2013
                //this function is always the last place entered the TIF is left in Use for all threads.
                //StackOverflow occurs from an infinite recursion, process keeps adding to the stack. 
                //creating an array that is too large. local objects get too large. using Release mode uses 
                //less data on the stack.

                //initialize licensing
                // ChartMoverDefaults.RtiEventLog("new AtalaImage", (int)EventLogEntryType.Information);

                tmp = new AtalaImage();
                tmp.Dispose();
                tmp = null;
                // ChartMoverDefaults.RtiEventLog("new PdfDecoder", (int)EventLogEntryType.Information);

                pdfdc = new PdfDecoder();
                pdfdc.Resolution = 300;
                //pdfdc.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                pdfdc.RenderSettings.AnnotationSettings = AnnotationRenderSettings.RenderAll;
                //ChartMoverDefaults.RtiEventLog("AtalaImage.PixelFormatChange = DocumentPixelFormatChanger(new AtalaPixelFormatChanger());", (int)EventLogEntryType.Information);
                ////Mark Lane Atalasoft recommends using this (http://www.atalasoft.com/KB/article.aspx?id=10374&cNode=8T0X5H)for black and white as opposed to GetChagnedPixelFormat
                //AtalaImage.PixelFormatChanger = new DocumentPixelFormatChanger(new AtalaPixelFormatChanger());
                PixelFormatChanger _originalChanger = new ThresholdPixelChanger(AtalaImage.PixelFormatChanger);
                //Old way
                //ChartMoverDefaults.RtiEventLog("changer = new thresholdpixelchanger(atalaimage.pixelformatchanger)", (int)EventLogEntryType.Information);
                // changer = new ThresholdPixelChanger(AtalaImage.PixelFormatChanger);
                // if (changer == null)
                // {
                //  //   ChartMoverDefaults.RtiEventLog("error changer = new thresholdpixelchanger(atalaimage.pixelformatchanger)", (int)EventLogEntryType.Information);

                //     throw new Exception("Couldn't create pixel changer.");
                // }
                //// ChartMoverDefaults.RtiEventLog("create atala pixelformatchanger", (int)EventLogEntryType.Information);
                // AtalaImage.PixelFormatChanger = changer;
                //AtalaImage.PixelFormatChanger = null;
                //MLane and Dejan fusing these together without a null check.
                //AtalaImage.PixelFormatChanger = new ThresholdPixelChanger(AtalaImage.PixelFormatChanger);
                // ChartMoverDefaults.RtiEventLog("filestream open file to read", (int)EventLogEntryType.Information);
                //Mark Lane changed file open to be in a USING statement
                using (fsOpen = File.OpenRead(istrFileIn))
                {

                    //fsSave = new AtalaFileStream(ostrFileOut, FileMode.Create, FileAccess.Write);
                    // ChartMoverDefaults.RtiEventLog("create new TiffEncoder", (int)EventLogEntryType.Information);

                    tiffec = new TiffEncoder(TiffCompression.Default);


                    //find number of pages
                    // ChartMoverDefaults.RtiEventLog("get pdfdecoder pagecount", (int)EventLogEntryType.Information);

                    ointNumPages = pdfdc.GetFrameCount(fsOpen);

                    using (fsSave = new AtalaFileStream(ostrFileOut, FileMode.Create, FileAccess.Write))
                    {
                        for (int i = 0; i < ointNumPages; i++)
                        {
                            // ChartMoverDefaults.RtiEventLog("pdfdecoder.read page:" + i, (int)EventLogEntryType.Information);
                            //Mark Lane changing to a Using statement per Atalasoft's website
                            using (imageRGB = pdfdc.Read(fsOpen, i, null))
                            {
                                if (i > 0)
                                    tiffec.Append = true;
                                // ChartMoverDefaults.RtiEventLog("atalafilestream.seek", (int)EventLogEntryType.Information);

                                fsSave.Seek(0, SeekOrigin.Begin);
                                //convert to black and white
                                // ChartMoverDefaults.RtiEventLog("convert2blackandwhite", (int)EventLogEntryType.Information);
                                //MLane: stack overflow occurs right here! Every time. very "brute force" method does not set key properties.
                                //MLane: fixing code to pass new object into separate Atalasoft.image object. will fix the stack overflow.
                                //ChartMoverDefaults.RtiEventLog("pdf smoothingmode:" + pdfdc.SmoothingMode.ToString(), 1);
                                // ChartMoverDefaults.RtiEventLog("pdf height:" + pdfdc.GetImageInfo(fsOpen, i).Size.Height.ToString() + " Width:" + pdfdc.GetImageInfo(fsOpen, i).Size.Width.ToString(), 1);
                                //ChartMoverDefaults.RtiEventLog("pdf ImageType:" + pdfdc.GetImageInfo(fsOpen, i).ImageType.ToString() + " Page:" + i, 1);
                                // ChartMoverDefaults.RtiEventLog("pdf Resolution:" + pdfdc.GetImageInfo(fsOpen, i).Resolution.ToString(), 1);
                                // ChartMoverDefaults.RtiEventLog("pdf ColorDepth:" + pdfdc.GetImageInfo(fsOpen, i).ColorDepth.ToString(), 1);
                                //ChartMoverDefaults.RtiEventLog("pdf ColorDepth:" + pdfdc.GetImageInfo(fsOpen, i).ColorDepth.ToString(), 1);
                                //Mark Lane Rearchitect to be take a linear design in order to build
                                //objects and remove upon use eliminating recursion. 12/12/2013
                                AdaptiveThresholdCommand _threshold = new AdaptiveThresholdCommand();
                                Atalasoft.Imaging.ColorManagement.ColorProfile destProfile = null;
                                AtalaImage newimageRGB = null;
                                PixelFormat targetPixelFormat = PixelFormat.Pixel1bppIndexed;
                                if (targetPixelFormat != PixelFormat.Pixel1bppIndexed ||
                                    !_threshold.IsPixelFormatSupported(imageRGB.PixelFormat))
                                {
                                    //ChartMoverDefaults.RtiEventLog("_originalChanger.ChangePixelFormat", (int)EventLogEntryType.Information);

                                    newimageRGB = _originalChanger.ChangePixelFormat(imageRGB, targetPixelFormat, destProfile);
                                }

                                //MLane added to not throw pixel format exception for this deeply nested function.
                                // ChartMoverDefaults.RtiEventLog("threshold.applytoanypixelformat", (int)EventLogEntryType.Information);
                                _threshold.ApplyToAnyPixelFormat = true;

                                // apply the threshold command to the
                                // source image, are return the resulting image

                                newimageRGB = _threshold.Apply(imageRGB).Image;
                                //AtalaImage newimageRGB = imageRGB;
                                //.GetChangedPixelFormat(PixelFormat.Pixel1bppIndexed);

                                //old way
                                //imageRGB = imageRGB.GetChangedPixelFormat(PixelFormat.Pixel1bppIndexed);
                                //  ChartMoverDefaults.RtiEventLog("tiffencoder.Save", (int)EventLogEntryType.Information);

                                tiffec.Save(fsSave, newimageRGB, null);
                                //  ChartMoverDefaults.RtiEventLog("newImageRGB.dispose", (int)EventLogEntryType.Information);
                                newimageRGB.Dispose();
                                newimageRGB = null;
                                _originalChanger = null;
                                _threshold = null;

                            }


                            imageRGB.Dispose();
                            imageRGB = null;

                        }
                    }
                }
                // ChartMoverDefaults.RtiEventLog("DONE convertPDF2TIF2: filein: " + istrFileIn + " fileout: " + ostrFileOut, (int)EventLogEntryType.Information);

                fsOpen.Close();
                fsOpen = null;
                //fsSave.Flush();
                //fsSave.Close();
                fsSave.Dispose();
                fsSave = null;

                //changer = null;
                tiffec = null;
                pdfdc.Dispose();
                pdfdc = null;
                File.Delete(istrFileIn);  //delete PDF
                //ChartMoverDefaults.RtiEventLog("DELETED Original filein: " + istrFileIn, (int)EventLogEntryType.Information);


            }
            catch (Exception ex)
            {
                ostrRet = "ERROR: convertPDF2TIF2 - " + ex.Message;
                
            }
            finally
            {
                if (fsSave != null)
                {
                    fsSave.Flush();
                    fsSave.Close();
                    fsSave.Dispose();
                    fsSave = null;
                }
                if (fsOpen != null)
                {
                    //fsOpen.Close();
                    fsOpen.Dispose();
                    fsOpen = null;
                }
                if (imageRGB != null)
                {
                    imageRGB.Dispose();
                    imageRGB = null;
                }
                // changer = null;
                tiffec = null;
                if (pdfdc != null)
                {
                    pdfdc.Dispose();
                    pdfdc = null;
                }

                //GC.Collect();
                //GC.WaitForPendingFinalizers();

                if (tmp != null)
                {
                    tmp.Dispose();
                    tmp = null;
                }
            }
        }
        /// <summary>
        /// PixelFormatChanger for Atalasoft PDFRasterizer
        /// </summary>
        public class ThresholdPixelChanger : PixelFormatChanger
        {
            private AdaptiveThresholdCommand _threshold;
            private PixelFormatChanger _originalChanger;

            public ThresholdPixelChanger(PixelFormatChanger originalChanger)
            {
                // we need an original changer to handle things
                // that we can't do.
                if (originalChanger == null)
                {
                    throw new ArgumentNullException("originalChanger",
                        "ThresholdPixelChanger requires a base pixel format changer");
                }
                _originalChanger = null;
                _originalChanger = originalChanger;
                originalChanger = null;
                _threshold = null; //MLane change to set to null since its being created as new each time.
                _threshold = new AdaptiveThresholdCommand();
            }

            protected override AtalaImage LowLevelChangePixelFormat(AtalaImage sourceImage,
                PixelFormat targetPixelFormat,
                Atalasoft.Imaging.ColorManagement.ColorProfile destProfile)
            {
                // if the target pixel format is not 1 bit or
                // if the thresholding command doesn't support the
                // source image pixel format, just pass it on
                try
                {
                    if (_originalChanger != null)
                    {
                        if (targetPixelFormat != PixelFormat.Pixel1bppIndexed ||
                            !_threshold.IsPixelFormatSupported(sourceImage.PixelFormat))
                        {
                            //ChartMoverDefaults.RtiEventLog("_originalChanger.ChangePixelFormat", (int)EventLogEntryType.Information);

                            return _originalChanger.ChangePixelFormat(sourceImage, targetPixelFormat, destProfile);
                        }

                        //MLane added to not throw pixel format exception for this deeply nested function.
                        // ChartMoverDefaults.RtiEventLog("threshold.applytoanypixelformat", (int)EventLogEntryType.Information);
                        _threshold.ApplyToAnyPixelFormat = true;

                        // apply the threshold command to the
                        // source image, are return the resulting image
                        // ChartMoverDefaults.RtiEventLog("AdaptiveThreshold _threshold.Apply", (int)EventLogEntryType.Information);

                        // return _threshold.Apply(sourceImage).Image;
                    }
                }
                catch (Exception err)
                {

                }

                return _threshold.Apply(sourceImage).Image;
            }

            //  public PixelFormatChanger OriginalChanger { get { return _originalChanger; } }
        }
  

        /// <summary>
        /// convert TIFF raw G3 to TIFF G4 
        /// </summary>
        /// <param name="istrFileName"></param>
        /// <param name="ostrRet"></param>
        public void convertTG3toTG4(string istrFileName, ref string ostrRet)
        {
            System.IO.FileStream fs = null;
            System.Drawing.Image OriginalImage = null;
            System.Drawing.Imaging.FrameDimension _OriginalImgDimension = null;
            Atalasoft.Imaging.Workspace ws = null;
            Atalasoft.Imaging.AtalaImage ai = null;
            Atalasoft.Imaging.ImageProcessing.Channels.InvertCommand inv = null;
            ostrRet = "";
            try
            {
                fs = new FileStream(istrFileName, System.IO.FileMode.Open);

                OriginalImage = System.Drawing.Image.FromStream(fs); //FromFile("C:\\temp\\EDMBILL_ACCT_V00104526611_DATE_SERVICE_07_24_09.tif");

                _OriginalImgDimension = new System.Drawing.Imaging.FrameDimension(OriginalImage.FrameDimensionsList[0]);

                ws = new Atalasoft.Imaging.Workspace();

                for (int i = 0; i < OriginalImage.GetFrameCount(_OriginalImgDimension); i++)
                {

                    OriginalImage.SelectActiveFrame(_OriginalImgDimension, i);

                    //FinalImage.SaveAdd(OriginalImage, PageFrame);

                    ai = Atalasoft.Imaging.AtalaImage.FromBitmap((Bitmap)OriginalImage.Clone());

                    if (ai.PixelFormat == Atalasoft.Imaging.PixelFormat.Pixel1bppIndexed)
                    {

                        // See if the 0 palette entry is less than the 1 palette entry.

                        Color p0 = ai.Palette.GetEntry(0);

                        Color p1 = ai.Palette.GetEntry(1);



                        if ((p0.R << 16 + p0.G << 8 + p0.B) < (p1.R << 16 + p1.G << 8 + p1.B))
                        {

                            // Swap the palette entries.

                            ai.Palette.SetEntry(0, p1);

                            ai.Palette.SetEntry(1, p0);



                            // Invert the image data.

                            inv = new Atalasoft.Imaging.ImageProcessing.Channels.InvertCommand();

                            inv.Apply(ai);

                        }
                    }
                    ws.Images.Add(ai);

                }

                //_OriginalImgDimension = null;
                fs.Close();
                fs.Dispose();
                fs = null;
                OriginalImage.Dispose();
                OriginalImage = null;
                ws.Save(istrFileName, new Atalasoft.Imaging.Codec.TiffEncoder(Atalasoft.Imaging.Codec.TiffCompression.Group4FaxEncoding));
                inv = null;
                ai.Dispose();
                ai = null;
                ws.Dispose();
                ws = null;
                _OriginalImgDimension = null;

            }
            catch (Exception ex)
            {
                ostrRet = ex.Message;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                if (OriginalImage != null)
                {
                    OriginalImage.Dispose();
                    OriginalImage = null;
                }
                if (inv != null)
                {
                    inv = null;
                }
                if (ai != null)
                {
                    ai.Dispose();
                    ai = null;
                }
                if (ws != null)
                {
                    ws.Dispose();
                    ws = null;
                }
                if (_OriginalImgDimension != null)
                {
                    _OriginalImgDimension = null;
                }
            }
        }

        /// <summary>
        /// Destuctor
        /// </summary>
        ~ImageProcessor()
        {
            //tiffHandle = null;
          
            //this.Cleanup();
        }

    }
}

