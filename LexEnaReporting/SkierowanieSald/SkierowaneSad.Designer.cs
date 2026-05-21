namespace LexEnaReporting
{
    partial class SkierowaneSad
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.LexEnaDS = new Telerik.Reporting.SqlDataSource();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.nrEwidCaptionTextBox = new Telerik.Reporting.TextBox();
            this.sygnaturaCaptionTextBox = new Telerik.Reporting.TextBox();
            this.dluznikCaptionTextBox = new Telerik.Reporting.TextBox();
            this.oddzialCaptionTextBox = new Telerik.Reporting.TextBox();
            this.pusteCaptionTextBox = new Telerik.Reporting.TextBox();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroup = new Telerik.Reporting.Group();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.nrEwidDataTextBox = new Telerik.Reporting.TextBox();
            this.sygnaturaDataTextBox = new Telerik.Reporting.TextBox();
            this.dluznikDataTextBox = new Telerik.Reporting.TextBox();
            this.oddzialDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.group1 = new Telerik.Reporting.Group();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.SelectCommand = "select Id, Oznaczenie from paczka";
            // 
            // LexEnaDS
            // 
            this.LexEnaDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.LexEnaDS.Name = "LexEnaDS";
            this.LexEnaDS.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@IdPaczki", System.Data.DbType.Int32, "=Parameters.IdPaczki.Value")});
            this.LexEnaDS.SelectCommand = "SELECT        NrEwid, Sygnatura, Dluznik, Oddzial, WPS\r\nFROM            vw_ListaD" +
    "okPaczkiDoSalda\r\nWhere        IdPaczki = @IdPaczki";
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D);
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
            // 
            // nrEwidCaptionTextBox
            // 
            this.nrEwidCaptionTextBox.CanGrow = true;
            this.nrEwidCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.nrEwidCaptionTextBox.Name = "nrEwidCaptionTextBox";
            this.nrEwidCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6856245994567871D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.nrEwidCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.nrEwidCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.nrEwidCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.nrEwidCaptionTextBox.StyleName = "Caption";
            this.nrEwidCaptionTextBox.Value = "nr ewidencyjny";
            // 
            // sygnaturaCaptionTextBox
            // 
            this.sygnaturaCaptionTextBox.CanGrow = true;
            this.sygnaturaCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3937091827392578D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.sygnaturaCaptionTextBox.Name = "sygnaturaCaptionTextBox";
            this.sygnaturaCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.sygnaturaCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.sygnaturaCaptionTextBox.StyleName = "Caption";
            this.sygnaturaCaptionTextBox.Value = "Sygnatura";
            // 
            // dluznikCaptionTextBox
            // 
            this.dluznikCaptionTextBox.CanGrow = true;
            this.dluznikCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.3047003746032715D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.dluznikCaptionTextBox.Name = "dluznikCaptionTextBox";
            this.dluznikCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.4950995445251465D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.dluznikCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dluznikCaptionTextBox.StyleName = "Caption";
            this.dluznikCaptionTextBox.Value = "D³u¿nik";
            // 
            // oddzialCaptionTextBox
            // 
            this.oddzialCaptionTextBox.CanGrow = true;
            this.oddzialCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.800000190734863D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.oddzialCaptionTextBox.Name = "oddzialCaptionTextBox";
            this.oddzialCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.199333667755127D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.oddzialCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.oddzialCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.oddzialCaptionTextBox.StyleName = "Caption";
            this.oddzialCaptionTextBox.Value = "Oddzia³";
            // 
            // pusteCaptionTextBox
            // 
            this.pusteCaptionTextBox.CanGrow = true;
            this.pusteCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.5D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.pusteCaptionTextBox.Name = "pusteCaptionTextBox";
            this.pusteCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.3998017311096191D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.pusteCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.pusteCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.pusteCaptionTextBox.StyleName = "Caption";
            this.pusteCaptionTextBox.Value = "Saldo na dzieñ";
            // 
            // labelsGroupFooter
            // 
            this.labelsGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D);
            this.labelsGroupFooter.Name = "labelsGroupFooter";
            this.labelsGroupFooter.Style.Visible = false;
            // 
            // labelsGroup
            // 
            this.labelsGroup.GroupFooter = this.labelsGroupFooter;
            this.labelsGroup.GroupHeader = this.labelsGroupHeader;
            this.labelsGroup.Name = "labelsGroup";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.728125274181366D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox,
            this.textBox1});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(26.099998474121094D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.73808318376541138D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.999998092651367D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.99999868869781494D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.StyleName = "PageInfo";
            this.textBox1.Value = "Strona";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.085625171661377D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.900001525878906D), Telerik.Reporting.Drawing.Unit.Cm(0.98552507162094116D));
            this.titleTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Wykaz spraw do potwierdzenia salda przed z³o¿eniem pozwu";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D);
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.0477594137191773D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.nrEwidDataTextBox,
            this.sygnaturaDataTextBox,
            this.dluznikDataTextBox,
            this.oddzialDataTextBox,
            this.textBox3,
            this.textBox5,
            this.textBox7});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // nrEwidDataTextBox
            // 
            this.nrEwidDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.nrEwidDataTextBox.CanGrow = true;
            this.nrEwidDataTextBox.CanShrink = true;
            this.nrEwidDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.nrEwidDataTextBox.Name = "nrEwidDataTextBox";
            this.nrEwidDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6856245994567871D), Telerik.Reporting.Drawing.Unit.Cm(1.047659158706665D));
            this.nrEwidDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.nrEwidDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.nrEwidDataTextBox.StyleName = "Data";
            this.nrEwidDataTextBox.Value = "=Fields.NrEwid";
            // 
            // sygnaturaDataTextBox
            // 
            this.sygnaturaDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.sygnaturaDataTextBox.CanGrow = true;
            this.sygnaturaDataTextBox.CanShrink = true;
            this.sygnaturaDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3937089443206787D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.sygnaturaDataTextBox.Name = "sygnaturaDataTextBox";
            this.sygnaturaDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(1.047659158706665D));
            this.sygnaturaDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.sygnaturaDataTextBox.StyleName = "Data";
            this.sygnaturaDataTextBox.Value = "=Fields.Sygnatura";
            // 
            // dluznikDataTextBox
            // 
            this.dluznikDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.dluznikDataTextBox.CanGrow = true;
            this.dluznikDataTextBox.CanShrink = true;
            this.dluznikDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.3047003746032715D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.dluznikDataTextBox.Name = "dluznikDataTextBox";
            this.dluznikDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.4950995445251465D), Telerik.Reporting.Drawing.Unit.Cm(1.047659158706665D));
            this.dluznikDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.dluznikDataTextBox.StyleName = "Data";
            this.dluznikDataTextBox.Value = "=Fields.Dluznik";
            // 
            // oddzialDataTextBox
            // 
            this.oddzialDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.oddzialDataTextBox.CanGrow = true;
            this.oddzialDataTextBox.CanShrink = true;
            this.oddzialDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.800000190734863D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.oddzialDataTextBox.Name = "oddzialDataTextBox";
            this.oddzialDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.199333667755127D), Telerik.Reporting.Drawing.Unit.Cm(1.047659158706665D));
            this.oddzialDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.oddzialDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.oddzialDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.oddzialDataTextBox.StyleName = "Data";
            this.oddzialDataTextBox.Value = "=Fields.Oddzial";
            // 
            // textBox3
            // 
            this.textBox3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox3.CanShrink = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.5232631983840292E-08D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.69980019330978394D), Telerik.Reporting.Drawing.Unit.Cm(1.0476592779159546D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Font.Name = "Tahoma";
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "= RowNumber()";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(26.946983337402344D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "Kancelaria Prawna";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.69980019330978394D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "Lp";
            // 
            // group1
            // 
            this.group1.GroupFooter = this.groupFooterSection1;
            this.group1.GroupHeader = this.groupHeaderSection1;
            this.group1.Groupings.AddRange(new Telerik.Reporting.Grouping[] {
            new Telerik.Reporting.Grouping("=Fields.Oddzial")});
            this.group1.Name = "group1";
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D);
            this.groupFooterSection1.Name = "groupFooterSection1";
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.7199486494064331D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox4,
            this.dluznikCaptionTextBox,
            this.pusteCaptionTextBox,
            this.sygnaturaCaptionTextBox,
            this.oddzialCaptionTextBox,
            this.nrEwidCaptionTextBox,
            this.textBox2,
            this.textBox6});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            // 
            // textBox4
            // 
            this.textBox4.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox4.CanGrow = true;
            this.textBox4.CanShrink = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.29187533259391785D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(26.647083282470703D), Telerik.Reporting.Drawing.Unit.Cm(0.629734218120575D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "=Fields.Oddzial";
            // 
            // textBox5
            // 
            this.textBox5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox5.CanShrink = true;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.5D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.3998017311096191D), Telerik.Reporting.Drawing.Unit.Cm(1.047659158706665D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox5.Value = "";
            // 
            // textBox6
            // 
            this.textBox6.CanGrow = true;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.999532699584961D), Telerik.Reporting.Drawing.Unit.Cm(1.0765928030014038D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.500267505645752D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.StyleName = "Caption";
            this.textBox6.Value = "Wartoœæ przedm. sporu";
            // 
            // textBox7
            // 
            this.textBox7.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox7.Format = "{0:C2}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.999532699584961D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.500267505645752D), Telerik.Reporting.Drawing.Unit.Cm(1.0476592779159546D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "= Fields.WPS";
            // 
            // SkierowaneSad
            // 
            this.DataSource = this.LexEnaDS;
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup,
            this.group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.groupHeaderSection1,
            this.groupFooterSection1,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AvailableValues.DataSource = this.sqlDataSource1;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.Oznaczenie";
            reportParameter1.AvailableValues.ValueMember = "= Fields.Id";
            reportParameter1.Name = "IdPaczki";
            reportParameter1.Text = "Wybierz paczkê z listy";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.BackgroundColor = System.Drawing.Color.Empty;
            styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            styleRule2.Style.Color = System.Drawing.Color.White;
            styleRule2.Style.Font.Bold = true;
            styleRule2.Style.Font.Italic = false;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            styleRule2.Style.Font.Strikeout = false;
            styleRule2.Style.Font.Underline = false;
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Color = System.Drawing.Color.Black;
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Color = System.Drawing.Color.Black;
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(26.999900817871094D);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource LexEnaDS;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
        private Telerik.Reporting.TextBox nrEwidCaptionTextBox;
        private Telerik.Reporting.TextBox sygnaturaCaptionTextBox;
        private Telerik.Reporting.TextBox dluznikCaptionTextBox;
        private Telerik.Reporting.TextBox oddzialCaptionTextBox;
        private Telerik.Reporting.TextBox pusteCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
        private Telerik.Reporting.Group labelsGroup;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox nrEwidDataTextBox;
        private Telerik.Reporting.TextBox sygnaturaDataTextBox;
        private Telerik.Reporting.TextBox dluznikDataTextBox;
        private Telerik.Reporting.TextBox oddzialDataTextBox;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private System.Diagnostics.EventLog eventLog1;
        private Telerik.Reporting.Group group1;
        private Telerik.Reporting.GroupFooterSection groupFooterSection1;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection1;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox6;

    }
}