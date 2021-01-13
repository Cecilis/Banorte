using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;


namespace Banorte.Models.xml
{
    [System.Xml.Serialization.XmlRoot("cfdi:Emisor")]
    public class Emisor
    {

        [XmlAttributeAttribute("nombre")]
        public string nombre { get; set; }

        [XmlAttributeAttribute("rfc")]
        public string rfc { get; set; }

        public DomicilioFiscal DomicilioFiscal { get; set; }

        public ExpedidoEn ExpedidoEn { get; set; }

        public RegimenFiscal RegimenFiscal { get; set; }       

        
    }
}