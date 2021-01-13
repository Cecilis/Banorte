using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{

    [System.Xml.Serialization.XmlRoot("cfdi:RegimenFiscal")]
    public class RegimenFiscal
    {
        [XmlAttributeAttribute("Regimen")]
        public string Regimen { get; set; }
      
    }
}