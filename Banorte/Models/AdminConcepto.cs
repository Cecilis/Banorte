using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Banorte.Persistencia;

namespace Banorte.Models
{
    public class AdminConcepto
    {
        Conexion conexion = new Conexion();

        public AdminConcepto() { }
        public DataSet getConcepto()
        {
            try
            {
                string strCommand = "select descripcion,cuentaMayor from dbo.Concepto order by descripcion;";
                DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    { return ds; }
                }
            }
            catch (Exception ex)
            {
                string h = ex.Message;
            }
            return null;
        }

       
    }
}