using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{
    [System.Xml.Serialization.XmlRoot("cfdi:Impuestos")]
    public class Impuestos
    {
        [XmlAttributeAttribute("totalImpuestosTrasladados")]
        public string totalImpuestosTrasladados { get; set; }

        public Traslado[] Traslados { get; set; }
        
        
    }
}