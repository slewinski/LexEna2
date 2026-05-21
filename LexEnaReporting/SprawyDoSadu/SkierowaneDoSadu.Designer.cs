namespace LexEnaReporting.SprawyDoSadu
{
    partial class SkierowaneDoSadu
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkierowaneDoSadu));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.SkierowaneDoSaduDS = new Telerik.Reporting.SqlDataSource();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroup = new Telerik.Reporting.Group();
            this.sygnaturaCaptionTextBox = new Telerik.Reporting.TextBox();
            this.nrEwidCaptionTextBox = new Telerik.Reporting.TextBox();
            this.dluznikCaptionTextBox = new Telerik.Reporting.TextBox();
            this.adresCaptionTextBox = new Telerik.Reporting.TextBox();
            this.wPSCaptionTextBox = new Telerik.Reporting.TextBox();
            this.notyOdsetkoweCaptionTextBox = new Telerik.Reporting.TextBox();
            this.dataDokCaptionTextBox = new Telerik.Reporting.TextBox();
            this.radcaCaptionTextBox = new Telerik.Reporting.TextBox();
            this.oddzialGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.oddzialDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.oddzialGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.wPSDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.oddzialGroup = new Telerik.Reporting.Group();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.sygnaturaDataTextBox = new Telerik.Reporting.TextBox();
            this.nrEwidDataTextBox = new Telerik.Reporting.TextBox();
            this.dluznikDataTextBox = new Telerik.Reporting.TextBox();
            this.adresDataTextBox = new Telerik.Reporting.TextBox();
            this.notyOdsetkoweDataTextBox = new Telerik.Reporting.TextBox();
            this.dataDokDataTextBox = new Telerik.Reporting.TextBox();
            this.radcaDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.textBox22 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // SkierowaneDoSaduDS
            // 
            this.SkierowaneDoSaduDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.SkierowaneDoSaduDS.Name = "SkierowaneDoSaduDS";
            this.SkierowaneDoSaduDS.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@IdPaczki", System.Data.DbType.Int32, "=Parameters.IdPaczki.Value")});
            this.SkierowaneDoSaduDS.SelectCommand = resources.GetString("SkierowaneDoSaduDS.SelectCommand");
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
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
            // sygnaturaCaptionTextBox
            // 
            this.sygnaturaCaptionTextBox.CanGrow = true;
            this.sygnaturaCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.sygnaturaCaptionTextBox.Name = "sygnaturaCaptionTextBox";
            this.sygnaturaCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4114654064178467D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.sygnaturaCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.sygnaturaCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.sygnaturaCaptionTextBox.StyleName = "Caption";
            this.sygnaturaCaptionTextBox.Value = "Sygnatura";
            // 
            // nrEwidCaptionTextBox
            // 
            this.nrEwidCaptionTextBox.CanGrow = true;
            this.nrEwidCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3116655349731445D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.nrEwidCaptionTextBox.Name = "nrEwidCaptionTextBox";
            this.nrEwidCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9883348941802979D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.nrEwidCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.nrEwidCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.nrEwidCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.nrEwidCaptionTextBox.StyleName = "Caption";
            this.nrEwidCaptionTextBox.Value = "Nr ewidencyjny";
            // 
            // dluznikCaptionTextBox
            // 
            this.dluznikCaptionTextBox.CanGrow = true;
            this.dluznikCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3002004623413086D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.dluznikCaptionTextBox.Name = "dluznikCaptionTextBox";
            this.dluznikCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4997994899749756D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.dluznikCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.dluznikCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dluznikCaptionTextBox.StyleName = "Caption";
            this.dluznikCaptionTextBox.Value = "Pozwany";
            // 
            // adresCaptionTextBox
            // 
            this.adresCaptionTextBox.CanGrow = true;
            this.adresCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.8002004623413086D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.adresCaptionTextBox.Name = "adresCaptionTextBox";
            this.adresCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6998002529144287D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.adresCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.adresCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.adresCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.adresCaptionTextBox.StyleName = "Caption";
            this.adresCaptionTextBox.Value = "Adres pozwanego";
            // 
            // wPSCaptionTextBox
            // 
            this.wPSCaptionTextBox.CanGrow = true;
            this.wPSCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.500199317932129D), Telerik.Reporting.Drawing.Unit.Cm(1.4605942964553833D));
            this.wPSCaptionTextBox.Name = "wPSCaptionTextBox";
            this.wPSCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.1039772033691406D), Telerik.Reporting.Drawing.Unit.Cm(0.89999985694885254D));
            this.wPSCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.wPSCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.wPSCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.wPSCaptionTextBox.StyleName = "Caption";
            this.wPSCaptionTextBox.Value = "Nale¿noœæ g³ówna";
            // 
            // notyOdsetkoweCaptionTextBox
            // 
            this.notyOdsetkoweCaptionTextBox.CanGrow = true;
            this.notyOdsetkoweCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.600200653076172D), Telerik.Reporting.Drawing.Unit.Cm(1.4605942964553833D));
            this.notyOdsetkoweCaptionTextBox.Name = "notyOdsetkoweCaptionTextBox";
            this.notyOdsetkoweCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1997995376586914D), Telerik.Reporting.Drawing.Unit.Cm(0.89999985694885254D));
            this.notyOdsetkoweCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.notyOdsetkoweCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.notyOdsetkoweCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.notyOdsetkoweCaptionTextBox.StyleName = "Caption";
            this.notyOdsetkoweCaptionTextBox.Value = "W tym noty odsetkowe";
            // 
            // dataDokCaptionTextBox
            // 
            this.dataDokCaptionTextBox.CanGrow = true;
            this.dataDokCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.5D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.dataDokCaptionTextBox.Name = "dataDokCaptionTextBox";
            this.dataDokCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8999990224838257D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.dataDokCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dataDokCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.dataDokCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dataDokCaptionTextBox.StyleName = "Caption";
            this.dataDokCaptionTextBox.Value = "Data wys³ania pozwu";
            // 
            // radcaCaptionTextBox
            // 
            this.radcaCaptionTextBox.CanGrow = true;
            this.radcaCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.400001525878906D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.radcaCaptionTextBox.Name = "radcaCaptionTextBox";
            this.radcaCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9468827247619629D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.radcaCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.radcaCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.radcaCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.radcaCaptionTextBox.StyleName = "Caption";
            this.radcaCaptionTextBox.Value = "Pe³nomocnik";
            // 
            // oddzialGroupHeader
            // 
            this.oddzialGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(2.3856251239776611D);
            this.oddzialGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.oddzialDataTextBox,
            this.textBox1,
            this.textBox2,
            this.radcaCaptionTextBox,
            this.sygnaturaCaptionTextBox,
            this.nrEwidCaptionTextBox,
            this.dluznikCaptionTextBox,
            this.adresCaptionTextBox,
            this.wPSCaptionTextBox,
            this.notyOdsetkoweCaptionTextBox,
            this.dataDokCaptionTextBox,
            this.textBox10,
            this.textBox15,
            this.textBox16,
            this.textBox17,
            this.textBox20,
            this.textBox21});
            this.oddzialGroupHeader.Name = "oddzialGroupHeader";
            // 
            // oddzialDataTextBox
            // 
            this.oddzialDataTextBox.CanGrow = true;
            this.oddzialDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.1002004146575928D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.oddzialDataTextBox.Name = "oddzialDataTextBox";
            this.oddzialDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(20.488334655761719D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.oddzialDataTextBox.Style.Font.Bold = true;
            this.oddzialDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.oddzialDataTextBox.StyleName = "Data";
            this.oddzialDataTextBox.Value = "=Fields.Oddzial";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.Value = "Oddzia³:";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.88562482595443726D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.84708327054977417D), Telerik.Reporting.Drawing.Unit.Cm(1.4854247570037842D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "Lp";
            // 
            // textBox10
            // 
            this.textBox10.CanGrow = true;
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.504575729370117D), Telerik.Reporting.Drawing.Unit.Cm(0.88562500476837158D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.0952239036560059D), Telerik.Reporting.Drawing.Unit.Cm(0.585224986076355D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.StyleName = "Caption";
            this.textBox10.Value = "Nale¿noœæ dochodzona w pozwie";
            // 
            // textBox15
            // 
            this.textBox15.CanGrow = true;
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0.88562500476837158D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8995981216430664D), Telerik.Reporting.Drawing.Unit.Cm(0.585224986076355D));
            this.textBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox15.StyleName = "Caption";
            this.textBox15.Value = "Koszty";
            // 
            // textBox16
            // 
            this.textBox16.CanGrow = true;
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(1.4605944156646729D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6997992992401123D), Telerik.Reporting.Drawing.Unit.Cm(0.89999985694885254D));
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.StyleName = "Caption";
            this.textBox16.Value = "Op³ata s¹dowa";
            // 
            // textBox17
            // 
            this.textBox17.CanGrow = true;
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(21.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(1.4605944156646729D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1995989084243774D), Telerik.Reporting.Drawing.Unit.Cm(0.89999985694885254D));
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.StyleName = "Caption";
            this.textBox17.Value = "Inne";
            // 
            // oddzialGroupFooter
            // 
            this.oddzialGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.68801677227020264D);
            this.oddzialGroupFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.wPSDataTextBox,
            this.textBox5,
            this.textBox6,
            this.textBox23});
            this.oddzialGroupFooter.Name = "oddzialGroupFooter";
            this.oddzialGroupFooter.PageBreak = Telerik.Reporting.PageBreak.After;
            // 
            // wPSDataTextBox
            // 
            this.wPSDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.wPSDataTextBox.CanGrow = true;
            this.wPSDataTextBox.CanShrink = true;
            this.wPSDataTextBox.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.wPSDataTextBox.Format = "{0:C2}";
            this.wPSDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.499999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.00010052680590888485D));
            this.wPSDataTextBox.Name = "wPSDataTextBox";
            this.wPSDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0998013019561768D), Telerik.Reporting.Drawing.Unit.Cm(0.58791589736938477D));
            this.wPSDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.wPSDataTextBox.Style.Font.Bold = true;
            this.wPSDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.wPSDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.wPSDataTextBox.StyleName = "Data";
            this.wPSDataTextBox.Value = "= Sum(Fields.WPS)";
            // 
            // textBox5
            // 
            this.textBox5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox5.CanGrow = true;
            this.textBox5.CanShrink = true;
            this.textBox5.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox5.Format = "{0:C2}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1997995376586914D), Telerik.Reporting.Drawing.Unit.Cm(0.58791589736938477D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.StyleName = "Data";
            this.textBox5.Value = "= Sum(Fields.NotyOdsetkowe)";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.00010052680590888485D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.58791667222976685D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "Razem";
            // 
            // oddzialGroup
            // 
            this.oddzialGroup.GroupFooter = this.oddzialGroupFooter;
            this.oddzialGroup.GroupHeader = this.oddzialGroupHeader;
            this.oddzialGroup.Groupings.AddRange(new Telerik.Reporting.Grouping[] {
            new Telerik.Reporting.Grouping("=Fields.Oddzial")});
            this.oddzialGroup.Name = "oddzialGroup";
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
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(26.147085189819336D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "Kancelaria Prawna";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox,
            this.textBox14});
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
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(26.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.071250490844249725D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66124999523162842D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.200000762939453D), Telerik.Reporting.Drawing.Unit.Cm(0.10000035166740418D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9999990463256836D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.Value = "Strona";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.98562496900558472D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(26.999898910522461D), Telerik.Reporting.Drawing.Unit.Cm(0.98562496900558472D));
            this.titleTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Wykaz spraw skierowanych do s¹du";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.78228425979614258D);
            this.reportFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox8,
            this.textBox7,
            this.textBox9,
            this.textBox11,
            this.textBox12,
            this.textBox13});
            this.reportFooter.Name = "reportFooter";
            // 
            // textBox8
            // 
            this.textBox8.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox8.CanGrow = true;
            this.textBox8.CanShrink = true;
            this.textBox8.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox8.Format = "{0:C2}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1997995376586914D), Telerik.Reporting.Drawing.Unit.Cm(0.65582519769668579D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.StyleName = "Data";
            this.textBox8.Value = "= Sum(Fields.NotyOdsetkowe)";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.09999942779541D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.58791667222976685D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "Razem:";
            // 
            // textBox9
            // 
            this.textBox9.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox9.CanGrow = true;
            this.textBox9.CanShrink = true;
            this.textBox9.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox9.Format = "{0:C2}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.504375457763672D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0998013019561768D), Telerik.Reporting.Drawing.Unit.Cm(0.65582519769668579D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.Font.Bold = true;
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.StyleName = "Data";
            this.textBox9.Value = "= Sum(Fields.WPS)";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.40000057220459D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0000003576278687D), Telerik.Reporting.Drawing.Unit.Cm(0.59989959001541138D));
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "= Count(1)";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.2999992370605469D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.58791667222976685D));
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.Value = "Pozwów:";
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.58791667222976685D));
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox13.Value = "Na kwotê";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1119825839996338D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.sygnaturaDataTextBox,
            this.nrEwidDataTextBox,
            this.dluznikDataTextBox,
            this.adresDataTextBox,
            this.notyOdsetkoweDataTextBox,
            this.dataDokDataTextBox,
            this.radcaDataTextBox,
            this.textBox3,
            this.textBox4,
            this.textBox18,
            this.textBox19,
            this.textBox22});
            this.detail.Name = "detail";
            // 
            // sygnaturaDataTextBox
            // 
            this.sygnaturaDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.sygnaturaDataTextBox.CanGrow = true;
            this.sygnaturaDataTextBox.CanShrink = true;
            this.sygnaturaDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.sygnaturaDataTextBox.Name = "sygnaturaDataTextBox";
            this.sygnaturaDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4114654064178467D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.sygnaturaDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.sygnaturaDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.sygnaturaDataTextBox.StyleName = "Data";
            this.sygnaturaDataTextBox.Value = "=Fields.Sygnatura";
            // 
            // nrEwidDataTextBox
            // 
            this.nrEwidDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.nrEwidDataTextBox.CanGrow = true;
            this.nrEwidDataTextBox.CanShrink = true;
            this.nrEwidDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3116655349731445D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.nrEwidDataTextBox.Name = "nrEwidDataTextBox";
            this.nrEwidDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9883348941802979D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.nrEwidDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.nrEwidDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.nrEwidDataTextBox.StyleName = "Data";
            this.nrEwidDataTextBox.Value = "=Fields.NrEwid";
            // 
            // dluznikDataTextBox
            // 
            this.dluznikDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.dluznikDataTextBox.CanGrow = true;
            this.dluznikDataTextBox.CanShrink = true;
            this.dluznikDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3002004623413086D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.dluznikDataTextBox.Name = "dluznikDataTextBox";
            this.dluznikDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4997994899749756D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.dluznikDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dluznikDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.dluznikDataTextBox.StyleName = "Data";
            this.dluznikDataTextBox.Value = "=Fields.Dluznik";
            // 
            // adresDataTextBox
            // 
            this.adresDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.adresDataTextBox.CanGrow = true;
            this.adresDataTextBox.CanShrink = true;
            this.adresDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.8002004623413086D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.adresDataTextBox.Name = "adresDataTextBox";
            this.adresDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6998002529144287D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.adresDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.adresDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.adresDataTextBox.StyleName = "Data";
            this.adresDataTextBox.Value = "=Fields.adres";
            // 
            // notyOdsetkoweDataTextBox
            // 
            this.notyOdsetkoweDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.notyOdsetkoweDataTextBox.CanGrow = true;
            this.notyOdsetkoweDataTextBox.CanShrink = true;
            this.notyOdsetkoweDataTextBox.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.notyOdsetkoweDataTextBox.Format = "{0:C2}";
            this.notyOdsetkoweDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.notyOdsetkoweDataTextBox.Name = "notyOdsetkoweDataTextBox";
            this.notyOdsetkoweDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1997995376586914D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.notyOdsetkoweDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.notyOdsetkoweDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.notyOdsetkoweDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.notyOdsetkoweDataTextBox.StyleName = "Data";
            this.notyOdsetkoweDataTextBox.Value = "=Fields.NotyOdsetkowe";
            // 
            // dataDokDataTextBox
            // 
            this.dataDokDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.dataDokDataTextBox.CanGrow = true;
            this.dataDokDataTextBox.CanShrink = true;
            this.dataDokDataTextBox.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.dataDokDataTextBox.Format = "{0:d}";
            this.dataDokDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.5D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.dataDokDataTextBox.Name = "dataDokDataTextBox";
            this.dataDokDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8999990224838257D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.dataDokDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.dataDokDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.dataDokDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.dataDokDataTextBox.StyleName = "Data";
            this.dataDokDataTextBox.Value = "=Fields.DataDok";
            // 
            // radcaDataTextBox
            // 
            this.radcaDataTextBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.radcaDataTextBox.CanGrow = true;
            this.radcaDataTextBox.CanShrink = true;
            this.radcaDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.400001525878906D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.radcaDataTextBox.Name = "radcaDataTextBox";
            this.radcaDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9468827247619629D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.radcaDataTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.radcaDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.radcaDataTextBox.StyleName = "Data";
            this.radcaDataTextBox.Value = "=Fields.radca";
            // 
            // textBox3
            // 
            this.textBox3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox3.CanShrink = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.84708327054977417D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "= RowNumber()";
            // 
            // textBox4
            // 
            this.textBox4.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox4.CanGrow = true;
            this.textBox4.CanShrink = true;
            this.textBox4.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox4.Format = "{0:C2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.499999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0998013019561768D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "=Fields.WPS";
            // 
            // textBox18
            // 
            this.textBox18.Format = "{0:C2}";
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6997992992401123D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.Value = "= Fields.Koszty";
            // 
            // textBox19
            // 
            this.textBox19.Format = "{0:C2}";
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(21.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1995989084243774D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.Value = "= Fields.InneKoszty";
            // 
            // textBox20
            // 
            this.textBox20.CanGrow = true;
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1997995376586914D), Telerik.Reporting.Drawing.Unit.Cm(0.89999985694885254D));
            this.textBox20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox20.StyleName = "Caption";
            this.textBox20.Value = "W tym noty odsetkowe";
            // 
            // textBox21
            // 
            this.textBox21.CanGrow = true;
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.800199508666992D), Telerik.Reporting.Drawing.Unit.Cm(1.485625147819519D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7995994091033936D), Telerik.Reporting.Drawing.Unit.Cm(0.89999985694885254D));
            this.textBox21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox21.StyleName = "Caption";
            this.textBox21.Value = "W tym ods. naliczone";
            // 
            // textBox22
            // 
            this.textBox22.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox22.CanGrow = true;
            this.textBox22.CanShrink = true;
            this.textBox22.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox22.Format = "{0:C2}";
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.799999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7997996807098389D), Telerik.Reporting.Drawing.Unit.Cm(1.1118824481964111D));
            this.textBox22.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox22.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox22.StyleName = "Data";
            this.textBox22.Value = "= Fields.OdsetkiKapital";
            // 
            // textBox23
            // 
            this.textBox23.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox23.CanGrow = true;
            this.textBox23.CanShrink = true;
            this.textBox23.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.textBox23.Format = "{0:C2}";
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.800199508666992D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7995994091033936D), Telerik.Reporting.Drawing.Unit.Cm(0.65582519769668579D));
            this.textBox23.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.Font.Bold = true;
            this.textBox23.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox23.StyleName = "Data";
            this.textBox23.Value = "= Sum(Fields.OdsetkiKapital)";
            // 
            // SkierowaneDoSadu
            // 
            this.DataSource = this.SkierowaneDoSaduDS;
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup,
            this.oddzialGroup});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.oddzialGroupHeader,
            this.oddzialGroupFooter,
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
            reportParameter1.Name = "IdPaczki";
            reportParameter1.Text = "IdPaczki";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.Style.Font.Name = "Tahoma";
            this.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(27.346885681152344D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource SkierowaneDoSaduDS;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
        private Telerik.Reporting.Group labelsGroup;
        private Telerik.Reporting.TextBox sygnaturaCaptionTextBox;
        private Telerik.Reporting.TextBox nrEwidCaptionTextBox;
        private Telerik.Reporting.TextBox dluznikCaptionTextBox;
        private Telerik.Reporting.TextBox adresCaptionTextBox;
        private Telerik.Reporting.TextBox wPSCaptionTextBox;
        private Telerik.Reporting.TextBox notyOdsetkoweCaptionTextBox;
        private Telerik.Reporting.TextBox dataDokCaptionTextBox;
        private Telerik.Reporting.TextBox radcaCaptionTextBox;
        private Telerik.Reporting.GroupHeaderSection oddzialGroupHeader;
        private Telerik.Reporting.TextBox oddzialDataTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.GroupFooterSection oddzialGroupFooter;
        private Telerik.Reporting.Group oddzialGroup;
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
        private Telerik.Reporting.TextBox nrEwidDataTextBox;
        private Telerik.Reporting.TextBox dluznikDataTextBox;
        private Telerik.Reporting.TextBox adresDataTextBox;
        private Telerik.Reporting.TextBox wPSDataTextBox;
        private Telerik.Reporting.TextBox notyOdsetkoweDataTextBox;
        private Telerik.Reporting.TextBox dataDokDataTextBox;
        private Telerik.Reporting.TextBox radcaDataTextBox;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox22;

    }
}