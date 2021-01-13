using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Banorte.Persistencia;
using System.IO;
using System.Xml.Serialization;
using Banorte.Utilities;
using System.Data.SqlClient;

namespace Banorte.Models
{
    public class AdminUsuario
    {
        Conexion conexion = new Conexion();
        public Usuario getUsuario(string login, string password)
        {
            Usuario usuario = null;
            string strCommandUpdate = string.Empty;
            string passwordBD = string.Empty;
            try
            {
                string strCommand = "select * from dbo.Usuario where login= '" + login + "'";
                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "get usuario"), ExceptionsManager.LOGLevel.INFO);

                DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);

                if (ds != null)
                {
                    bool esProveedor = true;
                    bool esSuperUsuario = false;
                    int bloqueado = 1;
                    int nroIntentos = 0;
                    bool cambiarClave = true;

                    var tabla = ds.Tables[0].Rows[0];

                    Boolean.TryParse((tabla["es_proveedor"].ToString()).ToLower(), out esProveedor);
                    Boolean.TryParse((tabla["esSuperUsuario"].ToString()).ToLower(), out esSuperUsuario);
                    Int32.TryParse((tabla["bloqueado"].ToString()).ToLower(), out bloqueado);
                    Int32.TryParse((tabla["nroIntentos"].ToString()).ToLower(), out nroIntentos);
                    Boolean.TryParse((tabla["cambiarClave"].ToString()).ToLower(), out cambiarClave);
                    passwordBD = ds.Tables[0].Rows[0]["password"].ToString();


                    //si esta bloqueado
                    if (bloqueado == 1)
                    {

                        usuario = new Usuario("", "", false, false, "", "", 6, false, 1, 0, true);
                    }
                    // no esta bloqueado
                    else if (bloqueado == 0)
                    {
                        if (password.Equals(passwordBD))
                        {
                            strCommandUpdate = "update dbo.Usuario set nroIntentos = 0 where login= '" + login + "'";
                            conexion.ExecuteCommand(strCommandUpdate, CommandType.Text);
                            Boolean.TryParse((ds.Tables[0].Rows[0]["es_proveedor"].ToString()).ToLower(), out esProveedor);
                            usuario = new Usuario(tabla["login"].ToString(), tabla["password"].ToString(), esProveedor, true, tabla["codigoProveedor"].ToString(), tabla["razonSocial"].ToString(), 1, esSuperUsuario, bloqueado, nroIntentos, cambiarClave);
                        }

                        //password incorrecto
                        //aqui es donde cuento los intentos fallidos
                        else
                        {
                            nroIntentos++;

                            if (nroIntentos > 2)
                            {
                                strCommandUpdate = "update dbo.Usuario set nroIntentos =" + nroIntentos + " ,bloqueado='1' where login= '" + login + "'";
                                conexion.ExecuteCommand(strCommandUpdate, CommandType.Text);

                                usuario = new Usuario("", "", false, false, "", "", 4, false, 1, 0, true);
                            }


                            else
                            {
                                strCommandUpdate = "update dbo.Usuario set nroIntentos =" + nroIntentos + "where login= '" + login + "'";
                                conexion.ExecuteCommand(strCommandUpdate, CommandType.Text);

                                if (nroIntentos == 2)
                                {

                                    usuario = new Usuario("", "", false, false, "", "", 5, false, 1, 0, true);
                                }
                                else
                                {
                                    usuario = new Usuario("", "", false, false, "", "", 3, false, 1, 0, true);
                                }
                            }


                        }

                    }

                }



                    /**/



                //usuario no encontrado
                else
                {
                    usuario = new Usuario("", "", false, false, "", "", 2, false, 1, 0, true);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al obtener data de usuario", ex);
            }

