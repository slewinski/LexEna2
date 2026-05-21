namespace LexEnaReporting.EOP.RodzajeObciazen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// </summary>
    public partial class RodzajeObciazen : Telerik.Reporting.Report
    {
        public RodzajeObciazen()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.LexEnaDS.CommandTimeout = 300;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}