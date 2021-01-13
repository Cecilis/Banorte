using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;

namespace SIMLA
{
    public partial class HttpErrorPage : System.Web.UI.Page
    {
        protected HttpException oHttpException = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            oHttpException = (HttpException)Server.GetLastError();

            int intHttpCode = oHttpException.GetHttpCode();

            // Filtra por código de error
            if (intHttpCode >= 400 && intHttpCode < 500)
                oHttpException = new HttpException
                    (intHttpCode, "Error HTTP tipo 4xx.", oHttpException);
            else if (intHttpCode > 499)
                oHttpException = new HttpException
                    (oHttpException.ErrorCode, "Error HTTP tipo 5xx.", oHttpException);
            else
                oHttpException = new HttpException
                    (intHttpCode, "Error HTTP no identificado.", oHttpException);

            // Registro en Archivo de Log
            ExceptionsManager.LogRegister(ExceptionsManager.Message(oHttpException, "HttpErrorPage"), ExceptionsManager.LOGLevel.ERROR);

            // Completa los campos
            exMessage.Text = oHttpException.Message;
            exTrace.Text = oHttpException.StackTrace;

            // Completa los campos para mostrar el detalle del error
            if (oHttpException.InnerException != null)
            {
                innerTrace.Text = oHttpException.InnerException.StackTrace;
                InnerErrorPanel.Visible = Request.IsLocal;
                innerMessage.Text = string.Format("HTTP {0}: {1}", intHttpCode, oHttpException.InnerException.Message);
            }
            exTrace.Visible = Request.IsLocal;

            // Limpia el error del server.
            Server.ClearError();
        }
    }
}