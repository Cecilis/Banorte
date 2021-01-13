using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Banorte.ConsultaCFDIService;
using Banorte.Models;
using Banorte.Models.xml;
using Banorte.Utilities;
using Banorte.FTP;
using Banorte.SAPConnector;
using SAP.Middleware.Connector;
using System.Text;
using System.Web.Security;
using log4net;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml.Schema;


namespace Banorte
{
    public partial class CargarFactura : System.Web.UI.Page
    {
        SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();

        private string strResumenResultados;
        private bool bHanOcurridoErrores = false;
        private string idProveedor;
        private string strXMLFileRemoteName = string.Empty;
        private string strPDFFileRemoteName = string.Empty;
        private string strUploadedXMLFileName = string.Empty;
        private string strUploadedPDFFileName = string.Empty;
        private bool errorArchivoXML;
        private bool errorArchivoPDF;

        string versionXSD = string.Empty;

        private string[] XSDVersiones = { "3.2", "3.3" };

        private DateTime dtFechaActual = DateTime.Now;
        public void setIdProveedor(string idProveedor)
        {
            this.idProveedor = idProveedor;
        }
        public string getIdProveedor()
        {
            return this.idProveedor = ((bool)ViewState["esProveedor"]) ? ViewState["codigoProveedor"].ToString() : txtCodigoProveedor.Text.Trim();
        }


        public void ocultarResultado()
        {

            lblResultado.Visible = false;
            lblArchXML.Visible = false;
            imagenArchXMLOk.Visible = false;
            imagenArchXMLNotOk.Visible = false;
            lblArchPDF.Visible = false;
            imagenArchPDFOk.Visible = false;
            imagenArchPDFNotOk.Visible = false;
            imagenGuardarFacturaPDFOk.Visible = false;
            imagenGuardarFacturaPDFNotOk.Visible = false;
            //lblCargarFacturaResultados.Visible = false;
            lblErrorArchXML.Visible = false;
            lblErrorArchPDF.Visible = false;
            lblGuardarFactura.Visible = false;
            strResumenResultados = "";
        }

        public void mostrarResultadoArchXML(bool valido)
        {
            lblResultado.Visible = true;
            lblArchXML.Visible = true;
            imagenArchXMLOk.Visible = valido;
            imagenArchXMLNotOk.Visible = !valido;
            lblErrorArchXML.Visible = !valido;
        }

        public void mostrarResultadoArchXML(bool valido, string mensaje)
        {
            lblResultado.Visible = true;
            lblArchXML.Visible = true;
            imagenArchXMLOk.Visible = valido;
            imagenArchXMLNotOk.Visible = !valido;
            lblErrorArchXML.Visible = !valido;
            lblErrorArchXML.Text = mensaje;
        }


        public void mostrarResultadoGuardarFactura(bool valido)
        {

            lblCargarFacturaResultados.Visible = true;
            lblGuardarFactura.Visible = true;
            imagenGuardarFacturaPDFOk.Visible = valido;
            imagenGuardarFacturaPDFNotOk.Visible = !valido;
            lblErrorGuardarFactura.Visible = !valido;

        }

        public void mostrarResultadoGuardarFactura(bool valido, string mensaje)
        {

            lblCargarFacturaResultados.Visible = true;
            lblGuardarFactura.Visible = true;
            imagenGuardarFacturaPDFOk.Visible = valido;
            imagenGuardarFacturaPDFNotOk.Visible = !valido;
            lblErrorGuardarFactura.Visible = !valido;
            lblErrorGuardarFactura.Text = mensaje;

        }

        public void mostrarResultadoArchPDF(bool valido)
        {

            lblArchPDF.Visible = true;
            imagenArchPDFOk.Visible = valido;
            imagenArchPDFNotOk.Visible = !valido;
            lblErrorArchPDF.Visible = !valido;

        }


