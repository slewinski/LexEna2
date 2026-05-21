namespace LexEnaReporting.EOB.WiekowanieNS
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
    public partial class WiekowanieNS : Telerik.Reporting.Report
    {
        public WiekowanieNS()
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