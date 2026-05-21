using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EpuProxy;

namespace EPUProxySrv
{
    [ServiceContract(Namespace = "http://proglex.pl")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EPUProxyJobSrv
    {
        [OperationContract]
        public int InsertJob(RodzajMetody rodzajMetodyP, TerminWykonania terminWykonaniaP)
        {
            EpuProxyClient nowyKlient = new EpuProxyClient();
           
            int wynik = nowyKlient.DodajZadanie(rodzajMetodyP, terminWykonaniaP);

            return wynik;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
