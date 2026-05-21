namespace LexEnaReporting.EOP.ZwrotyAkt
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// </summary>
    public partial class ZwrotyAkt : Telerik.Reporting.Report
    {
        public ZwrotyAkt()
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