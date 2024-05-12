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

                string SQL = "INSERT INTO RUTINA (id_nivel_rutina, id_objetivo, nombre_rutina,dia,descripcion, id_entrenador)"
                            + "VALUES (@id_nivel_rutina, @id_objetivo, @nombre_rutina, @dia, @descripcion, @id_entrenador)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {

                    command.Parameters.AddWithValue("@id_nivel_rutina", rutina.id_nivel_rutina);
                    command.Parameters.AddWithValue("@id_objetivo", rutina.id_objetivo);
                    command.Parameters.AddWithValue("@nombre_rutina", rutina.nombre_rutina);
                    command.Parameters.AddWithValue("@dia", rutina.dia);
                    command.Parameters.AddWithValue("@descripcion", rutina.descripcion);
                    command.Parameters.AddWithValue("@id_entrenador", rutina.id_entrenador);

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

        public int asignarRutina(UsuarioDto usuario)
        {
            int comando = 0;

            List<String> dias = new List<String> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };

            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                foreach (String dia in dias)
                {
                    string SQL = "SELECT id_rutina FROM RUTINA WHERE id_nivel_rutina = @id_nivel_rutina AND id_objetivo = @id_objetivo AND dia = @dia";
                    using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                    {
                        command.Parameters.AddWithValue("@id_nivel_rutina", usuario.id_nivel_entrenamiento);
                        command.Parameters.AddWithValue("@id_objetivo", usuario.id_objetivo);
                        command.Parameters.AddWithValue("@dia", dia);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int id_rutina = Convert.ToInt32(reader["id_rutina"]);
                                comando = asignarRutinaDia(usuario.persona.id_usuario, id_rutina);
                            }
                            else
                            {
                                comando = -1;

                            }
                        }
                    }
                }
                return comando;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally { conexion.Disconnect(); }

            return comando;
        }

        public int asignarRutinaDia(int id_usuario, int id_rutina)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "INSERT INTO USUARIO_RUTINA (id_usuario,id_rutina) VALUES (@id_usuario,@id_rutina)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@id_rutina", id_rutina);
                    comando = command.ExecuteNonQuery();
                    comando = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

        public RutinaDto buscarRutinaIdUsuario(int id_usuario, string dia)
        {
            RutinaDto rutinaDia = new RutinaDto();
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "SELECT r.id_rutina, r.nombre_rutina, r.dia, r.descripcion FROM RUTINA AS r " +
                    "INNER JOIN USUARIO_RUTINA AS ur ON ur.id_rutina = r.id_rutina " +
                    "WHERE ur.id_usuario = @id_usuario and dia = @dia";

                using (SqlCommand command  = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@dia", dia);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            rutinaDia.id_rutina = Convert.ToInt32(reader["id_rutina"]);
                            rutinaDia.nombre_rutina = reader["nombre_rutina"].ToString();
                            rutinaDia.dia = reader["dia"].ToString();
                            rutinaDia.descripcion = reader["descripcion"].ToString();
                            rutinaDia.respuesta = 1;
                            rutinaDia.mensaje = "Busqueda exitosa";
                            return rutinaDia;
                        }
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }finally
            {
                conexion.Disconnect();
            }
            return rutinaDia;
        }

        public List<EjercicioDto> ObtenerEjerciciosDia(int id_rutina)
        {
            List<EjercicioDto> ejerciciosDia = new List<EjercicioDto>();
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT eje.id_ejercicio, eje.nombre_ejercicio, eje.id_grupo_muscular, eje.apoyo_visual, rue.num_series, rue.num_repeticiones " +
                    "FROM EJERCICIO AS eje " +
                    "INNER JOIN RUTINA_EJERCICIO AS rue ON rue.id_ejercicio = eje.id_ejercicio " +
                    "WHERE rue.id_rutina = @id_rutina";

                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_rutina", id_rutina);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EjercicioDto ejercicio = new EjercicioDto()
                            {
                                id_ejercicio = Convert.ToInt32(reader["id_ejercicio"]),
                                nombre_ejercicio = reader["nombre_ejercicio"].ToString(),
                                id_grupo_muscular = Convert.ToInt32(reader["id_grupo_muscular"]),
                                apoyo_visual = reader["apoyo_visual"].ToString(),
                                num_series = Convert.ToInt32(reader["num_series"]),
                                repeticiones = Convert.ToInt32(reader["num_repeticiones"])
                            };

                            ejerciciosDia.Add(ejercicio);
                        }
                    }
                    return ejerciciosDia;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return ejerciciosDia;
        }

        public List<RutinaDto> MostrasRutinas()
        {
            List<RutinaDto> rutinas = new List<RutinaDto>();
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT id_rutina, id_nivel_rutina, id_objetivo, nombre_rutina, dia, descripcion FROM RUTINA";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RutinaDto rutina = new RutinaDto()
                            {
                                id_rutina = Convert.ToInt32(reader["id_rutina"]),
                                id_nivel_rutina = Convert.ToInt32(reader["id_nivel_rutina"]),
                                id_objetivo = Convert.ToInt32(reader["id_objetivo"]),
                                nombre_rutina = reader["nombre_rutina"].ToString(),
                                dia = reader["dia"].ToString(),
                                descripcion = reader["descripcion"].ToString()
                            };
                            rutinas.Add(rutina);
                        }
                    }
                    return rutinas;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return rutinas;
        } 

        public RutinaDto EliminarRutina (int id_rutina)
        {
            RutinaDto rutinaResp = new RutinaDto();
            DBContextUtility conexion = new DBContextUtility();
            int comando = 0;
            try
            {
                comando = EliminarUsuarioRutina(id_rutina);
                if(comando != 0)
                {
                    comando = EliminarRutinaEjercicio(id_rutina);
                    if(comando != 0)
                    {
                        conexion.Connect();
                        string SQL = "DELETE FROM RUTINA WHERE id_rutina = @id_rutina";
                        using(SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                        {
                            command.Parameters.AddWithValue("@id_rutina", id_rutina);
                            comando = command.ExecuteNonQuery();
                            rutinaResp.respuesta = 1;
                            rutinaResp.mensaje = "Rutina eliminada con exito";
                            return rutinaResp;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return rutinaResp;
        }


        public int EliminarUsuarioRutina(int id_rutina)
        {
            int comando = 0;

            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM USUARIO_RUTINA WHERE id_rutina = @id_rutina";
                using(SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_rutina", id_rutina);
                    comando = command.ExecuteNonQuery();
                    comando = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

        public int EliminarRutinaEjercicio(int id_rutina)
        {
            int comando = 0;

            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM RUTINA_EJERCICIO WHERE id_rutina = @id_rutina";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_rutina", id_rutina);
                    comando = command.ExecuteNonQuery();
                    comando = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

    }
}
