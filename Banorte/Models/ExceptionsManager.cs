using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace Banorte.Models
{
    public sealed class ExceptionsManager
    {
        private static readonly ILog appLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //ALL    DEBUG   INFO    WARN    ERROR   FATAL   OFF
        //•All                        
        //•DEBUG  •DEBUG                  
        //•INFO   •INFO   •INFO               
        //•WARN   •WARN   •WARN   •WARN           
        //•ERROR  •ERROR  •ERROR  •ERROR  •ERROR      
        //•FATAL  •FATAL  •FATAL  •FATAL  •FATAL  •FATAL  
        //•OFF    •OFF    •OFF    •OFF    •OFF    •OFF    •OFF

        public enum LOGLevel
        {
            DEBUG,
            INFO,
            ERROR,
            WARN,
            FATAL
        };


        public enum EXCEPTIONTypes
        {
            ArgumentException,
            InvalidOperationException
        }


        //No quitar
        private ExceptionsManager()
        { }

        public static string GetLogFileName()
        {
            var rootAppender = LogManager.GetRepository()
                                         .GetAppenders()
                                         .OfType<FileAppender>()
                                         .FirstOrDefault();
                                         //.FirstOrDefault(currentFileAppender => currentFileAppender.Name == name);

            return rootAppender != null ? rootAppender.File : string.Empty;
        }


        public static void SetLogFileName(string strFileName)
        {
            var rootAppender = LogManager.GetRepository()
                                         .GetAppenders()
                                         .OfType<FileAppender>()
                                         .FirstOrDefault();

            rootAppender.File = strFileName;
        }


        // Guarda el Log de la excepción
        public static void LogRegister(string errorMensage, LOGLevel loglevel)
        {

            bool bLogRegistrado = true;

            if (Enum.IsDefined(typeof(LOGLevel), loglevel))
            {
                switch (loglevel)
                {
                    case LOGLevel.DEBUG:
                        if (appLog.IsDebugEnabled)
                            appLog.Debug(errorMensage);
                        else
                            bLogRegistrado = false;
                        break;
                    case LOGLevel.INFO:
                        if (appLog.IsInfoEnabled)
                            appLog.Info(errorMensage);
                        else
                            bLogRegistrado = false;
                        break;
                    case LOGLevel.WARN:
                        if (appLog.IsWarnEnabled)
                            appLog.Warn(errorMensage);
                        else
                            bLogRegistrado = false;
                        break;
                    case LOGLevel.FATAL:
                        if (appLog.IsFatalEnabled)
                            appLog.Fatal(errorMensage);
                        else
                            bLogRegistrado = false;
                        break;
                    case LOGLevel.ERROR:
                        if (appLog.IsErrorEnabled)
                            appLog.Error(errorMensage);
                        else
                            bLogRegistrado = false;
                        break;
                    default:
                        bLogRegistrado = false;
                        break;
                }
            }
            else if (appLog.IsErrorEnabled)
                appLog.Error(errorMensage);
            else
                bLogRegistrado = false;

            //TODO: Decidir que hacer si no se puede cargar el log....
            //lock ("Grabar")
            //{

            //}
        } 

        public static string Message(Exception oException, string strOrigen)
        {
            string mensaje = string.Empty;

            if ((oException != null) && (oException.InnerException != null))
            {
                mensaje = "Exception Type: " + oException.InnerException.GetType().ToString() + " ";
                mensaje += "Inner Exception: " + oException.InnerException.Message.ToString() + " ";
                mensaje += "Inner Source: " + oException.InnerException.Source.ToString() + " ";
                if (oException.InnerException.StackTrace != null)
                {
                    mensaje += "Inner Stack Trace: " + oException.InnerException.StackTrace.ToString() + " ";
                }
            }
            else{
                mensaje = "Exception Type: " + oException.GetType().ToString() + " ";
                mensaje += "Exception: " + oException.Message.ToString() + " ";
                mensaje += "Source: " + strOrigen;
                if (oException.StackTrace != null)
                {
                    mensaje += "Stack Trace: " + oException.StackTrace + " ";
                }
            }
            return mensaje;
        }

        public static string Message(string strException, string strOrigen){
            string mensaje = string.Empty;
            mensaje += "Exception: " + strException + " ";
            mensaje += "Source: " + strOrigen;
            return mensaje;
        }

    }
}