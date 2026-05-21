namespace LexEnaReporting.EOP.WiekowanieWplat
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for KosztyRadcow.
    /// </summary>
    public partial class WiekowanieWplat : Telerik.Reporting.Report
    {
        public WiekowanieWplat()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.WienaDS.CommandTimeout = 240;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void KosztyRadcow_NeedDataSource(object sender, EventArgs e)
        {
            
        }
    }
}