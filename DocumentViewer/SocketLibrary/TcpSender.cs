using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml;


namespace DocumentViewer
{

    public abstract class TcpSender
    {
        public Socket _sender;
        public Socket _subSender;
        private static bool _connected = false;
        //private static string _userId = System.Windows.Forms.SystemInformation.UserName;
        private static StringBuilder _xmlViewerRequest;
        private static StringBuilder _xmlOut;
        //private System.Windows.Forms.Form oSocketServer;
        static public int TcpListenerPort = 7000;
        IAsyncResult m_result;
        public AsyncCallback m_pfnCallBack;

        IAsyncResult m_resultTwo;
        public AsyncCallback m_pfnCallBackTwo;
        public string PreviousDocno = "";

        #region Constructor

        /// <summary>
        /// Default empty constructor.


        /// </summary>
        public TcpSender()//System.Windows.Forms.Form oMainF)
        {
           // oSocketServer = oMainF;
        }

        #endregion
        #region Destructor
        /// <summary>
        /// Release resources.
        /// </summary>
        ~TcpSender()
        {

        }

        #endregion

        internal static bool Connected
        {
            get
            {
                return _connected;
            }
            set
            {
                _connected = value;
            }
        }

        public static StringBuilder OpenAndRegister()
        {
            string strRegister = "";
            string strPublish = "";
            StringBuilder sbMessage = new StringBuilder();
            strRegister = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><RtiMsg><Header><Dest>embillz</Dest><Source>ContractWizard</Source><UserId>localhost</UserId><Version>1.0</Version><MsgId>1</MsgId><MsgType>Register</MsgType></Header><Register><ID>ContractWizard</ID></Register></RtiMsg>";
            strPublish = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><RtiMsg><Header><Dest>embillz</Dest><Source>ContractWizard</Source><UserId>localhost</UserId><Version>1.0</Version><MsgId>13</MsgId><MsgType>Publish</MsgType></Header><Publish><Message>Action^GetContractInfo</Message></Publish></RtiMsg>";
            sbMessage.AppendFormat("{0}{1}{2}{3}", strRegister.Length.ToString().PadLeft(4, '0'), strRegister, strPublish.Length.ToString().PadLeft(4, '0'), strPublish);
            return sbMessage;
        }

        // MarkLane: rewrote the ParseRecievedBuffer to look and process the ContractInfo xml only. 6/14/2007
        public void ParseReceivedBuffer()
        {


            System.Xml.XmlDocument x = new System.Xml.XmlDocument();
            string findChar = _xmlViewerRequest.ToString();
            int findit = findChar.IndexOf("<", 0, findChar.Length);
            //ML:a second xml is processed by the OnDataRecieved but its not formed correctly
            //looking for the xml with the leading 4 chars is the only xml we want to check.
            if (findit == 4)
            {
                _xmlViewerRequest = _xmlViewerRequest.Remove(0, findit);
                //xml messages will be prefixed with 4 zeroes which will cause LoadXML to break
                // so remove them           

                x.LoadXml(_xmlViewerRequest.ToString());

                try
                {
                    if (x.SelectSingleNode("/RtiMsg").InnerXml.Contains("ContractInfo"))
                    {
                        /*
                        ((oSocketServer)this.SocketServer).Practice = x.SelectSingleNode("/RtiMsg/ContractInfo/Practice").InnerXml;
                        ((oSocketServer)this.ooSocketServer).Division = x.SelectSingleNode("/RtiMsg/ContractInfo/Division").InnerXml;
                        ((oSocketServer)this.ooSocketServer).Encounter = x.SelectSingleNode("/RtiMsg/ContractInfo/EnctrNo").InnerXml;
                        ((oSocketServer)this.ooSocketServer).DocumentNo = x.SelectSingleNode("/RtiMsg/ContractInfo/Docno").InnerXml;

                        if (((oSocketServer)this.ooSocketServer).Encounter.Length > 3)
                        {
                            ((oSocketServer)this.ooSocketServer).TripPrefix = ((oSocketServer)this.ooSocketServer).Encounter.Substring(0, 3);
                        }

                        ((oSocketServer)this.SocketServer).InputParameterList.Clear();

                        ((oSocketServer)this.SocketServer).InputParameterList.Add("_Init_By", x.SelectSingleNode("/RtiMsg/ContractInfo/InitBy").InnerXml);
                        ((oSocketServer)this.SocketServer).InputParameterList.Add("_Origin", x.SelectSingleNode("/RtiMsg/ContractInfo/Origin").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Destination", x.SelectSingleNode("/RtiMsg/ContractInfo/Destin").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Origin_Modifier", x.SelectSingleNode("/RtiMsg/ContractInfo/OriginMod").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Destination_Modifier", x.SelectSingleNode("/RtiMsg/ContractInfo/DestMod").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Line_of_Business", x.SelectSingleNode("/RtiMsg/ContractInfo/LOB").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Recieve_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/ReceiveTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Dispatch_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/DispatchTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Enroute_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/EnrouteTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_On_Scene_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/OnSceneTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Transport_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/TransportTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Arrival_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/ArrivalTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Available_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/AvailableTime").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Unit_No", x.SelectSingleNode("/RtiMsg/ContractInfo/UnitNo").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Service_Date", x.SelectSingleNode("/RtiMsg/ContractInfo/DOS").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Type_of_Business", x.SelectSingleNode("/RtiMsg/ContractInfo/TOB").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Age", x.SelectSingleNode("/RtiMsg/ContractInfo/Age").InnerXml);
                        ((oSocketServer)this.ooSocketServer).InputParameterList.Add("_Transport_Type", x.SelectSingleNode("/RtiMsg/ContractInfo/TransType").InnerXml);

                        ((oSocketServer)this.ooSocketServer).GetData();
                         * */

                    }
                    else if (x.SelectSingleNode("/RtiMsg").InnerXml.Contains("Publish"))
                    {
                        if (x.SelectSingleNode("/RtiMsg/Publish").InnerXml.Contains("WindowHandle^"))
                        {
                            string sHandle = x.SelectSingleNode("/RtiMsg/Publish/Message").InnerXml.Replace("WindowHandle^", "");
                            if (sHandle.Length > 0)
                            {
                                //ContractWizardDefaults.IHandle = Convert.ToInt32(sHandle);
                            }
                        }
                        else
                        {
                           // ((SocketServer)this.oSocketServer).Close();
                        }
                    }

                    _xmlViewerRequest = new StringBuilder();
                    _xmlViewerRequest.Remove(0, _xmlViewerRequest.Length);


                }
                catch (Exception myerror)
                {
                    _xmlViewerRequest = new StringBuilder();
                    _xmlViewerRequest.Remove(0, _xmlViewerRequest.Length);
                    //System.Windows.Forms.MessageBox.Show(myerror.ToString());

                }

            }

        }


