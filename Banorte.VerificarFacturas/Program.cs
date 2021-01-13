using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Banorte.VerificarFacturas
{
    static class Program
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new Servicio() 
                };
                ServiceBase.Run(ServicesToRun);

            }
            catch (Exception ex)
            {
                log.Error("Verificar Servicio SAT - Servicio", ex);
            }
        }
    }
}
