using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Persistencia;
using Banorte.SAPConnector;
using Banorte.Models;
using SAP.Middleware.Connector;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Banorte.Utilities;
using System.Web.Security;
using Banorte.SAPConnector;

namespace Banorte
{
    public partial class ConsultarFactura : System.Web.UI.Page
    {

        readonly PagedDataSource paginador = new PagedDataSource();
        private int primeraPosicion;
        private int ultimaPosicion;
        private int noRegistrosPagina = 10;


        private string idProveedor;
        private string razonSocial;
        private string fechaDesde;
        private string fechaHasta;

        SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
        private string strResumenResultados;


        public PagedDataSource Paginador
        {
            get { return paginador; }
        } 
        public int PrimeraPosicion
        {
            get { return primeraPosicion; }
            set { primeraPosicion = value; }
        }
        public int UltimaPosicion
        {
            get { return ultimaPosicion; }
            set { ultimaPosicion = value; }
        }
        public int NoRegistrosPagina
        {
            get { return noRegistrosPagina; }
            set { noRegistrosPagina = value; }
        }

      

        public void setIdProveedor(string idProveedor)
        {
            this.idProveedor = idProveedor;
        }
        public string getIdProveedor()
        {
            return this.idProveedor = ((bool)ViewState["esProveedor"]) ? ViewState["codigoProveedor"].ToString() : txtCodigoProveedor.Text.Trim();
        }

        public void setRazonSocial(string razonSocial)
        {
            this.razonSocial = razonSocial;
        }
        public string getRazonSocial()
        {
            return this.razonSocial;
        }

        public void setFechaDesde(string fechaDesde)
        {
            this.fechaDesde = fechaDesde;
        }

        public string getFechaDesde()
        {
            return txtFechaDesde.Text.Trim();
        }

        public void setFechaHasta(string fechaHasta)
        {
            this.fechaHasta = fechaHasta;
        }
        
        public string getFechaHasta()
        {
            return txtFechaHasta.Text.Trim();
        }

