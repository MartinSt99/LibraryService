﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.proxyServiceKunden {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="proxyServiceKunden.IKundenService")]
    public interface IKundenService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKundenService/begruesung", ReplyAction="http://tempuri.org/IKundenService/begruesungResponse")]
        string begruesung(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKundenService/begruesung", ReplyAction="http://tempuri.org/IKundenService/begruesungResponse")]
        System.Threading.Tasks.Task<string> begruesungAsync(string value);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IKundenServiceChannel : Client.proxyServiceKunden.IKundenService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KundenServiceClient : System.ServiceModel.ClientBase<Client.proxyServiceKunden.IKundenService>, Client.proxyServiceKunden.IKundenService {
        
        public KundenServiceClient() {
        }
        
        public KundenServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KundenServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KundenServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KundenServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string begruesung(string value) {
            return base.Channel.begruesung(value);
        }
        
        public System.Threading.Tasks.Task<string> begruesungAsync(string value) {
            return base.Channel.begruesungAsync(value);
        }
    }
}