using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;



namespace ZaładujPozew
{
    public partial class Form1 : Form
    {
        XElement xe;
        XmlDocument pozewXML = new XmlDocument();
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string wybranyPlik = null;
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Otwórz pozew w postaci paczki XML";
            openFileDialog1.Filter = "Plik pozwu|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("cancel button clicked");
            }
            else 
            {
                wybranyPlik = openFileDialog1.FileName;
                textBox4.Text = wybranyPlik;
                button4.Enabled = true;
                int i  = 1;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                XmlDocument xmlfile = new XmlDocument();
                xmlfile.Load(textBox4.Text);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlfile.NameTable);
                nsmgr.AddNamespace("curr", "http://www.currenda.pl/epu");
                XmlNode sn = xmlfile.SelectSingleNode("curr:Pozwy", nsmgr);
                
                 * 
                 * */
                pozewXML.Load(textBox4.Text);
                xe = XElement.Load(textBox4.Text);
                textBox5.Text= xe.Attribute("OznaczeniePaczki").Value.ToString();
                button4.Enabled = true;
                
                textBox6.Text = "";
//                XNamespace curr = "http://www.currenda.pl/epu";    
  
            }
            catch 
            {
                MessageBox.Show("Problem z załadowaniem pliku z pozwem");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            xe.Attribute("OznaczeniePaczki").Value = textBox5.Text;
           
            int i = 1;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                button4.Enabled = false;
                EpuProxy.EpuProxyClient klient = new EpuProxy.EpuProxyClient();
                klient.setUserData(textBox1.Text, textBox3.Text, textBox2.Text);
              /*
                XDocument xd = new XDocument(
                
                    new XDeclaration("1.0", "utf-8","no"),
                    xe
                );
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Indent = true;
                //xe.Save(tw);
                xd.Save(tw,settings);
               * */
                int wynikp = klient.zlozPozwy(pozewXML.InnerXml);
                if (wynikp < 0) { textBox6.Text = "Błąd pozwu"; }
                else
                {
                    textBox6.Text = "Pozew załadowano pomyślnie";
                    textBox5.Text = "";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("problem");
                textBox6.Text = ex.ToString();
                textBox5.Text = "";
             }
        }

       

        
    }
}
