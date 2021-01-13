using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Banorte.Persistencia;

namespace Banorte.Models
{
    public class AdminImpuesto
    {
          Conexion conexion = new Conexion();
        public Impuesto getIdImpuesto(string porcentajeImpuesto)
        {
            string strCommand = "select * from dbo.Impuesto where porcentaje= '" + porcentajeImpuesto + "'";
            DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
            Impuesto impuesto = null;
            if (ds != null)
            {
                //return ds.Tables[0].Rows[0]["id"].ToString(); ;
                impuesto = new Impuesto(Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString()), ds.Tables[0].Rows[0]["descripcion"].ToString(), ds.Tables[0].Rows[0]["porcentaje"].ToString());
                           
            }

           

            return impuesto; 
        }
    
    }
}