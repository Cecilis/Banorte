using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banorte.Models
{
    public class Departamento
    {
        private int id;
        private string descripcion;
        private string centroCosto;


        public void setId(int id)
        {
            this.id = id;
        }

        public int getId()
        {
            return this.id;
        }

        public void setDescripcion(string descripcion)
        {
            this.descripcion = descripcion;
        }

        public string getDescripcion()
        {
            return this.descripcion;
        }

        public void setCentroCosto(string centroCosto)
        {
            this.centroCosto = centroCosto;
        }

        public string getCentroCosto()
        {
            return this.centroCosto;

        }
    }
}