using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;

namespace DynamicsServices
{
    public partial class DynamicsProxy : ServiceBase
    {
        private Uri URL = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + Environment.MachineName + ":50000/DynamicsProxy");
        ServiceHost host;


        public DynamicsProxy()
        {
            InitializeComponent();

            this.host = new ServiceHost(new DynamicsServices(), URL);
            this.host.AddServiceEndpoint(typeof(IDynamicsServices), new BasicHttpBinding(), "");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = URL;
                
            this.host.Description.Behaviors.Add(smb);
        }

        protected override void OnStart(string[] args)
        {
            if (host.State != CommunicationState.Opened && host.State != CommunicationState.Opening)
                host.Open();

            
        }

        protected override void OnStop()
        {
            if (host.State != CommunicationState.Closed && host.State != CommunicationState.Closing)
                host.Close();
        }
    }
}