            return usuario;
        }

        public DataSet getUsuarioTodos(string CampoOrden = "login", string SentidoOrden = "DESC")
        {
            string strCommand = "select * from dbo.Usuario order by " + CampoOrden + " " + SentidoOrden;
            ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "get usuario"), ExceptionsManager.LOGLevel.INFO);
            DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
            return ds;
        }

        public DataSet getUsuarioTodos()
        {
            string strCommand = "select * from dbo.Usuario";
            ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "get usuario"), ExceptionsManager.LOGLevel.INFO);
            DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
            return ds;
        }


        public Usuario getUsuarioPorId(int UsuarioId)
        {
            Usuario usuario;
            try
            {
                string strCommand = "select * from dbo.Usuario where id= '" + UsuarioId + "'";
                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "obtener usuario por id"), ExceptionsManager.LOGLevel.INFO);

                DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
                if (ds != null)
                {
                    bool esProveedor = true;
                    bool esSuperUsuario = false;
                    int bloqueado = 1;
                    int nroIntentos = 0;
                    bool cambiarClave = true;

                    var tabla = ds.Tables[0].Rows[0];

                    Boolean.TryParse((tabla["es_proveedor"].ToString()).ToLower(), out esProveedor);
                    Boolean.TryParse((tabla["esSuperUsuario"].ToString()).ToLower(), out esSuperUsuario);
                    Int32.TryParse((tabla["bloqueado"].ToString()).ToLower(), out bloqueado);
                    Int32.TryParse((tabla["nroIntentos"].ToString()).ToLower(), out nroIntentos);
                    Boolean.TryParse((tabla["cambiarClave"].ToString()).ToLower(), out cambiarClave);

                    usuario = new Usuario(tabla["login"].ToString(), tabla["password"].ToString(), esProveedor, true, tabla["codigoProveedor"].ToString(), tabla["razonSocial"].ToString(), 1, esSuperUsuario, bloqueado, nroIntentos, cambiarClave);
                    usuario.Id = UsuarioId;
                }

                else
                {
                    usuario = new Usuario("", "", false, false, "", "", 2, false, 1, 0, true);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al obtener data de usuario", ex);
            }

            return usuario;
        }

        public bool incluirUsuario(string login, string contraseña, bool esProveedor, string codigoProveedor, string razonSocial, bool esSuperUsuario, int bloqueado, bool cambiarClave, int nroIntentos)
        {
            bool usuarioAgregado = false;
            try
            {
                string strCommand = "insert into dbo.Usuario (login, password, es_proveedor, codigoProveedor, razonSocial, esSuperUsuario, bloqueado, nroIntentos, cambiarClave) values ('" + login + "','" + contraseña + "', '" + esProveedor + "','" + codigoProveedor + "', '" + razonSocial + "', '" + esSuperUsuario + "', '" + bloqueado + "',  '" + nroIntentos + "', '" + cambiarClave + "') ";

                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "update usuario"), ExceptionsManager.LOGLevel.INFO);

                int resp = conexion.ExecuteCommand(strCommand, CommandType.Text);

                usuarioAgregado = resp > 0 ? true : false;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UNIQUE")){
                        throw new CustomException("Existe otro usuario con ese login", ex);
                    }
                    else {
                        throw new CustomException("Ha ocurrido un error al intentar crear el usuario, intente nuevamente.", ex);
                    }
                }
                else {
                    throw new CustomException("Ha ocurrido un error al intentar crear el usuario", ex);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al crear usuario", ex);
            }

            return usuarioAgregado;
        }

        public bool actualizarUsuario(int id, string login, string contraseña, bool esProveedor, string codigoProveedor, string razonSocial, bool esSuperUsuario, int bloqueado, bool cambiarClave, int nroIntentos)
        {
            bool usuarioActualizado = false;
            try
            {
                string strCommand = "update dbo.Usuario set login = '" + login + "', password = '" + contraseña + "', es_proveedor = '" + esProveedor + "', codigoProveedor = '" + codigoProveedor + "', razonSocial = '" + razonSocial + "', esSuperUsuario = '" + esSuperUsuario + "', bloqueado = '" + bloqueado + "', cambiarClave  = '" + cambiarClave + "', nroIntentos = '" + nroIntentos + "' where id = '" + id + "'";

                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "update usuario"), ExceptionsManager.LOGLevel.INFO);

                int resp = conexion.ExecuteCommand(strCommand, CommandType.Text);

                usuarioActualizado = resp > 0 ? true : false;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        throw new CustomException("Existe otro usuario con ese login", ex);
                    }
                    else
                    {
                        throw new CustomException("Ha ocurrido un error al intentar crear el usuario, intente nuevamente.", ex);
                    }
                }
                else
                {
                    throw new CustomException("Ha ocurrido un error al intentar crear el usuario", ex);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al obtener data de usuario", ex);
            }

            return usuarioActualizado;
        }

        /*Metodo que no guarda la contraseña*/
        public bool actualizarUsuario(int id, string login,  bool esProveedor, string codigoProveedor, string razonSocial, bool esSuperUsuario, int bloqueado, bool cambiarClave, int nroIntentos)
        {
            bool usuarioActualizado = false;
            try
            {
                string strCommand = "update dbo.Usuario set login = '" + login + "', es_proveedor = '" + esProveedor + "', codigoProveedor = '" + codigoProveedor + "', razonSocial = '" + razonSocial + "', esSuperUsuario = '" + esSuperUsuario + "', bloqueado = '" + bloqueado + "', cambiarClave  = '" + cambiarClave + "', nroIntentos = '" + nroIntentos + "' where id = '" + id + "'";

                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "update usuario"), ExceptionsManager.LOGLevel.INFO);

                int resp = conexion.ExecuteCommand(strCommand, CommandType.Text);

                usuarioActualizado = resp > 0 ? true : false;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        throw new CustomException("Existe otro usuario con ese login", ex);
                    }
                    else
                    {
                        throw new CustomException("Ha ocurrido un error al intentar crear el usuario, intente nuevamente.", ex);
                    }
                }
                else
                {
                    throw new CustomException("Ha ocurrido un error al intentar crear el usuario", ex);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al obtener data de usuario", ex);
            }

            return usuarioActualizado;
        }

        public bool actualizarEstatusBloqueoUsuario(string id, bool estatusBloqueo)
        {
            int estatus = 0;
            bool estatusBloqueoActualizado = false;
            try
            {
                estatus = estatusBloqueo ? 1 : 0;
                string strCommand = "update dbo.Usuario set(statusBloqueo) values ('" +  estatus + "')";
                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "update usuario"), ExceptionsManager.LOGLevel.INFO);

                int resp = conexion.ExecuteCommand(strCommand, CommandType.Text);
                estatusBloqueoActualizado = resp > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al actualizar estatus de bloqueo de usuario", ex);
            }

            return estatusBloqueoActualizado;
        }

        public bool actualizarPasswordUsuario(string UsuarioId, string contraseña)
        {
            int estatus = 0;
            bool estatusCambioContraseña = false;
            try
            {
                string strCommand = "update dbo.Usuario set(password) values ('" + contraseña + "') where ID = '" + UsuarioId + "'";
                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "update usuario"), ExceptionsManager.LOGLevel.INFO);

                int resp = conexion.ExecuteCommand(strCommand, CommandType.Text);
                estatusCambioContraseña = resp > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al actualizar contraseña de usuario", ex);
            }

            return estatusCambioContraseña;
        }


        public bool verificarContrasena(string login, string password)
        {
            bool existeUsuario = false;
            try
            {
                string strCommand = "select count(login) as existeUsuario from dbo.Usuario where login= '" + login + "' and password= '" + password + "'";
                ExceptionsManager.LogRegister(ExceptionsManager.Message("strCommand " + strCommand, "get verificar contrasena"), ExceptionsManager.LOGLevel.INFO);

                DataSet ds = conexion.GetDataSet(strCommand, CommandType.Text);
                existeUsuario = (Int32.Parse(ds.Tables[0].Rows[0]["existeUsuario"].ToString()) == 1);


            }
            catch (Exception ex)
            {
                throw new CustomException("Error al verificar la contrasena del usuario", ex);
            }
            return existeUsuario;
        }

        public bool modificarContrasena(string login, string password)
        {
            int respUpdate = 0;
            bool respuesta = false;
            try
            {
                string strCommandUpdate = "update dbo.Usuario set password =" + password + ",cambiarClave = 0  where login= '" + login + "'";
                respUpdate = conexion.ExecuteCommand(strCommandUpdate, CommandType.Text);
                respuesta = (respUpdate == 1);
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al verificar la modificar contrasena del usuario", ex);
            }

            return respuesta;
        }

        public string serialize(Usuario usuario)
        {
            string serializedData = string.Empty;
            try
            {
                XmlSerializer serializer = new XmlSerializer(usuario.GetType());
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, usuario);
                    serializedData = sw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return serializedData;
        }

        public Usuario deserialize(string serializedData)
        {
            Usuario deserializado = new Usuario();
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Usuario));
                using (TextReader tr = new StringReader(serializedData))
                {
                    deserializado = (Usuario)deserializer.Deserialize(tr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return deserializado;
        }



    }
}