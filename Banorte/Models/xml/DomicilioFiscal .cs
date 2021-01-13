using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{

    [System.Xml.Serialization.XmlRoot("cfdi:DomicilioFiscal")]
    public class DomicilioFiscal
    {
        [XmlAttributeAttribute("calle")]
        public string calle { get; set; }

        [XmlAttributeAttribute("codigoPostal")]
        public string codigoPostal { get; set; }

        [XmlAttributeAttribute("colonia")]
        public string colonia { get; set; }

        [XmlAttributeAttribute("estado")]
        public string estado { get; set; }

        [XmlAttributeAttribute("municipio")]
        public string municipio { get; set; }

        [XmlAttributeAttribute("pais")]
        public string pais { get; set; }



    }
}