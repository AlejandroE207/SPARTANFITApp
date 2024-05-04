using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class RutinaRepository
    {

        public int regitrarRutina(RutinaDto rutina)
        {
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO RUTINA (id_nivel_rutina, nombre_rutina,dia,descripcion)"
                            + "VALUES (@id_nivel_rutina, @nombre_rutina, @dia, @descripcion)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {

                    command.Parameters.AddWithValue("@id_nivel_rutina", rutina.id_nivel_rutina);
                    command.Parameters.AddWithValue("@nombre_rutina", rutina.nombre_rutina);
                    command.Parameters.AddWithValue("@dia", rutina.dia);
                    command.Parameters.AddWithValue("@descripcion", rutina.descripcion);


                    command.ExecuteNonQuery();
                    comando = 1;
                }
                conexion.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            RutinaDto rutinaResp = new RutinaDto();
            rutinaResp = obtenerUltimaRutina();
            if(rutinaResp.respuesta != 0)
            {
                comando = rutinaResp.id_rutina;
            }
            else
            {
                comando = -1;
            }


            return comando;
        }

        public RutinaDto obtenerUltimaRutina()
        {
            RutinaDto rutinaResp = new RutinaDto();
            RutinaDto rutina = new RutinaDto();
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "SELECT MAX(id_rutina) as id_max FROM RUTINA";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rutina.id_rutina = Convert.ToInt32(reader["id_max"]);
                            rutina.respuesta = 1;
                            rutina.mensaje = "Obtencion con exito";
                            return rutina;
                        }
                        else
                        {
                            rutina.respuesta = 0;
                            rutina.mensaje = "Error al momento de obtener id";
                            return rutina;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                rutina.respuesta = -1;
                rutina.mensaje = "Error al obtener id: " + ex.Message;

            }
            finally
            {
                conexion.Disconnect();
            }
            return rutina;
        }

        public int registrarEjerciciosRutina(List<EjercicioDto> ejerciciosRutina, int id_rutina)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                
                conexion.Connect();

                foreach (EjercicioDto ejercicio in ejerciciosRutina)
                {
                    string sql = "INSERT INTO RUTINA_EJERCICIO (id_ejercicio, id_rutina, num_series, num_repeticiones)" +
                                "VALUES (@id_ejercicio, @id_rutina, @num_series, @num_repeticiones)";

                    using(SqlCommand command = new SqlCommand(sql, conexion.Conexion()))
                    {
                        command.Parameters.AddWithValue("@id_ejercicio", ejercicio.id_ejercicio);
                        command.Parameters.AddWithValue("@id_rutina",id_rutina);
                        command.Parameters.AddWithValue("@num_series", ejercicio.num_series);
                        command.Parameters.AddWithValue("@num_repeticiones", ejercicio.repeticiones);

                        command.ExecuteNonQuery();
                    }
                    comando = 1;
                }
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
    }
}
