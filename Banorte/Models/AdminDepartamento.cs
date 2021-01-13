using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Banorte.Persistencia;

namespace Banorte.Models
{
    public class AdminDepartamento
    {
        Conexion conexion = new Conexion();
        public DataSet getDepartamento()
        {
            try
            {
                string strCommand = "select centroCosto,descripcion from dbo.Departamento order by descripcion;";
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

                //var strConectionString = Utilities.ObtenerCadenaConexion("DBConnection");
                
                //SqlConnection oSqlConnection = new SqlConnection(strConectionString);
                //SqlCommand cmd = new SqlCommand("Select * from ConfiguracionActualizadorPOS where usuario=@nombre and password=@clave", oSqlConnection);
                //cmd.Parameters.AddWithValue("@nombre", Nombre);
                //cmd.Parameters.AddWithValue("@clave", Clave);
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);

                //DataTable oDataTable = new DataTable();
                //sda.Fill(oDataTable);
                //oSqlConnection.Open();
                //int i = cmd.ExecuteNonQuery();
                //oSqlConnection.Close();

                //Existe = oDataTable.Rows.Count > 0 ? true : false;