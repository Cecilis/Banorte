using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;
using Banorte.SAPConnector;
using System.Web.Security;

namespace Banorte
{
    public partial class ModificarUsuario : System.Web.UI.Page
    {
        private string clave;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblModificarUsuarioResultados.Text = string.Empty;
            icoOK.Visible = false;
            icoNOTOK.Visible = false;


            if (!HttpContext.Current.User.IsInRole("Admin"))
                Response.Redirect(FormsAuthentication.DefaultUrl, false);

            string strClave = txtClave.Text;
            txtClave.Attributes.Add("value", strClave);

            string strConfirmarClave = txtConfirmarClave.Text;
            txtConfirmarClave.Attributes.Add("value", strConfirmarClave);

            try
            {
                if (!Page.IsPostBack)
                {

                    if (Request["id"] != null)
                    {
                        var UsuarioId = 0;
                        Int32.TryParse(Request["id"], out UsuarioId);

                        if (UsuarioId != 0)
                        {
                            AdminUsuario oAdminUsuario = new AdminUsuario();
                            Usuario oUsuario = oAdminUsuario.getUsuarioPorId(UsuarioId);
                            ViewState["UsuarioEnEdicionId"] = UsuarioId;
                            txtLogin.Text = oUsuario.Login;
                            txtClave.Attributes.Add("value", oUsuario.Password);
                            txtConfirmarClave.Attributes.Add("value", string.Empty);
                            chkEsSuperUsuario.Checked = oUsuario.EsSuperUsuario ? true : false;
                            chkEsProveedor.Checked = oUsuario.EsProveedor ? true : false;
                            txtCodigoProveedor.Text = oUsuario.CodigoProveedor;
                            txtRazonSocial.Text = oUsuario.RazonSocial;
                            ddlTiposBloqueo.Text = oUsuario.Bloqueado.ToString();
                            chkCambiarClave.Checked = oUsuario.CambiarClave ? true : false;
                            chkEsSuperUsuario.Checked = oUsuario.EsSuperUsuario ? true : false;
                            txtNroIntentos.Text = oUsuario.NroIntentos.ToString();

                            if (chkEsProveedor.Checked)
                            {
                                txtCodigoProveedor.ReadOnly = false;
                                txtRazonSocial.ReadOnly = false;
                            }
                            else
                            {
                                txtCodigoProveedor.ReadOnly = true;
                                txtRazonSocial.ReadOnly = true;
                            }

                            rfvClave.Enabled = false;
                            rfvContrasenna.Enabled = false;
                        }
                        else
                        {
                            lblModificarUsuarioResultados.Text = "Ha ocurrido un error consultar usuario, por favor intente nuevamente";
                            icoOK.Visible = false;
                            icoNOTOK.Visible = true;
                        }
                    }
                    else Response.Redirect("Default.aspx", true);
                }
            }
            catch (Exception ex)
            {
                lblModificarUsuarioResultados.Text = "Ha ocurrido un error modificar usuario, por favor intente nuevamente";
                icoOK.Visible = false;
                icoNOTOK.Visible = true;
            }
            finally
            {
                lblModificarUsuario.Visible = lblModificarUsuarioResultados.Text.Trim().Length > 0 ? true : false;
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                lblModificarUsuarioResultados.Text = string.Empty;
                icoOK.Visible = false;
                icoNOTOK.Visible = false;

                if (!HttpContext.Current.User.IsInRole("Admin"))
                    Response.Redirect(FormsAuthentication.DefaultUrl, false);

                if (Page.IsValid)
                {

                    int nroIntentos = 0;
                    int bloqueo = 0;
                    int IdUsuario = 0;

                    Usuario oUsuario = new Usuario();

                    if (validaProveedor())                   
                    {

                        Int32.TryParse(ViewState["UsuarioEnEdicionId"] != null ? ViewState["UsuarioEnEdicionId"].ToString() : "0", out IdUsuario);
                        oUsuario.Id = IdUsuario;

                        oUsuario.Login = txtLogin.Text.Trim().ToUpper(); 
                        oUsuario.Password = txtClave.Text;

                        oUsuario.EsProveedor = chkEsProveedor.Checked;
                        oUsuario.RazonSocial = txtRazonSocial.Text;
                        oUsuario.CodigoProveedor = txtCodigoProveedor.Text;

                        Int32.TryParse(ddlTiposBloqueo.SelectedValue, out bloqueo);
                        oUsuario.Bloqueado = bloqueo;

                        oUsuario.CambiarClave = chkCambiarClave.Checked;
                        Int32.TryParse(txtNroIntentos.Text, out nroIntentos);
                        oUsuario.NroIntentos = nroIntentos;

                        oUsuario.EsSuperUsuario = chkEsSuperUsuario.Checked;
                        
                        

                        AdminUsuario oAdminUsuario = new AdminUsuario();

                        bool usuarioActualizado = false;

                        if (chkCambioContrasena.Checked)
                        {
                            //Actualizo todo
                             usuarioActualizado = oAdminUsuario.actualizarUsuario(oUsuario.Id, oUsuario.Login, oUsuario.Password, oUsuario.EsProveedor, oUsuario.CodigoProveedor, oUsuario.RazonSocial, oUsuario.EsSuperUsuario, oUsuario.Bloqueado, oUsuario.CambiarClave, oUsuario.NroIntentos);
                        }

                        else
                        {
                            /*Actualizo todo menos  la contraseña*/
                            usuarioActualizado = oAdminUsuario.actualizarUsuario(oUsuario.Id, oUsuario.Login,  oUsuario.EsProveedor, oUsuario.CodigoProveedor, oUsuario.RazonSocial, oUsuario.EsSuperUsuario, oUsuario.Bloqueado, oUsuario.CambiarClave, oUsuario.NroIntentos);
                        }
                        
                        

                        if (usuarioActualizado)
                        {
                            lblModificarUsuarioResultados.Text = "Usuario actualizado.";
                            icoOK.Visible = true;
                            icoNOTOK.Visible = false;
                        }
                        else
                        {
                            lblModificarUsuarioResultados.Text = "Ha ocurrido un error al modificar el usuario, por favor intente nuevamente";
                            icoOK.Visible = false;
                            icoNOTOK.Visible = true;
                        }
                    }
                    else
                    {
                        lblModificarUsuarioResultados.Text = "El Código proveedor no existe";
                        icoOK.Visible = false;
                        icoNOTOK.Visible = true;
                        //throw new CustomException("El Código proveedor no existe", "CodigoProveedor");
                    }

                }// End if (Page.IsValid)
                else
                {
                    lblModificarUsuarioResultados.Text = "Hay errores en los datos ingresados, revise e intente nuevamente";
                    icoOK.Visible = false;
                    icoNOTOK.Visible = true;
                }
            }
            catch (CustomException ce)
            {
                lblModificarUsuarioResultados.Text = ce.Message.ToString();
            }
            catch (Exception ex)
            {
                lblModificarUsuarioResultados.Text = "Ha ocurrido un error al momento de crear el usuario";
            }
            finally
            {
                udpMainContent.Update();
                lblModificarUsuario.Visible = lblModificarUsuarioResultados.Text.Trim().Length > 0 ? true : false;
            }
        }

