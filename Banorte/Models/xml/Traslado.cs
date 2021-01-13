using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{

    [System.Xml.Serialization.XmlRoot("cfdi:Traslado")]
    public class Traslado
    {
        [XmlAttributeAttribute("importe")]
        public string importe { get; set; }

        [XmlAttributeAttribute("impuesto")]
        public string impuesto { get; set; }

        [XmlAttributeAttribute("tasa")]
        public string tasa { get; set; }

        //En Versión 3.3
        [XmlAttributeAttribute("Importe")]
        public string Importe { get; set; }

        [XmlAttributeAttribute("Impuesto")]
        public string Impuesto { get; set; }

        [XmlAttributeAttribute("TasaOCuota")]
        //public string TasaOCuota { get; set; }
        public double TasaOCuota { get; set; }

        [XmlAttributeAttribute("TipoFactor")]
        public string TipoFactor { get; set; } 

    }
}