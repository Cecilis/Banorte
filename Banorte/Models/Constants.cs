using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Banorte.Models
{
    public static class Constants
    {
        public static string destinationName = ConfigurationManager.AppSettings["NAME"];
    }
}