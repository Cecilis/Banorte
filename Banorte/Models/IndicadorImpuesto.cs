using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banorte.Models
{
    public class IndicadorImpuesto
    {
        private int idDepartamento;
        private int idImpuesto;
        private string indicador;

        public IndicadorImpuesto(int idDepartamento, int idImpuesto, string indicador)
        {
            this.idDepartamento = idDepartamento;
            this.idImpuesto = idImpuesto;
            this.indicador = indicador;
        }

           public IndicadorImpuesto()
        {

        }

        public void setIdDepartamento(int idDepartamento)
        {
            this.idDepartamento = idDepartamento;
        }

        public int getIdDepartamento()
        {
            return this.idDepartamento;
        }

        public void setIdImpuesto(int idImpuesto)
        {
            this.idImpuesto = idImpuesto;
        }

        public int getIdImpuesto()
        {
            return this.idImpuesto;
        }

        public void setIndicador(string indicador)
        {
            this.indicador = indicador;
        }

        public string getIndicador()
        {
            return this.indicador;
        }
    }
}