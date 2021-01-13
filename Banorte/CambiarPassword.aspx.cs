using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;
using Banorte.Utilities;
using System.Web.Security;
using System.Threading;

namespace Banorte
{
    public partial class CambiarPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ocultarResultado(); 
            }
        }

         public void ocultarResultado()
        {
            lblCambioContrasena.Visible = false;
            lblGuardarContrasena.Visible = false;
            imagenGuardarConntrasenaOk.Visible = false;
            imagenGuardarConntrasenaNotOk.Visible = false;
            lblErrorGuardarContrasena.Visible = false;
        }

         public void mostrarResultadoCambioContrasena(bool valido, string mensaje)
         {
             lblCambioContrasena.Visible = true;
             lblGuardarContrasena.Visible = valido;
             lblGuardarContrasena.Text = mensaje;
             imagenGuardarConntrasenaOk.Visible = valido;
             imagenGuardarConntrasenaNotOk.Visible = !valido;
             lblErrorGuardarContrasena.Visible = !valido;
             lblErrorGuardarContrasena.Text = mensaje;
         }

         public void mostrarResultadoCambioContrasena(bool valido)
         {
             lblCambioContrasena.Visible = true;
             lblGuardarContrasena.Visible = valido;             
             imagenGuardarConntrasenaOk.Visible = valido;
             imagenGuardarConntrasenaNotOk.Visible = !valido;
             lblErrorGuardarContrasena.Visible = !valido;
             
         }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            HttpCookie decryptedCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(decryptedCookie.Value);
            AdminUsuario admUsuario = new AdminUsuario();
            Usuario usuarioSession = admUsuario.deserialize(ticket.UserData);
            string contrasenaActual = txtContrasenaActual.Text;
            string contrasenaNueva = string.Empty;
            bool cambiarClave = usuarioSession.CambiarClave;

            bool existeUsuario = admUsuario.verificarContrasena(usuarioSession.Login, contrasenaActual);
            bool modificacionExitosa = false;
           
            if (existeUsuario == true)
            {
                lblErrorGuardarContrasena.Text = string.Empty;
                contrasenaNueva = txtContrasenaNueva.Text;
                modificacionExitosa = admUsuario.modificarContrasena(usuarioSession.Login, contrasenaNueva);
                if (modificacionExitosa == true)
                {
                    //Functions.SetResultLabelContent(lblCambiarContrasenaResultados, "validatorResultado", Environment.NewLine + "Contraseña Guardada Exitosamente");

                    if (cambiarClave == false)
                    {
                        mostrarResultadoCambioContrasena(true, "Exitoso");     
                    }
                    else
                    {
                        mostrarResultadoCambioContrasena(true, "Cambio de Contraseña Exitoso, <br /> Debe Salir del sistema y volver ingresar");
                    }

                    /*FormsAuthentication.SignOut();
                    Session.Abandon();
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty) { Expires = DateTime.Now.AddYears(-1) };
                    Response.Cookies.Add(authCookie);
                    var sessionCookie = new HttpCookie(Session.SessionID, string.Empty) { Expires = DateTime.Now.AddYears(-1) };
                    Response.Cookies.Add(sessionCookie);
                    FormsAuthentication.RedirectToLoginPage();*/
                    
                }

                else
                {
                    //Functions.SetResultLabelContent(lblCambiarContrasenaResultados, "error", Environment.NewLine + "Ha ocurrido un error, por favor intente nuevamente");
                    mostrarResultadoCambioContrasena(false, "Ha ocurrido un error, por favor intente nuevamente");
                }
                
            }

            else
            {
                //Functions.SetResultLabelContent(lblCambiarContrasenaResultados, "error", Environment.NewLine + "Contraseña actual invalida");
                mostrarResultadoCambioContrasena(false, "Contraseña actual invalida");
            }
            
        }

      
    }
}