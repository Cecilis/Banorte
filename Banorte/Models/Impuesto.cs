using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banorte.Models
{
    public class Impuesto
    {
        private int id;
        private string descripcion;
        private string porcentaje;

        public Impuesto(int id, string descripcion, string porcentaje)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.porcentaje = porcentaje;
        }

        public Impuesto()
        {

        }

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

        public void setPorcentaje(string porcentaje)
        {
            this.porcentaje = porcentaje;
        }

        public string getPorcentaje()
        {
            return this.porcentaje;
        }
    }
}