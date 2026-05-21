namespace LexEnaReporting.TerminowoscKlauzuli
{
    partial class TerminowoscKlauzuli
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminowoscKlauzuli));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.UsersListDS = new Telerik.Reporting.SqlDataSource();
            this.TerminowoscKlauzuliDS = new Telerik.Reporting.SqlDataSource();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.sygnaturaCaptionTextBox = new Telerik.Reporting.TextBox();
            this.dluznikCaptionTextBox = new Telerik.Reporting.TextBox();
            this.dataDekretacjiJednostkiCaptionTextBox = new Telerik.Reporting.TextBox();
            this.uzytkownik_NazwaCaptionTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroup = new Telerik.Reporting.Group();
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
            this.dluznikDataTextBox = new Telerik.Reporting.TextBox();
            this.dataDekretacjiJednostkiDataTextBox = new Telerik.Reporting.TextBox();
            this.uzytkownik_NazwaDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // UsersListDS
            // 
            this.UsersListDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.UsersListDS.Name = "TerminowoscKlauzuliDS";
            this.UsersListDS.SelectCommand = "SELECT        Id, Nazwisko + \'  \' + Imie AS Nazwa\r\nFROM            Uzytkownik\r\n\r\n" +
    "UNION\r\nselect 0 , \' <Wszyscy> \'\r\nORDER BY 1";
            // 
            // TerminowoscKlauzuliDS
            // 
            this.TerminowoscKlauzuliDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.TerminowoscKlauzuliDS.Name = "TerminowoscKlauzuliDS";
            this.TerminowoscKlauzuliDS.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@DataOd", System.Data.DbType.DateTime, "=Parameters.DataOd.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@DataDo", System.Data.DbType.DateTime, "=Parameters.DataDo.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@User", System.Data.DbType.Int32, "=Parameters.User.Value")});
            this.TerminowoscKlauzuliDS.SelectCommand = resources.GetString("TerminowoscKlauzuliDS.SelectCommand");
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.0316746234893799D);
            this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.sygnaturaCaptionTextBox,
            this.dluznikCaptionTextBox,
            this.dataDekretacjiJednostkiCaptionTextBox,
            this.uzytkownik_NazwaCaptionTextBox,
            this.textBox1,
            this.textBox4,
            this.textBox7,
            this.textBox3,
            this.textBox10});
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
            // 
            // sygnaturaCaptionTextBox
            // 
            this.sygnaturaCaptionTextBox.CanGrow = true;
            this.sygnaturaCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.95249998569488525D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.sygnaturaCaptionTextBox.Name = "sygnaturaCaptionTextBox";
            this.sygnaturaCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0502083301544189D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.sygnaturaCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaCaptionTextBox.Style.Font.Bold = true;
            this.sygnaturaCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.sygnaturaCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.sygnaturaCaptionTextBox.StyleName = "Caption";
            this.sygnaturaCaptionTextBox.Value = "Sygnatura";
            // 
            // dluznikCaptionTextBox
            // 
            this.dluznikCaptionTextBox.CanGrow = true;
            this.dluznikCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.0029087066650391D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.dluznikCaptionTextBox.Name = "dluznikCaptionTextBox";
            this.dluznikCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.9602246284484863D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.dluznikCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikCaptionTextBox.Style.Font.Bold = true;
            this.dluznikCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.dluznikCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dluznikCaptionTextBox.StyleName = "Caption";
            this.dluznikCaptionTextBox.Value = "D³u¿nik";
            // 
            // dataDekretacjiJednostkiCaptionTextBox
            // 
            this.dataDekretacjiJednostkiCaptionTextBox.CanGrow = true;
            this.dataDekretacjiJednostkiCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9708328247070312D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.dataDekretacjiJednostkiCaptionTextBox.Name = "dataDekretacjiJednostkiCaptionTextBox";
            this.dataDekretacjiJednostkiCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5739583969116211D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.dataDekretacjiJednostkiCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dataDekretacjiJednostkiCaptionTextBox.Style.Font.Bold = true;
            this.dataDekretacjiJednostkiCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.dataDekretacjiJednostkiCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dataDekretacjiJednostkiCaptionTextBox.StyleName = "Caption";
            this.dataDekretacjiJednostkiCaptionTextBox.Value = "Data dekretacji  do kanc.";
            // 
            // uzytkownik_NazwaCaptionTextBox
            // 
            this.uzytkownik_NazwaCaptionTextBox.CanGrow = true;
            this.uzytkownik_NazwaCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.25196647644043D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.uzytkownik_NazwaCaptionTextBox.Name = "uzytkownik_NazwaCaptionTextBox";
            this.uzytkownik_NazwaCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.4555354118347168D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.uzytkownik_NazwaCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.uzytkownik_NazwaCaptionTextBox.Style.Font.Bold = true;
            this.uzytkownik_NazwaCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.uzytkownik_NazwaCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.uzytkownik_NazwaCaptionTextBox.StyleName = "Caption";
            this.uzytkownik_NazwaCaptionTextBox.Value = "Referent";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.95230013132095337D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "Lp";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.563650131225586D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5739583969116211D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.StyleName = "Caption";
            this.textBox4.Value = "Data z³o¿enia pozwu";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = true;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.137807846069336D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1110503673553467D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox7.StyleName = "Caption";
            this.textBox7.Value = "Dni";
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.249057769775391D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5739583969116211D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.StyleName = "Caption";
            this.textBox3.Value = "Data nakazu";
            // 
            // textBox10
            // 
            this.textBox10.CanGrow = true;
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.849674224853516D), Telerik.Reporting.Drawing.Unit.Cm(0.03469930961728096D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.4020919799804688D), Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.StyleName = "Caption";
            this.textBox10.Value = "Dni         Poz-Nak";
            // 
            // labelsGroupFooter
            // 
            this.labelsGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.labelsGroupFooter.Name = "labelsGroupFooter";
            this.labelsGroupFooter.Style.Visible = false;
            // 
            // labelsGroup
            // 
            this.labelsGroup.GroupFooter = this.labelsGroupFooter;
            this.labelsGroup.GroupHeader = this.labelsGroupHeader;
            this.labelsGroup.Name = "labelsGroup";
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
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.708333015441895D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "System LexEna";
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
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.172817230224609D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1004166603088379D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(26.318023681640625D), Telerik.Reporting.Drawing.Unit.Cm(1.0475000143051148D));
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Terminowoœæ wydawania klauzul";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.9439583420753479D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.sygnaturaDataTextBox,
            this.dluznikDataTextBox,
            this.dataDekretacjiJednostkiDataTextBox,
            this.uzytkownik_NazwaDataTextBox,
            this.textBox2,
            this.textBox6,
            this.textBox8,
            this.textBox5,
            this.textBox9});
            this.detail.Name = "detail";
            // 
            // sygnaturaDataTextBox
            // 
            this.sygnaturaDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.sygnaturaDataTextBox.CanGrow = true;
            this.sygnaturaDataTextBox.CanShrink = true;
            this.sygnaturaDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.95249998569488525D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.sygnaturaDataTextBox.Name = "sygnaturaDataTextBox";
            this.sygnaturaDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0502083301544189D), Telerik.Reporting.Drawing.Unit.Cm(0.94385820627212524D));
            this.sygnaturaDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.sygnaturaDataTextBox.StyleName = "Data";
            this.sygnaturaDataTextBox.Value = "=Fields.sygnatura";
            // 
            // dluznikDataTextBox
            // 
            this.dluznikDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.dluznikDataTextBox.CanGrow = true;
            this.dluznikDataTextBox.CanShrink = true;
            this.dluznikDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.0029087066650391D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.dluznikDataTextBox.Name = "dluznikDataTextBox";
            this.dluznikDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.9602246284484863D), Telerik.Reporting.Drawing.Unit.Cm(0.94385820627212524D));
            this.dluznikDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.dluznikDataTextBox.StyleName = "Data";
            this.dluznikDataTextBox.Value = "=Fields.Dluznik";
            // 
            // dataDekretacjiJednostkiDataTextBox
            // 
            this.dataDekretacjiJednostkiDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.dataDekretacjiJednostkiDataTextBox.CanGrow = true;
            this.dataDekretacjiJednostkiDataTextBox.CanShrink = true;
            this.dataDekretacjiJednostkiDataTextBox.Format = "{0:d}";
            this.dataDekretacjiJednostkiDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9708328247070312D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.dataDekretacjiJednostkiDataTextBox.Name = "dataDekretacjiJednostkiDataTextBox";
            this.dataDekretacjiJednostkiDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5739583969116211D), Telerik.Reporting.Drawing.Unit.Cm(0.94385814666748047D));
            this.dataDekretacjiJednostkiDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dataDekretacjiJednostkiDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.dataDekretacjiJednostkiDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dataDekretacjiJednostkiDataTextBox.StyleName = "Data";
            this.dataDekretacjiJednostkiDataTextBox.Value = "= Fields.DataDekretacjiJednostki";
            // 
            // uzytkownik_NazwaDataTextBox
            // 
            this.uzytkownik_NazwaDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.uzytkownik_NazwaDataTextBox.CanGrow = true;
            this.uzytkownik_NazwaDataTextBox.CanShrink = true;
            this.uzytkownik_NazwaDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.25196647644043D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.uzytkownik_NazwaDataTextBox.Name = "uzytkownik_NazwaDataTextBox";
            this.uzytkownik_NazwaDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.4555354118347168D), Telerik.Reporting.Drawing.Unit.Cm(0.94385820627212524D));
            this.uzytkownik_NazwaDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.uzytkownik_NazwaDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.uzytkownik_NazwaDataTextBox.StyleName = "Data";
            this.uzytkownik_NazwaDataTextBox.Value = "=Fields.Uzytkownik_Nazwa";
            // 
            // textBox2
            // 
            this.textBox2.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox2.CanGrow = true;
            this.textBox2.CanShrink = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.95230013132095337D), Telerik.Reporting.Drawing.Unit.Cm(0.94385820627212524D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "=RowNumber()";
            // 
            // textBox6
            // 
            this.textBox6.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox6.CanGrow = true;
            this.textBox6.CanShrink = true;
            this.textBox6.Format = "{0:d}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.563650131225586D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5739583969116211D), Telerik.Reporting.Drawing.Unit.Cm(0.94385820627212524D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.StyleName = "Data";
            this.textBox6.Value = "= Fields.DataZloPozwu";
            // 
            // textBox8
            // 
            this.textBox8.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox8.CanGrow = true;
            this.textBox8.CanShrink = true;
            this.textBox8.Format = "{0:d}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.137807846069336D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1110503673553467D), Telerik.Reporting.Drawing.Unit.Cm(0.94385814666748047D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox8.StyleName = "Data";
            this.textBox8.Value = "= Fields.DniDoPozwu";
            // 
            // textBox5
            // 
            this.textBox5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox5.CanGrow = true;
            this.textBox5.CanShrink = true;
            this.textBox5.Format = "{0:d}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.271875381469727D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5739583969116211D), Telerik.Reporting.Drawing.Unit.Cm(0.94385820627212524D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.StyleName = "Data";
            this.textBox5.Value = "= Fields.DataNakazu.Date";
            // 
            // textBox9
            // 
            this.textBox9.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox9.CanGrow = true;
            this.textBox9.CanShrink = true;
            this.textBox9.Format = "{0:d}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.846033096313477D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.405733585357666D), Telerik.Reporting.Drawing.Unit.Cm(0.94385814666748047D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.StyleName = "Data";
            this.textBox9.Value = "= Fields.DniDoNakazu";
            // 
            // TerminowoscKlauzuli
            // 
            this.DataSource = this.TerminowoscKlauzuliDS;
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(15D);
            this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(15D);
            this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(15D);
            this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(15D);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "DataOd";
            reportParameter1.Text = "Skierowane do kanc.  w okresie od:";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter1.Value = "= Now()";
            reportParameter1.Visible = true;
            reportParameter2.Name = "DataDo";
            reportParameter2.Text = "do:";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Value = "= CDate(Now())";
            reportParameter2.Visible = true;
            reportParameter3.AvailableValues.DataSource = this.UsersListDS;
            reportParameter3.AvailableValues.DisplayMember = "= Fields.Nazwa";
            reportParameter3.AvailableValues.ValueMember = "= Fields.Id";
            reportParameter3.Name = "User";
            reportParameter3.Text = "U¿ytkownik";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter3.Value = "= Fields.Id";
            reportParameter3.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.Style.BackgroundColor = System.Drawing.Color.White;
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
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(26.707500457763672D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource TerminowoscKlauzuliDS;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
        private Telerik.Reporting.TextBox sygnaturaCaptionTextBox;
        private Telerik.Reporting.TextBox dluznikCaptionTextBox;
        private Telerik.Reporting.TextBox dataDekretacjiJednostkiCaptionTextBox;
        private Telerik.Reporting.TextBox uzytkownik_NazwaCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
        private Telerik.Reporting.Group labelsGroup;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox sygnaturaDataTextBox;
        private Telerik.Reporting.TextBox dluznikDataTextBox;
        private Telerik.Reporting.TextBox dataDekretacjiJednostkiDataTextBox;
        private Telerik.Reporting.TextBox uzytkownik_NazwaDataTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.SqlDataSource UsersListDS;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox9;

    }
}