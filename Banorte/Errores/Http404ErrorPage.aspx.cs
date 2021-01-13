using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banorte.Models;

namespace SIMLA
{
    public partial class Http404ErrorPage : System.Web.UI.Page
    {
        protected HttpException oHttpException = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            oHttpException = new HttpException("HTTP 404");
            string strUrlReferrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "Http404ErrorPage";
            ExceptionsManager.LogRegister(ExceptionsManager.Message(oHttpException, strUrlReferrer), ExceptionsManager.LOGLevel.ERROR);
        }
    }
}