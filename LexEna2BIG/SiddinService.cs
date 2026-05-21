using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LexEna2BIG.KRDSiddinService;
using System.Web.Hosting;
using System.Net;

namespace LexEna2BIG
{

    public class Utils
    {

        public static void LogNamedWriter(string logMesgParam, string fname)
        {
            //Ustawienia ustawienia = new Ustawienia();
            //switch(ustawienia.logowanie)
            //{
            //   case 1:

            using (StreamWriter w = File.AppendText(HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + fname) : fname))
            {

                Log(logMesgParam, w);

                // Close the writer and underlying file.
                w.Close();
            }
            //     break;
            //    case 2:
            //       System.Console.WriteLine(logMesgParam);
            //      break;

            //}

        }

        public static string exceptionMsg(Exception ex)
        {
            return ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : "");


        }




        public static void LogWriter(string logMesgParam)
        {
            //Ustawienia ustawienia = new Ustawienia();
            //switch(ustawienia.logowanie)
            //{
            //   case 1:
            using (StreamWriter w = File.AppendText(HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/LexEnaLog.txt") : "LexEnaLog.txt"))
            {
                Log(logMesgParam, w);

                // Close the writer and underlying file.
                w.Close();

            }

            //     break;
            //    case 2:
            //       System.Console.WriteLine(logMesgParam);
            //      break;

            //}

        }
        private static void Log(string logMessage, TextWriter w)
        {
            // w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            // w.WriteLine("  :");
            w.WriteLine("{0}\t", logMessage);
            // w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
        }
    }


    public class InspectorBehavior : IEndpointBehavior
    {
        public string LastRequestXML
        {
            get
            {


                return myMessageInspector.LastRequestXML;
            }
        }

        public string LastResponseXML
        {
            get
            {
                return myMessageInspector.LastResponseXML;
            }
        }





        private MyMessageInspector myMessageInspector = new MyMessageInspector();
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }


        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(myMessageInspector);
        }
    }


    public class MyMessageInspector : IClientMessageInspector
    {
        public string LastRequestXML { get; private set; }
        public string LastResponseXML { get; private set; }
        public string PackageFullId { get; set; }



        private void ChangeMessage(ref System.ServiceModel.Channels.Message message)
        {
            MemoryStream ms = new MemoryStream();
            Encoding encoding = Encoding.UTF8;
            XmlWriterSettings writerSettings = new XmlWriterSettings { Encoding = encoding };
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateDictionaryWriter(XmlWriter.Create(ms));
            message.WriteBodyContents(writer);
            writer.Flush();
            string messageBodyString = encoding.GetString(ms.ToArray());

            // change the message body

            messageBodyString = messageBodyString.Replace("<success />", "<success>empty</success>");
            messageBodyString = messageBodyString.Replace("<processingError />", "<processingError>empty</processingError>");
            messageBodyString = messageBodyString.Replace("<authError />", "<authError>empty</authError>");
            messageBodyString = messageBodyString.Replace("<operationError />", "<operationError>empty</operationError>");
            messageBodyString = messageBodyString.Replace("<validationError />", "<validationError>empty</validationError>");

            messageBodyString = messageBodyString.Replace("[MS]error", "failed");
            messageBodyString = messageBodyString.Replace("[MS]pending", "pending");

            ms = new MemoryStream(encoding.GetBytes(messageBodyString));
            XmlReader bodyReader = XmlReader.Create(ms);
            System.ServiceModel.Channels.Message originalMessage = message;
            message = System.ServiceModel.Channels.Message.CreateMessage(originalMessage.Version, null, bodyReader);
            message.Headers.CopyHeadersFrom(originalMessage);
        }


        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            LastResponseXML = reply.ToString();

            using (LexEnaProEntities context = new LexEnaProEntities())
            {
                BIG_Log bl = new BIG_Log();
                bl.ReqResp = true; // response
                bl.MsgText = LastResponseXML;
                bl.PackageFullId = PackageFullId;
                if (LastResponseXML.Contains("packageSubmit"))

                    bl.TypKom = 1;
                else
                    bl.TypKom = 0;

                bl.DataOper = DateTime.Now;
                context.BIG_Log.Add(bl);
                context.SaveChanges();
            }
            // this.ChangeMessage(ref reply);
            //MessageFault mf = new MessageFault();

            // System.ServiceModel.Channels.Message replacedMessage = System.ServiceModel.Channels.Message.CreateMessage(reply.Version, null , NewResp);
            // replacedMessage.Headers.CopyHeadersFrom(reply.Headers);
            // replacedMessage.Properties.CopyProperties(reply.Properties);

            //reply = replacedMessage;
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            LastRequestXML = request.ToString();

            PackageFullId = "";
            // 
            using (LexEnaProEntities context = new LexEnaProEntities())
            {
                BIG_Log bl = new BIG_Log();
                bl.ReqResp = false; // request
                bl.MsgText = LastRequestXML;
                bl.PackageFullId = PackageFullId;
                if (LastRequestXML.Contains("packageSubmit"))

                    bl.TypKom = 1;
                else
                    bl.TypKom = 0;


                bl.DataOper = DateTime.Now;
                context.BIG_Log.Add(bl);
                context.SaveChanges();
            }

            return request;
        }
    }




    public class SiddinService
    {
        KRDSiddinService.ImportClient theClient;
        string token;
        string errMessage;
        BIG_Konfig konf;

        public string getErrorMsg()
        {
            return errMessage;

        }





        private bool setupCilent()
        {// 
         // BIG_Konfig bk = null;

            try
            {
                if (this.theClient != null) return true;
                using (LexEnaProEntities context = new LexEnaProEntities())
                {
                    ;//; bk = context.BIG_Konfig.FirstOrDefault();
                    this.konf = context.BIG_Konfig.FirstOrDefault();
                    if (this.konf == null)
                    {
                        errMessage = "Nie zdefiniowano parametrów połączenia z Krd";
                        return false;
                    }
                    /*
                    if (bk == null)
                    {
                        MessageBox.Show("Brak konfiguracji usługi sieciowej KRD");
                        return false;
                    }
                    */

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    BasicHttpBinding basicHttpBinding;
                    //System.ServiceModel.Channels.Binding  basicHttpBinding;
                    basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);



                    basicHttpBinding.SendTimeout = new TimeSpan(0, 5, 0);//
                    basicHttpBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                    basicHttpBinding.OpenTimeout = new TimeSpan(0, 2, 0);
                    basicHttpBinding.MaxReceivedMessageSize = 2147483647;
                    basicHttpBinding.MaxBufferSize = 2147483647;
                    basicHttpBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
                    basicHttpBinding.MaxBufferPoolSize = 2147483647;
                    EndpointAddress thEndpoint = new EndpointAddress(new Uri(konf.Endpoint));
                    theClient = new KRDSiddinService.ImportClient(basicHttpBinding, thEndpoint);
                }

                /*
                ServiceReferenceCheckStatus.G2BIG_checkStatus_outClient theClient1 = new ServiceReferenceCheckStatus.G2BIG_checkStatus_outClient("HTTP_Status");
                

                theClient = new ServiceReferenceCheckStatus.G2BIG_checkStatus_outClient(cbind, basicAuthEndpoint);
                theClient.ClientCredentials.UserName.UserName = bk.CheckRqAuthUser;
                theClient.ClientCredentials.UserName.Password = Utils.Decrypt(bk.CheckRqAuthPass, "Application error");
                */

                var requestInterceptor = new InspectorBehavior();
                theClient.Endpoint.Behaviors.Add(requestInterceptor);

                return true;

            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd inicjalizacji klienta " + ex.Message);
                return false;
            }


        }

        private bool login()
        {
            if (!setupCilent())
                return false;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                KRDSiddinService.LoginExRequest lrq = new KRDSiddinService.LoginExRequest();
                lrq.UserName = konf.login;
                lrq.Password = konf.password;

                this.token = theClient.Login(lrq);
                if (!String.IsNullOrWhiteSpace(this.token))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;

            }

        }

        private bool logout()
        {
            if (!setupCilent())
                return false;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                KRDSiddinService.LogoutRequest lrq = new KRDSiddinService.LogoutRequest();
                lrq.Ticket = this.token;


                theClient.Logout(lrq);
                this.token = "";
                return true;

            }
            catch (Exception ex)
            {
                this.token = "";
                return false;

            }

        }

        private List<string> Split(string str, int chunkSize)
        {
            List<string> tab = new List<string>();
            if (String.IsNullOrWhiteSpace(str))
            {
                return new List<string>();


            }
            int parts;
            
            parts = (str.Length % chunkSize) == 0 ? (str.Length / chunkSize) : (str.Length / chunkSize) + 1;
            for (int i = 0; i < parts;i++)
            {
                tab.Add(str.Substring(i * chunkSize, i == parts - 1 ? str.Length - i * chunkSize : chunkSize));
            }
            return tab;
        }

        public Guid? sendData(string message)
        {
            Guid? JobID = null;

            if (!setupCilent())
                return null;
            if (!login())
            {
                Utils.LogWriter("Błąd logowania do KRD");
                this.errMessage = "Błąd logowanie. Czy nie zmieniono hasła do KRD ?";
                return null;

            }
            try
            {
                UpladChunkRequest theBag = new UpladChunkRequest();

                List<string> tab = this.Split(message, 3000000);
                theBag.Ticket = this.token;
                
                ChunkBag bag = null;

                foreach (string part in tab)
                {
                    theBag.Data = part;
                    bag = theClient.UploadChunk(theBag);
                    theBag.ChunkBag = bag;
                }
                Utils.LogWriter("bag Uploaded ");


                CloseChunkBagRequestEx chex = new CloseChunkBagRequestEx();
                chex.Ticket = this.token;
                chex.ExecuteDifference = false;
                chex.GetDifference = false;
                chex.ChunkBagEx = new ChunkBagEx();
                chex.ChunkBagEx.Count = bag.Count;
                chex.ChunkBagEx.ID = bag.ID;
                chex.ChunkBagEx.ProtocolType = ProtocolEnum.Nicci;
                chex.ChunkBagEx.ProtocolVersion = ProtocolVersionEnum.Version_3_1;
                chex.ChunkBagEx.Size = bag.Size;
                JobID = theClient.CloseChunkBagEx(chex);
                Utils.LogWriter("bag closed ");
                /*
                CloseChunkBagRequest bagRq = new CloseChunkBagRequest();
                bagRq.Ticket = this.token;
                bagRq.ChunkBag = bag;
                Guid JobID;
                try
                {
                    theClient.CloseChunkBag(bagRq);
                }
                catch (Exception ex)
                {
                    CloseChunkBagExRequest chex = new CloseChunkBagExRequest();
                    chex.CloseChunkRequestEx = new CloseChunkBagRequestEx();
                    chex.CloseChunkRequestEx.Ticket = this.token;

                    theClient.CloseChunkBagEx()


                }
                */
                this.logout();

                return JobID;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd podczas wysyłki pakietu " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : ""));
                if (JobID != null)
                    return JobID;
                this.errMessage = "Błąd podczas wysyłki pakietu " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return null;
            }
        }

        public int getConfirmation(Guid jobId, out string answer)
        {
            int statValue = -1;
            answer = string.Empty;
            if (!setupCilent())
            {
                answer = string.Empty;
                return -1;
            };
            if (!login())
            {
                Utils.LogWriter("Błąd logowania do KRD");
                this.errMessage = "Błąd logowania. Czy nie zmieniono hasła do KRD ?";
                answer = string.Empty;
                return -1;
            };
            try
            {
                GetJobsRequest gjr = new GetJobsRequest();
                gjr.JobId = jobId;
                gjr.Ticket = this.token;
                Job[] jobarr;

                jobarr = theClient.GetJobs(gjr);
                answer = String.Empty;
                if (jobarr != null && jobarr.Any())
                {
                    statValue = (int)(jobarr[0].StatusCode);
                    if (jobarr[0].StatusCode == JobStatusCodeEnum.PROCESSED)
                    {
                        GetChunkBagRequestEx gcbe = new GetChunkBagRequestEx();
                        //gcbe.ChunkSize =  jobarr[0].
                        gcbe.JobId = jobarr[0].JobID;
                        gcbe.Ticket = this.token;
                        gcbe.ChunkSize = Int32.MaxValue;
                        ChunkBagEx cbe = theClient.GetChunkBagEx(gcbe);
                        if (cbe != null)
                        {
                            DownloadChunkRequest dcr = new DownloadChunkRequest();
                            dcr.ChunkBag = new ChunkBag();
                            dcr.Number = 1;
                            dcr.ChunkBag.Count = cbe.Count;
                            dcr.ChunkBag.ID = cbe.ID;
                            dcr.ChunkBag.NicciVersion = NicciVersionEnum.Version_3_1;
                            dcr.ChunkBag.Size = cbe.Size;
                            dcr.Ticket = this.token;
                            answer = theClient.DownloadChunk(dcr);



                        }

                    }



                }

                this.logout();

                return statValue;
            }
            catch (Exception ex)
            {

                if (!String.IsNullOrEmpty(answer))
                    return 2;
                this.errMessage = ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return -1;
            }



        }

    }
}
