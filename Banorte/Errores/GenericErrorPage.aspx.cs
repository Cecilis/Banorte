using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;

namespace SIMLA
{
    public partial class GenericErrorPage : System.Web.UI.Page
    {
        protected Exception oException = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Exception oException = Server.GetLastError();

            string strMensajeSeguridad = "Ha ocurrido un error en la aplicación. ";

            if (oException.InnerException != null)
            {
                innerTrace.Text = oException.InnerException.StackTrace;
                InnerErrorPanel.Visible = Request.IsLocal;
                innerMessage.Text = oException.InnerException.Message;
            }

            if (Request.IsLocal)
                exTrace.Visible = true;
            else
                oException = new Exception(strMensajeSeguridad, oException);

            // Completa los campos
            exMessage.Text = oException.Message;
            exTrace.Text = oException.StackTrace;

            // Registro en Archivo de Log
            ExceptionsManager.LogRegister(ExceptionsManager.Message(oException, "GenericErrorPage"), ExceptionsManager.LOGLevel.ERROR);

            // Limpia el error del server.
            Server.ClearError();
        }
    }
}