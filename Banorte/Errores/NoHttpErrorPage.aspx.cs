using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;

namespace SIMLA
{
    public partial class NoHttpErrorPage : System.Web.UI.Page
    {
        protected Exception oNoHttpException = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            string strMensajeError = string.Empty;
            if (Server.GetLastError() != null)
            {
                oNoHttpException = (Exception)Server.GetLastError();
                if (oNoHttpException.InnerException != null)
                    strMensajeError = oNoHttpException.InnerException.Message;
                else
                    strMensajeError = oNoHttpException.Message;
            }
            else
                oNoHttpException = new HttpException("Error desconocido.");

            string strUrlReferrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "NoHttpErrorPage";
            ExceptionsManager.LogRegister(ExceptionsManager.Message(oNoHttpException, strUrlReferrer), ExceptionsManager.LOGLevel.ERROR);

            exMessage.Text = ExceptionsManager.Message(oNoHttpException, strUrlReferrer);

            Server.ClearError();
        }
    }
}