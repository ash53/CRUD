using System.Data;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceContracts;

namespace Rti.InternalInterfaces.ServiceProxies
{
    public class EagleIqGatewayClient : ServiceProxyBase<IEagleIqGatewayService>
    {
        /// <summary>
        /// 
        /// Example Client Config (portion)
        /// 
        /// <system.serviceModel>
        ///  <bindings>
        ///    <netTcpBinding>
        ///      <binding name="TCPEndPoint"/>
        ///      <binding>
        ///        <security mode="Transport">
        ///          <transport protectionLevel="None" />
        ///        </security>
        ///      </binding>
        ///    </netTcpBinding>
        ///  </bindings>
        ///  <client>
        ///    <endpoint name="netTcpLocal"
        ///      address="net.tcp://localhost:8044/Rti.EagleIqGatewayService/" 
        ///      binding="netTcpBinding" 
        ///      bindingConfiguration="TCPEndPoint" 
        ///      contract="Rti.InternalInterfaces.ServiceContracts.IEagleIqGatewayService" 
        ///      />
        ///    <endpoint name="netTcpRemote"
        ///      address="net.tcp://localhost:8044/Rti.EagleIqGatewayService/"
        ///      binding="netTcpBinding"
        ///      bindingConfiguration="TCPEndPoint"
        ///      contract="Rti.InternalInterfaces.ServiceContracts.IEagleIqGatewayService"
        ///      />
        ///  </client>
        /// </system.serviceModel>
        ///
        /// Example Server Config (portion)
        ///
        /// <system.serviceModel>
        ///  <services>
        ///    <service name="Rti.EagleIqGatewayServer.Servers.EagleIqGatewayServer"
        ///             behaviorConfiguration="ServiceBehavior" >
        ///      <endpoint name="eagleIqGatewayEndpoint"
        ///                address="" 
        ///                binding="netTcpBinding" 
        ///                bindingConfiguration="DefaultBinding"
        ///                contract="Rti.InternalInterfaces.ServiceContracts.IEagleIqGatewayService" />
        ///      <host>
        ///        <baseAddresses>
        ///          <add baseAddress="net.tcp://localhost:8044/Rti.EagleIqGatewayService/" />
        ///        </baseAddresses>
        ///      </host>
        ///    </service>
        ///  </services>
        ///  <bindings>
        ///    <netTcpBinding>
        ///      <binding name="DefaultBinding">
        ///        <security mode="Transport">
        ///          <transport protectionLevel="None" />
        ///        </security>
        ///      </binding>
        ///    </netTcpBinding>
        ///  </bindings>
        ///  <behaviors>
        ///    <serviceBehaviors>
        ///      <behavior name="ServiceBehavior">
        ///        <serviceDebug includeExceptionDetailInFaults="true"/>
        ///      </behavior>
        ///    </serviceBehaviors>
        ///  </behaviors>
        /// </system.serviceModel>
        ///
        /// </summary>
        /// 
        
        public EagleIqGatewayClient(string serviceEndpointUri, string serviceEndpointName)
            : base(serviceEndpointUri, serviceEndpointName)
        {
        }

        public bool IsAlive()
        {
            return Channel.IsAlive();
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            return Channel.GetServiceInfo(workstation, username);
        }

        #region Stored Procedures

        public DataTable GetPostBd(string workstation, string username,
            string mapName, string actName, string practice, string division,
            string depDtStart, string depDtEnd, string embAcct, string reason, string checkNum,
            string checkAmountMin, string checkAmountMax, string paidAmountMin, string paidAmountMax,
            string docType, string docDet, string who, string docNo, string parDocNo)
        {
            return Channel.GetPostBd(workstation, username,
                mapName, actName, practice, division,
                depDtStart, depDtEnd, embAcct, reason, checkNum,
                checkAmountMin, checkAmountMax, paidAmountMin, paidAmountMax,
                docType, docDet, who, docNo, parDocNo);
        }

        #endregion

        #region RPCs

        public RpcOutMessage ApproveMatch(RpcInMessage rpcInMessage, string server, int matchId)
        {
            return Channel.ApproveMatch(rpcInMessage, server, matchId);
        }

        public RpcOutMessage CreateMatch(RpcInMessage rpcInMessage, string server, DataTable dataTable)
        {
            return Channel.CreateMatch(rpcInMessage, server, dataTable);
        }

        public string ProcessR2P(string strWorkstation, UpdR2PInputParams rpcInMessage)
        {
            return Channel.ProcessR2P(strWorkstation, rpcInMessage);
        }

        #endregion
    }
}
