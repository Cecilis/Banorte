using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;

namespace SIMLA
{
    public partial class DefaultRedirectErrorPage : System.Web.UI.Page
    {
        protected HttpException oHttpException = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            oHttpException = new HttpException("defaultRedirect");
            string strUrlReferrer =  Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "DefaultRedirect";
            ExceptionsManager.LogRegister(ExceptionsManager.Message(oHttpException, strUrlReferrer), ExceptionsManager.LOGLevel.ERROR);

        }
    }
}