//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IRequestManagerService")]
public interface IRequestManagerService
{

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IRequestManagerService/IsAlive", ReplyAction = "http://tempuri.org/IRequestManagerService/IsAliveResponse")]
    bool IsAlive();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IRequestManagerServiceChannel : IRequestManagerService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class RequestManagerServiceClient : System.ServiceModel.ClientBase<IRequestManagerService>, IRequestManagerService
{

    public RequestManagerServiceClient()
    {
    }

    public RequestManagerServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName)
    {
    }

    public RequestManagerServiceClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public RequestManagerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public RequestManagerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public bool IsAlive()
    {
        return base.Channel.IsAlive();
    }
}
