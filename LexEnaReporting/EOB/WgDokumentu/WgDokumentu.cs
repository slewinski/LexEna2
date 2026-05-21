namespace LexEnaReporting.EOB.WgDokumentu
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
    public partial class WgDokumentu : Telerik.Reporting.Report
    {
        public WgDokumentu()
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