using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.ReportViewer.Silverlight;
using Telerik.Reporting.Service.SilverlightClient;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace LexEnaTrs.Views
{
    public partial class LargeReportWindow : UserControl, IReportServiceClientFactory
    {
        public int Mode { get; set; }  // wybór raportu 

        public LargeReportWindow()
        {
            InitializeComponent();
            this.ReportViewer.ReportServiceClientFactory = this;
            this.ReportViewer.RenderBegin += new RenderBeginEventHandler(ReportViewer_RenderBegin);
        }

        ReportServiceClient IReportServiceClientFactory.Create(Uri remoteAddress)
        {
            Binding binding = new BasicHttpBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 20, 0),
                SendTimeout = new TimeSpan(0, 20, 0),
                OpenTimeout = new TimeSpan(0, 20, 0),
                CloseTimeout = new TimeSpan(0, 20 , 0),
                MaxBufferSize = Int32.MaxValue,
                MaxReceivedMessageSize = Int32.MaxValue
            };
            EndpointAddress endpointAddress = new EndpointAddress(remoteAddress);

            return new ReportServiceClient(binding, endpointAddress);
        }


        private void SetReport()
        {
            string assemblyName;

            switch (this.Mode)
            {
                case 1:
                    assemblyName = "LexEnaReporting.DatyDekretacji.DatyDekretacji, LexEnaReporting";
                    break;
                case 2:
                    assemblyName = "LexEnaReporting.EOB.Wierzytelnosci.Wierzytelnosci, LexEnaReporting";
                    break;
                case 3:
                    assemblyName = "LexEnaReporting.EOB.BezUz2.BezUz2, LexEnaReporting";
                    break;
                case 4:
                    assemblyName = "LexEnaReporting.EOP.ZwrotyAkt.ZwrotyAkt, LexEnaReporting";
                    break;
                case 5:
                    assemblyName = "LexEnaReporting.ObrotyDziennik.ObrotyDziennik, LexEnaReporting";
                    break;
                default:
                    assemblyName = "";
                    break;
            }
            this.ReportViewer.Report = assemblyName;

        }

        private void ReportViewer_RenderBegin(object sender, Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {
            SetReport();
            this.ReportViewer.RenderBegin -= ReportViewer_RenderBegin;

        }

        
    }
    
}
