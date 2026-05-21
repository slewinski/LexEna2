namespace LexEnaReporting.EOB.WiekowanieSaldSummary
{
    partial class WiekowaneSaldSummary
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WiekowaneSaldSummary));
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox28 = new Telerik.Reporting.TextBox();
            this.LexEnaDS = new Telerik.Reporting.SqlDataSource();
            this.textBox29 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox22 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60009992122650146D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox6,
            this.textBox7,
            this.textBox8,
            this.textBox9,
            this.textBox28});
            this.detail.Name = "detail";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = false;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.799999475479126D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "= Fields.konto";
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = false;
            this.textBox3.Format = "{0:N2}";
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.8000993728637695D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.799999475479126D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "= Fields.Saldo";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = false;
            this.textBox4.Format = "{0:N2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.6002888679504395D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "= Fields.[ do 30 dni]";
            // 
            // textBox5
            // 
            this.textBox5.CanGrow = false;
            this.textBox5.Format = "{0:N2}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.000089645385742D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "= Fields.[31-60 dni]";
            // 
            // textBox6
            // 
            this.textBox6.CanGrow = false;
            this.textBox6.Format = "{0:N2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.399889945983887D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "= Fields.[61-90 dni]";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = false;
            this.textBox7.Format = "{0:N2}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.7996883392334D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "= Fields.[91-180 dni]";
            // 
            // textBox8
            // 
            this.textBox8.CanGrow = false;
            this.textBox8.Format = "{0:N2}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(21.199487686157227D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "= Fields.[181-360 dni]";
            // 
            // textBox9
            // 
            this.textBox9.CanGrow = false;
            this.textBox9.Format = "{0:N2}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.599897384643555D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.Value = "= Fields.[powy¿ej 360 dni]";
            // 
            // textBox28
            // 
            this.textBox28.CanGrow = false;
            this.textBox28.Format = "{0:N2}";
            this.textBox28.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(27.99969482421875D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox28.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox28.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox28.Value = "= Fields.Saldo - (Fields.[ do 30 dni] + Fields.[31-60 dni] + Fields.[61-90 dni] +" +
    " Fields.[91-180 dni] + Fields.[181-360 dni] + Fields.[powy¿ej 360 dni])";
            // 
            // LexEnaDS
            // 
            this.LexEnaDS.ConnectionString = "LexEnaReporting.Properties.Settings.LexEnaPro";
            this.LexEnaDS.Name = "LexEnaDS";
            this.LexEnaDS.SelectCommand = resources.GetString("LexEnaDS.SelectCommand");
            // 
            // textBox29
            // 
            this.textBox29.CanGrow = false;
            this.textBox29.Format = "{0:N2}";
            this.textBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(28.000383377075195D), Telerik.Reporting.Drawing.Unit.Cm(0.00030036925454623997D));
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox29.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox29.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox29.Value = "= Sum(Fields.Saldo - (Fields.[ do 30 dni] + Fields.[31-60 dni] + Fields.[61-90 dn" +
    "i] + Fields.[91-180 dni] + Fields.[181-360 dni] + Fields.[powy¿ej 360 dni]) )";
            // 
            // textBox13
            // 
            this.textBox13.CanGrow = false;
            this.textBox13.Format = "{0:N2}";
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.600584030151367D), Telerik.Reporting.Drawing.Unit.Cm(0.00040210722363553941D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox13.Value = "= Sum(Fields.[powy¿ej 360 dni])";
            // 
            // textBox23
            // 
            this.textBox23.CanGrow = false;
            this.textBox23.Format = "{0:N2}";
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.8007857799530029D), Telerik.Reporting.Drawing.Unit.Cm(0.00040049233939498663D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.799999475479126D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox23.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox23.Value = "= Sum(Fields.Saldo)";
            // 
            // textBox22
            // 
            this.textBox22.CanGrow = false;
            this.textBox22.Format = "{0:N2}";
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.6009750366210938D), Telerik.Reporting.Drawing.Unit.Cm(0.00040049233939498663D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox22.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox22.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox22.Value = "= Sum( Fields.[ do 30 dni])";
            // 
            // textBox18
            // 
            this.textBox18.CanGrow = false;
            this.textBox18.Format = "{0:N2}";
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(21.200176239013672D), Telerik.Reporting.Drawing.Unit.Cm(0.00040049233939498663D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.Value = "= Sum(Fields.[181-360 dni])";
            // 
            // textBox20
            // 
            this.textBox20.CanGrow = false;
            this.textBox20.Format = "{0:N2}";
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.400577545166016D), Telerik.Reporting.Drawing.Unit.Cm(0.00040049233939498663D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox20.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox20.Value = "= Sum( Fields.[61-90 dni])";
            // 
            // textBox19
            // 
            this.textBox19.CanGrow = false;
            this.textBox19.Format = "{0:N2}";
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.800378799438477D), Telerik.Reporting.Drawing.Unit.Cm(0.00040049233939498663D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.Value = "= Sum(Fields.[91-180 dni])";
            // 
            // textBox21
            // 
            this.textBox21.CanGrow = false;
            this.textBox21.Format = "{0:N2}";
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.000777244567871D), Telerik.Reporting.Drawing.Unit.Cm(0.00040049233939498663D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996007442474365D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.textBox21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox21.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox21.Value = "= Sum(Fields.[31-60 dni])";
            // 
            // textBox27
            // 
            this.textBox27.CanGrow = false;
            this.textBox27.Format = "{0}";
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(27.999698638916016D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996002674102783D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox27.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.Font.Bold = true;
            this.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox27.Value = "ró¿nica";
            // 
            // textBox24
            // 
            this.textBox24.CanGrow = false;
            this.textBox24.Format = "{0}";
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.8001978397369385D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.7998995780944824D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox24.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.Font.Bold = true;
            this.textBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox24.Value = "Saldo";
            // 
            // textBox17
            // 
            this.textBox17.CanGrow = false;
            this.textBox17.Format = "{0}";
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.599897384643555D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996002674102783D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.Value = "powy¿ej 360 dni";
            // 
            // textBox16
            // 
            this.textBox16.CanGrow = false;
            this.textBox16.Format = "{0}";
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(21.200092315673828D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996002674102783D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.Font.Bold = true;
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox16.Value = "181-360 dni";
            // 
            // textBox15
            // 
            this.textBox15.CanGrow = false;
            this.textBox15.Format = "{0}";
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.800289154052734D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996002674102783D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.Font.Bold = true;
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox15.Value = "91-180 dni";
            // 
            // textBox14
            // 
            this.textBox14.CanGrow = false;
            this.textBox14.Format = "{0}";
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.400490760803223D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996002674102783D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.Font.Bold = true;
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.Value = "61-90 dni";
            // 
            // textBox12
            // 
            this.textBox12.CanGrow = false;
            this.textBox12.Format = "{0}";
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.000686645507813D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3996002674102783D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.Value = "31-60 dni";
            // 
            // textBox11
            // 
            this.textBox11.CanGrow = false;
            this.textBox11.Format = "{0}";
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.6002874374389648D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4002041816711426D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "do 30 dni";
            // 
            // textBox10
            // 
            this.textBox10.CanGrow = false;
            this.textBox10.Format = "{0}";
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.7998995780944824D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "Konto";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60000014305114746D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox27,
            this.textBox24,
            this.textBox11,
            this.textBox12,
            this.textBox14,
            this.textBox15,
            this.textBox16,
            this.textBox17,
            this.textBox10});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60040187835693359D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox13,
            this.textBox22,
            this.textBox21,
            this.textBox20,
            this.textBox19,
            this.textBox18,
            this.textBox23,
            this.textBox29});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // WiekowaneSaldSummary
            // 
            this.DataSource = this.LexEnaDS;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.reportHeaderSection1,
            this.reportFooterSection1});
            this.Name = "WiekowaneSald";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(31.399986267089844D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.SqlDataSource LexEnaDS;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox28;
        private Telerik.Reporting.TextBox textBox29;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox22;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
    }
}