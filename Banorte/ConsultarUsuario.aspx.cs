using Banorte.Models;
using Banorte.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Banorte
{
    public partial class ConsultarUsuario : System.Web.UI.Page
    {

        readonly PagedDataSource paginador = new PagedDataSource();
        private int primeraPosicion;
        private int ultimaPosicion;
        private int noRegistrosPagina = 10;

        private static DataTable infoUsuario = new DataTable();

        private string strResumenResultados;

        private string idUsuarioEnEdicion;

        public string IdUsuarioEnEdicion
        {
            get { return idUsuarioEnEdicion; }
        }

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

        public DataTable InfoUsuario
        {
            get { return infoUsuario; }
            set { infoUsuario = value; }
        }

        private int NoPagActual
        {
            get
            {
                if (ViewState["NoPagActual"] == null)
                {
                    return 0;
                }
                if ((int)ViewState["NoPagActual"] < -1)
                {
                    return -1;
                }
                return ((int)ViewState["NoPagActual"]);
            }
            set
            {
                ViewState["NoPagActual"] = value;
            }
        }

        private string OrdenColumna
        {
            get
            {
                if (ViewState["OrdenColumna"] == null)
                {
                    return "login";
                }
                return (ViewState["OrdenColumna"].ToString());
            }
            set
            {
                ViewState["OrdenColumna"] = value;
            }
        }

        private string OrdenSentido
        {
            get
            {
                if (ViewState["OrdenSentido"] == null)
                {
                    return "ASC";
                }
                return (ViewState["OrdenSentido"].ToString());
            }
            set
            {
                ViewState["OrdenSentido"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Cuentas por Cobrar - Consultar";

            string strResumenResultados = string.Empty;

            lblConsultarUsuario.Text = string.Empty;
            icoOK.Visible = false;
            icoNOTOK.Visible = false;
            lblConsultarUsuarioResultados.Text = string.Empty;

            

            if (!HttpContext.Current.User.IsInRole("Admin"))
                Response.Redirect(FormsAuthentication.DefaultUrl, false);

            try
            {
               
                if (!Page.IsPostBack)
                {
                    NoPagActual = 0;
                }

                rptUsuarios.DataSource = new DataTable();
                rptUsuarios.DataBind();

                
                CargarPaginaEnTabla();

                pnlHayRegistros.Visible = (rptUsuarios.Items.Count == 0);
                olPaginador.Visible = rptUsuarios.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                pnlHayRegistros.Visible = false;
                ExceptionsManager.LogRegister(ExceptionsManager.Message(ex, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                icoOK.Visible = false;
                icoNOTOK.Visible = true;
                lblConsultarUsuarioResultados.Text = "Ha ocurrido un error, por favor intente nuevamente";
            }
            finally
            {
                lblConsultarUsuario.Visible = lblConsultarUsuarioResultados.Text.Trim().Length > 0 ? true : false;
            }
        }


        //public DataTable CargarData()
        //{
        //    try
        //    {
        //        AdminUsuario oAdminUsuario = new AdminUsuario();
        //        DataSet ds = oAdminUsuario.getUsuarioTodos(OrdenColumna, OrdenSentido);
        //        DataTable dt = ds.Tables[0];
        //        InfoUsuario = dt;
        //        return dt;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw;
        //    }
        //}


        // Carga los registros de una página en el repetidor asociado oFileAppender las facturas
        private void CargarPaginaEnTabla()
        {
            try 
            {

                AdminUsuario oAdminUsuario = new AdminUsuario();
                DataSet ds = oAdminUsuario.getUsuarioTodos(OrdenColumna, OrdenSentido);
                InfoUsuario = ds.Tables[0];

                Paginador.DataSource = InfoUsuario.DefaultView;
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
                rptUsuarios.DataSource = Paginador;
                rptUsuarios.DataBind();

                olPaginador.Visible = rptUsuarios.Items.Count > 0 ? true : false;

                // Efectua el paginado de los registros
                GestorDePaginado();

            }
            catch(Exception ex)
            {
                throw;
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


        protected void rptUsuarios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!e.CommandName.Equals("E")) return;
            string IdUsuario = e.CommandArgument.ToString();
            Response.Redirect(string.Format("ModificarUsuario.aspx?id={0}", IdUsuario), false);
        }



        protected void rptUsuarios_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListItemType.Header)
                {

                    ScriptManager oScriptManager = ScriptManager.GetCurrent(this);
                    LinkButton oLinkButton = new LinkButton();
                    TextBox oTextBox = new TextBox();


                    oTextBox = ((TextBox)e.Item.FindControl("txtFltUsuario"));
                    if (oTextBox != null)
                    {

                        oScriptManager.RegisterPostBackControl(oTextBox);
                    }



                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkLoginASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkLoginDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }


                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkEsProveedorASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkEsProveedorDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkCodProvASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkCodProvDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkRazonSocialASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkRazonSocialDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }


                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkSuperUsuarioASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkSuperUsuarioDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }


                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkCambiarClaveASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkCambiarClaveDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkTipoBloqueoASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkTipoBloqueoDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkNroFallosASC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                    oLinkButton = ((LinkButton)e.Item.FindControl("lnkNroFallosDESC"));
                    if (oLinkButton != null)
                    {
                        oLinkButton.Visible = oLinkButton.CommandName == OrdenColumna && oLinkButton.CommandArgument == OrdenSentido ? false : true;
                        oScriptManager.RegisterAsyncPostBackControl(oLinkButton);
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected void lnkOrdenarPor_Click(object sender, EventArgs e)
        {
            try
            {

                LinkButton lnkOrden = (LinkButton)sender;
                OrdenColumna = lnkOrden.CommandName.Trim();
                OrdenSentido = lnkOrden.CommandArgument.Trim();

                NoPagActual = 0;
                CargarPaginaEnTabla();
                udpMainContent.Update();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void txtFltUsuario_TextChanged(object sender, EventArgs e)
        {                    
            TextBox txtFltUsuario = (TextBox)rptUsuarios.Controls[0].Controls[0].FindControl("txtFltUsuario");
            infoUsuario.Select("login =" + txtFltUsuario.Text.Trim());
            rptUsuarios.DataSource = infoUsuario;
            rptUsuarios.DataBind();
        }

    }
}