namespace LexEnaReporting.SkutKanc
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for SkutKanc.
    /// </summary>
    public partial class SkutKanc : Telerik.Reporting.Report
    {
        public SkutKanc()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.skutKancds.CommandTimeout = 240;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}