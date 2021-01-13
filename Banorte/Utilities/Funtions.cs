using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Banorte.Models;

namespace Banorte.Utilities
{
    public class Functions
    {

        internal static string GetConfigurationValue(string value)
        {
            try{
                return System.Configuration.ConfigurationManager.AppSettings[value];
            }
            catch (Exception ex){
                return ex.Message.ToString();
            }
        }

        internal static string GetUniqueFileName(string prefix = "")
        {
            try
            {
                var currentDate = DateTime.Now;
                string uno = "1".ToString().PadRight(2, '0');
                string dos = "2".ToString().PadLeft(2, '0');

                var fileName = prefix + currentDate.Year.ToString() + currentDate.Month.ToString().PadLeft(2) + currentDate.Day.ToString().PadLeft(2) + currentDate.Hour.ToString().PadLeft(2) + currentDate.Minute.ToString().PadLeft(2) + currentDate.Second.ToString().PadLeft(2);
                return fileName;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        internal static bool PathExists(string strLocalPath, bool create = true)
        {
            try
            {                
                if (!Directory.Exists(strLocalPath))
                {
                    if (create){
                        Directory.CreateDirectory(strLocalPath);
                        return true;
                    }
                    else return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static string GetDateStringFromYYYY_MM_DD(string dateToFormat, bool onErrorSetToDefault = false)
        {
            string dateFormatted = string.Empty;
            try
            {
                var arrFecha = dateToFormat.Split('-');
                return arrFecha[2] + "/" + arrFecha[1] +  "/" + arrFecha[0];

            }
            catch (Exception ex)
            {
                dateFormatted = onErrorSetToDefault ? String.Format("{0, dd/MM/yyyy}", DateTime.Now.ToString()) : "00/00/0000";
            }
            
            return dateFormatted;
            
        }



        internal static string GetLogFilePath()
        {
            string strLogFilePath = string.Empty;

            try
            { 
                string strHoy = DateTime.Today.ToString("yyyyMMdd");
                string strLOGFilePrefix = Configuracion.ObtenerAppSettings("LOGFilePrefix");
                string strPDFFileServerPath = Configuracion.ObtenerAppSettings("LOGFileServerPath");
                string strLogFileName = String.Format("{0}{1}.{2}", strLOGFilePrefix, strHoy, "log");

                strLogFilePath =  Path.Combine(@strPDFFileServerPath, strLogFileName);
            }
            catch(Exception ex){
                strLogFilePath = "C:/Banorte/VerificarFacturas/Log/Banorte.log";
            }
            return strLogFilePath;
        }

        internal static string GetLogFileName()
        {
            string strLogFilePath = string.Empty;

            try
            {
                string strHoy = DateTime.Today.ToString("yyyyMMdd");
                string strLOGFilePrefix = Configuracion.ObtenerAppSettings("LOGFilePrefix");
                string strPDFFileServerPath = Configuracion.ObtenerAppSettings("LOGFileServerPath");
                string strLogFileName = String.Format("{0}{1}.{2}", strLOGFilePrefix, strHoy, "log");

            }
            catch (Exception ex)
            {
                strLogFilePath = "";
            }
            return strLogFilePath;
        }


        internal static string GetCurrentPageName(string strURLPath)
        {
            string pageName = "ErrorGettingCurrentPage";
            try
            {
                FileInfo fileInfo = new FileInfo(strURLPath);
                pageName = fileInfo.Name;
                return pageName;
            }
            catch (Exception ex)
            {
                return pageName;
            }

        }


        internal static void SetResultLabelContent(HtmlGenericControl labelResult, string strCSSClass, string strMenssage)
        {
            labelResult.Attributes.CssStyle.Clear();
            labelResult.Attributes.Add("class", strCSSClass);
            labelResult.InnerText = strMenssage;
        }

        internal static void SetResultLabelContent(Label labelResult, string strCSSClass, string strMenssage)
        {
            labelResult.Attributes.CssStyle.Clear();
            labelResult.Attributes.Add("class", strCSSClass);
            labelResult.Text = strMenssage;
        }

    }
}