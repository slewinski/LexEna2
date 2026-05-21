using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;

namespace LexEnaTrs.Views
{
    public partial class PozewViewWindow : ChildWindow
    {
        public PozewViewWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewPozew != null)
            {
                this.ViewPozew.serializePozew();
                this.ViewPozew.SaveContent(null);
            }
            if (this.ViewDokEPU != null)
            {
                this.ViewDokEPU.serializeDokumentEPU();
                this.ViewDokEPU.SaveContent(null);
                
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void WysylkaButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.ViewPozew != null)
            {
                if (this.ViewPozew.docStatus != 2)
                {

                    this.ViewPozew.docStatus = 2;
                    this.ViewPozew.statusChanged = true;
                }

                this.ViewPozew.serializePozew();
                this.ViewPozew.pozewEPUvalidated += (obj, ex) =>
                    {
                        pozewEventArgs pea;
                        pea = (ex as pozewEventArgs);
                        if (pea.Status > 0)
                        {
                            this.DialogResult = true;
                            return;

                        }
                        else
                        {
                            ErrorWindow.CreateNew(pea.Message);
                            return;
                        }

                    };
                this.ViewPozew.ValidatePozewEPU();

            }

            else // dokument EPU
            {
                if (this.ViewDokEPU.docStatus != 2)
                {

                    this.ViewDokEPU.docStatus = 2;
                    this.ViewDokEPU.statusChanged = true;
                }

                this.ViewDokEPU.serializeDokumentEPU();
                this.ViewDokEPU.ValidateDokumentEPU();
                this.ViewDokEPU.dokumentEPUvalidated += (obj, ex) =>
                {
                    pozewEventArgs pea;
                    pea = (ex as pozewEventArgs);
                    if (pea.Status > 0)
                    {
                        this.ViewDokEPU.SaveContent(null);
                        this.DialogResult = true;
                        return;

                    }
                    else
                    {
                        ErrorWindow.CreateNew(pea.Message);
                        return;
                    }

                };
        

            }

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ViewDokEPU != null) this.Title = "Dokument EPU";
            if (this.ViewPozew != null) this.Title = "Pozew EPU";

        }
    }
        }


