using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SAP.Middleware.Connector;



namespace Banorte.VerificarFacturas.SAPConnector
{
    public class SAPDestinationConfig : IDestinationConfiguration
    {

        private static bool inicializado = false;

        public static bool Inicializado
        {
            get { return SAPDestinationConfig.inicializado; }
            set { SAPDestinationConfig.inicializado = value; }
        }
        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            if (Inicializado)
            {
                parms.Add(RfcConfigParameters.Name, destinationName.Trim());
                parms.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_APPSERVERHOST"]);
                parms.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEMNUM"]);
                parms.Add(RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_CLIENT"]);
                parms.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"]);
                parms.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]);
                parms.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"]);
                parms.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]);
                parms.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]);
            }
            return parms;
        }

    }
}