        private int NoPagActual
        {
            get
            {
                if (ViewState["NoPagActual"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["NoPagActual"]);
            }
            set
            {
                ViewState["NoPagActual"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Cuentas por Cobrar - Consultar";

            DataTable oDataTable = new DataTable();
            rptFacturas.DataSource = oDataTable;
            rptFacturas.DataBind();
            //lblRazonSocial.Text = getRazonSocial();
            olPaginador.Visible = rptFacturas.Items.Count > 0 ? true : false;
            lblRazonSocial.Text = string.Empty;
            if (!Page.IsPostBack)
            {
                //Recupera de la data del usuario logeado
                HttpCookie decryptedCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(decryptedCookie.Value);
                AdminUsuario admUsuario = new AdminUsuario();
                Usuario usuario = admUsuario.deserialize(ticket.UserData);
                ViewState["codigoProveedor"] = usuario.CodigoProveedor;
                ViewState["esProveedor"] = usuario.EsProveedor;
                ViewState["esProveedor"] = usuario.EsProveedor;
                ViewState["razonSocial"] = usuario.RazonSocial;
                lblRazonSocial.Text = usuario.RazonSocial;
                pnlCodigoProveedor.Visible = !usuario.EsProveedor;
            }
        }


        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            bool esProveedor = (bool)ViewState["esProveedor"]; 
            bool bCamposValidos = true;
            string strResumenResultados = string.Empty;
            string strCodigoProveedor = txtCodigoProveedor.Text.Trim();
            lblResultados.InnerText = string.Empty;
            lblRazonSocial.Text = string.Empty;
            Proveedor proveedor;

            try
            {
                lblRazonSocial.CssClass = "control-label text-center titulo-consulta";
                if (esProveedor == true)
                {
                    //TODO: 
                    if (String.IsNullOrEmpty(getIdProveedor()))
                    {
                        strResumenResultados += "* Codigo Proveedor: Formato de código no válido" + "<br/>";
                        bCamposValidos = false;
                    }
                }


                if (bCamposValidos)
                {
                    rptFacturas.DataSource = new DataTable();
                    rptFacturas.DataBind();

                    this.setFechaDesde(txtFechaDesde.Text.Trim());
                    this.setFechaHasta(txtFechaHasta.Text.Trim());

                    NoPagActual = 0; 
                    proveedor = validaProveedor();
                    if (proveedor.Existe)
                    {
                        lblRazonSocial.Text = (proveedor.RazonSocial == "" ? ViewState["razonSocial"].ToString() : proveedor.RazonSocial);
                        ViewState["razonSocial"] = (proveedor.RazonSocial == "" ? ViewState["razonSocial"].ToString() : proveedor.RazonSocial);
                        CargarPaginaEnTabla();
                        pnlHayRegistros.Visible = (rptFacturas.Items.Count == 0);
                    }
                    else
                    {
                        lblRazonSocial.Text = "El Código proveedor no existe";
                        lblRazonSocial.CssClass = "control-label error";
                    }
                }
            
            }
            catch (Exception ex)
            {
                pnlHayRegistros.Visible = false;  
                ExceptionsManager.LogRegister(ExceptionsManager.Message(ex, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                Functions.SetResultLabelContent(lblResultados, "error", "Ha ocurrido un error al consultar las facturas, por favor intente nuevamente");

            }
        }


        public DataTable CargarData()
        {
            try
            {
                DataSet ds = consultaFacturaByFecha();
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public DataSet consultaFacturaByFecha()
        {
            try
            { 
                SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
                DataSet ds = sapConnectorInterface.consultaFacturaByFecha(this.getFechaDesde(), this.getFechaHasta(), this.getIdProveedor(), Constants.destinationName);
                return ds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Carga los registros de una página en el repetidor asociado oFileAppender las facturas
        private void CargarPaginaEnTabla()
        {
            try 
            {

                lblRazonSocial.Text = ViewState["razonSocial"].ToString();
                var oDataTable = CargarData();
                Paginador.DataSource = oDataTable.DefaultView;
                Paginador.AllowPaging = true;

                // Número de posiciones oFileAppender ser mostradas en el paginador
                Paginador.PageSize = NoRegistrosPagina;
                Paginador.CurrentPageIndex = NoPagActual;

                // Guarda el número de páginas en la vista de estado
                ViewState["NoTotalPaginas"] = Paginador.PageCount;
                // Indicador de pagina
                lblPaginaActual.Text =  "Pág. " + (NoPagActual + 1) + " de " + Paginador.PageCount;

                // Habilita los botónes Previo, Siguiente, Primero y Último
                lblPagPrevio.Enabled = !Paginador.IsFirstPage;
                lblPagSiguiente.Enabled = !Paginador.IsLastPage;
                lblPagPrimero.Enabled = !Paginador.IsFirstPage;
                lblPagUltimo.Enabled = !Paginador.IsLastPage;

                // Cargar registros en el repetidor
                rptFacturas.DataSource = Paginador;
                rptFacturas.DataBind();

                olPaginador.Visible = rptFacturas.Items.Count > 0 ? true : false;

                // Efectua el paginado de los registros
                GestorDePaginado();

            }
            catch(Exception ex)
            {
                throw;
            }
        }


        public Proveedor validaProveedor()
        {
            Proveedor proveedor; 
            try
            {
                
                if (!(bool)ViewState["esProveedor"])
                {
                    proveedor = sapConnectorInterface.existeProveedor(txtCodigoProveedor.Text, Constants.destinationName);
                }
                else
                {
                    proveedor = new Proveedor("","",true);
                   
                }
                return proveedor;

            }
            catch (Exception exc)
            {
                //bHanOcurridoErrores = true;
                strResumenResultados += "Error al validar código de proveedor <br/>";
                throw new CustomException("Error al validar código proveedor", exc);
            }
        }

        private void GestorDePaginado()
        {
            try 
            { 
                var oDataTable = new DataTable();

                oDataTable.Columns.Add("PageIndex"); //Inicia en 0
                oDataTable.Columns.Add("PageText"); //Inicia en 1

                PrimeraPosicion = NoPagActual - 5;
                if (NoPagActual > 5)
                    UltimaPosicion = NoPagActual + 5;
                else
                    UltimaPosicion = 10;

                //Verifica que la última pagina sea mayor que el total,
                //si es asi último indice será el total de páginas
                if (UltimaPosicion > Convert.ToInt32(ViewState["NoTotalPaginas"]))
                {
                    UltimaPosicion = Convert.ToInt32(ViewState["NoTotalPaginas"]);
                    PrimeraPosicion = UltimaPosicion - 10;
                }

                if (PrimeraPosicion < 0)
                    PrimeraPosicion = 0;

                // Generación de los números de pagina 
                for (var i = PrimeraPosicion; i < UltimaPosicion; i++)
                {
                    var dr = oDataTable.NewRow();
                    dr[0] = i;
                    dr[1] = i + 1;
                    oDataTable.Rows.Add(dr);
                }

                rptPaginador.DataSource = oDataTable;
                rptPaginador.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected void lblPagPrimero_Click(object sender, EventArgs e)
        {
            try 
            { 
                NoPagActual = 0;               
                CargarPaginaEnTabla();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void lblPagUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                NoPagActual = (Convert.ToInt32(ViewState["NoTotalPaginas"]) - 1);                
                CargarPaginaEnTabla();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void lblPagPrevio_Click(object sender, EventArgs e)
        {
            try 
            { 
                NoPagActual -= 1;                
                CargarPaginaEnTabla();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void lblPagSiguiente_Click(object sender, EventArgs e)
        {
            try
            { 
                NoPagActual += 1;                
                CargarPaginaEnTabla();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void rptPaginador_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try 
            { 
                if (!e.CommandName.Equals("newPage")) return;
                NoPagActual = Convert.ToInt32(e.CommandArgument.ToString());                
                CargarPaginaEnTabla();
                udpMainContent.Update();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void rptPaginador_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try 
            { 
                var lnkPagSelecionada = (LinkButton)e.Item.FindControl("lblPagSelecionada");
                if (lnkPagSelecionada.CommandArgument != NoPagActual.ToString()) return;
                lnkPagSelecionada.ClientIDMode = System.Web.UI.ClientIDMode.AutoID;
                lnkPagSelecionada.Enabled = false;
                lnkPagSelecionada.CssClass = "active";
                lnkPagSelecionada.ForeColor = Color.FromName("#FFFFFF");
                lnkPagSelecionada.BackColor = Color.FromName("#337ab7");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void rptPaginador_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                ScriptManager oScriptManager = ScriptManager.GetCurrent(this);
                LinkButton lnkPagSelecionada = (LinkButton)e.Item.FindControl("lblPagSelecionada");
                if (lnkPagSelecionada != null)
                {
                    oScriptManager.RegisterAsyncPostBackControl(lnkPagSelecionada);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string formatFecha(string fecha, string formatoFecha, string formatoAConvertir)
        {
            string fechaFormateada = "";
            if (fecha != "")
            {
                DateTime dt = DateTime.ParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                fechaFormateada = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            return fechaFormateada;
        }

        protected void rptFacturas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            { 
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    string datre = ((Label)e.Item.FindControl("lblDATRE")).Text;
                    ((Label)e.Item.FindControl("lblDATRE")).Text = Functions.GetDateStringFromYYYY_MM_DD(datre);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}              