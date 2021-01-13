using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using SAP.Middleware.Connector;
using Banorte.SAPConnector;
using System.Security.Principal;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Banorte.Utilities;
using System.IO;
using Banorte.Models;


namespace Banorte
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /*implementacion de sapConnector*/

            string destinationConfigName = "QA";
            IDestinationConfiguration destinationConfig = null;
            bool destinationIsInicialized = false;
            if (!destinationIsInicialized)
            {
                destinationConfig = new SAPDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);
                if (RfcDestinationManager.TryGetDestination(destinationConfigName) == null)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                    destinationIsInicialized = true;
                }
            }

            /*configuracion log4NET*/
            // Iniciamos log4net
            log4net.Config.XmlConfigurator.Configure();

            
        }


        public void Application_AuthenticateRequest(Object src, EventArgs e)
        {
            if (!(HttpContext.Current.User == null))
            {
                if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
                {
                    System.Web.Security.FormsIdentity InfoUsuario;
                    InfoUsuario = (System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity;                    
                    AdminUsuario admUsuario = new AdminUsuario();
                    Usuario usuario = admUsuario.deserialize(InfoUsuario.Ticket.UserData);
                    String[] rolesUsuario = new String[3];
                    int i = -1;
                    if (usuario.EsSuperUsuario)
                    {
                        rolesUsuario[i + 1] = "Admin";
                        i++;
                    }
                    if (usuario.EsProveedor)
                    {
                        rolesUsuario[i+1] = "Proveedor";
                    }
                    else
                    {
                        rolesUsuario[i+1] = "NoProveedor";
                    }
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(InfoUsuario, rolesUsuario);
                    
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            HttpApplication app = (HttpApplication)sender;
             Exception oException = Server.GetLastError();

            if (oException.GetType() == typeof(HttpException))
            {
                if (oException.Message.Contains("NoCatch") || oException.Message.Contains("maxUrlLength"))
                    return;
                app.Server.Transfer("~/Errores/HttpErrorPage.aspx");
            }
            else
            {
                app.Server.Transfer("~/Errores/NoHttpErrorPage.aspx");
            }
        }

    }
}