using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class EjercicioRepository
    {
        public int registroEjercicio(EjercicioDto ejercicio)
        {
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO EJERCICIO (nombre_ejercicio, id_grupo_muscular,apoyo_visual)"
                            + "VALUES (@nombre_ejercicio, @id_grupo_muscular, @apoyo_visual)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
           
                    command.Parameters.AddWithValue("@nombre_ejercicio", ejercicio.nombre_ejercicio);
                    command.Parameters.AddWithValue("@id_grupo_muscular", ejercicio.id_grupo_muscular);
                    command.Parameters.AddWithValue("@apoyo_visual", ejercicio.id_grupo_muscular);
                  
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
        public List<EjercicioDto> MostrarEjercicio()
        {
            List<EjercicioDto> Ejercicios = new List<EjercicioDto>();
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();

            string SQL = "SELECT  nombre_ejercicio, id_grupo_muscular,apoyo_visual FROM EJERCICIO";

            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EjercicioDto ejercicio = new EjercicioDto();
                        {

                            ejercicio.nombre_ejercicio = reader["nombre_ejercicio"].ToString();
                            ejercicio.id_grupo_muscular = Convert.ToInt32(reader["id_grupo_muscular"]);
                            ejercicio.apoyo_visual = reader["apoyo_visual"].ToString();

                        }

                        Ejercicios.Add(ejercicio);
                    }
                }
            }
            conexion.Disconnect();
            return Ejercicios;
        }
        public int EliminarEjercicio(string nombre_ejercicio)
        {
            int filasAfectadas = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM EJERCICIO WHERE nombre_ejercicio = @nombre_ejercicio";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))

                {
                    command.Parameters.AddWithValue("@nombre_ejercicio", nombre_ejercicio);
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
        public int ActualizarEjercicio(EjercicioDto ejercicio)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();
                string SQL = "UPDATE EJERCICIO SET nombre_ejercicio = @nombre_ejercicio, apoyo_visual = @apoyo_visual  " + "WHERE nombre_ejercicio = @nombre_ejercicio";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@nombre_ejercicio", ejercicio.nombre_ejercicio);
                   // command.Parameters.AddWithValue("@id_grupo_muscular", ejercicio.id_grupo_muscular);
                    command.Parameters.AddWithValue("@apoyo_visual", ejercicio.apoyo_visual);
                    command.ExecuteNonQuery();
                }
                comando = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conexion.Disconnect();
            }



            return comando;
        }
        public bool buscarEjercicio(string nombre_ejercicio)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM EJERCICIO WHERE nombre_ejercicio = @nombre_ejercicio";
            int ejercicioEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@nombre_ejercicio",nombre_ejercicio);
                ejercicioEncontrado = (int)command.ExecuteScalar();
            }
            conexion.Disconnect();
            if (ejercicioEncontrado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
