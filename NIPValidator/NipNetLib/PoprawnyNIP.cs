using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipNetLib
{
    class PoprawnyNIP
    {

        
        public static bool SprawdzNIP(string NIPNumer)
        {
           
            if(NIPNumer.Length != 10 )
            {
                return false;
            } else
            {
                foreach (char c in NIPNumer)
                {
                    if (c < '0' || c > '9')
                        return false;
                }

                byte[] NIP = NIPNumer.Select(c => byte.Parse(c.ToString())).ToArray();
                int sum = 6 * NIP[0] +
                          5 * NIP[1] +
                          7 * NIP[2] +
                          2 * NIP[3] +
                          3 * NIP[4] +
                          4 * NIP[5] +
                          5 * NIP[6] +
                          6 * NIP[7] +
                          7 * NIP[8];
                sum %= 11;
                if (NIP[9] == sum) { return true; }  else { return false; }
            }
           
            
        }

        
    }
}
