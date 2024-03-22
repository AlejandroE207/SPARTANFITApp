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
            PersonaDto persona = null;
            PersonaDto personaResp = new PersonaDto();


            try
            {
                conexion.Connect();
                string SQL = "SELECT id_usuario,id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE (correo = @correo AND contrasena = @contrasena)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@contrasena", contrasena);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            persona = new PersonaDto
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