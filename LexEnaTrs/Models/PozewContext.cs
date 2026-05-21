using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel.DomainServices.Client;

namespace LexEnaTrs.Web
{
    public partial class PozewDomainContext
    {
        
        partial void OnCreated()
        {
            ((WebDomainClient<LexEnaTrs.Web.PozewDomainContext.IPozewDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.SendTimeout = new TimeSpan(0, 20, 0);
            ((WebDomainClient<LexEnaTrs.Web.PozewDomainContext.IPozewDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 20, 0);
            ((WebDomainClient<LexEnaTrs.Web.PozewDomainContext.IPozewDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 20, 0);
            ((WebDomainClient<LexEnaTrs.Web.PozewDomainContext.IPozewDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 20, 0);
            
        }
         
    }

    public partial class LexEnaMeritumDomainContext
    {

        partial void OnCreated()
        {
            ((WebDomainClient<LexEnaTrs.Web.LexEnaMeritumDomainContext.ILexEnaMeritumDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.SendTimeout = new TimeSpan(0, 40, 0);
            ((WebDomainClient<LexEnaTrs.Web.LexEnaMeritumDomainContext.ILexEnaMeritumDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 40, 0);
            ((WebDomainClient<LexEnaTrs.Web.LexEnaMeritumDomainContext.ILexEnaMeritumDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 40, 0);
            ((WebDomainClient<LexEnaTrs.Web.LexEnaMeritumDomainContext.ILexEnaMeritumDomainServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 40, 0);

        }

    }

    public partial class EpuContext
    {

        partial void OnCreated()
        {
            ((WebDomainClient<LexEnaTrs.Web.EpuContext.IEpuServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.SendTimeout = new TimeSpan(0, 40, 0);
            ((WebDomainClient<LexEnaTrs.Web.EpuContext.IEpuServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 40, 0);
            ((WebDomainClient<LexEnaTrs.Web.EpuContext.IEpuServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 40, 0);
            ((WebDomainClient<LexEnaTrs.Web.EpuContext.IEpuServiceContract>)this.DomainClient).ChannelFactory.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 40, 0);

        }

    }
}