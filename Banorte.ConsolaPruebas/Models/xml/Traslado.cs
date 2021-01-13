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
    }
}