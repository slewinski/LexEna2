namespace LexEnaReporting.UZD_FakturyPaczki
{
    partial class UZD_FakturyPaczki
    { 
        #region Component Designer generated code
    /// <summary>
    /// Required method for telerik Reporting designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UZD_FakturyPaczki));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.UZD_FakturyPakietuDS = new Telerik.Reporting.SqlDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.textBox26 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox22 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox25 = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // UZD_FakturyPakietuDS
            // 
            this.UZD_FakturyPakietuDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.UZD_FakturyPakietuDS.Name = "UZD_FakturyPakietuDS";
            this.UZD_FakturyPakietuDS.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@IdPakiet", System.Data.DbType.Int32, "= Parameters.IdPakiet.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@Mode", System.Data.DbType.Int32, "= Parameters.Mode.Value")});
            this.UZD_FakturyPakietuDS.SelectCommand = resources.GetString("UZD_FakturyPakietuDS.SelectCommand");
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.451114296913147D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox1,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox6,
            this.textBox7,
            this.textBox8,
            this.textBox9,
            this.textBox24,
            this.textBox26,
            this.textBox27});
            this.detail.Name = "detail";
            // 
            // textBox2
            // 
            this.textBox2.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox2.CanGrow = true;
            this.textBox2.CanShrink = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.269800066947937D), Telerik.Reporting.Drawing.Unit.Cm(1.4510141611099243D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "=RowNumber()";
            // 
            // textBox1
            // 
            this.textBox1.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox1.CanGrow = true;
            this.textBox1.CanShrink = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2699999809265137D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.6727170944213867D), Telerik.Reporting.Drawing.Unit.Cm(1.4508137702941895D));
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Data";
            this.textBox1.Value = "= Fields.Nazwa";
            // 
            // textBox3
            // 
            this.textBox3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox3.CanGrow = true;
            this.textBox3.CanShrink = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.9429168701171875D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3545918464660645D), Telerik.Reporting.Drawing.Unit.Cm(1.451014518737793D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.StyleName = "Data";
            this.textBox3.Value = "= Fields.NIP";
            // 
            // textBox4
            // 
            this.textBox4.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox4.CanGrow = true;
            this.textBox4.CanShrink = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.297707557678223D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.07438325881958D), Telerik.Reporting.Drawing.Unit.Cm(1.4510138034820557D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "= Fields.NrFaktury";
            // 
            // textBox5
            // 
            this.textBox5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox5.CanGrow = true;
            this.textBox5.CanShrink = true;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.372289657592773D), Telerik.Reporting.Drawing.Unit.Cm(0.00010032494901679456D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4868834018707275D), Telerik.Reporting.Drawing.Unit.Cm(1.4509141445159912D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.StyleName = "Data";
            this.textBox5.Value = "= Fields.DataFaktury";
            // 
            // textBox6
            // 
            this.textBox6.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox6.CanGrow = true;
            this.textBox6.CanShrink = true;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.859373092651367D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281316757202148D), Telerik.Reporting.Drawing.Unit.Cm(1.4509141445159912D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.StyleName = "Data";
            this.textBox6.Value = "= Fields.DataWymagalnosci";
            // 
            // textBox7
            // 
            this.textBox7.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox7.CanGrow = true;
            this.textBox7.CanShrink = true;
            this.textBox7.Format = "{0:#.00}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.187704086303711D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(1.4510141611099243D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.StyleName = "Data";
            this.textBox7.Value = "= Fields.Netto";
            // 
            // textBox8
            // 
            this.textBox8.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox8.CanGrow = true;
            this.textBox8.CanShrink = true;
            this.textBox8.Format = "{0:#.00}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.807077407836914D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(1.4510141611099243D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.StyleName = "Data";
            this.textBox8.Value = "= Fields.Vat";
            // 
            // textBox9
            // 
            this.textBox9.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox9.CanGrow = true;
            this.textBox9.CanShrink = true;
            this.textBox9.Format = "{0:#.00}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(25.426450729370117D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550115585327148D), Telerik.Reporting.Drawing.Unit.Cm(1.4510146379470825D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.StyleName = "Data";
            this.textBox9.Value = "= Fields.Zaleglosc";
            // 
            // textBox24
            // 
            this.textBox24.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox24.CanGrow = true;
            this.textBox24.CanShrink = true;
            this.textBox24.Format = "{0:#.00}";
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(28.181667327880859D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(1.4510136842727661D));
            this.textBox24.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.textBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox24.StyleName = "Data";
            this.textBox24.Value = "= Fields.Sygnatura";
            // 
            // textBox26
            // 
            this.textBox26.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox26.CanGrow = true;
            this.textBox26.CanShrink = true;
            this.textBox26.Format = "{0}";
            this.textBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(30.823957443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.023525292053818703D));
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550115585327148D), Telerik.Reporting.Drawing.Unit.Cm(1.4158762693405151D));
            this.textBox26.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox26.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox26.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox26.StyleName = "Data";
            this.textBox26.Value = "= Fields.OkresPodatkowy";
            // 
            // textBox27
            // 
            this.textBox27.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox27.CanGrow = true;
            this.textBox27.CanShrink = true;
            this.textBox27.Format = "";
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(33.579170227050781D), Telerik.Reporting.Drawing.Unit.Cm(0.035238076001405716D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550115585327148D), Telerik.Reporting.Drawing.Unit.Cm(1.4158762693405151D));
            this.textBox27.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox27.StyleName = "Data";
            this.textBox27.Value = "= Fields.KwartalPodatkowy";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.5360260009765625D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox10,
            this.textBox11,
            this.textBox13,
            this.textBox14,
            this.textBox15,
            this.textBox16,
            this.textBox17,
            this.textBox18,
            this.textBox22,
            this.textBox12,
            this.textBox23,
            this.textBox25});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox10
            // 
            this.textBox10.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(-3.041407403703289E-12D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2697000503540039D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "A. Lp.";
            // 
            // textBox11
            // 
            this.textBox11.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2699999809265137D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.67271614074707D), Telerik.Reporting.Drawing.Unit.Cm(1.5358262062072754D));
            this.textBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "B. Nazwa podatnika (d³u¿nika)";
            // 
            // textBox13
            // 
            this.textBox13.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.297707557678223D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000218398985453D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.07438325881958D), Telerik.Reporting.Drawing.Unit.Cm(1.5357259511947632D));
            this.textBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox13.Value = "D.1. Nr faktury";
            // 
            // textBox14
            // 
            this.textBox14.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.372289657592773D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4868834018707275D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.Font.Bold = true;
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.Value = "D.2. Data wystawienia faktury (dzieñ - miesi¹c - rok)";
            // 
            // textBox15
            // 
            this.textBox15.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.859373092651367D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281319141387939D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.Font.Bold = true;
            this.textBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox15.Value = "E. Data up³ywu terminu p³atnoœci (dzieñ - miesi¹c - rok) \r\n";
            // 
            // textBox16
            // 
            this.textBox16.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.187704086303711D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.Font.Bold = true;
            this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox16.Value = "F. Kwota korekty podstawy opodatkowania\r\n";
            // 
            // textBox17
            // 
            this.textBox17.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.807077407836914D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6191718578338623D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.Value = "G. Kwota korekty podatku nale¿nego\r\n";
            // 
            // textBox18
            // 
            this.textBox18.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(25.426448822021484D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550129890441895D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.Font.Bold = true;
            this.textBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.Value = "Ogó³em kwoty Suma kwot z pól Fi G.\t\r\n";
            // 
            // textBox22
            // 
            this.textBox22.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(28.181667327880859D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6191740036010742D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox22.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox22.Style.Font.Bold = true;
            this.textBox22.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(11D);
            this.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox22.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox22.Value = "Sygnatura";
            // 
            // textBox12
            // 
            this.textBox12.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.9429168701171875D), Telerik.Reporting.Drawing.Unit.Cm(0.00010002253111451864D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3545918464660645D), Telerik.Reporting.Drawing.Unit.Cm(1.5357260704040527D));
            this.textBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.Value = "C. Identyfikator podatkowy NIP podatnika (d³u¿nika)";
            // 
            // textBox23
            // 
            this.textBox23.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(33.602081298828125D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7321000099182129D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox23.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.Font.Bold = true;
            this.textBox23.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(11D);
            this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox23.Value = "Kwarta³ w którym mo¿na skorzystaæ z ulgi";
            // 
            // textBox25
            // 
            this.textBox25.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(30.823957443237305D), Telerik.Reporting.Drawing.Unit.Cm(-3.0413449536581538E-12D));
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550129890441895D), Telerik.Reporting.Drawing.Unit.Cm(1.5358259677886963D));
            this.textBox25.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox25.Style.Font.Bold = true;
            this.textBox25.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox25.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox25.Value = "Okres w którym mo¿na skorzystaæ z ulgi";
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.68448346853256226D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox19,
            this.textBox20,
            this.textBox21});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // textBox19
            // 
            this.textBox19.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox19.CanGrow = true;
            this.textBox19.CanShrink = true;
            this.textBox19.Format = "{0:#.00}";
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.187707901000977D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.6843833327293396D));
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox19.StyleName = "Data";
            this.textBox19.Value = "= Sum(Fields.Netto)";
            // 
            // textBox20
            // 
            this.textBox20.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox20.CanGrow = true;
            this.textBox20.CanShrink = true;
            this.textBox20.Format = "{0:#.00}";
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.807079315185547D), Telerik.Reporting.Drawing.Unit.Cm(0.00010052680590888485D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.68428283929824829D));
            this.textBox20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.Font.Bold = true;
            this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox20.StyleName = "Data";
            this.textBox20.Value = "= Sum(Fields.Vat)";
            // 
            // textBox21
            // 
            this.textBox21.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox21.CanGrow = true;
            this.textBox21.CanShrink = true;
            this.textBox21.Format = "{0:#.00}";
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(25.426458358764648D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550115585327148D), Telerik.Reporting.Drawing.Unit.Cm(0.68438321352005D));
            this.textBox21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.Font.Bold = true;
            this.textBox21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox21.StyleName = "Data";
            this.textBox21.Value = "= Sum(Fields.Zaleglosc)";
            // 
            // UZD_FakturyPaczki
            // 
            this.DataSource = this.UZD_FakturyPakietuDS;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.reportHeaderSection1,
            this.reportFooterSection1});
            this.Name = "UZD_FaktruPaczki";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A3;
            reportParameter1.Name = "IdPakiet";
            reportParameter1.Text = "IdPakiet";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.AllowNull = true;
            reportParameter2.Name = "Mode";
            reportParameter2.Text = "Mode";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.Font.Name = "Tahoma";
            this.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(36.334182739257812D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }
    #endregion

    private Telerik.Reporting.SqlDataSource UZD_FakturyPakietuDS;
    private Telerik.Reporting.Group labelsGroup;
    private Telerik.Reporting.DetailSection detail;
    private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
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
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox textBox22;
        private Telerik.Reporting.TextBox textBox26;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox25;
    }
}