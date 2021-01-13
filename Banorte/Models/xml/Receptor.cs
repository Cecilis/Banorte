﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{
     [System.Xml.Serialization.XmlRoot("cfdi:Receptor")]
    public class Receptor
    {

        [XmlAttributeAttribute("nombre")]
        public string nombre { get; set; }

        [XmlAttributeAttribute("rfc")]
        public string rfc { get; set; }

        public Domicilio Domicilio { get; set; }

         //Version 3.3
        [XmlAttributeAttribute("Rfc")]
        public string Rfc { get; set; }
    }
}