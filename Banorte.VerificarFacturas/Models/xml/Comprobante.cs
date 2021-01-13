using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{
    //[System.Xml.Serialization.XmlRoot("cfdi:Comprobante")]
    [XmlRootAttribute(Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Comprobante
    {
        [XmlAttributeAttribute("LugarExpedicion")]
        public string lugarExpedicion { get; set; }

        [XmlAttributeAttribute("Moneda")]
        public string moneda { get; set; }

        [XmlAttributeAttribute("NumCtaPago")]
        public string numCtaPago { get; set; }

        [XmlAttributeAttribute("TipoCambio")]
        public string tipoCambio { get; set; }

        [XmlAttributeAttribute("certificado")]
        public string certificado { get; set; }

        [XmlAttributeAttribute("condicionesDePago")]
        public string condicionesDePago { get; set; }

        [XmlAttributeAttribute("fecha")]
        public string fecha { get; set; }

        [XmlAttributeAttribute("folio")]
        public string folio { get; set; }

        [XmlAttributeAttribute("formaDePago")]
        public string formaDePago { get; set; }

        [XmlAttributeAttribute("metodoDePago")]
        public string metodoDePago { get; set; }

        [XmlAttributeAttribute("noCertificado")]
        public string noCertificado { get; set; }

        [XmlAttributeAttribute("sello")]
        public string sello { get; set; }

        [XmlAttributeAttribute("serie")]
        public string serie { get; set; }

        [XmlAttributeAttribute("subTotal")]
        public string subTotal { get; set; }

        [XmlAttributeAttribute("tipoDeComprobante")]
        public string tipoDeComprobante { get; set; }

        [XmlAttributeAttribute("total")]
        public string total { get; set; }

        [XmlAttributeAttribute("version")]
        public string version { get; set; }

        
        public Emisor Emisor { get; set; }

        public Receptor Receptor { get; set; }

        public Concepto[] Conceptos { get; set; }

        public Impuestos Impuestos { get; set; }

        public Complemento Complemento { get; set; }

        private string expresionImpresa;

        public void setExpresionImpresa(string expresionImpresa)
        {
            this.expresionImpresa = expresionImpresa;
        }

        public string getExpresionImpresa()
        {
            string expresionImpresa = "?re=" +this.Emisor.rfc + "&rr=" + this.Receptor.rfc + "&tt=" +this.total + "&id=" + this.Complemento.TimbreFiscalDigital.UUID;
            return expresionImpresa;
        }
    }


}