using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using System.Data;
using System.Globalization;
using Banorte.Models.xml;
using Banorte.Persistencia;
using Banorte.Models;
using System.Configuration;

namespace Banorte.SAPConnector
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
                throw new Exception("Fallo la conexion " + ex.Message);
            }

            return result;
        }

        public DateTime convertToDate(string fecha)
        {

            // Parse date and time with custom specifier.
            //string format = "dd/mm/yyyy 00:00:00";
            string format = "dd.mm.yyyy hh:mm:tt";
            DateTime date = new DateTime();
            DateTime f2 = DateTime.Now;
            try
            {
                date = DateTime.ParseExact(fecha, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                throw new ArgumentException("dtFechaActual", e);
            }

            return date;
        }


        public DataSet consultaFacturaByStatus(string destinationName)
        {
            DataSet dsFactura = new DataSet();
            try
            {
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }

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
                throw new Exception("Error en consultaFacturaByStatus " + ex);
            }

            return dsFactura;
        }

        public DataSet consultaFacturaByFecha(string fecha_desde, string fecha_hasta, string idProveedor, string destinationName)
        {
            DataSet dsFactura = new DataSet();
            try
            {
                
                DateTime dateToDisplay = new DateTime(2008, 10, 1);
                CultureInfo ci = new CultureInfo("de-DE");

                string fechaDesdeFormateada = formatFecha(fecha_desde, "dd/MM/yyyy", "yyyyMMdd");
                string fechaHastaFormateada = formatFecha(fecha_hasta, "dd/MM/yyyy", "yyyyMMdd");

                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }

                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_CONSULTA_ESTATUS_PROV");

                rfcFunction.SetValue("BUDAT_DESDE", fechaDesdeFormateada);
                rfcFunction.SetValue("BUDAT_HASTA", fechaHastaFormateada);
                rfcFunction.SetValue("LIFNR", idProveedor);
                rfcFunction.Invoke(rfcDestination);

                dsFactura.Tables.Add(ConvertToDotNetTable(rfcFunction.GetTable("ESTATUS")));

            }
            catch (Exception ex)
            {
                throw new Exception("Error en consultaFacturaByFecha " + ex);
            }

            return dsFactura;
        }


        public void incluirFactura2(Comprobante comprobante, string versionCFDI, string concepto,string departamento, string indicadorIva,string idProveedor,string destinationName)
        {
            
            try
            {
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }               

                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_INTERMEDIO_PROVEEDORES");
               
                //RfcStructureMetadata metadata_header = rfcRepository.GetStructureMetadata("ZFIAP_EST_INTERM_CAB");
                RfcStructureMetadata metadata_header = rfcRepository.GetStructureMetadata("ZFIAP_INTERM_CAB_EST");
                
                IRfcStructure structutre_header = metadata_header.CreateStructure();
                structutre_header = getHeader(metadata_header, comprobante, versionCFDI);

                /*LLENO LA ESTRUCTURA DE HEADER*/
               /* structutre_header.SetValue("GJAHR", "2017");
                structutre_header.SetValue("XBLNR", "2459419");
                structutre_header.SetValue("STATU", "001");
                structutre_header.SetValue("BUDAT", "20171103");
                structutre_header.SetValue("WAERS", "MXN");
                structutre_header.SetValue("VESAT", "");
                structutre_header.SetValue("BKTXT", "TOMAGRAFIA A.C. DE CRANEO SYC");*/

        

                /*LLENO LA ESTRUCTURA DE ITEM*/
                RfcStructureMetadata metadata_item = rfcRepository.GetStructureMetadata("ZFIAP_EST_INTERM_POS");
                IRfcStructure strutcture_item = metadata_item.CreateStructure();

                strutcture_item = getItem(metadata_item, comprobante, versionCFDI, concepto, departamento, indicadorIva, idProveedor);

                rfcFunction.SetValue("HEADER", structutre_header);
                rfcFunction.SetValue("ITEM", strutcture_item);
                
                rfcFunction.Invoke(rfcDestination);
                
                try
                {
                    char result = rfcFunction.GetChar("RESULT");
                    bool val = ConvertRespuestaBD(result);
                    bool resp = val;

                }
                catch (Exception ex)
                {
                    throw new Exception("obteniendo el export " + ex);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error en incluirFactura " + ex);
            }
        }


        public bool incluirFactura(Comprobante comprobante, string versionCFDI, string concepto, string departamento, string indicadorIva, string idProveedor, string destinationName)
        {

            try
            {
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }

                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_INTERMEDIO_PROVEEDORES");                
                RfcStructureMetadata metadata_header = rfcRepository.GetStructureMetadata("ZFIAP_INTERM_CAB_EST");
                IRfcStructure structutre_header = metadata_header.CreateStructure();
                structutre_header = getHeader(metadata_header, comprobante, versionCFDI);               
                RfcStructureMetadata metadata_item = rfcRepository.GetStructureMetadata("ZFIAP_EST_INTERM_POS");
                IRfcStructure strutcture_item = metadata_item.CreateStructure();
                strutcture_item = getItem(metadata_item, comprobante, versionCFDI, concepto, departamento, indicadorIva, idProveedor);
                rfcFunction.SetValue("HEADER", structutre_header);
                rfcFunction.SetValue("ITEM", strutcture_item);              
                rfcFunction.Invoke(rfcDestination);
                char result = rfcFunction.GetChar("RESULT");
                bool respuesta = ConvertRespuestaBD(result);
                return respuesta;                   
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error en incluirFactura " + ex);
            }
        }

        public bool actualizarEstatusFactura(string estatus, string uuid_factura, string destinationName)
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
                bool respuesta = ConvertRespuestaBD(result);
                return respuesta;                   


            }
            catch (Exception ex)
            {
                throw new Exception("Error en actualizarEstatusFactura " + ex);
            }
        }
        //public DataTable ConvertToDotNetTable(IRfcTable rfcTable)
        //{

        //    DataTable dtTable = new DataTable();
        //    //create table

        //    for (int item = 0; item < rfcTable.ElementCount; item++)
        //    {
        //        RfcElementMetadata metadata = rfcTable.GetElementMetadata(item);
        //        dtTable.Columns.Add(metadata.Name);
        //    }

        //    foreach (IRfcStructure row in rfcTable)
        //    {
        //        DataRow dr = dtTable.NewRow();
        //        for (int item = 0; item < rfcTable.ElementCount; item++)
        //        {
        //            RfcElementMetadata metadata = rfcTable.GetElementMetadata(item);
        //            dr[item] = row.GetString(metadata.Name);
        //        }
        //        dtTable.Rows.Add(dr);
        //    }
        //    return dtTable;
        //}


       /* public DataTable ConvertToDotNetTable(IRfcTable rfcTable)
        {

            DataTable dtTable = new DataTable();

            AdminEstatusFactura oAdminEstatusFactura = new AdminEstatusFactura();
            //List<EstatusFactura> lstEstadosFacturas = oAdminEstatusFactura.cargarEstatusFactura();
        

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
                /*if ((string.IsNullOrEmpty(dr["TXTST"].ToString().Trim())) && (!string.IsNullOrEmpty(dr["STATU"].ToString().Trim())))
                {
                    var status = ((EstatusFactura)lstEstadosFacturas.SingleOrDefault(p => p.Status.ToString() == dr["STATU"].ToString()));
                    dr["TXTST"] = status != null ? status.TxtST : dr["STATU"].ToString();
                }
                dtTable.Rows.Add(dr);
            }
            return dtTable;
        }*/


        public DataTable ConvertToDotNetTable(IRfcTable rfcTable)
        {

            DataTable dtTable = new DataTable();
            //create table

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
                    if (metadata.DataType == RfcDataType.BCD && metadata.Name == "ABC")
                    {
                        dr[item] = row.GetInt(metadata.Name);
                    }
                    else
                    {
                        dr[item] = row.GetString(metadata.Name);
                    }
                }
                dtTable.Rows.Add(dr);
            }
            return dtTable;
        }


        public IRfcStructure getHeader(RfcStructureMetadata metadata_header, Comprobante comprobante, string versionCFDI )
        {
            IRfcStructure structutre_header = metadata_header.CreateStructure();
            try
            {
                
                string comprobanteFecha = string.Empty;
                string comprobanteFolio = string.Empty;
                string comprobanteConceptoDescripcion = string.Empty;

                switch (versionCFDI)
                {
                    case "3.2":
                        comprobanteFecha = comprobante.fecha;
                        comprobanteFolio = comprobante.folio;
                        comprobanteConceptoDescripcion = comprobante.Conceptos[0].descripcion;
                    break;
                    case "3.3":
                        comprobanteFecha = comprobante.Fecha;
                        comprobanteFolio = comprobante.Folio;
                        comprobanteConceptoDescripcion = comprobante.Conceptos[0].Descripcion;
                    break;

                }

                
                string fecha_registro = DateTime.Now.ToString("yyyyMMdd",
                                     CultureInfo.InvariantCulture);

                //formateo la dtFechaActual como la pide el RFC  
                string fecha = formatFecha(comprobanteFecha, "yyyy-MM-ddTHH:mm:ss", "yyyyMMdd");

                //tomo solamente el año del año actual
                string annio = formatFecha(fecha_registro, "yyyyMMdd", "yyyy");
                structutre_header.SetValue("GJAHR", annio);
                structutre_header.SetValue("XBLNR", comprobanteFolio);
                //STATU IGUAL A 001 = En Proceso SAT
                structutre_header.SetValue("STATU", "001");
                structutre_header.SetValue("BUDAT", fecha);
                structutre_header.SetValue("WAERS", comprobante.moneda);
                // Se envia vacio ya que despues sera marcado con una X
                structutre_header.SetValue("VESAT", "");
                //si hay mas de una descrpcion como se hace?
                structutre_header.SetValue("BKTXT", comprobanteConceptoDescripcion);
               
            }
            catch (Exception ex)
            {
                throw new Exception("Error en getHeader " + ex);
                
            }

            return structutre_header;
        }

        public IRfcStructure getItem(RfcStructureMetadata metadata_item, Comprobante comprobante, string versionCFDI, string concepto,string departamento,string indicadorIva,string idProveedor)
        {
            IRfcStructure structutre_item = metadata_item.CreateStructure();
            try
            {


                string comprobanteFecha = string.Empty;
                string comprobanteFolio = string.Empty;
                string comprobanteConceptoDescripcion = string.Empty;
                string comprobanteTotal = string.Empty;
                string comprobanteEmisorRfc = string.Empty;
                string comprobanteReceptorRfc = string.Empty;

                switch (versionCFDI)
                {
                    case "3.2":
                        comprobanteFecha = comprobante.fecha;
                        comprobanteFolio = comprobante.folio;
                        comprobanteTotal = comprobante.total;
                        comprobanteEmisorRfc = comprobante.Emisor.rfc;
                        comprobanteReceptorRfc = comprobante.Emisor.rfc;
                        break;
                    case "3.3":
                        comprobanteFecha = comprobante.Fecha;
                        comprobanteFolio = comprobante.Folio;
                        comprobanteTotal = comprobante.Total;
                        comprobanteEmisorRfc = comprobante.Emisor.Rfc;
                        comprobanteReceptorRfc = comprobante.Emisor.Rfc;
                        break;
                }

               
                string fecha_registro = DateTime.Now.ToString("yyyyMMdd",
                                     CultureInfo.InvariantCulture);

                //formateo la dtFechaActual como la pide el RFC
                string fecha = formatFecha(comprobanteFecha, "yyyy-MM-ddTHH:mm:ss", "yyyyMMdd");
                //tomo solamente el año del año actual
                string annio = formatFecha(fecha_registro, "yyyyMMdd", "yyyy");
                //dtFechaActual de registro               
              

                string total = comprobanteTotal.Replace(",",".");

                string fechaTimbrado = formatFecha(comprobante.Complemento.TimbreFiscalDigital.FechaTimbrado, "yyyy-MM-ddTHH:mm:ss", "yyyyMMdd");
                structutre_item.SetValue("GJAHR", annio);
                //Este el codigo del proveedor que va tener el proveedor al logearse en el portal
                structutre_item.SetValue("XBLNR", comprobanteFolio);
                /*Lo quemo hasta que se tome desde la cesion*/
                structutre_item.SetValue("LIFNR", idProveedor);

                structutre_item.SetValue("MWSKZ", indicadorIva);//CONVERSION IVA

                structutre_item.SetValue("WRBTR", total);
                structutre_item.SetValue("HKONT", concepto);
                structutre_item.SetValue("KOSTL", departamento);
                structutre_item.SetValue("FUUID", comprobante.Complemento.TimbreFiscalDigital.UUID);
                structutre_item.SetValue("RFCEM", comprobanteEmisorRfc);
                structutre_item.SetValue("RFCRE", comprobanteReceptorRfc);
                structutre_item.SetValue("DATTI", fechaTimbrado);
                structutre_item.SetValue("DATRE", fecha_registro); ;
            }
            catch (Exception ex)
            {
                throw new Exception("Al obtener valores del archivo XML " + ex);

            }

            return structutre_item;
        }
        public string formatFecha(string fecha, string formatoFecha,string formatoAConvertir)
         {
             string fechaFormateada = "";
             if (fecha != "") 
             {
                fechaFormateada = DateTime.ParseExact(fecha, formatoFecha, CultureInfo.InvariantCulture)
                  .ToString(formatoAConvertir);
             }
             return fechaFormateada;

         }

        public static bool ConvertRespuestaBD(char input)
        {
            int result = -1;

            int tempInt = 0;
            bool val = false;
            if (int.TryParse(input.ToString(), out tempInt) == true)
            {

                result = tempInt;
                if (result == 1)
                {
                    val =  true;

                }
                else
                {
                    val = false;
                }
            }

            return val;
        }

        public static int CharToInt1(char input)
        {
            int result = -1;

            if (input >= 48 && input <= 57)
            {
                result = input - '0';
            }

            return result;
        }

        public bool existeIdProveedor(string idProveedor, string destinationName)
        {
            bool existe = false;

            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(destinationName);
            }
            RfcRepository rfcRepository = rfcDestination.Repository;
            IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_CONSULTA_PROVEEDOR");
            rfcFunction.SetValue("LIFNR", idProveedor);
            rfcFunction.Invoke(rfcDestination);
            char estatus = rfcFunction.GetChar("ESTATUS");
         

            existe = ConvertRespuestaBD(estatus);
            return existe;
        }
        /*public bool existeIdProveedor(string idProveedor)
        {
            bool existe = false;
            Conexion conexion = new Conexion();
            string strCommand = "select codigoProveedor from dbo.Proveedor  where codigoProveedor= '" + idProveedor + "' ";

          

            DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);

            if ((ds != null) && (ds.Tables[0].Rows[0]["codigoProveedor"].ToString() != ""))
            {
                existe = true;

            }

            return existe;
        }*/


        public Proveedor existeProveedor(string idProveedor, string destinationName)
        {
            Proveedor proveedor = null;
            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(destinationName);
            }

            RfcRepository rfcRepository = rfcDestination.Repository;
            IRfcFunction rfcFunction = rfcRepository.CreateFunction("ZIFAP_CONSULTA_PROVEEDOR");
            rfcFunction.SetValue("LIFNR", idProveedor);          
            rfcFunction.Invoke(rfcDestination);
            char result = rfcFunction.GetChar("ESTATUS");
            string mensaje = rfcFunction.GetString("MENSAJE");
            string razonSocial = rfcFunction.GetString("RAZON_SOCIAL");

            bool respuesta = ConvertRespuestaBD(result);
            if (respuesta == true)
            {
                proveedor = new Proveedor(idProveedor, razonSocial, respuesta);
            }
            else
            {
                proveedor = new Proveedor("", "", respuesta);
            }
                             

            return proveedor;
        }
        /*public Proveedor existeProveedor(string idProveedor)
        {
            bool existe = false;
            Proveedor proveedor = null;
            Conexion conexion = new Conexion();
            string strCommand = "select codigoProveedor,razonSocial from dbo.Proveedor  where codigoProveedor= '" + idProveedor + "' ";
            


            DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);

            if ((ds != null) && (ds.Tables[0].Rows[0]["codigoProveedor"].ToString() != ""))
            {

                proveedor = new Proveedor(ds.Tables[0].Rows[0]["codigoProveedor"].ToString(), ds.Tables[0].Rows[0]["razonSocial"].ToString(),true);
            }
            else
            {
                proveedor = new Proveedor("","",false);
            }
            
            return proveedor;
        }*/

    }
}