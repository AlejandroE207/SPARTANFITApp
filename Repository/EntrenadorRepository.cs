using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;

namespace SPARTANFITApp.Repository
{
    public class EntrenadorRepository
    {
        public int registroEntrenador(PersonaDto entrenador)
        {
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO USUARIO ( id_rol,nombres, apellidos, correo, contrasena, fecha_nacimiento, genero)"
                            + "VALUES ( @id_rol,@nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento,@genero)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_rol", 2);
                    command.Parameters.AddWithValue("@nombres", entrenador.nombres);
                    command.Parameters.AddWithValue("@apellidos", entrenador.apellidos);
                    command.Parameters.AddWithValue("@correo", entrenador.correo);
                    command.Parameters.AddWithValue("@contrasena", entrenador.contrasena);
                    command.Parameters.AddWithValue("@fecha_nacimiento", entrenador.fecha_nacimiento);
                    command.Parameters.AddWithValue("@genero", entrenador.genero);
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
        public List<PersonaDto> MostrarEntrenadores()
        {
            List<PersonaDto> entrenadores = new List<PersonaDto>();
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();

            string SQL = "SELECT  id_usuario, id_rol,nombres, apellidos, correo, contrasena, fecha_nacimiento, genero FROM USUARIO WHERE id_rol=2";

            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PersonaDto entrenador = new PersonaDto
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            id_rol = Convert.ToInt32(reader["id_rol"]),
                            nombres = reader["nombres"].ToString(),
                            apellidos = reader["apellidos"].ToString(),
                            correo = reader["correo"].ToString(),
                            contrasena = reader["contrasena"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            genero = reader["genero"].ToString()
                        };

                        entrenadores.Add(entrenador);
                    }
                }
            }
            conexion.Disconnect();
            return entrenadores;
        }

        public bool buscarEntrenador(string correo)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";
            int entrenadorEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@correo", correo);
                entrenadorEncontrado = (int)command.ExecuteScalar();
            }
            conexion.Disconnect();
            if (entrenadorEncontrado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public PersonaDto IniciarSesion(string correo, string contrasena)
        {
            DBContextUtility conexion = new DBContextUtility();
            PersonaDto entrenador = null;
            PersonaDto usuarioResp = new PersonaDto();
            EncriptarContrasenaUtility encr = new EncriptarContrasenaUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT id_usuario,id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE (correo = @correo)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", correo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string contrasenaAlmacenada = reader["contrasena"].ToString();
                            if (encr.ValidarContrasena(contrasena, contrasenaAlmacenada))
                            {
                                entrenador = new PersonaDto
                                {
                                    id_rol = Convert.ToInt32(reader["id_rol"]),
                                    nombres = reader["nombres"].ToString(),
                                    apellidos = reader["apellidos"].ToString(),
                                    correo = reader["correo"].ToString(),
                                    contrasena = reader["contrasena"].ToString(),
                                    fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                                    genero = reader["genero"].ToString(),
                                };
                                conexion.Disconnect();
                                entrenador.respuesta = 1;
                                entrenador.mensaje = "Inicio correcto";
                                return entrenador;
                            }                            
                        }
                        else
                        {
                            usuarioResp.respuesta = 0;
                            usuarioResp.mensaje = "Inicio Incorrecto";
                            return usuarioResp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                entrenador = new PersonaDto
                {
                    respuesta = -1,
                    mensaje = "Error al inicio sesión: " + ex.Message
                };
            }
            finally
            {
                conexion.Disconnect();
            }
            return entrenador;

        }
        public int EliminarEntrenador(string correo)
        {
            int filasAfectadas = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM USUARIO WHERE correo = @correo";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))

                {
                    command.Parameters.AddWithValue("@correo",correo);
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
        public int ActualizarEntrenador(PersonaDto entrenador)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();
                string SQL = "UPDATE USUARIO SET nombres = @nombres, apellidos = @apellidos, correo = @correo, contrasena=@contrasena, genero=@genero " + "WHERE correo = @correo";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@nombres", entrenador.nombres);
                    command.Parameters.AddWithValue("@apellidos", entrenador.apellidos);
                    command.Parameters.AddWithValue("@correo", entrenador.correo);
                    command.Parameters.AddWithValue("@contrasena", entrenador.contrasena);
                    command.Parameters.AddWithValue("@genero", entrenador.genero);
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
    }
}


    