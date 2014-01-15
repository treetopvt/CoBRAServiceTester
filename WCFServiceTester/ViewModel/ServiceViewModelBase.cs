using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Net.Http.Headers;

namespace WCFServiceTester.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public abstract class ServiceViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ServiceViewModelBase class.
        /// </summary>
        public ServiceViewModelBase(string serviceName)
        {
            ServiceName = serviceName;
            RegisterMessaging();
        }


        public string ServiceName { get; set; }

        public Models.CredentialModel Credientials { get; set; }

        internal void RegisterMessaging()
        {
            Messenger.Default.Register<NotificationMessage<Models.CredentialModel>>
    (this, updateCredentials);

        }

        private void updateCredentials(NotificationMessage<Models.CredentialModel> msg)
        {
            this.Credientials = msg.Content;
        }

        internal AuthenticationHeaderValue GetAuthHeader()
        {
 

                return new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format(@"ServiceUser:{0}:{1}", Credientials.UserName, Credientials.Password))));


        }

    }

}