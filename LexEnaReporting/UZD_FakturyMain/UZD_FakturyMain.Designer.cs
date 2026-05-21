namespace LexEnaReporting.UZD_FakturyMain
{
    partial class UZD_FakturyMain
    { 
        #region Component Designer generated code
    /// <summary>
    /// Required method for telerik Reporting designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UZD_FakturyMain));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.UZD_FakturyPakietuMainDS = new Telerik.Reporting.SqlDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.textBox26 = new Telerik.Reporting.TextBox();
            this.textBox30 = new Telerik.Reporting.TextBox();
            this.textBox33 = new Telerik.Reporting.TextBox();
            this.textBox35 = new Telerik.Reporting.TextBox();
            this.textBox38 = new Telerik.Reporting.TextBox();
            this.textBox39 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox43 = new Telerik.Reporting.TextBox();
            this.textBox45 = new Telerik.Reporting.TextBox();
            this.textBox47 = new Telerik.Reporting.TextBox();
            this.textBox49 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox22 = new Telerik.Reporting.TextBox();
            this.textBox28 = new Telerik.Reporting.TextBox();
            this.textBox50 = new Telerik.Reporting.TextBox();
            this.textBox51 = new Telerik.Reporting.TextBox();
            this.textBox53 = new Telerik.Reporting.TextBox();
            this.textBox56 = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox25 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.textBox29 = new Telerik.Reporting.TextBox();
            this.textBox31 = new Telerik.Reporting.TextBox();
            this.textBox32 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox34 = new Telerik.Reporting.TextBox();
            this.textBox36 = new Telerik.Reporting.TextBox();
            this.textBox37 = new Telerik.Reporting.TextBox();
            this.textBox40 = new Telerik.Reporting.TextBox();
            this.textBox41 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox42 = new Telerik.Reporting.TextBox();
            this.textBox44 = new Telerik.Reporting.TextBox();
            this.textBox46 = new Telerik.Reporting.TextBox();
            this.textBox48 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox52 = new Telerik.Reporting.TextBox();
            this.textBox54 = new Telerik.Reporting.TextBox();
            this.textBox55 = new Telerik.Reporting.TextBox();
            this.textBox57 = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // UZD_FakturyPakietuMainDS
            // 
            this.UZD_FakturyPakietuMainDS.CommandTimeout = 240;
            this.UZD_FakturyPakietuMainDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.UZD_FakturyPakietuMainDS.Name = "UZD_FakturyPakietuMainDS";
            this.UZD_FakturyPakietuMainDS.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@IdPakiet", System.Data.DbType.Int32, "= Parameters.IdPakiet.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@Mode", System.Data.DbType.Int32, "= Parameters.Mode.Value")});
            this.UZD_FakturyPakietuMainDS.SelectCommand = resources.GetString("UZD_FakturyPakietuMainDS.SelectCommand");
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.9122997522354126D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox1,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox7,
            this.textBox8,
            this.textBox9,
            this.textBox24,
            this.textBox26,
            this.textBox30,
            this.textBox33,
            this.textBox35,
            this.textBox38,
            this.textBox39,
            this.textBox6,
            this.textBox43,
            this.textBox45,
            this.textBox47,
            this.textBox49,
            this.textBox18,
            this.textBox22,
            this.textBox28,
            this.textBox50,
            this.textBox51,
            this.textBox53,
            this.textBox56});
            this.detail.Name = "detail";
            // 
            // textBox2
            // 
            this.textBox2.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox2.CanGrow = true;
            this.textBox2.CanShrink = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.269800066947937D), Telerik.Reporting.Drawing.Unit.Cm(0.91219961643218994D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Data";
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // textBox1
            // 
            this.textBox1.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox1.CanGrow = true;
            this.textBox1.CanShrink = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.8893733024597168D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.6727170944213867D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Data";
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // textBox3
            // 
            this.textBox3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox3.CanGrow = true;
            this.textBox3.CanShrink = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(31.652997970581055D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3545918464660645D), Telerik.Reporting.Drawing.Unit.Cm(0.89679121971130371D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.StyleName = "Data";
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // textBox4
            // 
            this.textBox4.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox4.CanGrow = true;
            this.textBox4.CanShrink = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.562291145324707D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.07438325881958D), Telerik.Reporting.Drawing.Unit.Cm(0.91209948062896729D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.StyleName = "Data";
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // textBox5
            // 
            this.textBox5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox5.CanGrow = true;
            this.textBox5.CanShrink = true;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.53154182434082D), Telerik.Reporting.Drawing.Unit.Cm(0.00010032494901679456D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4868834018707275D), Telerik.Reporting.Drawing.Unit.Cm(0.91209948062896729D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.StyleName = "Data";
            resources.ApplyResources(this.textBox5, "textBox5");
            // 
            // textBox7
            // 
            this.textBox7.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox7.CanGrow = true;
            this.textBox7.CanShrink = true;
            this.textBox7.Format = "{0:#.00}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(45.808204650878906D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6985485553741455D), Telerik.Reporting.Drawing.Unit.Cm(0.91219961643218994D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.StyleName = "Data";
            resources.ApplyResources(this.textBox7, "textBox7");
            // 
            // textBox8
            // 
            this.textBox8.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox8.CanGrow = true;
            this.textBox8.CanShrink = true;
            this.textBox8.Format = "{0:#.00}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(48.506954193115234D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.91219961643218994D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.StyleName = "Data";
            resources.ApplyResources(this.textBox8, "textBox8");
            // 
            // textBox9
            // 
            this.textBox9.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox9.CanGrow = true;
            this.textBox9.CanShrink = true;
            this.textBox9.Format = "{0:#.00}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(43.109458923339844D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6985485553741455D), Telerik.Reporting.Drawing.Unit.Cm(0.91220009326934814D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.StyleName = "Data";
            resources.ApplyResources(this.textBox9, "textBox9");
            // 
            // textBox24
            // 
            this.textBox24.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox24.CanGrow = true;
            this.textBox24.CanShrink = true;
            this.textBox24.Format = "{0:#.00}";
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2699999809265137D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.91220009326934814D));
            this.textBox24.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.textBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox24.StyleName = "Data";
            resources.ApplyResources(this.textBox24, "textBox24");
            // 
            // textBox26
            // 
            this.textBox26.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox26.CanGrow = true;
            this.textBox26.CanShrink = true;
            this.textBox26.Format = "{0:#.00}";
            this.textBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.018625259399414D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550115585327148D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox26.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox26.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox26.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox26.StyleName = "Data";
            resources.ApplyResources(this.textBox26, "textBox26");
            // 
            // textBox30
            // 
            this.textBox30.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox30.CanGrow = false;
            this.textBox30.CanShrink = false;
            this.textBox30.Format = "{0:#.00}";
            this.textBox30.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.774665832519531D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.89679110050201416D));
            this.textBox30.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox30.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox30.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox30.StyleName = "Data";
            resources.ApplyResources(this.textBox30, "textBox30");
            // 
            // textBox33
            // 
            this.textBox33.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox33.CanGrow = true;
            this.textBox33.CanShrink = true;
            this.textBox33.Format = "{0:#.00}";
            this.textBox33.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(30.039041519165039D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox33.Name = "textBox33";
            this.textBox33.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6137536764144898D), Telerik.Reporting.Drawing.Unit.Cm(0.89679116010665894D));
            this.textBox33.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox33.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox33.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox33.StyleName = "Data";
            resources.ApplyResources(this.textBox33, "textBox33");
            // 
            // textBox35
            // 
            this.textBox35.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox35.CanGrow = true;
            this.textBox35.CanShrink = true;
            this.textBox35.Format = "{0:#.00}";
            this.textBox35.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(34.007789611816406D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox35.Name = "textBox35";
            this.textBox35.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.20125675201416D), Telerik.Reporting.Drawing.Unit.Cm(0.91219961643218994D));
            this.textBox35.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox35.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox35.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox35.StyleName = "Data";
            resources.ApplyResources(this.textBox35, "textBox35");
            // 
            // textBox38
            // 
            this.textBox38.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox38.CanGrow = true;
            this.textBox38.CanShrink = true;
            this.textBox38.Format = "{0:#.00}";
            this.textBox38.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(40.410701751708984D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox38.Name = "textBox38";
            this.textBox38.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6985485553741455D), Telerik.Reporting.Drawing.Unit.Cm(0.91219961643218994D));
            this.textBox38.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox38.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox38.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox38.StyleName = "Data";
            resources.ApplyResources(this.textBox38, "textBox38");
            // 
            // textBox39
            // 
            this.textBox39.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox39.CanGrow = true;
            this.textBox39.CanShrink = true;
            this.textBox39.Format = "";
            this.textBox39.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(37.209243774414062D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox39.Name = "textBox39";
            this.textBox39.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.20125675201416D), Telerik.Reporting.Drawing.Unit.Cm(0.9122997522354126D));
            this.textBox39.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox39.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox39.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox39.StyleName = "Data";
            resources.ApplyResources(this.textBox39, "textBox39");
            // 
            // textBox6
            // 
            this.textBox6.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox6.CanGrow = true;
            this.textBox6.CanShrink = true;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(53.481128692626953D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281316757202148D), Telerik.Reporting.Drawing.Unit.Cm(0.912299633026123D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.StyleName = "Data";
            resources.ApplyResources(this.textBox6, "textBox6");
            // 
            // textBox43
            // 
            this.textBox43.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox43.CanGrow = true;
            this.textBox43.CanShrink = true;
            this.textBox43.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(55.809452056884766D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox43.Name = "textBox43";
            this.textBox43.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281316757202148D), Telerik.Reporting.Drawing.Unit.Cm(0.896791398525238D));
            this.textBox43.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox43.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox43.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox43.StyleName = "Data";
            resources.ApplyResources(this.textBox43, "textBox43");
            // 
            // textBox45
            // 
            this.textBox45.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox45.CanGrow = true;
            this.textBox45.CanShrink = true;
            this.textBox45.Format = "{0}";
            this.textBox45.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(58.137786865234375D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox45.Name = "textBox45";
            this.textBox45.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8008420467376709D), Telerik.Reporting.Drawing.Unit.Cm(0.89679133892059326D));
            this.textBox45.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox45.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox45.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox45.StyleName = "Data";
            resources.ApplyResources(this.textBox45, "textBox45");
            // 
            // textBox47
            // 
            this.textBox47.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox47.CanGrow = true;
            this.textBox47.CanShrink = true;
            this.textBox47.Format = "{0}";
            this.textBox47.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(60.9388313293457D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox47.Name = "textBox47";
            this.textBox47.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8008420467376709D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox47.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox47.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox47.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox47.StyleName = "Data";
            resources.ApplyResources(this.textBox47, "textBox47");
            // 
            // textBox49
            // 
            this.textBox49.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox49.CanGrow = true;
            this.textBox49.CanShrink = true;
            this.textBox49.Format = "{0}";
            this.textBox49.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(63.739871978759766D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox49.Name = "textBox49";
            this.textBox49.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9277201890945435D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox49.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox49.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox49.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox49.StyleName = "Data";
            resources.ApplyResources(this.textBox49, "textBox49");
            // 
            // textBox18
            // 
            this.textBox18.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox18.CanGrow = true;
            this.textBox18.CanShrink = true;
            this.textBox18.Format = "{0:d}";
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(65.667793273925781D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5891752243041992D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.StyleName = "Data";
            resources.ApplyResources(this.textBox18, "textBox18");
            // 
            // textBox22
            // 
            this.textBox22.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox22.CanGrow = false;
            this.textBox22.CanShrink = false;
            this.textBox22.Format = "{0:#.00}";
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(27.419670104980469D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.89679110050201416D));
            this.textBox22.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox22.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox22.StyleName = "Data";
            resources.ApplyResources(this.textBox22, "textBox22");
            // 
            // textBox28
            // 
            this.textBox28.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox28.CanGrow = true;
            this.textBox28.CanShrink = true;
            this.textBox28.Format = "";
            this.textBox28.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(72.067123413085938D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5891866683959961D), Telerik.Reporting.Drawing.Unit.Cm(0.9122997522354126D));
            this.textBox28.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox28.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox28.StyleName = "Data";
            resources.ApplyResources(this.textBox28, "textBox28");
            // 
            // textBox50
            // 
            this.textBox50.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox50.CanGrow = true;
            this.textBox50.CanShrink = true;
            this.textBox50.Format = "{0:d}";
            this.textBox50.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(68.257171630859375D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox50.Name = "textBox50";
            this.textBox50.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9753419160842896D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox50.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox50.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox50.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox50.StyleName = "Data";
            resources.ApplyResources(this.textBox50, "textBox50");
            // 
            // textBox51
            // 
            this.textBox51.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox51.CanGrow = true;
            this.textBox51.CanShrink = true;
            this.textBox51.Format = "{0:d}";
            this.textBox51.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(70.232704162597656D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox51.Name = "textBox51";
            this.textBox51.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8342289924621582D), Telerik.Reporting.Drawing.Unit.Cm(0.90438282489776611D));
            this.textBox51.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox51.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox51.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox51.StyleName = "Data";
            resources.ApplyResources(this.textBox51, "textBox51");
            // 
            // textBox53
            // 
            this.textBox53.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox53.CanGrow = true;
            this.textBox53.CanShrink = true;
            this.textBox53.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.636873245239258D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox53.Name = "textBox53";
            this.textBox53.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.894467830657959D), Telerik.Reporting.Drawing.Unit.Cm(0.91209948062896729D));
            this.textBox53.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox53.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox53.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox53.StyleName = "Data";
            resources.ApplyResources(this.textBox53, "textBox53");
            // 
            // textBox56
            // 
            this.textBox56.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox56.CanGrow = true;
            this.textBox56.CanShrink = true;
            this.textBox56.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(51.143955230712891D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox56.Name = "textBox56";
            this.textBox56.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281316757202148D), Telerik.Reporting.Drawing.Unit.Cm(0.912299633026123D));
            this.textBox56.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox56.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox56.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox56.StyleName = "Data";
            resources.ApplyResources(this.textBox56, "textBox56");
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.0751855373382568D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox10,
            this.textBox11,
            this.textBox13,
            this.textBox14,
            this.textBox16,
            this.textBox25,
            this.textBox27,
            this.textBox29,
            this.textBox31,
            this.textBox32,
            this.textBox12,
            this.textBox34,
            this.textBox36,
            this.textBox37,
            this.textBox40,
            this.textBox41,
            this.textBox15,
            this.textBox42,
            this.textBox44,
            this.textBox46,
            this.textBox48,
            this.textBox17,
            this.textBox23,
            this.textBox52,
            this.textBox54,
            this.textBox55,
            this.textBox57});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.061597499996423721D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2697000503540039D), Telerik.Reporting.Drawing.Unit.Cm(1.9704328775405884D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox10, "textBox10");
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.8893749713897705D), Telerik.Reporting.Drawing.Unit.Cm(0.061597295105457306D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.67271614074707D), Telerik.Reporting.Drawing.Unit.Cm(1.9704328775405884D));
            this.textBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox11, "textBox11");
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.562291145324707D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.07438325881958D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox13, "textBox13");
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.53154182434082D), Telerik.Reporting.Drawing.Unit.Cm(0.061597093939781189D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4868834018707275D), Telerik.Reporting.Drawing.Unit.Cm(1.9704327583312988D));
            this.textBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.Font.Bold = true;
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox14, "textBox14");
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(48.506954193115234D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.Font.Bold = true;
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox16, "textBox16");
            // 
            // textBox25
            // 
            this.textBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2699999809265137D), Telerik.Reporting.Drawing.Unit.Cm(0.061597499996423721D));
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6191749572753906D), Telerik.Reporting.Drawing.Unit.Cm(1.9704327583312988D));
            this.textBox25.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox25.Style.Font.Bold = true;
            this.textBox25.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox25, "textBox25");
            // 
            // textBox27
            // 
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.018625259399414D), Telerik.Reporting.Drawing.Unit.Cm(0.061597295105457306D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7550115585327148D), Telerik.Reporting.Drawing.Unit.Cm(1.9704327583312988D));
            this.textBox27.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.Font.Bold = true;
            this.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox27, "textBox27");
            // 
            // textBox29
            // 
            this.textBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(27.419670104980469D), Telerik.Reporting.Drawing.Unit.Cm(0.061497174203395844D));
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6191718578338623D), Telerik.Reporting.Drawing.Unit.Cm(1.970531702041626D));
            this.textBox29.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox29.Style.Font.Bold = true;
            this.textBox29.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox29, "textBox29");
            // 
            // textBox31
            // 
            this.textBox31.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.773836135864258D), Telerik.Reporting.Drawing.Unit.Cm(0.061597093939781189D));
            this.textBox31.Name = "textBox31";
            this.textBox31.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6191718578338623D), Telerik.Reporting.Drawing.Unit.Cm(1.9704327583312988D));
            this.textBox31.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox31.Style.Font.Bold = true;
            this.textBox31.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox31.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox31, "textBox31");
            // 
            // textBox32
            // 
            this.textBox32.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(30.039041519165039D), Telerik.Reporting.Drawing.Unit.Cm(0.07799588143825531D));
            this.textBox32.Name = "textBox32";
            this.textBox32.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6137552261352539D), Telerik.Reporting.Drawing.Unit.Cm(1.9704316854476929D));
            this.textBox32.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox32.Style.Font.Bold = true;
            this.textBox32.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox32.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox32, "textBox32");
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(31.652996063232422D), Telerik.Reporting.Drawing.Unit.Cm(0.07799588143825531D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3545918464660645D), Telerik.Reporting.Drawing.Unit.Cm(1.9706313610076904D));
            this.textBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox12, "textBox12");
            // 
            // textBox34
            // 
            this.textBox34.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(34.007789611816406D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox34.Name = "textBox34";
            this.textBox34.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.2012584209442139D), Telerik.Reporting.Drawing.Unit.Cm(1.9704315662384033D));
            this.textBox34.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox34.Style.Font.Bold = true;
            this.textBox34.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox34.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox34, "textBox34");
            // 
            // textBox36
            // 
            this.textBox36.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(37.209243774414062D), Telerik.Reporting.Drawing.Unit.Cm(0.0613972507417202D));
            this.textBox36.Name = "textBox36";
            this.textBox36.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.2012584209442139D), Telerik.Reporting.Drawing.Unit.Cm(1.9706321954727173D));
            this.textBox36.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox36.Style.Font.Bold = true;
            this.textBox36.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox36.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox36, "textBox36");
            // 
            // textBox37
            // 
            this.textBox37.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(40.410701751708984D), Telerik.Reporting.Drawing.Unit.Cm(0.0613972507417202D));
            this.textBox37.Name = "textBox37";
            this.textBox37.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.69854998588562D), Telerik.Reporting.Drawing.Unit.Cm(1.9706321954727173D));
            this.textBox37.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox37.Style.Font.Bold = true;
            this.textBox37.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox37.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox37, "textBox37");
            // 
            // textBox40
            // 
            this.textBox40.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(43.109458923339844D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox40.Name = "textBox40";
            this.textBox40.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.69854998588562D), Telerik.Reporting.Drawing.Unit.Cm(1.9704320430755615D));
            this.textBox40.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox40.Style.Font.Bold = true;
            this.textBox40.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox40.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox40, "textBox40");
            // 
            // textBox41
            // 
            this.textBox41.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(45.808204650878906D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox41.Name = "textBox41";
            this.textBox41.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.69854998588562D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox41.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox41.Style.Font.Bold = true;
            this.textBox41.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox41.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox41, "textBox41");
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(53.481128692626953D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281319141387939D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.Font.Bold = true;
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox15, "textBox15");
            // 
            // textBox42
            // 
            this.textBox42.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(55.809452056884766D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox42.Name = "textBox42";
            this.textBox42.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281333446502686D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox42.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox42.Style.Font.Bold = true;
            this.textBox42.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox42.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox42, "textBox42");
            // 
            // textBox44
            // 
            this.textBox44.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(58.137786865234375D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox44.Name = "textBox44";
            this.textBox44.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8008434772491455D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox44.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox44.Style.Font.Bold = true;
            this.textBox44.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox44.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox44, "textBox44");
            // 
            // textBox46
            // 
            this.textBox46.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(60.9388313293457D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox46.Name = "textBox46";
            this.textBox46.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8008434772491455D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox46.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox46.Style.Font.Bold = true;
            this.textBox46.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox46.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox46, "textBox46");
            // 
            // textBox48
            // 
            this.textBox48.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(63.739871978759766D), Telerik.Reporting.Drawing.Unit.Cm(0.0613972507417202D));
            this.textBox48.Name = "textBox48";
            this.textBox48.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9277185201644898D), Telerik.Reporting.Drawing.Unit.Cm(1.9706330299377441D));
            this.textBox48.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox48.Style.Font.Bold = true;
            this.textBox48.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox48.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox48, "textBox48");
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(65.667793273925781D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5891788005828857D), Telerik.Reporting.Drawing.Unit.Cm(1.9704328775405884D));
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox17, "textBox17");
            // 
            // textBox23
            // 
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(72.067123413085938D), Telerik.Reporting.Drawing.Unit.Cm(0.0613972507417202D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5891788005828857D), Telerik.Reporting.Drawing.Unit.Cm(1.9704328775405884D));
            this.textBox23.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.Font.Bold = true;
            this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox23, "textBox23");
            // 
            // textBox52
            // 
            this.textBox52.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.636873245239258D), Telerik.Reporting.Drawing.Unit.Cm(0.061597656458616257D));
            this.textBox52.Name = "textBox52";
            this.textBox52.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8944692611694336D), Telerik.Reporting.Drawing.Unit.Cm(1.9702324867248535D));
            this.textBox52.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox52.Style.Font.Bold = true;
            this.textBox52.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox52.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox52, "textBox52");
            // 
            // textBox54
            // 
            this.textBox54.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(70.232704162597656D), Telerik.Reporting.Drawing.Unit.Cm(0.061597980558872223D));
            this.textBox54.Name = "textBox54";
            this.textBox54.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8342223167419434D), Telerik.Reporting.Drawing.Unit.Cm(1.9704328775405884D));
            this.textBox54.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox54.Style.Font.Bold = true;
            this.textBox54.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox54.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox54, "textBox54");
            // 
            // textBox55
            // 
            this.textBox55.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(68.257171630859375D), Telerik.Reporting.Drawing.Unit.Cm(0.061597980558872223D));
            this.textBox55.Name = "textBox55";
            this.textBox55.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9753434658050537D), Telerik.Reporting.Drawing.Unit.Cm(1.9706330299377441D));
            this.textBox55.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox55.Style.Font.Bold = true;
            this.textBox55.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox55.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox55, "textBox55");
            // 
            // textBox57
            // 
            this.textBox57.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(51.126327514648438D), Telerik.Reporting.Drawing.Unit.Cm(0.061597701162099838D));
            this.textBox57.Name = "textBox57";
            this.textBox57.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3281333446502686D), Telerik.Reporting.Drawing.Unit.Cm(1.9704324007034302D));
            this.textBox57.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox57.Style.Font.Bold = true;
            this.textBox57.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox57.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox57, "textBox57");
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
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(45.808204650878906D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6985485553741455D), Telerik.Reporting.Drawing.Unit.Cm(0.6843833327293396D));
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox19.StyleName = "Data";
            resources.ApplyResources(this.textBox19, "textBox19");
            // 
            // textBox20
            // 
            this.textBox20.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox20.CanGrow = true;
            this.textBox20.CanShrink = true;
            this.textBox20.Format = "{0:#.00}";
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(48.506950378417969D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.619173526763916D), Telerik.Reporting.Drawing.Unit.Cm(0.68428283929824829D));
            this.textBox20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.Font.Bold = true;
            this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox20.StyleName = "Data";
            resources.ApplyResources(this.textBox20, "textBox20");
            // 
            // textBox21
            // 
            this.textBox21.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox21.CanGrow = true;
            this.textBox21.CanShrink = true;
            this.textBox21.Format = "{0:#.00}";
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(43.109458923339844D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.6985485553741455D), Telerik.Reporting.Drawing.Unit.Cm(0.68438321352005D));
            this.textBox21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.Font.Bold = true;
            this.textBox21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(9D);
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox21.StyleName = "Data";
            resources.ApplyResources(this.textBox21, "textBox21");
            // 
            // UZD_FakturyMain
            // 
            this.DataSource = this.UZD_FakturyPakietuMainDS;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.reportHeaderSection1,
            this.reportFooterSection1});
            this.Name = "UZD_FakturyMain";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A2;
            reportParameter1.Name = "IdPakiet";
            resources.ApplyResources(reportParameter1, "reportParameter1");
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.AllowNull = true;
            reportParameter2.Name = "Mode";
            resources.ApplyResources(reportParameter2, "reportParameter2");
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(74.841522216796875D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }
    #endregion

    private Telerik.Reporting.SqlDataSource UZD_FakturyPakietuMainDS;
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
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox textBox25;
        private Telerik.Reporting.TextBox textBox26;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox30;
        private Telerik.Reporting.TextBox textBox33;
        private Telerik.Reporting.TextBox textBox29;
        private Telerik.Reporting.TextBox textBox31;
        private Telerik.Reporting.TextBox textBox32;
        private Telerik.Reporting.TextBox textBox35;
        private Telerik.Reporting.TextBox textBox38;
        private Telerik.Reporting.TextBox textBox39;
        private Telerik.Reporting.TextBox textBox34;
        private Telerik.Reporting.TextBox textBox36;
        private Telerik.Reporting.TextBox textBox37;
        private Telerik.Reporting.TextBox textBox40;
        private Telerik.Reporting.TextBox textBox41;
        private Telerik.Reporting.TextBox textBox42;
        private Telerik.Reporting.TextBox textBox43;
        private Telerik.Reporting.TextBox textBox45;
        private Telerik.Reporting.TextBox textBox47;
        private Telerik.Reporting.TextBox textBox49;
        private Telerik.Reporting.TextBox textBox44;
        private Telerik.Reporting.TextBox textBox46;
        private Telerik.Reporting.TextBox textBox48;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox22;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox28;
        private Telerik.Reporting.TextBox textBox50;
        private Telerik.Reporting.TextBox textBox51;
        private Telerik.Reporting.TextBox textBox53;
        private Telerik.Reporting.TextBox textBox52;
        private Telerik.Reporting.TextBox textBox54;
        private Telerik.Reporting.TextBox textBox55;
        private Telerik.Reporting.TextBox textBox56;
        private Telerik.Reporting.TextBox textBox57;
    }
}