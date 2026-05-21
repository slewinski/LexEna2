namespace LexEnaReporting.ObrotyDziennik
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Web;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for ObrotyDziennik.
    /// </summary>
    public partial class ObrotyDziennik : Telerik.Reporting.Report
    {
        public ObrotyDziennik()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        private void ObrotyDziennik_NeedDataSource(object sender, EventArgs e)
        {
           
            var report = (Telerik.Reporting.Processing.Report)sender;

            report = report;

            //var conn = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString;

                /*
                string st =  (string)report.ReportParameters["ConnectionStringParam"].Value;

                string path = HttpContext.Current.Server.MapPath("~/conn_log.txt");

                File.AppendAllText(path,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    " ConnectionStringParam = " + st +
                    Environment.NewLine);
                    */
        }
    }
}