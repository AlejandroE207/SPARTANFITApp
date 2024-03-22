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
        public int registrarEntrenador(PersonaDto entrenador)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();


            string SQL = "INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, genero)"
                        + "VALUES (@id_rol, @nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento, @genero)";


            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@id_rol", entrenador.id_rol);
                command.Parameters.AddWithValue("@nombres", entrenador.nombres);
                command.Parameters.AddWithValue("@apellidos", entrenador.apellidos);
                command.Parameters.AddWithValue("@correo", entrenador.correo);
                command.Parameters.AddWithValue("@contrasena", entrenador.contrasena);
                command.Parameters.AddWithValue("@fecha_nacimiento", entrenador.fecha_nacimiento);
                command.Parameters.AddWithValue("@genero", entrenador.genero);

                command.ExecuteNonQuery();
            }
            conexion.Disconnect();
            return comando;
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
                        else
                        {
                            usuarioResp.respuesta = 0;
                            usuarioResp.mensaje = "Inicio Incorrecto";
                            return usuarioResp;
                        }
                    }
                }

                //}
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

        private bool VerificarCredenciales(string correo, string contrasena)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM USUARIO WHERE (correo = @correo AND contrasena = @contrasena)";
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@correo", correo);
                command.Parameters.AddWithValue("@contrasena", contrasena);

                var count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}


    