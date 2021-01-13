using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banorte.SAPConnector;
using Banorte.ConsolaPruebas.ConsultaCFDIService;
using SAP.Middleware.Connector;
using System.Threading;
using log4net;
using System.Configuration;

namespace Banorte.ConsolaPruebas
{
    class Program
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();

            log4net.Config.XmlConfigurator.Configure();

            while (true)
            {
                try
                {
                    log.Info("BVFS - Verificando Inicialización");
                    if (sapConnectorInterface.Inicializado())
                    {
                        log.Info("BVFS - Filtro - Facturas pendientes por verficar en SAT");
                        using (DataSet dsFacturasSinVerificar = sapConnectorInterface.consultaFacturaPoEstatus(ConfigurationManager.AppSettings["NAME"]))
                        {
                            try
                            {
                                log.Info("BVFS - Consulta - Facturas en SAT ");
                                DataTable dtFacturasSinVerificar = dsFacturasSinVerificar.Tables[0];
                                Task callConsultaCFDIServiceTask = Task.Run(() => ConsultarRegistrosCFDIServiceTask(dtFacturasSinVerificar));
                                callConsultaCFDIServiceTask.Wait();
                            }
                            catch (Exception ex)
                            {
                                log.Error("BVFS - Consulta - Facturas en SAT ", ex);
                                throw;
                            }
                        }
                    }
                }
                catch (RfcLogonException ex)
                {
                    log.Error("RfcLogonException: ", ex);
                }
                catch (RfcCommunicationException ex)
                {
                    log.Error("RfcCommunicationException: ", ex);
                }
                catch (RfcAbapRuntimeException ex)
                {
                    log.Error("RfcAbapRuntimeException: ", ex);
                }
                catch (RfcAbapBaseException ex)
                {
                    log.Error("RfcAbapBaseException: ", ex);
                }
                catch (RfcInvalidStateException ex)
                {
                    log.Error("RfcInvalidStateException: ", ex);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("NullReferenceException: ", ex);
                }
                catch (IndexOutOfRangeException ex)
                {
                    log.Error("IndexOutOfRangeException: ", ex);
                }
                catch (Exception ex)  
                {
                    log.Error("Exception: ", ex);
                }
            }

        }

        static public async Task ConsultarRegistrosCFDIServiceTask(DataTable dtFacturasSinVerificar)
        {
            try
            {
                string estatusConsulta = "N";
                string mensajeConsulta = string.Empty;
                bool resultadoActualizarSAP = false;

                log.Info("BVFS - Consulta Registros CFDI");

                foreach (DataRow row in dtFacturasSinVerificar.Rows)
                {

                    string strSTATU = row["STATU"].ToString().Trim().ToUpper();
                    string strVESAT = row["VESAT"].ToString().Trim();
                    string strFUUID = row["FUUID"].ToString().Trim();
                    string strRFCEM = row["RFCEM"].ToString().Trim();
                    string strRFCRE = row["RFCRE"].ToString().Trim();
                    string strWRBTR = row["WRBTR"].ToString().Trim().Replace(".", "").Replace(",", ".");

                    

                    string expresionimpresa = "?re=" + strRFCEM.ToUpper() + "&rr=" + strRFCRE.ToUpper() + "&tt=" + strWRBTR + "&id=" + strFUUID;
                    log.Info("BVFS - Consulta Registros CFDI - Expresión Impresas: " + expresionimpresa);

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
                        catch(Exception ex)
                        {
                            throw;
                        }
                    }

                    log.Info("BVFS - Consulta Registros CFDI - Mensaje consulta: " + mensajeConsulta);

                    log.Info("BVFS - Consulta Registros CFDI - Estatus: " + estatusConsulta);

                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine(expresionimpresa);
                    Console.WriteLine(mensajeConsulta);
                    Console.WriteLine(estatusConsulta);

                    if (bConsultaSinErrores)
                    {
                        SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
                        resultadoActualizarSAP = sapConnectorInterface.actualizarEstatusFactura(estatusConsulta, strFUUID, ConfigurationManager.AppSettings["NAME"]);
                    }
                    else
                    {
                        log.Error("BVFS - Consulta Registros CFDI - Ha ocurrido un error al intentar consultar en SAT");
                    }
                    Console.WriteLine(resultadoActualizarSAP);

                    log.Info("BVFS - Actualizar Estatus SAP: " + resultadoActualizarSAP);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
