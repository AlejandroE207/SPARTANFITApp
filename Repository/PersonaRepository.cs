using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class PersonaRepository
    {
        public PersonaDto IniciarSesion(string correo, string contrasena)
        {
            DBContextUtility conexion = new DBContextUtility();
            PersonaDto persona = new PersonaDto();
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

                            if (encr.ValidarContrasena(contrasena, contrasenaAlmacenada)){

                                persona.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                                persona.id_rol = Convert.ToInt32(reader["id_rol"]);
                                persona.nombres = reader["nombres"].ToString();
                                persona.apellidos = reader["apellidos"].ToString();
                                persona.correo = reader["correo"].ToString();
                                persona.contrasena = reader["contrasena"].ToString();
                                persona.fecha_nacimiento = reader["fecha_nacimiento"].ToString();
                                persona.genero = reader["genero"].ToString();
                                
                                conexion.Disconnect();
                                persona.respuesta = 1;
                                persona.mensaje = "Inicio correcto";
                                return persona;
                            }
                            else
                            {
                                persona.respuesta = 0;
                                persona.mensaje = "Inicio Incorrecto";
                            }
                        }
                        else
                        {
                            persona.respuesta = 0;
                            persona.mensaje = "Inicio Incorrecto";
                            return persona;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                persona = new PersonaDto
                {
                    respuesta = -1,
                    mensaje = "Error al inicio sesión: " + ex.Message
                };
            }
            finally
            {
                conexion.Disconnect();
            }
            return persona;
        }
        public int ActualizarContrasena(string correo, string contrasena)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();
                string SQL = "UPDATE USUARIO SET  contrasena=@contrasena " + "WHERE correo = @correo";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@contrasena", contrasena);

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
        public bool buscarPersona(string correo)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";
            int personaEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@correo", correo);

                personaEncontrado = (int)command.ExecuteScalar();
            }
            conexion.Disconnect();
            if (personaEncontrado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public PersonaDto SeleccionarPersona(string correo)
        {
            DBContextUtility conexion = new DBContextUtility();
            PersonaDto persona = null;
            PersonaDto personaResp = new PersonaDto();
            EncriptarContrasenaUtility encr = new EncriptarContrasenaUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT id_usuario,correo FROM USUARIO WHERE (correo = @correo)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", correo);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           
                                persona = new PersonaDto
                                {
                                    id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                    correo = reader["correo"].ToString()
                                };
                                conexion.Disconnect();
                                persona.respuesta = 1;
                                persona.mensaje = "Inicio correcto";
                                return persona;
                            
                        }
                        else
                        {
                            personaResp.respuesta = 0;
                            personaResp.mensaje = "Inicio Incorrecto";
                            return personaResp;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                persona = new PersonaDto
                {
                    respuesta = -1,
                    mensaje = "Error al inicio sesión: " + ex.Message
                };
            }
            finally
            {
                conexion.Disconnect();
            }
            return persona;
        }
    }
}