using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Banorte.Models.xml
{
    public class ComprobanteSerializer
    {
        public Comprobante Deserialize(String XmlString)
        {
            using (TextReader sr = new StringReader(XmlString))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Comprobante));
                Comprobante comprobante = (Comprobante)serializer.Deserialize(sr);
                return comprobante;
            }
        }
    }
}