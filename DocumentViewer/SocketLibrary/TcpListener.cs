using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace DocumentViewer
{

    public abstract class TcpListener : EventArgs
    {
        public Socket listener;
       // private string _userId = System.Windows.Forms.SystemInformation.UserName;
        private StringBuilder _xmlViewerRequest;
       // public System.Windows.Forms.Form objFWorkGrid;
        static private int TcpListenerPort = 7000;
        //public string PreviousDocno = "";
        IAsyncResult m_result;
        public AsyncCallback m_pfnCallBack;
        public int ConnectionCount;
       // public static ManualResetEvent allDone = new ManualResetEvent(false);

        /// create all events
        public event OnConnectedHandler evnConnectedStatus;
        public event MessageReceived evnMessageReceived;
        
        /// create all delegates
        public delegate int OnConnectedHandler(int Conn, EventArgs e);
        public delegate string MessageReceived(int Conn, EventArgs e); 

        #region Constructor
        /// <summary>
            public class SocketPacket
            {
                public System.Net.Sockets.Socket thisSocket;
                public byte[] dataBuffer = new byte[1024];
            }
            public class StateObject
            {
                // Client  socket.
                 public Socket workSocket = null;
                // Size of receive buffer.
                public const int BufferSize = 1024;
                // Receive buffer.
                public byte[] buffer = new byte[BufferSize];
                // Received data string.
                public StringBuilder sb = new StringBuilder();
            }    
        /// </summary>
        public TcpListener()//System.Windows.Forms.Form oMainF)
        {
           // objFWorkGrid = oMainF;
           // evnMessageReceived += new MessageReceived(tcpipListener_evnMessageReceived);
            
           
        }
        string tcpipListener_evnMessageReceived(int Conn, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Destructor
        /// <summary>
        /// Release resources.
        /// </summary>
        ~TcpListener()
        {
            
        }

        #endregion
      
        private void WaitForData()
        {

            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = listener;

                // Start listening to the data asynchronously
                listener.ReceiveBufferSize = 1024;
                m_result = listener.BeginReceive(theSocPkt.dataBuffer,
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
                //((WorkGrid)this.objFWorkGrid).DisplayListenerMessage(szData.ToString(), theSockId.thisSocket.Handle.ToInt32());
                ParseReceivedBuffer();

                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
               // System.Windows.Forms.MessageBox.Show(se.Message);
            }
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
                        ((WorkGrid)this.oWorkGrid).Practice = x.SelectSingleNode("/RtiMsg/ContractInfo/Practice").InnerXml;
                        ((WorkGrid)this.oWorkGrid).Division = x.SelectSingleNode("/RtiMsg/ContractInfo/Division").InnerXml;
                        ((WorkGrid)this.oWorkGrid).Encounter = x.SelectSingleNode("/RtiMsg/ContractInfo/EnctrNo").InnerXml;
                        ((WorkGrid)this.oWorkGrid).DocumentNo = x.SelectSingleNode("/RtiMsg/ContractInfo/Docno").InnerXml;

                        if (((WorkGrid)this.oWorkGrid).Encounter.Length > 3)
                        {
                            ((WorkGrid)this.oWorkGrid).TripPrefix = ((WorkGrid)this.oWorkGrid).Encounter.Substring(0, 3);
                        }

                        ((WorkGrid)this.oWorkGrid).InputParameterList.Clear();

                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Init_By", x.SelectSingleNode("/RtiMsg/ContractInfo/InitBy").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Origin", x.SelectSingleNode("/RtiMsg/ContractInfo/Origin").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Destination", x.SelectSingleNode("/RtiMsg/ContractInfo/Destin").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Origin_Modifier", x.SelectSingleNode("/RtiMsg/ContractInfo/OriginMod").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Destination_Modifier", x.SelectSingleNode("/RtiMsg/ContractInfo/DestMod").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Line_of_Business", x.SelectSingleNode("/RtiMsg/ContractInfo/LOB").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Recieve_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/ReceiveTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Dispatch_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/DispatchTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Enroute_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/EnrouteTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_On_Scene_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/OnSceneTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Transport_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/TransportTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Arrival_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/ArrivalTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Available_Time", x.SelectSingleNode("/RtiMsg/ContractInfo/AvailableTime").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Unit_No", x.SelectSingleNode("/RtiMsg/ContractInfo/UnitNo").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Service_Date", x.SelectSingleNode("/RtiMsg/ContractInfo/DOS").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Type_of_Business", x.SelectSingleNode("/RtiMsg/ContractInfo/TOB").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Age", x.SelectSingleNode("/RtiMsg/ContractInfo/Age").InnerXml);
                        ((WorkGrid)this.oWorkGrid).InputParameterList.Add("_Transport_Type", x.SelectSingleNode("/RtiMsg/ContractInfo/TransType").InnerXml);

                        ((WorkGrid)this.oWorkGrid).GetData();
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


          public void StartListening(int portnum)
          {
              // Data buffer for incoming data.
              byte[] bytes = new Byte[1024];

              // Establish the local endpoint for the socket.
              // The DNS name of the computer
              // running the listener is "host.contoso.com".
              IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
              IPAddress ipAddress = ipHostInfo.AddressList[1];//1 for 64 bit IPV4
              IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portnum);

              // Create a TCP/IP socket.
              Socket listener = new Socket(AddressFamily.InterNetwork,
                  SocketType.Stream, ProtocolType.Tcp);

              // Bind the socket to the local endpoint and listen for incoming connections.
              try
              {
                  listener.Bind(localEndPoint);
                  listener.Listen(100);
                  //((WorkGrid)this.objFWorkGrid).DisplayConnectionStatus("Listening on port 7000");
                  while (true)
                  {
                      // Set the event to nonsignaled state.
                    //  allDone.Reset();

                      // Start an asynchronous socket to listen for connections.
                      //Console.WriteLine("Waiting for a connection...");
                      listener.BeginAccept(
                          new AsyncCallback(AcceptCallback),
                          listener);

                      // Wait until a connection is made before continuing.
                  //    allDone.WaitOne();
                  }

              }
              catch (Exception e)
              {
                 // Console.WriteLine(e.ToString());
              }

       

          }

          public void AcceptCallback(IAsyncResult ar)
          {
              // Signal the main thread to continue.
             // allDone.Set();

              // Get the socket that handles the client request.
              listener = (Socket)ar.AsyncState;
              Socket handler = listener.EndAccept(ar);
              ConnectionCount = ConnectionCount + 1;
              //((WorkGrid)this.objFWorkGrid).TCPIP_OnConnected("Connected Sockets# (" + ConnectionCount + ")", ConnectionCount);
              
              // Create the state object.
              StateObject state = new StateObject();
              state.workSocket = handler;
              handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                  new AsyncCallback(ReadCallback), state);

              //Return listener back to listening state.
              listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
          }

          public void ReadCallback(IAsyncResult ar)
          {
              String content = String.Empty;

              // Retrieve the state object and the handler socket
              // from the asynchronous state object.
              StateObject state = (StateObject)ar.AsyncState;
              Socket handler = state.workSocket;

              // Read data from the client socket. 
              int bytesRead = handler.EndReceive(ar);

              if (bytesRead > 0)
              {
                  // There  might be more data, so store the data received so far.
                  state.sb.Append(Encoding.ASCII.GetString(
                      state.buffer, 0, bytesRead));

                  // Check for end-of-file tag. If it is not there, read 
                  // more data.
                  content = state.sb.ToString();
                  DisplayReceivedMessage(Convert.ToString(content.ToString()), handler.Handle.ToInt32());
                  if (content.IndexOf("<EOF>") > -1)
                  {
                      // All the data has been read from the 
                      // client. Display it on the console.
                      //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                      //    content.Length, content);
                      DisplayReceivedMessage(Convert.ToString(content.ToString()), handler.Handle.ToInt32());
                      // Echo the data back to the client.
                      Send(handler, content);
                  }
                  else
                  {
                      //send back received
                      Send(handler, content);
                      DisplaySentMessage(Convert.ToString(content.ToString()));
                      state.sb.Clear(); //clear the socket strbuilder.
                      // after send begin waiting again.
                      handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                      new AsyncCallback(ReadCallback), state);
                  }
              }
          }
          public void DisplayReceivedMessage(string InMessage, int ConnectionHandle)
          {
              
              //((WorkGrid)this.objFWorkGrid).DisplayListenerMessage(InMessage, ConnectionHandle);
          }
          public void DisplaySentMessage(string InMessage)
          {
              //((WorkGrid)this.objFWorkGrid).DisplaySenderMessage("Connection(" + ConnectionCount + ") Sent: " + InMessage.ToString());
          }
          private static void Send(Socket handler, String data)
          {
              // Convert the string data to byte data using ASCII encoding.
              byte[] byteData = Encoding.ASCII.GetBytes(data);

              // Begin sending the data to the remote device.
              handler.BeginSend(byteData, 0, byteData.Length, 0,
                  new AsyncCallback(SendCallback), handler);
          }

          private static void SendCallback(IAsyncResult ar)
          {
              try
              {
                  // Retrieve the socket from the state object.
                  Socket handler = (Socket)ar.AsyncState;

                  // Complete sending the data to the remote device.
                  int bytesSent = handler.EndSend(ar);
                 // Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                  //Don't disconnect yet.
                 // handler.Shutdown(SocketShutdown.Both);
                 // handler.Close();

              }
              catch (Exception e)
              {
                //  Console.WriteLine(e.ToString());
              }
          }
      }
}
