using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class AdministradorRepository
    {
        public int registrarAdministrador(PersonaDto administrador)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();


            string SQL = "INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, genero)"
                        + "VALUES (@id_rol, @nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento, @genero)";


            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@id_rol", administrador.id_rol);
                command.Parameters.AddWithValue("@nombres", administrador.nombres);
                command.Parameters.AddWithValue("@apellidos", administrador.apellidos);
                command.Parameters.AddWithValue("@correo", administrador.correo);
                command.Parameters.AddWithValue("@contrasena", administrador.contrasena);
                command.Parameters.AddWithValue("@fecha_nacimiento", administrador.fecha_nacimiento);
                command.Parameters.AddWithValue("@genero", administrador.genero);

                command.ExecuteNonQuery();
            }
            conexion.Disconnect();
            return comando;
        }
        public bool buscarAdministrador(string correo)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";
            int administradorEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@correo", correo);

                administradorEncontrado = (int)command.ExecuteScalar();
            }
            conexion.Disconnect();
            if (administradorEncontrado > 0)
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
            PersonaDto administrador = null;
            PersonaDto administradorResp = new PersonaDto();


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
                            administrador = new PersonaDto
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
                            administrador.respuesta = 1;
                            administrador.mensaje = "Inicio correcto";
                            return administrador;
                        }
                        else
                        {
                            administradorResp.respuesta = 0;
                            administradorResp.mensaje = "Inicio Incorrecto";
                            return administradorResp;
                        }
                    }
                }

                //}
            }
            catch (Exception ex)
            {
                administrador = new PersonaDto
                {
                    respuesta = -1,
                    mensaje = "Error al inicio sesión: " + ex.Message
                };
            }
            finally
            {
                conexion.Disconnect();
            }
            return administrador;

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
