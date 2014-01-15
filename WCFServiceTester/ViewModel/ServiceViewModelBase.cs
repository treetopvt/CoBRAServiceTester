using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
        public ServiceViewModelBase(string serviceName, string rootURL, string userName, string password)
        {
            ServiceName = serviceName;
            _rootURL = rootURL;
            this.Credientials = new Models.CredentialModel(userName, password);
            RegisterMessaging();
        }


        public string ServiceName { get; set; }
        
        private string _rootURL = "";

        public Models.CredentialModel Credientials { get; set; }

        internal void RegisterMessaging()
        {
            Messenger.Default.Register<NotificationMessage<Models.CredentialModel>>
    (this, updateCredentials);
            Messenger.Default.Register<NotificationMessage<string>>
    (this, updateServer );
            Messenger.Default.Register<NotificationMessage<Models.OrgProjectModel>>
(this, updateOrgProject);
        }

        private void updateOrgProject(NotificationMessage<Models.OrgProjectModel> msg)
        {

        }

        private void updateCredentials(NotificationMessage<Models.CredentialModel> msg)
        {
            this.Credientials = msg.Content;
        }

        private void updateServer(NotificationMessage<string> msg){
            if (msg.Notification == "ServerUpdated"){
                _rootURL = msg.Content;
            }
        }

        internal AuthenticationHeaderValue GetAuthHeader()
        {
 

                return new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format(@"ServiceUser:{0}:{1}", Credientials.UserName, Credientials.Password))));


        }

        internal virtual string BuildFullURL(string relativeURL, string serviceName)
        {
            return _rootURL + @"/cobrawcfservices/" + serviceName + @"/" + relativeURL;

        }
        
        internal async Task<string> AuthenticatedGetData(string relativeURL, string serviceName, FormUrlEncodedContent data)
        {
            var authValue = GetAuthHeader();

            var url = BuildFullURL(relativeURL, serviceName);
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authValue.Parameter);
            client.DefaultRequestHeaders.Add("Authorization", @"ServiceUser:test:TestUser");
            HttpResponseMessage response = await client.PostAsync(new Uri(url), data);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }


        internal bool CanMakeServiceCall()
        {
            if (this.Credientials == null)
                return false;
            if (string.IsNullOrEmpty(_rootURL))
                return false;

            return !String.IsNullOrEmpty(Credientials.UserName) && !string.IsNullOrEmpty(Credientials.Password);
        }

    }

}