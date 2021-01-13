using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banorte.Models
{
    public class Usuario
    {
        private int id;
        private string login;
        private string password;
        private bool existe;
        private bool esProveedor;
        private string codigoProveedor;
        private string razonSocial;
        private int codigoMensaje;
        private bool esSuperUsuario;
        private int bloqueado;
        private int nroIntentos;
        private bool cambiarClave;




        public Usuario(string login, string password, bool esProveedor, bool existe, string codigoProveedor, string razonSocial, int codigoMensaje, bool esSuperUsuario, int bloqueado, int nroIntentos, bool cambiarClave)
        {
            this.login = login;
            this.password = password;
            this.esProveedor = esProveedor;
            this.existe = existe;
            this.codigoProveedor = codigoProveedor;
            this.razonSocial = razonSocial;
            this.codigoMensaje = codigoMensaje;
            this.esSuperUsuario = esSuperUsuario;
            this.bloqueado = bloqueado;
            this.nroIntentos = nroIntentos;
            this.cambiarClave = cambiarClave;

        }

        public Usuario()
        {

        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        /*public void setPassword(string password)
        {
            this.password = password;
        }

        public string getPassword()
        {
            return this.password;
        }*/

        public void setExiste(Boolean existe)
        {
            this.existe = existe;
        }

        public bool EsProveedor
        {
            get { return esProveedor; }
            set { esProveedor = value; }
        }


        public bool getExiste()
        {
            return this.existe;
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

        public void setCodigoMensaje(int codigoMensaje)
        {
            this.codigoMensaje = codigoMensaje;
        }

        public int getCodigoMensaje()
        {
            return this.codigoMensaje;
        }
        public bool EsSuperUsuario
        {
            get { return esSuperUsuario; }
            set { esSuperUsuario = value; }
        }
        public int Bloqueado
        {
            get { return bloqueado; }
            set { bloqueado = value; }
        }

        public int NroIntentos
        {
            get { return nroIntentos; }
            set { nroIntentos = value; }
        }

        public bool CambiarClave
        {
            get { return cambiarClave; }
            set { cambiarClave = value; }
        }
    }
}