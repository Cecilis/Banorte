using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Banorte.Persistencia;

namespace Banorte.Models
{
    public class AdminIndicadorImpuesto
    {
        Conexion conexion = new Conexion();
        public IndicadorImpuesto getIndicador(string centroCosto, string tasa)
        {
            //string strCommand = "select * from dbo.Indicador_Impuesto where id_departamento= '" + id_departamento + "' and id_impuesto= '" + id_impuesto + "'";
            string strCommand = "select * from dbo.Indicador_Impuesto  where id_departamento in (select id from dbo.Departamento where centroCosto = '"+ centroCosto+"') "+"and id_impuesto in (select id from dbo.Impuesto where porcentaje ="+ tasa+")";
            
            

            DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
            IndicadorImpuesto indicadorImpuesto = null;
            if (ds != null)
            {
                //return ds.Tables[0].Rows[0]["id"].ToString(); ;
                indicadorImpuesto = new IndicadorImpuesto((Int32.Parse(ds.Tables[0].Rows[0]["id_departamento"].ToString())), Int32.Parse(ds.Tables[0].Rows[0]["id_impuesto"].ToString()), ds.Tables[0].Rows[0]["indicador"].ToString());

            }

           

            return indicadorImpuesto;
        }
    }
}