using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class Recuperacion_contrasenaRepository
    {
        public int registroRecuperacion(int id_usuario, string codigo)
        {
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO RECUPERACION_CONTRASEÑA (id_usuario, codigo,fecha) " +
                             "VALUES (@id_usuario, @codigo, GETDATE())";

                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@codigo", codigo);

                    command.ExecuteNonQuery();
                }
                conexion.Disconnect();
                comando = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        return comando;
        }
        public Recuperacion_ContrasenaDto SeleccionarCodigo(int id_usuario)
        {
            DBContextUtility conexion = new DBContextUtility();
            Recuperacion_ContrasenaDto codigo = null;
            try
            {
                conexion.Connect();
                string SQL = "SELECT id_usuario,codigo FROM RECUPERACION_CONTRASENA WHERE (id_usuario = @id_usuario)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            codigo = new Recuperacion_ContrasenaDto
                            {
                                id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                codigo = reader["codigo"].ToString()
                            };
                            conexion.Disconnect();

                            return codigo;

                        }
                        else
                        {

                            return codigo;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                codigo = new Recuperacion_ContrasenaDto
                {

                    mensaje = "Error al traer la informacion: " + ex.Message
                };
            }
            finally
            {
                conexion.Disconnect();
            }
            return codigo;
        }
        public int EliminarCodigo(int id_usuario)
        {
            int filasAfectadas = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM RECUPERACION_CONTRASEÑA WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))

                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.ExecuteNonQuery();
                }
                filasAfectadas = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conexion.Disconnect();
            }

            return filasAfectadas;
        }
    }
}