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

namespace LexEnaTrs.Views
{
    public partial class ReportWindow : ChildWindow
    {
        public int Mode {get;set;}  // wybór raportu 
        public int IdPaczki {get; set;}
        public object[] tab { get; set;}
        public int StatusDok { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
        public string StringArg { get; set; }

        public string StringArg2 { get; set; }
        public ReportWindow()
        {
            InitializeComponent();
            this.ReportViewer.RenderBegin += new RenderBeginEventHandler(ReportViewer_RenderBegin); 
            //this.ReportViewer.ReportServiceClientFactory 

        }

        private void SetReport()
        {
            string assemblyName;

            switch (this.Mode)
            {
                case 1 :
                    assemblyName = "LexEnaReporting.SkierowaneSad, LexEnaReporting";
                    break;
                case 2 :
                    assemblyName = "LexEnaReporting.SprawyDoSadu.SkierowaneDoSadu, LexEnaReporting";
                    break;
                case 3 :
                    assemblyName = "LexEnaReporting.SkierowanieSaldaById.SkierowaneDoSaduById, LexEnaReporting";
                    break;
                case 101:
                    assemblyName = "LexEnaReporting.TerminowoscPozwu.TerminowoscPozwu, LexEnaReporting";
                    break;
                case 102:
                    assemblyName = "LexEnaReporting.TerminowoscNakazu.TerminowoscNakazu, LexEnaReporting";
                    break;
                case 103:
                    assemblyName = "LexEnaReporting.ListaDokumentow.ListaDokumentow, LexEnaReporting";
                    break;
                case 1000:
                    assemblyName = "LexEnaReporting.TerminowoscCzynn.TerminowoscCzynn, LexEnaReporting";
                    break;
                case 9000:
                case 9001:
                    assemblyName = "LexEnaReporting.UZD_FakturyMain.UZD_FakturyMain, LexEnaReporting";
                    break;
                case 9002:
                    assemblyName = "LexEnaReporting.UZD_FakturyMain.UZD_FakturyMain, LexEnaReporting";
                    break;
                case 6000:
                case 6001:
                case 6002:
                    assemblyName = "LexEnaReporting.UZD_EksportPrzypisSAP.UZD_EksportPrzypisSAP, LexEnaReporting";
                    break;
                case 7000:
                case 7001:
                case 7002:
                    assemblyName = "LexEnaReporting.UZD_FakturyPaczki.UZD_FakturyPaczki, LexEnaReporting";
                    break;
                case 9010:
                    assemblyName = "LexEnaReporting.UZD_KontrahenciSAP.UZD_KontrahenciSAP, LexEnaReporting";
                    break;
                case 9030:
                    assemblyName = "LexEnaReporting.UZD_Wplaty.UZD_Wplaty, LexEnaReporting";
                    break;
                case 9031:
                    assemblyName = "LexEnaReporting.UZD_WplatyNew.UZD_WplatyNew, LexEnaReporting";
                    break;
                case 5000:
                    assemblyName = "LexEnaReporting.Tools_SpadyEPU.SpadyEPUReport, LexEnaReporting";
                    break;

                case 4000:
                    assemblyName = "LexEnaReporting.ExtractPdf.ExtractPdf, LexEnaReporting";
                    break;
                case 4001:
                    assemblyName = "LexEnaReporting.ExtractPdfValidate.ExtractPdfValidate, LexEnaReporting";
                    break;
                case 4070:
                    assemblyName = "LexEnaReporting.SkutKanc.SkutKanc, LexEnaReporting";
                    break;
                case 4080:
                    assemblyName = "LexEnaReporting.SaldoEgz.SaldoEgz, LexEnaReporting";
                    break;
                case 4081: 
                    assemblyName = "LexEnaReporting.SaldoEgzFormat2.SaldoEgzFormat2, LexEnaReporting";
                    break;
                case 4090:
                    assemblyName = "LexEnaReporting.NalSpraw.NaleznosciSpraw, LexEnaReporting";
                    break;
                case 5001:
                    assemblyName= "LexEnaReporting.EOB.UmorzeniaZgony.UmorzZgony, LexEnaReporting";
                    break;
                case 10001:
                    assemblyName = "LexEnaReporting.DatyDekretacji.DatyDekretacji, LexEnaReporting";
                    break;
                default:
                    assemblyName="";
                    break;
            }
            this.ReportViewer.Report = assemblyName;
        
        }
        private void SetReportArgs(ref Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {
            switch (this.Mode)
            {
                case 1:
                    args.ParameterValues["IdPaczki"] =IdPaczki;
                    break;
                case 2:
                    args.ParameterValues["IdPaczki"] =IdPaczki;
                    break;
                case 3:
                    args.ParameterValues["StatusDok"] = StatusDok;
                    args.ParameterValues["IdDokWys"] = tab;
                    break;
                case 101:
                    args.ParameterValues["DataOd"] = DataOd;
                    args.ParameterValues["DataDo"] = DataDo;
                    args.ParameterValues["User"] = (UserProfile.Rola == 2 ? 0: UserProfile.IdJednostki);
                    args.ParameterValues["CzyKanc"] = (UserProfile.Rola == 2 ? 0 : UserProfile.IdJednostki);
                    break;
                case 102:
                    args.ParameterValues["User"] = (UserProfile.Rola == 2 ? 0 : UserProfile.IdJednostki);
                    args.ParameterValues["CzyKanc"] = (UserProfile.Rola == 2 ? 0 : UserProfile.IdJednostki);
                    //UserProfile.Rola == 2 ? 
                    break;
                case 103:
                    args.ParameterValues["ListaId"] = StringArg;
                 
                    break;
                case 1000:
                    args.ParameterValues["DataOd"] = DataOd;
                    args.ParameterValues["DataDo"] = DataDo;
                    break;
                case 9000:
                case 9001:
                case 9002:  
                    args.ParameterValues["IdPakiet"] = IdPaczki;
                    args.ParameterValues["Mode"] = this.Mode - 9000;
                    break;
                case 6000:
                case 6001:
                case 6002:
                    args.ParameterValues["IdPakiet"] = IdPaczki;
                    args.ParameterValues["Mode"] = this.Mode - 6000;
                    break;
                case 7000:
                case 7001:
                case 7002:
                    args.ParameterValues["IdPakiet"] = IdPaczki;
                    args.ParameterValues["Mode"] = this.Mode - 7000;
                    break;
                case 9010:
                    args.ParameterValues["IdPakiet"] = IdPaczki;
                    break;
                case 9030:
                    args.ParameterValues["IdPakiet"] = IdPaczki;
                    break;
                case 9031:
                    args.ParameterValues["IdPakiet"] = IdPaczki;
                    break;
                case 5000:
                    args.ParameterValues["Sygnatury"] = StringArg;
                    break;
                case 4000:
                    args.ParameterValues["GuidPack"] = StringArg;
                    break;
                case 4001:
                    args.ParameterValues["GuidPack"] = StringArg;
                    args.ParameterValues["NazwaLst"] = StringArg2;
                    break;
                case 4070:
                    args.ParameterValues["PakietId"] = StringArg;
                    args.ParameterValues["DataDo"] = DateTime.Today;
                    break;
                case 4080:
                    args.ParameterValues["PakietId"] = StringArg;
                    break;
                case 4081:
                    args.ParameterValues["PakietId"] = StringArg;
                    break;
                case 4090:
                    args.ParameterValues["PakietId"] = StringArg;
                    break;
                case 5001:
                    args.ParameterValues["ZgonyHeader_ID"] = IdPaczki;
                    break;
                default:
                    break;
            }
        
        
        
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ReportViewer_RenderBegin(object sender, Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {
            SetReport();
            SetReportArgs(ref args) ; 
            this.ReportViewer.RenderBegin -= ReportViewer_RenderBegin; 

        }

            }
}

