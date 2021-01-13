using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{
    [System.Xml.Serialization.XmlRoot("cfdi:Concepto")]
    public class Concepto
    {
         [XmlAttributeAttribute("cantidad")]
         public string cantidad { get; set; }

         [XmlAttributeAttribute("descripcion")]
         public string descripcion { get; set; }

         [XmlAttributeAttribute("importe")]
         public string importe { get; set; }

         [XmlAttributeAttribute("unidad")]
         public string unidad { get; set; }

         [XmlAttributeAttribute("valorUnitario")]
         public string valorUnitario { get; set; }
    }
}