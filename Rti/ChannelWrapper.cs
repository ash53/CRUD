using System;
using System.ServiceModel;

namespace Rti
{
    public class ChannelWrapper<T> : IDisposable where T : class
    {
        private readonly string _endpointConfigName;                
        private ChannelFactory<T> _channelFactory;        
        private T _channel;
        
        public ChannelWrapper(string endpointConfigurationName) { _endpointConfigName = endpointConfigurationName; }

        public void Dispose()
        {                        
            try
            {
                if (_channel != null)
                {
                    ((ICommunicationObject)_channel).Close();
                }

                if (_channelFactory != null)
                {
                    ((IDisposable)_channelFactory).Dispose();
                }

                _channel = null;
                _channelFactory = null;
            }
            catch (Exception)
            {
                if (_channelFactory != null)
                {
                    _channelFactory.Abort();
                }
                throw;
            }               
        }

        public T Channel
        {
            get
            {
                Initialize();

                return _channel;
            }
        }

        private void Initialize()
        {            
            if (_channel != null) return;

            _channelFactory = new ChannelFactory<T>(_endpointConfigName);
            _channel = _channelFactory.CreateChannel();            
        }
    }
}