        public void mostrarResultadoArchPDF(bool valido, string mensaje)
        {
            lblArchPDF.Visible = true;
            imagenArchPDFOk.Visible = valido;
            imagenArchPDFNotOk.Visible = !valido;
            lblErrorArchPDF.Visible = !valido;
            lblErrorArchPDF.Text = mensaje;
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Cuentas por Cobrar - Cargar";

            dtFechaActual = DateTime.Now;
            string fecha_formateada = String.Format("{0:dd.MM.yyyy}", dtFechaActual);

            if (!Page.IsPostBack)
            {
                limpiarFormulario(false);
                loadDepartamento();
                loadConcepto();
                cmbConcepto.SelectedValue = ViewState["Concepto"] == null ? "0" : ViewState["Concepto"].ToString();
                cmbDepartamento.SelectedValue = ViewState["Departamento"] == null ? "0" : ViewState["Departamento"].ToString();

                //Recupera de la data del usuario logeado
                HttpCookie decryptedCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(decryptedCookie.Value);
                AdminUsuario admUsuario = new AdminUsuario();
                Usuario usuario = admUsuario.deserialize(ticket.UserData);

                ViewState["codigoProveedor"] = usuario.CodigoProveedor;
                ViewState["esProveedor"] = usuario.EsProveedor;
                pnlCodigoProveedor.Visible = !usuario.EsProveedor;
                ocultarResultado();

            }
            else
            {
                ViewState["Concepto"] = "0";
                ViewState["Departamento"] = "0";
            }

            if (Session["fupArchivoXML"] == null && fupArchivoXML.HasFile)
            {
                Session["fupArchivoXML"] = fupArchivoXML;
                txtArchivoXML.Text = Path.GetFileName(fupArchivoXML.PostedFile.FileName);
            }
            else if (Session["fupArchivoXML"] != null && (!fupArchivoXML.HasFile))
            {
                fupArchivoXML = (FileUpload)Session["fupArchivoXML"];
                txtArchivoXML.Text = Path.GetFileName(fupArchivoXML.PostedFile.FileName); ;
            }
            else if (fupArchivoXML.HasFile)
            {
                Session["fupArchivoXML"] = fupArchivoXML;
                txtArchivoXML.Text = Path.GetFileName(fupArchivoXML.PostedFile.FileName); ;
            }


            if (Session["fupArchivoPDF"] == null && fupArchivoPDF.HasFile)
            {
                Session["fupArchivoPDF"] = fupArchivoPDF;
                txtArchivoPDF.Text = Path.GetFileName(fupArchivoPDF.PostedFile.FileName);
            }
            else if (Session["fupArchivoPDF"] != null && (!fupArchivoPDF.HasFile))
            {
                fupArchivoPDF = (FileUpload)Session["fupArchivoPDF"];
                txtArchivoPDF.Text = Path.GetFileName(fupArchivoPDF.PostedFile.FileName);
            }
            else if (fupArchivoPDF.HasFile)
            {
                Session["fupArchivoPDF"] = fupArchivoPDF;
                txtArchivoPDF.Text = Path.GetFileName(fupArchivoPDF.PostedFile.FileName);
            }


        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string strCodigoConcepto = cmbConcepto.SelectedValue.Trim();
            string strCodigoDescripcion = cmbConcepto.SelectedValue.Trim();
            string strNumeroPedido = txtNumeroPedido.Text.Trim();
            string strPosicion = txtPosicion.Text.Trim();

            string strUniqueFileName = string.Empty;
            string centroCosto = cmbDepartamento.SelectedValue.Trim();
            string strCodigoProveedor = txtCodigoProveedor.Text.Trim();

            Comprobante comprobante = null;
            AdminImpuesto adminImpuesto = new AdminImpuesto();
            IndicadorImpuesto indicadorImpuesto = null;

            bool esProveedor = (bool)ViewState["esProveedor"];
            string indicador = string.Empty;

            Stream strmArchivoXML = null;
            Stream strmArchivoPDF = null;

            strResumenResultados = string.Empty;

            bHanOcurridoErrores = false;
            lblCargarFacturaResultados.Text = "";

            try
            {


                ocultarResultado();

                if (String.IsNullOrEmpty(strCodigoConcepto))
                {
                    bHanOcurridoErrores = true;
                    throw new ArgumentNullException("Concepto: Formato de código no válido");
                }

                if (String.IsNullOrEmpty(strCodigoDescripcion))
                {
                    bHanOcurridoErrores = true;
                    throw new ArgumentNullException("Descripción: Formato de código no válido");
                }

                if ((!string.IsNullOrEmpty(strNumeroPedido)) && (!Regex.IsMatch(strNumeroPedido, @"^\d+$")))
                {
                    bHanOcurridoErrores = true;
                    throw new ArgumentNullException("N° Pedido: Formato no válido");
                }

                if ((!string.IsNullOrEmpty(strPosicion)) && (!Regex.IsMatch(strPosicion, @"^\d+$")))
                {
                    bHanOcurridoErrores = true;
                    throw new ArgumentNullException("Posición: Formato no válido");
                }

                if (!esProveedor)
                {
                    if (String.IsNullOrEmpty(strCodigoProveedor))
                    {
                        bHanOcurridoErrores = true;
                        throw new ArgumentNullException("Codigo Proveedor: Formato de código no válido");
                    }
                }

                if (fupArchivoXML.HasFile)
                {
                    strmArchivoXML = fupArchivoXML.PostedFile.InputStream;
                }
                else
                {
                    bHanOcurridoErrores = true;
                    throw new ArgumentNullException("No se ha seleccionado un archivo XML");
                }

                if (fupArchivoPDF.HasFile)
                {
                    strmArchivoPDF = fupArchivoPDF.PostedFile.InputStream;
                }
                else
                {
                    bHanOcurridoErrores = true;
                    throw new ArgumentNullException("No se ha seleccionado un archivo PDF");
                }



                if (validaProveedor())
                {
                    //strResumenResultados += "Archivo XML: " + "<br/>";

                    if (!bHanOcurridoErrores && fupArchivoXML.HasFile)
                    {
                        /*Valido la estructura del XML*/
                        CustomXmlSchemaValidator validator = ValidateXml(strmArchivoXML);

                        if (validator.IsValidXML)
                        {
                            comprobante = deserializarComprobante(strmArchivoXML);

                            /*Se obtiene El indicador de Impuesto*/
                            indicador = getIndicador(centroCosto, comprobante);

                            if (versionXSD == "3.2")
                            {
                                strXMLFileRemoteName = comprobante.folio + ".xml";
                            }
                            else
                            {
                                strXMLFileRemoteName = comprobante.Folio + ".xml";
                            }

                            strmArchivoXML.Seek(0, SeekOrigin.Begin);
                            enviarAlSFTP(strmArchivoXML, strXMLFileRemoteName);

                            //strResumenResultados += "Archivo recibido exitosamente." + "<br/>";
                            mostrarResultadoArchXML(true);

                        }//FIN if (validator.IsValidXML)
                        else
                        {
                            bHanOcurridoErrores = true;
                            //strResumenResultados += "El Archivo XML tiene errores en su estructura" + "<br/>";
                            mostrarResultadoArchXML(false, "Tiene errores en su estructura, Por favor verifique la versión");
                        }
                    }
                    else
                    {
                        bHanOcurridoErrores = true;
                        //Functions.SetResultLabelContent(lblCargarFacturaResultados, "error", strResumenResultados);
                        return;
                    }


                    // valido que todos los datos sean validos antes de hacer upload del PDF
                    if (!bHanOcurridoErrores && fupArchivoPDF.HasFile)
                    {
                        //Carga y validación de archivo PDF 
                        //strResumenResultados += "Archivo PDF: " + "<br/>";

                        strPDFFileRemoteName = versionXSD == "3.2" ? comprobante.folio + ".pdf" : comprobante.Folio + ".pdf";

                        enviarAlSFTP(strmArchivoPDF, strPDFFileRemoteName);

                        //strResumenResultados += "Recibido exitosamente." + "<br/>"; 
                        mostrarResultadoArchPDF(true);

                    }

                    /*******************aqui guardo en la BD*********************************************/
                    bool resp = false;
                    if (!bHanOcurridoErrores)
                    {
                        resp = guardarFactura(comprobante, strCodigoDescripcion, centroCosto, indicador);
                        //resp = true;
                        if (resp == true)
                        {
                            mostrarResultadoGuardarFactura(true);
                            Acuse acuse = consultarSAP(comprobante);
                            if (acuse != null)
                            {
                                //strResumenResultados += "Consulta en SAT: " + "<br/>" + acuse.CodigoEstatus + " Estatus: " + acuse.Estado + "<br/>";

                                //modificar el estatus de la factura
                                string estatusConsulta = acuse.CodigoEstatus.Split('-')[0].ToString().Trim() == "S" ? "002" : "006";
                                resp = actualizarFactura(estatusConsulta, comprobante.Complemento.TimbreFiscalDigital.UUID);

                                mostrarModal();
                                limpiarFormulario(bHanOcurridoErrores);
                            }

                        }
                        else
                        {
                            bHanOcurridoErrores = true;
                            mostrarResultadoGuardarFactura(false, "Factura cargada previamente. Favor de validar número de folio");
                        }
                        mostrarModal();
                    }

                }//End validaProveedor
                else
                {
                    strResumenResultados += "Resultado:" + "<br/>";
                    strResumenResultados += "El Código proveedor no existe";
                    bHanOcurridoErrores = true;
                    throw new System.ArgumentException("El Código proveedor no existe", "CodigoProveedor");
                }

                if (bHanOcurridoErrores)
                {
                    Functions.SetResultLabelContent(lblCargarFacturaResultados, "error", strResumenResultados);
                    return;
                }

            }
            catch (CustomException ce)
            {
                ExceptionsManager.LogRegister(ExceptionsManager.Message(ce, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                //Functions.SetResultLabelContent(lblCargarFacturaResultados, "error", strResumenResultados);
                if (errorArchivoXML)
                    mostrarResultadoArchXML(false, "Error en red. Favor intentar en unos minutos");
                else if (errorArchivoPDF)
                    mostrarResultadoArchPDF(false, "Error en red. Favor intentar en unos minutos");
                else
                {
                    bHanOcurridoErrores = true;
                    mostrarResultadoGuardarFactura(false, "Error en incluirFactura");
                }
            }
            catch (Exception ex)
            {
                if (ex is SoapException)
                {
                    ExceptionsManager.LogRegister(ExceptionsManager.Message(ex, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                    //lblCargarFacturaResultados.Text = strResumenResultados;
                }
                else if ((ex is XmlException) || (ex is XmlSchemaException))
                {
                    ExceptionsManager.LogRegister(ExceptionsManager.Message(ex, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                    //lblCargarFacturaResultados.Text = strResumenResultados;
                    mostrarResultadoArchXML(false, "Tiene errores en su estructura, Por favor verifique la versión");
                }
                else
                {
                    ExceptionsManager.LogRegister(ExceptionsManager.Message(ex, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                    if (bHanOcurridoErrores)
                    {
                        Functions.SetResultLabelContent(lblCargarFacturaResultados, "error", strResumenResultados);
                    }
                    else
                    {
                        //Functions.SetResultLabelContent(lblCargarFacturaResultados, "error", "Ha ocurrido un error al cargar la factura, por favor intente nuevamente");
                    }
                }
            }
        }



        private void loadConcepto()
        {
            AdminConcepto adminConcepto = new AdminConcepto();
            this.cmbConcepto.ClearSelection();
            this.cmbConcepto.DataSource = adminConcepto.getConcepto();
            this.cmbConcepto.DataTextField = "descripcion";
            this.cmbConcepto.DataValueField = "cuentaMayor";
            this.cmbConcepto.DataBind();
            this.cmbConcepto.Items.Insert(0, new ListItem("Seleccione", "0"));
            this.cmbConcepto.SelectedValue = "0";
        }

        private void loadDepartamento()
        {
            AdminDepartamento adminDepartamento = new AdminDepartamento();
            this.cmbDepartamento.ClearSelection();
            this.cmbDepartamento.DataSource = adminDepartamento.getDepartamento();
            this.cmbDepartamento.DataTextField = "descripcion";
            this.cmbDepartamento.DataValueField = "centroCosto";
            this.cmbDepartamento.DataBind();
            this.cmbDepartamento.Items.Insert(0, new ListItem("Seleccione", "0"));
            this.cmbDepartamento.SelectedValue = "0";
        }

        protected void cmbConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbConcepto.SelectedValue != "0")
            {
                Session["Concepto_"] = cmbConcepto.SelectedValue;
                ViewState["Concepto"] = cmbConcepto.SelectedValue;
            }
        }

        protected void cmbDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDepartamento.SelectedValue != "0")
            {
                Session["Departamento"] = cmbDepartamento.SelectedValue;
                ViewState["Departamento"] = cmbDepartamento.SelectedValue;
            }
        }

        private void enviarAlFTP(string nombreArchivoLocal, string nombreArchivoRemoto)
        {
            try
            {
                ftp ftpClient = new ftp(Functions.GetConfigurationValue("FTPIP"), Functions.GetConfigurationValue("FTPUser"), Functions.GetConfigurationValue("FTPUserPassword"));
                ftpClient.upload(Functions.GetConfigurationValue("FTPRemoteLocation") + nombreArchivoRemoto, nombreArchivoLocal);
            }
            catch (Exception ex)
            {
                bHanOcurridoErrores = true;
                //strResumenResultados += "Error en transferencia de archivo";
                throw new CustomException("Error en transferencia via FTP");
            }
        }


        private void enviarAlSFTP(Stream strmArchivoLocal, string nombreArchivoRemoto)
        {

            try
            {
                string destination = Functions.GetConfigurationValue("FTPRemoteLocation").ToString();
                string host = Functions.GetConfigurationValue("FTPIP").ToString();
                string username = Functions.GetConfigurationValue("FTPUser");
                string password = Functions.GetConfigurationValue("FTPUserPassword");
                int port = Int32.Parse(Functions.GetConfigurationValue("FTPPort"));  //Port 22 is defaulted for SFTP upload


                sftp.UploadSFTPFile(host, username, password, strmArchivoLocal, nombreArchivoRemoto, destination, port);

            }
            catch (Exception ex)
            {
                if ((nombreArchivoRemoto.Contains(".xml") == true) || (nombreArchivoRemoto.Contains(".XML") == true))
                {
                    errorArchivoXML = true;
                }
                else if ((nombreArchivoRemoto.Contains(".pdf") == true) || (nombreArchivoRemoto.Contains(".PDF") == true))
                {
                    errorArchivoPDF = true;
                }

                bHanOcurridoErrores = true;
                //strResumenResultados += "Error en transferencia de archivo";
                throw new CustomException("Error en transferencia via SFTP");
            }
        }


        public CustomXmlSchemaValidator ValidateXml(Stream strmArchivoXML)
        {

            CustomXmlSchemaValidator validator = new CustomXmlSchemaValidator();
            List<string> listaErroresArchivoXML = new List<string>();

            try
            {
                strmArchivoXML.Seek(0, SeekOrigin.Begin);
                XDocument xdocArchivoXML = XDocument.Load(XmlReader.Create(strmArchivoXML));

                //Obtiene el número de  versión del CFDI para identificar el XSD a utilizar para validar la estructura del XML
                //En la versión 3.2 del CFDI el atributo en el xml es "version" y en la 3.3 es "Version" 
                var attVersion = xdocArchivoXML.Root.Attribute("version") != null ? xdocArchivoXML.Root.Attribute("version") : xdocArchivoXML.Root.Attribute("Version");

                if (attVersion != null)
                {
                    versionXSD = attVersion.Value.ToString();
                }
                else
                    throw new ArgumentNullException("Version CFDI no soportada");

                //Determina si la versión del XSD es una de las válidas
                validator.IsValidXSD = XSDVersiones.Contains(versionXSD) ? true : false;

                //Si la versión del XSD  es válida
                if (validator.IsValidXSD)
                {
                    //Carga namespace de los xsd a utilizar en la validación del xml la versión                    
                    XmlSchemaSet oXmlSchemaSet = new XmlSchemaSet();

                    string nsSchema = @"http://www.sat.gob.mx/cfd/" + versionXSD.Replace(".", "").ToArray()[0].ToString();
                    string schemeRutaLocal = Server.MapPath("~/Schemes/v" + versionXSD.Replace(".", "") + "/");
                    string nombreArchivoXSD = schemeRutaLocal + "cfdv" + versionXSD.Replace(".", "") + ".xsd";

                    oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                    switch (versionXSD.Trim())
                    {
                        case "3.2":
                            nsSchema = @"http://www.sat.gob.mx/TimbreFiscalDigital";
                            nombreArchivoXSD = schemeRutaLocal + "TimbreFiscalDigital.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ecc";
                            nombreArchivoXSD = schemeRutaLocal + "ecc.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/donat";
                            nombreArchivoXSD = schemeRutaLocal + "donat11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/divisas";
                            nombreArchivoXSD = schemeRutaLocal + "Divisas.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/implocal";
                            nombreArchivoXSD = schemeRutaLocal + "implocal.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/leyendasFiscales";
                            nombreArchivoXSD = schemeRutaLocal + "leyendasFisc.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/pfic";
                            nombreArchivoXSD = schemeRutaLocal + "pfic.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/TuristaPasajeroExtranjero";
                            nombreArchivoXSD = schemeRutaLocal + "TuristaPasajeroExtranjero.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/spei";
                            nombreArchivoXSD = schemeRutaLocal + "spei.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/detallista";
                            nombreArchivoXSD = schemeRutaLocal + "detallista.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/registrofiscal";
                            nombreArchivoXSD = schemeRutaLocal + "cfdiregistrofiscal.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/nomina";
                            nombreArchivoXSD = schemeRutaLocal + "nomina11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/pagoenespecie";
                            nombreArchivoXSD = schemeRutaLocal + "pagoenespecie.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/valesdedespensa";
                            nombreArchivoXSD = schemeRutaLocal + "valesdedespensa.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/consumodecombustibles";
                            nombreArchivoXSD = schemeRutaLocal + "consumodecombustibles.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/aerolineas";
                            nombreArchivoXSD = schemeRutaLocal + "aerolineas.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/notariospublicos";
                            nombreArchivoXSD = schemeRutaLocal + "notariospublicos.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/vehiculousado";
                            nombreArchivoXSD = schemeRutaLocal + "vehiculousado.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/servicioparcialconstruccion";
                            nombreArchivoXSD = schemeRutaLocal + "servicioparcialconstruccion.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/renovacionysustitucionvehiculos";
                            nombreArchivoXSD = schemeRutaLocal + "renovacionysustitucionvehiculos.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/certificadodestruccion";
                            nombreArchivoXSD = schemeRutaLocal + "certificadodedestruccion.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/arteantiguedades";
                            nombreArchivoXSD = schemeRutaLocal + "obrasarteantiguedades.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/iedu";
                            nombreArchivoXSD = schemeRutaLocal + "iedu.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ventavehiculos";
                            nombreArchivoXSD = schemeRutaLocal + "ventavehiculos11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/terceros";
                            nombreArchivoXSD = schemeRutaLocal + "terceros11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/acreditamiento";
                            nombreArchivoXSD = schemeRutaLocal + "AcreditamientoIEPS10.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/EstadoDeCuentaCombustible";
                            nombreArchivoXSD = schemeRutaLocal + "ecc11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ine";
                            nombreArchivoXSD = schemeRutaLocal + "ine10.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ComercioExterior";
                            nombreArchivoXSD = schemeRutaLocal + "ComercioExterior10.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/nomina12";
                            nombreArchivoXSD = schemeRutaLocal + "nomina12.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/Pagos";
                            nombreArchivoXSD = schemeRutaLocal + "Pagos10.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            break;
                        case "3.3":

                            nsSchema = @"http://www.sat.gob.mx/TimbreFiscalDigital";
                            nombreArchivoXSD = schemeRutaLocal + "TimbreFiscalDigitalv11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/sitio_internet/cfd/catalogos";
                            nombreArchivoXSD = schemeRutaLocal + "catCFDI.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/sitio_internet/cfd/tipoDatos/tdCFDI";
                            nombreArchivoXSD = schemeRutaLocal + "tdCFDI.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/EstadoDeCuentaCombustible";
                            nombreArchivoXSD = schemeRutaLocal + "ecc11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/donat";
                            nombreArchivoXSD = schemeRutaLocal + "donat11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/divisas";
                            nombreArchivoXSD = schemeRutaLocal + "Divisas.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/implocal";
                            nombreArchivoXSD = schemeRutaLocal + "implocal.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/leyendasFiscales";
                            nombreArchivoXSD = schemeRutaLocal + "leyendasFisc.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/pfic";
                            nombreArchivoXSD = schemeRutaLocal + "pfic.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/TuristaPasajeroExtranjero";
                            nombreArchivoXSD = schemeRutaLocal + "TuristaPasajeroExtranjero.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/spei";
                            nombreArchivoXSD = schemeRutaLocal + "spei.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/detallista";
                            nombreArchivoXSD = schemeRutaLocal + "detallista.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/registrofiscal";
                            nombreArchivoXSD = schemeRutaLocal + "cfdiregistrofiscal.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/nomina12";
                            nombreArchivoXSD = schemeRutaLocal + "nomina12.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/pagoenespecie";
                            nombreArchivoXSD = schemeRutaLocal + "pagoenespecie.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/valesdedespensa";
                            nombreArchivoXSD = schemeRutaLocal + "valesdedespensa.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/consumodecombustibles";
                            nombreArchivoXSD = schemeRutaLocal + "consumodecombustibles.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/aerolineas";
                            nombreArchivoXSD = schemeRutaLocal + "aerolineas.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/notariospublicos";
                            nombreArchivoXSD = schemeRutaLocal + "notariospublicos.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/vehiculousado";
                            nombreArchivoXSD = schemeRutaLocal + "vehiculousado.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/servicioparcialconstruccion";
                            nombreArchivoXSD = schemeRutaLocal + "servicioparcialconstruccion.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/renovacionysustitucionvehiculos";
                            nombreArchivoXSD = schemeRutaLocal + "renovacionysustitucionvehiculos.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/certificadodestruccion";
                            nombreArchivoXSD = schemeRutaLocal + "certificadodedestruccion.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/arteantiguedades";
                            nombreArchivoXSD = schemeRutaLocal + "obrasarteantiguedades.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ine";
                            nombreArchivoXSD = schemeRutaLocal + "ine11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ComercioExterior11";
                            nombreArchivoXSD = schemeRutaLocal + "ComercioExterior11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/Pagos";
                            nombreArchivoXSD = schemeRutaLocal + "Pagos10.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/iedu";
                            nombreArchivoXSD = schemeRutaLocal + "iedu.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/ventavehiculos";
                            nombreArchivoXSD = schemeRutaLocal + "ventavehiculos11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);

                            nsSchema = @"http://www.sat.gob.mx/terceros";
                            nombreArchivoXSD = schemeRutaLocal + "terceros11.xsd";
                            oXmlSchemaSet.Add(nsSchema, nombreArchivoXSD);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Versión del schema no soportado");
                            break;
                    }

                    xdocArchivoXML.Validate(oXmlSchemaSet, (o, er) =>
                    {
                        listaErroresArchivoXML.Add(er.Message.ToString());
                    });

                }
                else
                {
                    bHanOcurridoErrores = true;
                    validator.IsValidXML = false;
                    throw new ArgumentOutOfRangeException("Versión de archivo XML no soportada");
                }

            }
            catch (Exception ex)
            {
                bHanOcurridoErrores = true;
                validator.IsValidXML = false;
                throw;
            }
            finally
            {
                validator.ListaErroresArchivoXML = listaErroresArchivoXML;
                if (validator.ListaErroresArchivoXML.Count > 0)
                {
                    bHanOcurridoErrores = true;
                }
                else
                {
                    validator.IsValidXML = true;
                }
            }

            return validator;
        }

        public bool guardarFacturaSapConnector(Comprobante comprobante, string strCodigoDescripcion, string centroCosto, string indicadorImpuesto)
        {
            bool resp = false;
            try
            {
                resp = sapConnectorInterface.incluirFactura(comprobante, versionXSD, strCodigoDescripcion, centroCosto, indicadorImpuesto, this.getIdProveedor(), Constants.destinationName);
            }
            catch (Exception exc)
            {
                resp = false;
                //elimino el archivo XML del FTP
                eliminarArchivoSFTP(strXMLFileRemoteName);
                //elimino el archivo PDF del FTP
                eliminarArchivoSFTP(strPDFFileRemoteName);
                //deleteUploadFile(strUploadedXMLFileName);
                //deleteUploadFile(strUploadedPDFFileName);
                strResumenResultados += "Error al incluir factura en la BD <br/>";
                bHanOcurridoErrores = true;
                throw new CustomException("Error al incluir factura", exc);
            }

            return resp;
        }

        public bool actualizarFactura(string strEstatus, string strUUIDFactura)
        {
            bool resp = false;
            try
            {
                resp = sapConnectorInterface.actualizarEstatusFactura(strEstatus, strUUIDFactura, Constants.destinationName);
                //resp = sapConnectorInterface.incluirFactura(comprobante, strCodigoDescripcion, centroCosto, indicadorImpuesto, this.getIdProveedor(), Constants.destinationName);
            }
            catch (Exception exc)
            {
                resp = false;
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al actualizar factura en la BD <br/>";
                throw new CustomException("Error al actualizar factura", exc);
            }

            return resp;
        }

        public void limpiarFormulario(bool bHanOcurridoErrores)
        {
            if (!bHanOcurridoErrores)
            {
                cmbConcepto.ClearSelection();
                cmbDepartamento.ClearSelection();
                txtNumeroPedido.Text = string.Empty;
                txtPosicion.Text = string.Empty;
                txtArchivoXML.Text = string.Empty;
                txtArchivoPDF.Text = string.Empty;

                Session.Remove("fupArchivoXML");
                Session.Remove("fupArchivoPDF");

                if (pnlCodigoProveedor.Visible == true)
                {
                    txtCodigoProveedor.Text = string.Empty;
                }
            }
        }

        public Acuse consultarSAP(Comprobante comprobante)
        {
            Acuse acuse = null;
            try
            {
                ConsultaCFDIServiceClient oConsultaCFDIService = new ConsultaCFDIServiceClient();
                acuse = oConsultaCFDIService.Consulta(comprobante.getExpresionImpresa(versionXSD));
                oConsultaCFDIService.Close();
            }
            catch (Exception exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al consultar en SAT: <br/>";
                throw new SoapException("Error al consultar factura en SAT",
                SoapException.ServerFaultCode, "SAT Service", exc);
            }
            return acuse;
        }

        public bool validaProveedor()
        {
            try
            {
                bool existe = false;
                if (!(bool)ViewState["esProveedor"])
                {
                    existe = sapConnectorInterface.existeIdProveedor(txtCodigoProveedor.Text, Constants.destinationName);
                }
                else
                {
                    existe = true;
                }
                return existe;

            }
            catch (Exception exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al validar código de proveedor <br/>";
                throw new CustomException("Error al validar código proveedor", exc);
            }
        }

        public bool guardarFactura(Comprobante comprobante, string strCodigoDescripcion, string centroCosto, string indicadorImpuesto)
        {
            bool resp = false; ;
            try
            {
                resp = guardarFacturaSapConnector(comprobante, strCodigoDescripcion, centroCosto, indicadorImpuesto);
                if (resp == true)
                {
                    strResumenResultados += "Factura Guardada Exitosamente";
                    ExceptionsManager.LogRegister(ExceptionsManager.Message("Factura Guardada Exitosamente", Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.INFO);
                }
                else
                {
                    //elimino el archivo XML del FTP
                    eliminarArchivoSFTP(strXMLFileRemoteName);
                    //elimino el archivo PDF del FTP
                    eliminarArchivoSFTP(strPDFFileRemoteName);
                    //deleteUploadFile(strUploadedXMLFileName);   
                    //deleteUploadFile(strUploadedPDFFileName);
                    strResumenResultados += "No se pudo Guardar la Factura";

                    ExceptionsManager.LogRegister(ExceptionsManager.Message("No se pudo Guardar la Factura", Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                }
            }
            catch (CustomException ex)
            {
                bHanOcurridoErrores = true;
                throw;
            }
            catch (Exception exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al guardar factura <br/>";
                throw new CustomException("Error al guardar factura ", exc);
            }
            return resp;
        }

        public void eliminarArchivoSFTP(string nombreArchivoRemoto)
        {
            try
            {

                string destination = Functions.GetConfigurationValue("FTPRemoteLocation").ToString();
                string host = Functions.GetConfigurationValue("FTPIP").ToString();
                string username = Functions.GetConfigurationValue("FTPUser");
                string password = Functions.GetConfigurationValue("FTPUserPassword");
                int port = Int32.Parse(Functions.GetConfigurationValue("FTPPort"));
                string rutaArchivoRemoto = destination + nombreArchivoRemoto;
                sftp.DeleteSFTPFile(host, username, password, rutaArchivoRemoto, destination, port);
            }
            catch (System.Web.HttpException exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al subir archivo XML <br/>";
                throw new CustomException("Error al subir archivo XML ", exc);
            }

        }
        public string uploadFileXML(string strLocalPath, string strXMLFileNewName)
        {
            string strUploadedFileNewName = string.Empty;
            string strUploadedFileExtension = string.Empty;
            try
            {
                strUploadedFileExtension = Path.GetExtension(fupArchivoXML.PostedFile.FileName);
                strUploadedFileNewName = strLocalPath + strXMLFileNewName + strUploadedFileExtension;
                fupArchivoXML.SaveAs(strUploadedFileNewName);

            }
            catch (System.Web.HttpException exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al subir archivo XML <br/>";
                throw new CustomException("Error al subir archivo XML ", exc);
            }
            return strUploadedFileNewName;
        }

        public string uploadFilePDF(string strLocalPath, string strPDFFileNewName)
        {
            string strUploadedPDFFileNewName = string.Empty;
            string strUploadedPDFFileExtension = string.Empty;
            try
            {
                strUploadedPDFFileExtension = Path.GetExtension(fupArchivoPDF.PostedFile.FileName);
                strUploadedPDFFileNewName = strLocalPath + strPDFFileNewName + strUploadedPDFFileExtension;
                fupArchivoPDF.SaveAs(strUploadedPDFFileNewName);
            }
            catch (System.Web.HttpException exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al subir archivo PDF <br/>";
                throw new CustomException("Error al subir archivo PDF", exc);
            }

            return strUploadedPDFFileNewName;
        }



        public XmlDocument deserializarXML(Stream strmXMLComprobanteSAT)
        {
            XmlDocument xmlComprobanteSAT = new XmlDocument();
            try
            {

                strmXMLComprobanteSAT.Seek(0L, SeekOrigin.Begin);
                xmlComprobanteSAT.Load(XmlReader.Create(strmXMLComprobanteSAT));

            }
            catch (XmlException exc)
            {
                bHanOcurridoErrores = true;
                xmlComprobanteSAT = null;
                mostrarResultadoArchXML(false, "Tiene errores en su estructura, Tiene errores en su estructura, Por favor verifique la versión");
                strResumenResultados += "Error al validar archivo XML <br/>";
                throw new CustomException("Error al deserializar archivo XML", exc);
            }

            return xmlComprobanteSAT;
        }

        public Comprobante deserializarComprobante(Stream strmXMLComprobanteSAT)
        {
            Comprobante comprobante = null;
            try
            {
                XmlDocument xmlComprobanteSAT = deserializarXML(strmXMLComprobanteSAT);
                if (xmlComprobanteSAT != null)
                {
                    ComprobanteSerializer comprobanteSerializer = new ComprobanteSerializer();
                    XmlElement nodoComprobante = xmlComprobanteSAT.DocumentElement;
                    comprobante = comprobanteSerializer.Deserialize(xmlComprobanteSAT.OuterXml.ToString());
                }
            }
            catch (XmlException exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al validar archivo XML <br/>";
                throw new CustomException("Error al deserializar archivo XML", exc);
            }

            return comprobante;

        }

        public string getTasa(Comprobante comprobante)
        {
            string tasa = "0";
            string tipoFactor = string.Empty;
            double tasaOCuota = 0;
            try
            {

                if ((comprobante != null) && (comprobante.Impuestos.Traslados[0] != null))
                {
                    switch (versionXSD.Trim())
                    {
                        case "3.2":
                            tasa = comprobante.Impuestos.Traslados[0].tasa;
                            break;
                        case "3.3":
                            //TODO: ¿Qué ocurre si es couta en vez de tasa?
                            tipoFactor = comprobante.Impuestos.Traslados[0].TipoFactor;
                            if ((tipoFactor.Trim().ToLower()) == "tasa")
                            {
                                tasaOCuota = comprobante.Impuestos.Traslados[0].TasaOCuota;
                                tasaOCuota = tasaOCuota * 100;
                                //tasa = tasaOCuota.ToString();
                                //tasa = Convert.ToSingle(tasaOCuota.ToString()); 
                                tasa = String.Format("{0:0.0000}", tasaOCuota);
                                //var montoTasa = Convert.ToSingle(tasa.Replace(".",",")) * 100;
                                //tasa = String.Format("{0:0.0000}", montoTasa).Replace(",", ".");
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Al obtener tasa: Versión de CFDI no soportado");
                    }
                }
            }
            catch (Exception exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al obtener tasa del comprobante <br/>";
                throw new CustomException("Error al obtener tasa del comprobante", exc);
            }

            return tasa;
        }

        public string leerXML(string strUploadedFileNewName)
        {
            string strXMLComprobanteSAT = "";
            try
            {
                strXMLComprobanteSAT = File.ReadAllText(strUploadedFileNewName);
            }
            catch (Exception exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al acceder al archivo XML <br/>";
                throw new CustomException("Error al acceder al archivo XML", exc);
            }

            return strXMLComprobanteSAT;
        }

        public string getIndicadorFromBD(string centroCosto, string tasa)
        {
            AdminIndicadorImpuesto adminIndicadorImpuesto = new AdminIndicadorImpuesto();
            IndicadorImpuesto indicadorImpuesto = null;
            string indicador = string.Empty;
            try
            {

                indicadorImpuesto = adminIndicadorImpuesto.getIndicador(centroCosto, tasa);
                indicador = indicadorImpuesto.getIndicador();

            }
            catch (Exception exc)
            {
                bHanOcurridoErrores = true;
                strResumenResultados += "Error al obtener el indicador desde la BD <br/>";
                throw new CustomException("Error al obtener el indicador desde la BD", exc);
            }

            return indicador;

        }

        public string getIndicador(string centroCosto, Comprobante comprobante)
        {

            string indicador = string.Empty;
            try
            {
                if (comprobante != null)
                {
                    string tasa = getTasa(comprobante);
                    if (!tasa.Equals("0"))
                        indicador = getIndicadorFromBD(centroCosto, tasa);
                }
                else
                {
                    bHanOcurridoErrores = true;
                    strResumenResultados += "Error tratando de obtener el comprobante <br/>";
                    throw new CustomException("Comprobante Nulo");
                }
            }
            catch (CustomException ce)
            {
                throw;
            }
            catch (Exception exc)
            {
                strResumenResultados += "Error al obtener el indicador desde la BD <br/>";
                throw new CustomException("Error al obtener el indicador", exc);

            }
            return indicador;
        }
        public void mostrarModal()
        {

            //ContentPlaceHolder cphMasterPage = (ContentPlaceHolder)Master.FindControl("MainContent");


            //UpdatePanel udpLoader = (UpdatePanel)cphMasterPage.FindControl("udpLoader");

            /*Label lblModalTitle = (Label)udpLoader.FindControl("lblModalTitle");
            lblModalTitle.Text = "Carga de Factura";*/

            /*Label lblModalBody = (Label)udpLoader.FindControl("lblModalBody");
            lblModalBody.Text = strResumenResultados;*/

            //lblCargarFacturaResultados.Text = strResumenResultados;
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalLoader", "$('#MainContent_uppMainContent').show();", false);
            //ClientScript.RegisterStartupScript(Page.GetType(), "showLoader", "showLoader();", false);
            //udpLoader.Update();
        }

        public void deleteUploadFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        protected void Button1_Click1(object sender, EventArgs e)
        {
            SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
            //sapConnectorInterface.consultaFacturaByStatus(ConfigurationManager.AppSettings["NAME"]);
            //sapConnectorInterface.consultaFacturaByFecha("03/11/2017", "03/11/2017","0021030123", ConfigurationManager.AppSettings["NAME"]);

            //sapConnectorInterface.IncluirFactura(ConfigurationManager.AppSettings["NAME"]);
            bool resp = sapConnectorInterface.actualizarEstatusFactura("033", "ba90a9b4-a3d3-44f2-a9a3-60b6d8249b2h", Constants.destinationName);

        }


    }
}