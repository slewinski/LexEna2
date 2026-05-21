namespace LexEnaReporting.EOB.Wierzytelnosci
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for Wierzytelnosci.
    /// </summary>
    public partial class Wierzytelnosci : Telerik.Reporting.Report
    {
        public Wierzytelnosci()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.DSWierzytelnosci.CommandTimeout = 1200;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}