using System;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace Rti
{
    public abstract class ServiceProxyBase<T> : IDisposable where T : class
    {
        private readonly string _serviceEndpointUri;
        private readonly string _serviceEndpointName;
        private readonly object _sync = new object();
        private IChannelFactory<T> _channelFactory;
        private T _channel;
        private bool _disposed = false;

        protected ServiceProxyBase(string serviceEndpointUri, string serviceEndPointName)
        {
            _serviceEndpointUri = serviceEndpointUri;
            _serviceEndpointName = serviceEndPointName;
        }

        #region IDisposable
        ~ServiceProxyBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManaged)
        {
            if (_disposed) return;

            if (disposeManaged)
            {
                lock (_sync)
                {
                    CloseChannel();

                    if (_channelFactory != null)
                    {
                        ((IDisposable)_channelFactory).Dispose();
                    }

                    _channel = null;
                    _channelFactory = null;
                }
            }
            _disposed = true;
        }
        #endregion

        protected T Channel
        {
            get
            {
                Initialize();

                return _channel;
            }
        }

        protected void CloseChannel()
        {
            if (_channel != null)
            {
                ((ICommunicationObject) _channel).Close();
            }
        }

        private void Initialize()
        {
            lock (_sync)
            {
                if (_channel != null) return;
                //_channelFactory = new ChannelFactory<T>(new NetTcpBinding());
                _channelFactory = new ChannelFactory<T>( _serviceEndpointName );
                _channel = _channelFactory.CreateChannel(
                    new EndpointAddress(_serviceEndpointUri));
            }
        }
    }
}
