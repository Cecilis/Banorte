using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Banorte.Models.xml
{
    [XmlRootAttribute(Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
    public class Complemento
    {
        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }
    }
}