using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{
    [System.Xml.Serialization.XmlRoot("tfd:TimbreFiscalDigital")]
    public class TimbreFiscalDigital
    {
        [XmlAttributeAttribute("FechaTimbrado")]
        public string FechaTimbrado { get; set; }

        [XmlAttributeAttribute("UUID")]
        public string UUID { get; set; }

        [XmlAttributeAttribute("noCertificadoSAT")]
        public string noCertificadoSAT { get; set; }

        [XmlAttributeAttribute("selloCFD")]
        public string selloCFD { get; set; }

        [XmlAttributeAttribute("version")]
        public string version { get; set; }
    }
}