using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Banorte.Models
{
    public class Configuracion
    {
        public void ModificarConnectionStrings(string pClave)
        {
            try
            {
                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
                section.ConnectionStrings[pClave].ConnectionString = "Data Source=...";
                configuration.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AgregarAppSettings(string pClave, string pValor)
        {
            try
            {
                Configuration oConfiguration = WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection AppSection = oConfiguration.GetSection("appSettings") as AppSettingsSection;
                AppSection.Settings.Add(new KeyValueConfigurationElement(pClave, pValor));
                oConfiguration.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ObtenerCadenaConexion(string pClave)
        {
            return ConfigurationManager.ConnectionStrings[pClave].ConnectionString;
        }

        public static string GetFullyQualifiedUrl(string strURL)
        {
            Uri oUri = new Uri(HttpContext.Current.Request.Url, strURL);
            return oUri.ToString();
        }
        public static Uri GetFullyQualifiedUrl2Uri(string strURL)
        {
            Uri oUri = new Uri(HttpContext.Current.Request.Url, strURL);
            return oUri;
        }

        public static string ObtenerAppSettings(string pClave)
        {
            return ConfigurationManager.AppSettings[pClave].ToString();
        }

        public static void EditarAppSettings(string pClave, string pValor)
        {
            try
            {

                Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                //Edit
                if (objAppsettings != null)
                {
                    objAppsettings.Settings[pClave].Value = pValor;
                    objConfig.Save();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}