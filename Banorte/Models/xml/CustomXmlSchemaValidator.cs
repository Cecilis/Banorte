using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections.Generic;

namespace Banorte.Models.xml
{
    /// <SUMMARY>
    /// This class validates an xml string or xml document against an xml
    /// schema.
    /// It has public methods that return oFileAppender boolean value depending on 
    /// the validation
    /// of the xml.
    /// </SUMMARY>
    public class CustomXmlSchemaValidator
    {
        private bool isValidXml = false;
        private bool isValidXSD = false;
        private string validationError = "";
        private bool xsdEncontrado = false;

        public List<string> ListaErroresArchivoXML
        {
            get { return CustomXmlSchemaValidator.listaErroresArchivoXML; }
            set { CustomXmlSchemaValidator.listaErroresArchivoXML = value; }
        }

        public enum VersionXSD
        {
            v32,
            v33
        }

        /// <SUMMARY>
        /// Empty Constructor.
        /// </SUMMARY>
        public CustomXmlSchemaValidator()
        {
        }

        /// <SUMMARY>
        /// Public get/set access to the validation error.
        /// </SUMMARY>
        public string ValidationError
        {
            get
            {
                return "<VALIDATIONERROR>" + this.validationError
                       + "</VALIDATIONERROR>";
            }
            set
            {
                this.validationError = value;
            }
        }

        /// <SUMMARY>
        /// Public get access to the isValidXml attribute.
        /// </SUMMARY>

        public bool IsValidXML
        {
            get { return isValidXml; }
            set { isValidXml = value; }
        }

        private static List<string> listaErroresArchivoXML = new List<string>();
        public bool IsValidXSD
        {
            get { return isValidXSD; }
            set { isValidXSD = value; }
        }

        /// <SUMMARY>
        /// This method is invoked when the XML does not match
        /// the XML Schema.
        /// </SUMMARY>
        /// <PARAM name="sender"></PARAM>
        /// <PARAM name="args"></PARAM>
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            // The xml does not match the schema.
            isValidXml = false;
            this.ValidationError = args.Message;
        }


        /// <SUMMARY>
        /// This method validates an xml string against an xml schema.
        /// </SUMMARY>
        /// <PARAM name="xml">XML string</PARAM>
        /// <PARAM name="nsSchemaComprobante">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema Uri</PARAM>
        /// <RETURNS>bool</RETURNS>
        public bool ValidXmlDoc(string xml, string schemaNamespace, string schemaUri)
        {
            try
            {
                if ((string.IsNullOrEmpty(xml.Trim())) || (xml.Trim().Length < 1))
                {
                    return false;
                }

                StringReader srXml = new StringReader(xml);
                return ValidXmlDoc(srXml, schemaNamespace, schemaUri);
            }
            catch (Exception ex)
            {
                this.isValidXml = false;
                this.ValidationError += ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <SUMMARY>
        /// This method validates an xml document against an xml 
        /// schema.
        public bool ValidXmlDoc(XmlDocument xml, string schemaNamespace, string schemaUri)
        {
            try
            {
                if (xml == null)
                {
                    return false;
                }

                // Create oFileAppender new string writer.
                StringWriter sw = new StringWriter();
                // Set the string writer as the text writer 
                // to write to.
                XmlTextWriter xw = new XmlTextWriter(sw);
                // Write to the text writer.
                xml.WriteTo(xw);
                // Get 
                string strXml = sw.ToString();

                StringReader srXml = new StringReader(strXml);

                return ValidXmlDoc(srXml, schemaNamespace, schemaUri);
            }
            catch (Exception ex)
            {
                this.ValidationError = ex.Message;
                return false;
            }
        }

        /// <SUMMARY>
        /// This method validates an xml string against an xml schema.
        /// </SUMMARY>
        /// <PARAM name="xml">StringReader containing xml</PARAM>
        /// <PARAM name="nsSchemaComprobante">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema Uri</PARAM>
        /// <RETURNS>bool</RETURNS>
        public bool ValidXmlDoc(StringReader xml, string schemaNamespace, string schemaUri)
        {
            // Continue?
            if (xml == null || schemaNamespace == null || schemaUri == null)
            {
                return false;
            }

            isValidXml = true;
            XmlValidatingReader vr;
            XmlTextReader tr;
            XmlSchemaCollection schemaCol = new XmlSchemaCollection();
            schemaCol.Add(schemaNamespace, schemaUri);

            try
            {
                // Read the xml.
                tr = new XmlTextReader(xml);
                // Create the validator.
                vr = new XmlValidatingReader(tr);
                // Set the validation tyep.
                vr.ValidationType = ValidationType.Schema;
                // Add the schema.
                if (schemaCol != null)
                {
                    vr.Schemas.Add(schemaCol);
                }
                // Set the validation event handler.
                vr.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                // Read the xml schema.
                while (vr.Read()) {}

                vr.Close();

                return isValidXml;
            }
            catch (Exception ex)
            {
                this.ValidationError = ex.Message;
                return false;
            }
            finally
            {
                vr = null;
                tr = null;
            }
        }


    }
}