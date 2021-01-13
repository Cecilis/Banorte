using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banorte.Models
{
    public class Proveedor
    {

        private string codigoProveedor;
        private string razonSocial;
        private bool existe;

        public Proveedor(string codigoProveedor, String razonSocial, bool existe)
        {
            this.codigoProveedor = codigoProveedor;
            this.razonSocial = razonSocial;
            this.existe = existe;
        }
        
        public string CodigoProveedor
        {
            get { return codigoProveedor; }
            set { codigoProveedor = value; }
        }

        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }
        

        public bool Existe
        {
            get { return existe; }
            set { existe = value; }
        }
    }
}