        protected void txtCodigoProveedor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            lblModificarUsuarioResultados.Text = string.Empty;
            if ((chkEsProveedor.Checked) && (string.IsNullOrEmpty(txtCodigoProveedor.Text)))
            {
                args.IsValid = false;
            }
        }

        protected void txtRazonSocial_ServerValidate(object source, ServerValidateEventArgs args)
        {
            lblModificarUsuarioResultados.Text = string.Empty;
            if ((chkEsProveedor.Checked) && (string.IsNullOrEmpty(txtRazonSocial.Text)))
            {
                args.IsValid = false;
            }
        }


        protected void txtConfirmaClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            lblModificarUsuarioResultados.Text = string.Empty;
            if ((chkCambioContrasena.Checked) && (string.IsNullOrEmpty(txtConfirmarClave.Text)))
            {
                args.IsValid = false;
            }
        }

        protected void chkEsProveedor_CheckedChanged(object sender, EventArgs e)
        {
            lblModificarUsuarioResultados.Text = string.Empty;
            if (chkEsProveedor.Checked)
            {
                txtCodigoProveedor.ReadOnly = false;
                txtRazonSocial.ReadOnly = false;
                txtCodigoProveedor.Focus();
            }
            else
            {
                txtCodigoProveedor.Text = string.Empty;
                txtRazonSocial.Text = string.Empty;
                txtCodigoProveedor.ReadOnly = true;
                txtRazonSocial.ReadOnly = true;
            }
            udpMainContent.Update();
        }


        public bool validaProveedor()
        {
            SapConnectorInterface sapConnectorInterface = new SapConnectorInterface();
            try
            {
                bool existe = false;
                if (chkEsProveedor.Checked == true)
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
                lblModificarUsuarioResultados.Text = "Error al validar código proveedor";
                throw new CustomException("Error al validar código proveedor", exc);
            }
        }

        protected void chkCambioContrasena_CheckedChanged(object sender, EventArgs e)
        {
            
            lblModificarUsuarioResultados.Text = string.Empty;
            clave = txtClave.Text;
            if (chkCambioContrasena.Checked)
            {
                txtClave.ReadOnly = false;
                rfvClave.Enabled = true;
                txtClave.Attributes.Add("value", string.Empty);                
                txtClave.Focus();                
                txtConfirmarClave.ReadOnly = false;
                txtConfirmarClave.Attributes.Add("value", string.Empty);
                rfvContrasenna.Enabled = true;
            }
            else
            {                
                txtClave.Attributes.Add("value", clave);
                txtClave.ReadOnly = true;
                rfvClave.Enabled = false;
                txtConfirmarClave.Attributes.Add("value", string.Empty);
                txtConfirmarClave.ReadOnly = true;
                rfvContrasenna.Enabled = false;
            }
            udpMainContent.Update();
        }

    }
}