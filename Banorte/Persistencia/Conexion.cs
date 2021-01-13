using System;
using System.Data;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;


namespace Banorte.Persistencia
{
    public class Conexion
    {
        string _strconnection;
        string _strNameConnection;
		SqlConnection _connection;

        public Conexion()
		{			
			GetConnectionString();
		}

		public void GetConect()
		{
			try
			{
				_connection = new SqlConnection(_strconnection);
                
				return;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		public void OpenConect()
		{
			try
			{
				GetConect();
				Connection.Open();
				return;
			}
			catch(Exception ex)
			{
				string k = ex.Message;
			}
		}


		public void CloseConect()
		{
			try
			{
				if(_connection.State==System.Data.ConnectionState.Open)
				{
					_connection.Close();
				}                
			}
			catch(Exception ex)
			{
                throw;
			}
		}

        public void CloseConect(SqlConnection Connection)
		{
			try
			{
				if(Connection.State==System.Data.ConnectionState.Open)
				{
					Connection.Close();
				}                
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		public DataSet GetDataSet(string strcommand, System.Data.CommandType commandtype)
	
		{
			try
			{
				OpenConect();
                System.Data.SqlClient.SqlCommand Command = new SqlCommand(strcommand);
				
                Command.Connection = Connection;
				Command.CommandText = strcommand;
				Command.CommandType = commandtype;              
		
				SqlDataAdapter DAdapter = new SqlDataAdapter();
                

				DAdapter.SelectCommand = Command;
				DataSet ds = new DataSet();
				DAdapter.Fill(ds, "OnlyTable");
				if (ds.Tables[0].Rows.Count > 0)
				{
					ds.Dispose();
					Command.Dispose();
					this.CloseConect();
					return ds;
				}
			}
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConect();
            }
			
			return null;
		}

		public int ExecuteCommand(string strcommand, System.Data.CommandType commandtype)
		{
			int val=0;
			try
			{
				 OpenConect();
				if(Connection!=null)
				{
                    System.Data.SqlClient.SqlCommand Command = new SqlCommand(strcommand);					
					Command.Connection = Connection;
					Command.CommandText = strcommand;
					Command.CommandType = commandtype;
					val = Command.ExecuteNonQuery();
					Command.Dispose();
					CloseConect();
				}
			}
			catch(Exception ex)
			{
				throw;
			} 
            finally
            {
                CloseConect();
            }
			
			return val;
		}
		
		
		
		
		
		
		private void GetConnectionString()
		{
			try
			{
				//NameValueCollection nvc = (NameValueCollection)
				//System.Configuration.ConfigurationSettings.GetConfig("intranetSectionGroup/database");
                //System.Configuration.ConfigurationManager.GetSection("intranetSectionGroup/database");
				//_strconnection = Convert.ToString(nvc[ "ConnectionString" ]);
				//_strconnection = valuestr_connection;
                _strNameConnection = "CuentasPagarBDConnection";
                //_strconnection = "data source=.;integrated security=SSPI;initial catalog=CuentasPagarBD4";
                _strconnection = ConfigurationManager.ConnectionStrings[_strNameConnection].ToString();
            }
            catch(Exception ex)
			{
				string h = ex.Message;				
				//_strconnection = System.Configuration.ConfigurationSettings.AppSettings.Get("strconnectionNpgsql");
			}
		}

		public int ReturnSecuential(string tabla, string campo)
		{
			try
			{
				DataSet dsSecuential = this.GetDataSet("select " + campo + " from " + tabla + " order by " + campo + " Desc", CommandType.Text);
				if(dsSecuential!=null)
				{
					if(dsSecuential.Tables[0].Rows.Count>0)
					{
						return Convert.ToInt32(dsSecuential.Tables[0].Rows[0][0].ToString()) + 1;
					}					
				}
			}
			catch(Exception ex)
			{
				string h=ex.Message;
				return 0;
			}
			return 1;
		}


        //CREATE LOGIN [IIS APPPOOL\banorte] FROM WINDOWS;
        //CREATE USER MyAppPoolUser FOR LOGIN [IIS APPPOOL\banorte];

        //USE master
        //GO
        //sp_grantlogin 'IIS APPPOOL\<AppPoolName>'

        //USE <yourdb>
        //GO
        //sp_grantdbaccess 'IIS APPPOOL\<AppPoolName>', '<AppPoolName>'
        //sp_addrolemember 'aspnet_Membership_FullAccess', '<AppPoolName>'
        //sp_addrolemember 'aspnet_Roles_FullAccess', '<AppPoolName>'



        //USE [CuentasPagarBD]
        //GO
        ///****** Object:  User [IIS APPPOOL\BanorteAppPool]    Script Date: 26/11/2017 12:07:22 a.m. ******/
        //CREATE USER [IIS APPPOOL\BanorteAppPool] FOR LOGIN [IIS APPPOOL\BanorteAppPool] WITH DEFAULT_SCHEMA=[dbo]
        //GO


		public int ReturnLastSerial(string tabla, string campo)
		{
			try
			{
				DataSet dsSecuential = this.GetDataSet("select " + campo + " from " + tabla + " order by " + campo + " Desc", CommandType.Text);
				if(dsSecuential!=null)
				{
					if(dsSecuential.Tables[0].Rows.Count>0)
					{
						return Convert.ToInt32(dsSecuential.Tables[0].Rows[0][0].ToString());
					}					
				}
			}
			catch(Exception ex)
			{
				string h=ex.Message;
				return 0;
			}
			return 1;
		}

		// Propiedades de la Clase

        public SqlConnection Connection
		{
			get{return _connection;}
			set{_connection=value;}
		}

	}   
}