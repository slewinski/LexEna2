namespace LexEnaReporting.SkierowanieSaldaById
{
    partial class SkierowaneDoSaduById
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkierowaneDoSaduById));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule6 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule7 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.SkierowaneByIdDds = new Telerik.Reporting.SqlDataSource();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.sygnaturaDataTextBox = new Telerik.Reporting.TextBox();
            this.oddzialDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.nrEwidDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.dluznikDataTextBox = new Telerik.Reporting.TextBox();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.nrEwidCaptionTextBox = new Telerik.Reporting.TextBox();
            this.sygnaturaCaptionTextBox = new Telerik.Reporting.TextBox();
            this.oddzialCaptionTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.dluznikCaptionTextBox = new Telerik.Reporting.TextBox();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // SkierowaneByIdDds
            // 
            this.SkierowaneByIdDds.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.SkierowaneByIdDds.Name = "SkierowaneByIdDds";
            this.SkierowaneByIdDds.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@StatusDok", System.Data.DbType.Int32, "=Parameters.StatusDok.Value")});
            this.SkierowaneByIdDds.SelectCommand = resources.GetString("SkierowaneByIdDds.SelectCommand");
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
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(-5.6458848707308107E-10D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(29.261159896850586D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "Kancelaria Prawna";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.3941669464111328D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.5D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.9612503051757812D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\'Strona: \' + PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.7856247425079346D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(-5.6458848707308107E-10D), Telerik.Reporting.Drawing.Unit.Cm(9.9922239314764738E-05D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(29.261159896850586D), Telerik.Reporting.Drawing.Unit.Cm(1.7854249477386475D));
            this.titleTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.titleTextBox.Style.Font.Bold = false;
            this.titleTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.titleTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Wykaz spraw do potwierdzenia salda przed z³o¿eniem pozwu";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(1.3381240367889404D);
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.sygnaturaDataTextBox,
            this.oddzialDataTextBox,
            this.textBox2,
            this.textBox5,
            this.nrEwidDataTextBox,
            this.textBox6,
            this.dluznikDataTextBox,
            this.textBox9});
            this.detail.Name = "detail";
            // 
            // sygnaturaDataTextBox
            // 
            this.sygnaturaDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.sygnaturaDataTextBox.CanGrow = true;
            this.sygnaturaDataTextBox.CanShrink = true;
            this.sygnaturaDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.sygnaturaDataTextBox.Name = "sygnaturaDataTextBox";
            this.sygnaturaDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6612582206726074D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.sygnaturaDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.sygnaturaDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.sygnaturaDataTextBox.StyleName = "Data";
            this.sygnaturaDataTextBox.Value = "=Fields.Sygnatura";
            // 
            // oddzialDataTextBox
            // 
            this.oddzialDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.oddzialDataTextBox.CanGrow = true;
            this.oddzialDataTextBox.CanShrink = true;
            this.oddzialDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.661470413208008D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.oddzialDataTextBox.Name = "oddzialDataTextBox";
            this.oddzialDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0383396148681641D), Telerik.Reporting.Drawing.Unit.Cm(0.9999997615814209D));
            this.oddzialDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.oddzialDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.oddzialDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.oddzialDataTextBox.StyleName = "Data";
            this.oddzialDataTextBox.Value = "=Fields.Oddzial";
            // 
            // textBox2
            // 
            this.textBox2.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox2.CanShrink = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(25.500009536743164D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.7612512111663818D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Name = "Tahoma";
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.Value = "   ";
            // 
            // textBox5
            // 
            this.textBox5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox5.CanShrink = true;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0468835830688477D), Telerik.Reporting.Drawing.Unit.Cm(0.99989980459213257D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Name = "Tahoma";
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "= RowNumber()";
            // 
            // nrEwidDataTextBox
            // 
            this.nrEwidDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.nrEwidDataTextBox.CanGrow = true;
            this.nrEwidDataTextBox.CanShrink = true;
            this.nrEwidDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.nrEwidDataTextBox.Name = "nrEwidDataTextBox";
            this.nrEwidDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6856245994567871D), Telerik.Reporting.Drawing.Unit.Cm(0.99989980459213257D));
            this.nrEwidDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.nrEwidDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.nrEwidDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.nrEwidDataTextBox.StyleName = "Data";
            this.nrEwidDataTextBox.Value = "=Fields.NrEwid";
            // 
            // textBox6
            // 
            this.textBox6.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox6.CanShrink = true;
            this.textBox6.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox6.Format = "{0:C2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.700010299682617D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.79979944229126D), Telerik.Reporting.Drawing.Unit.Cm(0.99989986419677734D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "=Fields.WPS";
            // 
            // dluznikDataTextBox
            // 
            this.dluznikDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.dluznikDataTextBox.CanGrow = true;
            this.dluznikDataTextBox.CanShrink = true;
            this.dluznikDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.2614660263061523D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.dluznikDataTextBox.Name = "dluznikDataTextBox";
            this.dluznikDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3997998237609863D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.dluznikDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.dluznikDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dluznikDataTextBox.StyleName = "Data";
            this.dluznikDataTextBox.Value = "=Fields.Dluznik";
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D);
            this.groupFooterSection1.Name = "groupFooterSection1";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0468835830688477D), Telerik.Reporting.Drawing.Unit.Cm(0.94750058650970459D));
            this.textBox4.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Color = System.Drawing.Color.White;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.StyleName = "Caption";
            this.textBox4.Value = "Lp";
            // 
            // textBox3
            // 
            this.textBox3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox3.CanGrow = true;
            this.textBox3.CanShrink = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(-5.6458848707308107E-10D), Telerik.Reporting.Drawing.Unit.Cm(0.00010010907135438174D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(29.261161804199219D), Telerik.Reporting.Drawing.Unit.Cm(1.5674078464508057D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox3.StyleName = "Data";
            this.textBox3.Value = "=Fields.Oddzial";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = true;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.700010299682617D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.7997989654541016D), Telerik.Reporting.Drawing.Unit.Cm(0.94750058650970459D));
            this.textBox7.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Color = System.Drawing.Color.White;
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox7.StyleName = "Caption";
            this.textBox7.Value = "Wartoœæ przedm. sporu";
            // 
            // nrEwidCaptionTextBox
            // 
            this.nrEwidCaptionTextBox.CanGrow = true;
            this.nrEwidCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.nrEwidCaptionTextBox.Name = "nrEwidCaptionTextBox";
            this.nrEwidCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6856245994567871D), Telerik.Reporting.Drawing.Unit.Cm(0.94750028848648071D));
            this.nrEwidCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.nrEwidCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.nrEwidCaptionTextBox.Style.Color = System.Drawing.Color.White;
            this.nrEwidCaptionTextBox.Style.Font.Bold = true;
            this.nrEwidCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.nrEwidCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.nrEwidCaptionTextBox.StyleName = "Caption";
            this.nrEwidCaptionTextBox.Value = "Nr kontrahenta z bilingu";
            // 
            // sygnaturaCaptionTextBox
            // 
            this.sygnaturaCaptionTextBox.CanGrow = true;
            this.sygnaturaCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.sygnaturaCaptionTextBox.Name = "sygnaturaCaptionTextBox";
            this.sygnaturaCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6612582206726074D), Telerik.Reporting.Drawing.Unit.Cm(0.94750058650970459D));
            this.sygnaturaCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.sygnaturaCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaCaptionTextBox.Style.Color = System.Drawing.Color.White;
            this.sygnaturaCaptionTextBox.Style.Font.Bold = true;
            this.sygnaturaCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.sygnaturaCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.sygnaturaCaptionTextBox.StyleName = "Caption";
            this.sygnaturaCaptionTextBox.Value = "Nr sprawy z Wieny";
            // 
            // oddzialCaptionTextBox
            // 
            this.oddzialCaptionTextBox.CanGrow = true;
            this.oddzialCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.661470413208008D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.oddzialCaptionTextBox.Name = "oddzialCaptionTextBox";
            this.oddzialCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0383396148681641D), Telerik.Reporting.Drawing.Unit.Cm(0.94750058650970459D));
            this.oddzialCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.oddzialCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.oddzialCaptionTextBox.Style.Color = System.Drawing.Color.White;
            this.oddzialCaptionTextBox.Style.Font.Bold = true;
            this.oddzialCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.oddzialCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.oddzialCaptionTextBox.StyleName = "Caption";
            this.oddzialCaptionTextBox.Value = "Oddzia³";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(25.500009536743164D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.7612507343292236D), Telerik.Reporting.Drawing.Unit.Cm(0.94750058650970459D));
            this.textBox1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.Color = System.Drawing.Color.White;
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "Saldo na dzieñ";
            // 
            // dluznikCaptionTextBox
            // 
            this.dluznikCaptionTextBox.CanGrow = true;
            this.dluznikCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.2614660263061523D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.dluznikCaptionTextBox.Name = "dluznikCaptionTextBox";
            this.dluznikCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3997998237609863D), Telerik.Reporting.Drawing.Unit.Cm(0.94750058650970459D));
            this.dluznikCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.dluznikCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikCaptionTextBox.Style.Color = System.Drawing.Color.White;
            this.dluznikCaptionTextBox.Style.Font.Bold = true;
            this.dluznikCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.dluznikCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dluznikCaptionTextBox.StyleName = "Caption";
            this.dluznikCaptionTextBox.Value = "Imiê Nazwisko";
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5677087306976318D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.dluznikCaptionTextBox,
            this.textBox1,
            this.oddzialCaptionTextBox,
            this.sygnaturaCaptionTextBox,
            this.nrEwidCaptionTextBox,
            this.textBox7,
            this.textBox3,
            this.textBox4,
            this.textBox8});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            // 
            // textBox8
            // 
            this.textBox8.CanGrow = true;
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.0470836162567139D), Telerik.Reporting.Drawing.Unit.Cm(1.5677082538604736D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8527162075042725D), Telerik.Reporting.Drawing.Unit.Cm(0.94750028848648071D));
            this.textBox8.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Color = System.Drawing.Color.White;
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox8.StyleName = "Caption";
            this.textBox8.Value = "Nasz nr sprawy(kancelarii)";
            // 
            // textBox9
            // 
            this.textBox9.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox9.CanShrink = true;
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.0470833778381348D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8527164459228516D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.Font.Name = "Tahoma";
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.Value = "   ";
            // 
            // SkierowaneDoSaduById
            // 
            this.DataSource = this.SkierowaneByIdDds;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.Id", Telerik.Reporting.FilterOperator.In, "=Parameters.IdDokWys.Value"));
            group1.GroupFooter = this.groupFooterSection1;
            group1.GroupHeader = this.groupHeaderSection1;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.Oddzial"));
            group1.Name = "Oddzial";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail,
            this.groupHeaderSection1,
            this.groupFooterSection1});
            this.Name = "SkierowaneDoSaduById";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "StatusDok";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "IdDokWys";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.Color = System.Drawing.Color.Black;
            styleRule1.Style.Font.Bold = true;
            styleRule1.Style.Font.Italic = false;
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(20D);
            styleRule1.Style.Font.Strikeout = false;
            styleRule1.Style.Font.Underline = false;
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Normal.TableNormal")});
            styleRule5.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule5.Style.Color = System.Drawing.Color.Black;
            styleRule5.Style.Font.Name = "Tahoma";
            styleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableBody")});
            styleRule6.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule6.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule6.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule6.Style.Font.Name = "Tahoma";
            styleRule6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableHeader")});
            styleRule7.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule7.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule7.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule7.Style.Font.Name = "Tahoma";
            styleRule7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5,
            styleRule6,
            styleRule7});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(29.261260986328125D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource SkierowaneByIdDds;
        private Telerik.Reporting.Group labelsGroup;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.Group group1;
        private Telerik.Reporting.TextBox sygnaturaDataTextBox;
        private Telerik.Reporting.TextBox oddzialDataTextBox;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox nrEwidDataTextBox;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox dluznikDataTextBox;
        private Telerik.Reporting.GroupFooterSection groupFooterSection1;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox nrEwidCaptionTextBox;
        private Telerik.Reporting.TextBox sygnaturaCaptionTextBox;
        private Telerik.Reporting.TextBox oddzialCaptionTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox dluznikCaptionTextBox;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection1;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
    }
}