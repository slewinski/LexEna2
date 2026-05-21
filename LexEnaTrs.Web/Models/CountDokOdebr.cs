using System;
using System.Collections.Generic;
using System.Linq;

namespace LexEnaTrs.Web
{

    public class CountDokOdebr
    {
        public int TypDok { get; set; }
        public int Id_Jednostki { get; set; }
        public int Id_User { get; set; }
        public int IsChecked { get; set; }
        public int Liczba { get; set; }

    }

    public class impdet
    {

        public int Code { get; set; }
        public DateTime DataImportu { get; set; }
        public int ErrLevel { get; set; }
        public string Sygnatura { get; set; }
        public string ErrDescription { get; set; }


    }


        public class impDescriptor 
    {
        public Import imp { get; set; }
        public List<ImportDetail> impdet { get; set; }

       
    }


  


    public class impDescr
    {
        //   public Import imp { get; set; }
        public DateTime DataTransferu { get; set; }
        public int ContentType { get; set; }
        public int ImpExp { get; set; }
        public int StatusOperacji { get; set; }
        public string OpisOperacji { get; set; }
        public List<impdet> impdt { get; set; }
        
    }

}