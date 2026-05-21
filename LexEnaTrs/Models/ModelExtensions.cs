using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel.DomainServices.Client;
using LexEnaTrs.Web;

namespace LexEnaTrs
{
    public class PaczkaNoEventArgs : EventArgs
    {
        private Paczka paczka;
        public Paczka Paczka
        {
            get { return paczka; }
            set { this.paczka = value; }

        }

    } 

   public class PaczkaExtnd
    {
        public event EventHandler paczkaNoCompleted; 
         protected virtual void OnpaczkaNoCompleted(PaczkaNoEventArgs e)
     {
         if (paczkaNoCompleted != null)
             paczkaNoCompleted(this, e);
     }

         public void GetMaxPaczka(int IdJednostki)
         {
            string _paczkaPrefix = (UserProfile.Firma > 0 ? "EOB " : "EOP ");
            Paczka _paczka;
             PaczkaNoEventArgs eventargs;
             LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
             LoadOperation<Paczka> loadop;
             EntityQuery<Paczka> query =
                 from c in _context.GetMaxPaczkaQuery(IdJednostki)
                 select c;
             loadop = _context.Load(query);
             loadop.Completed += (sender, e) =>
             {
                 _paczka = loadop.Entities.FirstOrDefault();
                 if (_paczka != null)
                 {
                     if (_paczka.rok != DateTime.Today.Year)
                     {
                         _paczka.rok = DateTime.Today.Year;
                         _paczka.nr = 1;

                     }
                     else
                         _paczka.nr++;

                     _paczka.Oznaczenie = _paczkaPrefix + _paczka.nr.ToString() + "/" + IdJednostki.ToString() + "/" + _paczka.rok.ToString();
                 }
                 else
                 {
                     _paczka = new Paczka();
                     _paczka.rok = DateTime.Today.Year;
                     _paczka.nr = 1;
                     _paczka.miesiac = IdJednostki;
                     _paczka.Oznaczenie = _paczkaPrefix + _paczka.nr.ToString() + "/" + IdJednostki.ToString() + "/" + _paczka.rok.ToString();

                 }
                 eventargs = new PaczkaNoEventArgs();
                 eventargs.Paczka = _paczka;
                 OnpaczkaNoCompleted(eventargs);
             };

         }

    }
}