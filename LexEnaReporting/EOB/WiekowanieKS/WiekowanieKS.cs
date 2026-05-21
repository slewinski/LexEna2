namespace LexEnaReporting.EOB.WiekowanieKS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for WiekowanieNS.
    /// </summary>
    public partial class WiekowanieKS : Telerik.Reporting.Report
    {
        public WiekowanieKS()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.WiekowanieOSDS.CommandTimeout = 240;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}