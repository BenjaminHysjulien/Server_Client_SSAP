using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Server_Client_SSAP.ServiceReference1;
using Server_Client_SSAP.ServiceReference2;
using Onvif_Interface.WsSecurity;


namespace Server_Client_SSAP
{
    class CustomBindingsAndClients
    {
        public static ServiceReference1.Media2Client GetMedia2Client(string uri, string username = "", string password = "", double devideviceTimeOffset = 0)
        {
            UriBuilder deviceUri = new UriBuilder("");
            System.ServiceModel.Channels.Binding binding;
            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;
            binding = new System.ServiceModel.Channels.CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8), httpTransport);
            deviceUri.Host = uri;
            Media2Client media2Client = new Media2Client(binding, new EndpointAddress(deviceUri.ToString()));

            return media2Client;
        }

        public static DeviceClient GetClient(string ip, int port, double deviceTimeOffset, out System.ServiceModel.Channels.CustomBinding binding, out UriBuilder deviceUri, string username = "", string password = "")
        {

            deviceUri = new UriBuilder("");

            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;
            deviceUri.Host = ip;
            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            System.ServiceModel.Channels.CustomBinding customBinding = new System.ServiceModel.Channels.CustomBinding(messageElement, httpTransport);
            binding = new System.ServiceModel.Channels.CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8), httpTransport);
            DeviceClient Device = new DeviceClient(binding, new EndpointAddress(deviceUri.ToString()));

            return Device;
        }
        public static DeviceClient GetOnvifClientWithAuthorization(string ip, int port, double deviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(string.Format("", ip, port));

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            System.ServiceModel.Channels.CustomBinding bind = new System.ServiceModel.Channels.CustomBinding(messageElement, httpBinding);

            DeviceClient deviceClient = new DeviceClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, deviceTimeOffset);
                deviceClient.Endpoint.Behaviors.Add(behavior);
            }

            return deviceClient;
        }

        public static DeviceClient GetClient(string ip, int port, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(string.Format("", ip, port));

            HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
            httpBindingElement.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            System.ServiceModel.Channels.CustomBinding customBinding = new System.ServiceModel.Channels.CustomBinding(messageElement, httpBindingElement);

            DeviceClient deviceClient = new DeviceClient(customBinding, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password);
                deviceClient.Endpoint.Behaviors.Add(behavior);
            }

            return deviceClient;
        }

        public static ServiceReference1.MediaClient GetMedia1Client(string uri, double devideviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress endpointAddress = new EndpointAddress(uri);
            HttpTransportBindingElement httpTransportBindingElement = new HttpTransportBindingElement();
            httpTransportBindingElement.AuthenticationScheme = AuthenticationSchemes.Digest;

            var MessageEl = new TextMessageEncodingBindingElement();
            MessageEl.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            System.ServiceModel.Channels.CustomBinding customBinding = new System.ServiceModel.Channels.CustomBinding(MessageEl, httpTransportBindingElement);

            ServiceReference1.MediaClient media1Client = new ServiceReference1.MediaClient(customBinding, endpointAddress);

            if (username != string.Empty)
            {
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, devideviceTimeOffset);
                media1Client.Endpoint.EndpointBehaviors.Add(behavior);
            }
            return media1Client;
        }
    }

}