        public StringBuilder xmlOut
        {
            get
            {
                return _xmlOut;
            }
            set
            {
                _xmlOut = value;
            }
        }


        public void SessionConnect(int iPort)
        {

            string computerName;
            IPHostEntry ipHostInfo;
            IPAddress ipAddress;
            IPEndPoint remoteEP;

            try
            {
                computerName = Environment.MachineName;
                ipHostInfo = Dns.GetHostEntry(computerName);  //Resolve(computerName);
                ipAddress = ipHostInfo.AddressList[1]; //0 for IPv6 and 1 for IPv4
                remoteEP = new IPEndPoint(ipAddress, iPort);
                _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _sender.Connect(remoteEP);

                SendSubscribe();
                if (_sender.Connected == true)
                {
                    _connected = true;
                }
                
            }
            catch (Exception err)
            {
               //write errlog
            }
            
        }
        public void SendSubscribe()
        {
            try
            {

               // ((SocketServer)this.oSocketServer).DisplaySentMessage(Subscribe().ToString());
                byte[] msg = Encoding.ASCII.GetBytes(Subscribe().ToString());
                int bytesSend = _sender.Send(msg);
               // WaitForSubScribeData();
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("Cannot connect to Embillz Messenger. Please ensure that it is running.");
                //System.Windows.Forms.MessageBox.Show(ex.ToString() + "TcpListener SendMessage");
            }

        }
        public void SendMessages(string RtiMsg)
        {
            try
            {

                _xmlOut = new StringBuilder();

                _xmlOut.Remove(0, _xmlOut.Length);

                _xmlOut.Append(RtiMsg);
                try
                {

                    if (!_connected)
                    {

                        SessionConnect(7000);
                        SendSubscribe();
                        _connected = true;
                    }

                    byte[] msg = Encoding.ASCII.GetBytes(_xmlOut.ToString());

                    int bytesSend = _sender.Send(msg);

                }

                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.ToString() + "TcpListener SendMessage");
                }

            }
            catch (Exception ex2)
            {
                throw ex2;
            }
        }

        //Mark Lane's addition for making the socket object asyncronously wait for 
        //data so the application does not need to be in a do while loop and can operate
        //normally until a new message is received. 6/13/2007
        //Wait for data needs to be called everytime something has been sent to have it
        //in a asyncronous beginreceive.
        public void WaitForData()
        {


            try
            {



                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = _sender;

                // Start listening to the data asynchronously
                _sender.ReceiveBufferSize = 1024;
                m_result = _sender.BeginReceive(theSocPkt.dataBuffer,
                                                        0, theSocPkt.dataBuffer.Length,
                                                        SocketFlags.None,
                                                        m_pfnCallBack,
                                                        theSocPkt);


            }
            catch (SocketException se)
            {
                //System.Windows.Forms.MessageBox.Show(se.Message);
            }

        }
        public void WaitForSubScribeData()
        {
            try
            {

                if (m_pfnCallBackTwo == null)
                {
                    m_pfnCallBackTwo = new AsyncCallback(OnSubScribeDataReceived);
                }
                SocketPacket theSubSocPkt = new SocketPacket();
                theSubSocPkt.thisSocket = _sender;

                // Start listening to the data asynchronously
                _sender.ReceiveBufferSize = 1024;
                m_resultTwo = _sender.BeginReceive(theSubSocPkt.dataBuffer,
                                                        0, theSubSocPkt.dataBuffer.Length,
                                                        SocketFlags.None,
                                                        m_pfnCallBackTwo,
                                                        theSubSocPkt);


            }
            catch (SocketException se)
            {
                //System.Windows.Forms.MessageBox.Show(se.Message);
            }

        }
        public void OnSubScribeDataReceived(IAsyncResult asyn)
        {

            byte[] bytesReceived = new byte[4];
            try
            {
                _xmlViewerRequest = new StringBuilder();
                _xmlViewerRequest.Remove(0, _xmlViewerRequest.Length);

                SocketPacket theSubSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSubSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSubSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                //  System.String selectChar = szData.Substring(0, 1);
                //  _socketOutPut.Text = _socketOutPut.Text + selectChar;

                _xmlViewerRequest.Append(szData.Replace("\r\n", ""));

                ParseSubScribeReceivedBuffer();

                WaitForSubScribeData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                //System.Windows.Forms.MessageBox.Show(se.Message);
            }
        }
        public static StringBuilder Subscribe()
        {
            string strRegister = "";
            StringBuilder sbMessage = new StringBuilder();
            strRegister = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><RtiMsg><Header><Dest>WorkFlowAgent</Dest><Source>SocketServer</Source><UserId>localhost</UserId><Version>1.0</Version><MsgId>1</MsgId><MsgType>Register</MsgType></Header><Register><ID>subscribe</ID><SubscribeTo>DisplayImage</SubscribeTo></Register></RtiMsg>";

            sbMessage.AppendFormat("{0}{1}", strRegister.Length.ToString().PadLeft(4, '0'), strRegister);
            return sbMessage;
        }
        public void ParseSubScribeReceivedBuffer()
        {

            string DisplayImage = "";

            System.Xml.XmlDocument x = new System.Xml.XmlDocument();
            string findChar = _xmlViewerRequest.ToString();
            int findit = findChar.IndexOf("<", 0, findChar.Length);
            //ML:a second xml is processed by the OnDataRecieved but its not formed correctly
            //looking for the xml with the leading 4 chars is the only xml we want to check.
            if (findit == 4)
            {
                _xmlViewerRequest = _xmlViewerRequest.Remove(0, findit);

                //xml messages will be prefixed with 4 zeroes which will cause LoadXML to break
                // so remove them           
                try
                {
                    x.LoadXml(_xmlViewerRequest.ToString());

                    string strMessage = x.SelectSingleNode("/RtiMsg/DisplayImage/DocNum").InnerXml;
                    DisplayImage = strMessage.ToString();
                    /*
                    if (DisplayImage.ToString() != PreviousDocno.ToString())
                    {
                        PreviousDocno = DisplayImage.ToString();
                        ((oSocketServer)this.ooSocketServer).BeginFadeToWait1();
                    }
                    else
                    {
                        //do nothing for now
                    }
                     * */
                    _xmlViewerRequest = new StringBuilder();
                    _xmlViewerRequest.Remove(0, _xmlViewerRequest.Length);

                }
                catch (Exception myerror)
                {
                    _xmlViewerRequest = new StringBuilder();
                    _xmlViewerRequest.Remove(0, _xmlViewerRequest.Length);
                    //System.Windows.Forms.MessageBox.Show(myerror.ToString());
                    //WaitForData();

                }

            }
            //  WaitForData();

        }
        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[1024];
        }

        //Mark Lane's addition for making the socket object asyncronously wait for 
        //data so the application does not need to be in a do while loop and can operate
        //normally until a new message is received. 6/13/2007
        public void OnDataReceived(IAsyncResult asyn)
        {

            byte[] bytesReceived = new byte[1];
            try
            {
                _xmlViewerRequest = new StringBuilder();
                _xmlViewerRequest.Remove(0, _xmlViewerRequest.Length);

                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                //  System.String selectChar = szData.Substring(0, 1);
                //  _socketOutPut.Text = _socketOutPut.Text + selectChar;
                _xmlViewerRequest.Append(szData.ToString());

                ParseReceivedBuffer();

                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                //System.Windows.Forms.MessageBox.Show(se.Message);
            }
        }


    }
}
