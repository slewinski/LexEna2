using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractPdf
{
    public class ExtractBin
    {

        /*
        select sp.sygnatura, sp.ident, dn.nazwa, s.skan, s.typ ,  do.d_dok from dok_odebr do  inner join skan s on s.id_dok_odebr = do.ident inner join sprawa sp on sp.ident = do.id_sprawy inner join dok_odebr_nazwy dn on dn.ident = do.id_dok_typ
where isnull(do.czyus,0) = 0  

    */


    }
}
