using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;
using Banorte.Utilities;


namespace Banorte
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {

            try
            {
                Session.Clear();
                string login = txtUsuario.Text;
                string password = txtPassword.Text;



                AdminUsuario admUsuario = new AdminUsuario();
                Usuario usuario = admUsuario.getUsuario(login, password);

                Boolean usuarioExiste = usuario.getExiste();

                if (usuarioExiste)
                {

                    //FormsAuthentication.RedirectFromLoginPage(txtUsuario.Value, chkRecordarme.Checked)
                    lblResultados.InnerText = "";

                    bool isPersistent = false;

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        txtUsuario.Text,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(50),
                        isPersistent,
                        admUsuario.serialize(usuario),
                        FormsAuthentication.FormsCookiePath);

                    // Encripta el ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    // Crea la cookie 
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    if (usuario.CambiarClave == false)
                    {
                        // Redirecciona a la página solicitada originalmente
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsuario.Text, isPersistent), false);
                    }
                    else
                    {
                        //obligo al usuario a cambiar la clave
                        Response.Redirect("CambiarPassword.aspx", false);
                    }
                }
                else
                {
                    Session.Clear();
                    Session.RemoveAll();
                    string mensaje = getMensaje(usuario.getCodigoMensaje());
                    Functions.SetResultLabelContent(lblResultados, "error", "* " + mensaje);
                    ExceptionsManager.LogRegister(ExceptionsManager.Message("Usuario " + usuario.Login + " no registrado o clave invalida", Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.INFO);
                    ExceptionsManager.LogRegister(ExceptionsManager.Message("Usuario txt " + txtUsuario.Text + " no registrado o clave invalida", Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.INFO);

                }
            }
            catch (Exception ex)
            {
                ExceptionsManager.LogRegister(ExceptionsManager.Message(ex, Functions.GetCurrentPageName(Request.Url.AbsolutePath)), ExceptionsManager.LOGLevel.ERROR);
                Functions.SetResultLabelContent(lblResultados, "error", Environment.NewLine + "Ha ocurrido un error, por favor intente nuevamente");
            }
        }


        public string getMensaje(int codigoMensaje)
        {
            string mensaje = string.Empty;

            switch (codigoMensaje)
            {
                case 2:
                    mensaje = "Usuario no registrado";
                    break;

                case 3:
                    mensaje = "Clave invalida";
                    break;

                case 4:
                    mensaje = "Usuario Bloqueado porque supero el numero de intentos permitidos, por favor pongase en contacto con el Administrador";
                    break;

                case 5:
                    mensaje = "Si introduce nuevamente la contraseña errada el usuario será Bloqueado";
                    break;

                case 6:
                    mensaje = "Usuario esta Bloqueado por favor pongase en contacto con el Administrador";
                    break;
            }

            return mensaje;
        }



    }
}