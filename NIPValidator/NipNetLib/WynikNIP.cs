using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipNetLib
{
    public class WynikNIP
    {
        public int Status { get; set; }
        public string Tresc { get; set; }
        public string Data { get; set; }

        public string NIP { get; set; }
        public WynikNIP( int  _status, string _tresc, string _data, string _nip)
        {
            this.Status = _status;
            this.Tresc = _tresc;
            this.Data = _data;
            this.NIP = _nip;

        }
        public override string ToString()
        {
            return NIP+":"+Status.ToString()+Tresc+Data;
        }

    }
}
