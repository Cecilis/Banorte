using Banorte.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.SAPConnector;
using System.Web.Security;

namespace Banorte
{
    public partial class IncluirUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblIncluirUsuarioResultados.Text = string.Empty;
            icoOK.Visible = false;
            icoNOTOK.Visible = false;

            if (!HttpContext.Current.User.IsInRole("Admin"))
                Response.Redirect(FormsAuthentication.DefaultUrl, false);

            string strClave = txtClave.Text;
            txtClave.Attributes.Add("value", strClave);

            string strClaveConfirmacion = txtClave.Text;
            txtClaveConfirmar.Attributes.Add("value", strClaveConfirmacion);

            lblIncluirUsuario.Visible = lblIncluirUsuarioResultados.Text.Trim().Length > 0 ? true : false;
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                lblIncluirUsuarioResultados.Text = string.Empty;
                icoOK.Visible = false;
                icoNOTOK.Visible = false;

                if (!HttpContext.Current.User.IsInRole("Admin"))
                    Response.Redirect(FormsAuthentication.DefaultUrl, false);

                if (Page.IsValid)
                {

                    int nroIntentos = 0;
                    int bloqueo = 0;
                    Usuario oUsuario = new Usuario();

                    if (validaProveedor())
                    
                    {
                        oUsuario.Login = txtLogin.Text.Trim().ToUpper(); 
                        oUsuario.Password = txtClaveConfirmar.Text;

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

                        bool usuarioAgregado = oAdminUsuario.incluirUsuario(oUsuario.Login, oUsuario.Password, oUsuario.EsProveedor, oUsuario.CodigoProveedor, oUsuario.RazonSocial, oUsuario.EsSuperUsuario, oUsuario.Bloqueado, oUsuario.CambiarClave, oUsuario.NroIntentos);

                        if (usuarioAgregado)
                        {           
                            txtLogin.Text = string.Empty;
                            txtClave.Text = string.Empty;
                            txtClaveConfirmar.Text = string.Empty;
                            chkEsProveedor.Checked = false;
                            txtCodigoProveedor.Text = string.Empty;
                            txtRazonSocial.Text = string.Empty;
                            txtCodigoProveedor.ReadOnly = true;
                            txtRazonSocial.ReadOnly = true;
                            chkEsSuperUsuario.Checked = false;
                            ddlTiposBloqueo.ClearSelection();
                            txtNroIntentos.Text = "0";
                            chkCambiarClave.Checked = true;       
     
                            lblIncluirUsuarioResultados.Text = "Usuario creado.";
                            icoOK.Visible = true;
                            icoNOTOK.Visible = false;
                        }
                        else
                        {
                            lblIncluirUsuarioResultados.Text = "Ha ocurrido un error, por favor intente nuevamente";
                            icoOK.Visible = false;
                            icoNOTOK.Visible = true;
                        }

                    }
                    else
                    {
                        lblIncluirUsuarioResultados.Text = "El Código proveedor no existe";
                        throw new CustomException("El Código proveedor no existe", "CodigoProveedor");                       
                    }
                }
                else
                {
                    lblIncluirUsuarioResultados.Text = "Hay errores en los datos ingresados, revise e intente nuevamente";
                    icoOK.Visible = false;
                    icoNOTOK.Visible = true;
                }
            }
            catch (CustomException ce)
            {
                lblIncluirUsuarioResultados.Text = ce.Message.ToString();
                icoOK.Visible = false;
                icoNOTOK.Visible = true;
            }
            catch (Exception ex)
            {
                lblIncluirUsuarioResultados.Text = "Ha ocurrido un error al momento de crear el usuario";
                icoOK.Visible = false;
                icoNOTOK.Visible = true;
            }
            finally
            {
                udpMainContent.Update();
                lblIncluirUsuario.Visible = lblIncluirUsuarioResultados.Text.Trim().Length > 0 ? true : false;
            }
        }

        protected void txtCodigoProveedor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            lblIncluirUsuarioResultados.Text = string.Empty;
            if ((chkEsProveedor.Checked) && (string.IsNullOrEmpty(txtCodigoProveedor.Text)))
            {
                args.IsValid = false;
            }
        }

        protected void txtRazonSocial_ServerValidate(object source, ServerValidateEventArgs args)
        {
            lblIncluirUsuarioResultados.Text = string.Empty;
            if ((chkEsProveedor.Checked) && (string.IsNullOrEmpty(txtRazonSocial.Text)))
            {
                args.IsValid = false;                
            }
        }

        protected void chkEsProveedor_CheckedChanged(object sender, EventArgs e)
        {
            lblIncluirUsuarioResultados.Text = string.Empty;
            
            if (chkEsProveedor.Checked)
            {
                txtCodigoProveedor.ReadOnly = false;
                txtRazonSocial.ReadOnly = false;
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
                lblIncluirUsuarioResultados.Text = "Error al validar código proveedor";
                throw new CustomException("Error al validar código proveedor", exc);
            }
        }


    }
}