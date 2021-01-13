using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using System.Data;
using System.Globalization;
using Banorte.Models.xml;
using System.Configuration;

namespace Banorte.VerificarFacturas.SAPConnector
{
    public class SapConnectorInterface
    {
        
        private RfcDestination rfcDestination;

        public bool testConnection(string destinationName)
        {
            bool result = false;
            try
            {
                rfcDestination = RfcDestinationManager.GetDestination(ConfigurationManager.AppSettings["NAME"]);
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw; 
            }

            return result;
        }

        public bool Inicializado()
        {
            if (!SAPDestinationConfig.Inicializado)
            {
                SAPDestinationConfig cfg = new SAPDestinationConfig();
                RfcDestinationManager.RegisterDestinationConfiguration(cfg);
                SAPDestinationConfig.Inicializado = true;
            }
            return SAPDestinationConfig.Inicializado;
        }
        
        public DataSet consultaFacturaPorEstatus(string destinationName)
        {
            DataSet dsFactura = new DataSet();
            try
            {
                RfcDestination rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                
                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_CONSULTA_ESTATUS");
                rfcFunction.SetValue("STATU", "001");
                rfcFunction.Invoke(rfcDestination);
                dsFactura.Tables.Add(ConvertToDotNetTable(rfcFunction.GetTable("ESTATUS")));
                int nro = dsFactura.Tables[0].Rows.Count;
                IRfcDataContainer rfcDataContainer = rfcFunction.GetTable("ESTATUS");
            }
            catch (Exception ex)
            {
                throw;
            }

            return dsFactura;
        }

        public bool actualizarEstatusFactura(string estatus,string uuid_factura,string destinationName)
        {
             try
            {
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }

                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_MODIFICAR_ESTATUS");
                rfcFunction.SetValue("FUUID", uuid_factura);
                rfcFunction.SetValue("STATU", estatus);                
                rfcFunction.Invoke(rfcDestination);

                char result = rfcFunction.GetChar("RESULT");
                string mensaje = rfcFunction.GetString("MENSAJE");
                bool respuesta = ConvertRespuestaBD(result);

                Console.WriteLine(mensaje);


                return respuesta;     

            }
            catch (Exception ex)
            {
                throw new Exception("Error en actualizarEstatusFactura " + ex);
            }
        }
        public DataTable ConvertToDotNetTable(IRfcTable rfcTable)
        {
            DataTable dtTable = new DataTable();
            for (int item = 0; item < rfcTable.ElementCount; item++)
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(item);
                dtTable.Columns.Add(metadata.Name);
            }
            foreach (IRfcStructure row in rfcTable)
            {
                DataRow dr = dtTable.NewRow();
                for (int item = 0; item < rfcTable.ElementCount; item++)
                {
                    RfcElementMetadata metadata = rfcTable.GetElementMetadata(item);
                    dr[item] = row.GetString(metadata.Name);
                }
                dtTable.Rows.Add(dr);
            }
            return dtTable;
        }

        public static bool ConvertRespuestaBD(char input)
        {
            int result = 0;
            int.TryParse(input.ToString(), out result);
            return result == 1 ? true : false;
        }


    }
}