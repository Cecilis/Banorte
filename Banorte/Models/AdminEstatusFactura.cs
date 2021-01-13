using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Banorte.Persistencia;

namespace Banorte.Models
{
    public  class AdminEstatusFactura
    {
        Conexion conexion = new Conexion();

        public List<EstatusFactura> cargarEstatusFactura()
        {
            List<EstatusFactura> lstEstadosFactura = new List<EstatusFactura>();
            try
            {
                string strCommand = "select * from dbo.EstatusFactura";

                DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);

                if (ds != null)
                {
                    lstEstadosFactura = ds.Tables[0].AsEnumerable().Select(row => new EstatusFactura { Status = row["statu"] != null ? row["statu"].ToString() : "", TxtST = row["txtst"] != null ? row["txtst"].ToString() : "" }).ToList();
                }               
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al obtener estatus por factura", ex);
            }

            return lstEstadosFactura;
        }

    }
}