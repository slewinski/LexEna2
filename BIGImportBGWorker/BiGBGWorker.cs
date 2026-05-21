using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexEnaTrs.Web;
using LexEnaTrs.Web.BIG;

namespace BIGImportBGWorker
{
    class BiGBGWorker
    {
        static void Main(string[] args)
        {
            Utils.LogWriter("Start Big Worker.exe");
            int idJob = 0;
            int Big_ImportId = 0;

            string option = "";
            if (args.Length == 0)
            {

                Utils.LogWriter("Brak parametrów wejściowych");
                return;
            }
            try
            {   if (args.Length == 2)
                {
                    option = args[0].ToUpper();
                    idJob = Int32.Parse(args[1]);
                }

            else if (args.Length == 3)
                {
                    option = args[0].ToUpper();
                    idJob = Int32.Parse(args[1]);
                    Big_ImportId  = Int32.Parse(args[2]);
                }
            else
                idJob = Int32.Parse(args[0]);
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błędny parametr wejściowy");
                return;

            }
            Utils.LogWriter("Start Big Worker proceed");
            BIGProceed bp = new BIGProceed(idJob, option,Big_ImportId);

            bp.ProceedJob();


        }
    }
}
