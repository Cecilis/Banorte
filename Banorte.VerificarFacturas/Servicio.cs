using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Banorte.VerificarFacturas.ConsultaCFDIService;
using System.IO;
using Banorte.VerificarFacturas.SAPConnector;
using log4net;
using SAP.Middleware.Connector;
using System.Configuration;



namespace Banorte.VerificarFacturas
{
	public partial class Servicio : ServiceBase
	{
		private EventLog eventLog;
        private string nl = Environment.NewLine;
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread oThread;
        private static int eventId = 0;

        public static int EventID
        {
            get { return Servicio.eventId; }
            set { Servicio.eventId = value; }
        }
        


        ///// <summary> 
        ///// Variable requerida por el Windows.Forms Component Designer.
        ///// </summary>
        //private System.ComponentModel.Container components = null;

		public Servicio()
		{

            try
            {
			    log4net.Config.XmlConfigurator.Configure();
                
                InitializeComponent();
                
                SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();

                if (EventLog.SourceExists("BanorteVerificarFacturaSATLogSource"))
                { 
                    EventLog.DeleteEventSource("BanorteVerificarFacturaSATLogSource");
                }

                if (!EventLog.SourceExists("BanorteVerificarFacturaSATLogSource"))
                {
                    EventLog.CreateEventSource("BanorteVerificarFacturaSATLogSource", "Application");
                }

                log.Info("Verificar Facturas SAT Servicio - Iniciado");

                EventLog.WriteEntry("Verificar Facturas SAT - Servicio - Iniciado", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                log.Error("Verificar Facturas SAT Servicio - Iniciado", ex);
                EventLog.WriteEntry("Verificar Facturas SAT - Servicio - Excepción: ", ex.Message + " Trace: " + ex.StackTrace, EventLogEntryType.Error, EventID, short.MaxValue);
            }			
		}

		protected override void OnStart(string[] args)
		{
            try
            {
                log.Info("Verificar Facturas SAT Servicio SAT - Iniciando");
                EventLog.WriteEntry("Verificar Facturas SAT - Iniciando ", EventLogEntryType.Information);

                oThread = new Thread(GestorServicio);
                oThread.Name = "Banorte Verificar Facturas";
                oThread.IsBackground = true;
                oThread.Start();

                log.Info("Iniciado");
                EventLog.WriteEntry("Verificar Facturas SAT - Iniciado ", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                log.Error("OnStart ", ex);
                EventLog.WriteEntry("Verificar Facturas SAT  - OnStart - Excepción: ", ex.Message + " Trace: " + ex.StackTrace, EventLogEntryType.Error, EventID, short.MaxValue);
            }
		}



        private void GestorServicio(object state)
        {
            try
            {

                SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
                log4net.Config.XmlConfigurator.Configure();

                while (!_shutdownEvent.WaitOne(0))
                {
                    try
                    {
                        if (sapConnectorInterface.Inicializado())
                        {
                            using (DataSet dsFacturasSinVerificar = sapConnectorInterface.consultaFacturaPorEstatus(ConfigurationManager.AppSettings["NAME"]))
                            {
                                try
                                {
                                    log.Info("Verificar Facturas SAT - Consulta - Facturas en SAT ");
                                    DataTable dtFacturasSinVerificar = dsFacturasSinVerificar.Tables[0];
                                    Task callConsultaCFDIServiceTask = Task.Run(() => ConsultarRegistrosCFDIServiceTask(dtFacturasSinVerificar));
                                    callConsultaCFDIServiceTask.Wait();
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Verificar Facturas SAT - RFC - Excepción ", ex);
                                    EventLog.WriteEntry("Verificar Facturas SAT - RFC - Excepción: ", ex.Message + " Trace: " + ex.StackTrace, EventLogEntryType.Error, EventID, short.MaxValue);
                                    throw;
                                }
                            }
                        }
                        else throw new ArgumentNullException("Verificar Facturas SAT - SAP Connector Interface no ha sido inicializado");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Verificar Servicio SAT - Exception: ", ex);
                        EventLog.WriteEntry("Verificar Facturas SAT - GestorServicio - RFC - Excepción: ", ex.Message + " Trace: " + ex.StackTrace, EventLogEntryType.Error, EventID, short.MaxValue);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error("Verificar Servicio SAT - Gestor Servicio ", ex);
                EventLog.WriteEntry("Verificar Facturas SAT - GestorServicio - Excepción: ", ex.Message + " Trace: " + ex.StackTrace, EventLogEntryType.Error, EventID, short.MaxValue);
            }

        }



        static public async Task ConsultarRegistrosCFDIServiceTask(DataTable dtFacturasSinVerificar)
        {
            try
            {
                string estatusConsulta = "N";
                string mensajeConsulta = string.Empty;
                bool resultadoActualizarSAP = false;

                log.Info("Verificar Facturas SAT - Consulta Registros CFDI");
                foreach (DataRow row in dtFacturasSinVerificar.Rows)
                {
                    string strSTATU = row["STATU"].ToString().Trim().ToUpper();
                    string strVESAT = row["VESAT"].ToString().Trim();
                    string strFUUID = row["FUUID"].ToString().Trim();
                    string strRFCEM = row["RFCEM"].ToString().Trim();
                    string strRFCRE = row["RFCRE"].ToString().Trim();
                    string strWRBTR = row["WRBTR"].ToString().Trim().Replace(".", "").Replace(",", ".");

                    string expresionimpresa = "?re=" + strRFCEM.ToUpper() + "&rr=" + strRFCRE.ToUpper() + "&tt=" + strWRBTR + "&id=" + strFUUID;


                    bool bConsultaSinErrores = false;

                    using (ConsultaCFDIServiceClient oConsultaCFDIService = new ConsultaCFDIServiceClient())
                    {
                        try
                        {
                            Acuse oAcuse = await oConsultaCFDIService.ConsultaAsync(expresionimpresa);
                            oConsultaCFDIService.Close();
                            estatusConsulta = oAcuse.CodigoEstatus.Split('-')[0].ToString().Trim() == "S" ? "002" : "006";
                            mensajeConsulta = oAcuse.CodigoEstatus;
                            bConsultaSinErrores = true;
                        }
                        catch (Exception ex)
                        {   
                            log.Error("Verificar Facturas SAT - Envio SAT - Expresión: " + expresionimpresa, ex);
                            throw;
                        }
                    }

                    if (bConsultaSinErrores)
                    {
                        SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
                        resultadoActualizarSAP = sapConnectorInterface.actualizarEstatusFactura(estatusConsulta, strFUUID, ConfigurationManager.AppSettings["NAME"]);

                        log.Info("Verificar Facturas SAT - Consulta SAT " + Environment.NewLine + "Expresión: " + expresionimpresa + Environment.NewLine + "Respuesta SAT: " + mensajeConsulta + Environment.NewLine + "Respuesta Actualización SAP: " + resultadoActualizarSAP);
                    }

                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Verificar Facturas SAT - Consulta SAT - Excepción: ", e.Message + " Trace: " + e.StackTrace, EventLogEntryType.Error, EventID, short.MaxValue);
                throw;
            }
        }


		protected override void OnStop()
		{
            log.Info("OnStop - Deteniendo Servicio");
			EventLog.WriteEntry("Verificar Facturas SAT - Deteniendo el servicio");
            _shutdownEvent.Set();
            if (!oThread.Join(3000))
            { 
                oThread.Abort();
            }
            log.Info("OnStop - Servicio Detenido");
            EventLog.WriteEntry("Verificar Facturas SAT - OnStop - Servicio Detenido");
            
		}
		protected override void OnContinue()
		{
            log.Info("OnContinue - Reiniciando Servicio");
            EventLog.WriteEntry("Verificar Facturas SAT - OnContinue - Reiniciando Servicio");
        }

        private void eventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}

//Para instalarlo
//Ubicar en bin/debug lo compilado 
//copiarlo en c o en una ubicación facil de usar por ejemplo C:\VerificarFacturas
//abrir cmd IMPORTANTE  ejecutar como administrador
//en cmd ejecutar >> CD C:\Windows\Microsoft.NET\Framework\v4.0.30319\
// ahi, en esa ruta debe estar installutil.exe  
//en cmd ejecutar >> installutil.exe C:\VerificarFacturas\Banorte.VerificarFacturas.exe    

//Para correrlo en servicios: Actualizar Estátus Facturas
//Este nombre se coloca en ProjectInstaller  --> serviceInstaller --> Propiedades

//Para desinstalarlo
//installutil.exe /u C:\VerificarFacturas\Banorte.VerificarFacturas.exe  