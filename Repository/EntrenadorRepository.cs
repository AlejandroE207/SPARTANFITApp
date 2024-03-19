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
        public int registrarEntrenador(EntrenadorDto entrenador)
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

        public EntrenadorDto IniciarSesion(string correo, string contrasena)
        {
            DBContextUtility conexion = new DBContextUtility();
            EntrenadorDto entrenador = null;
            EntrenadorDto entrenadorResp = new EntrenadorDto();
            conexion.Connect();
            if (VerificarCredenciales(correo, contrasena))
            {
                string SQL = "SELECT id_usuario,id_rol, nombres, apellidos, correo ,contrasena, fecha_nacimiento, genero FROM USUARIO WHERE (correo = @correo AND contrasena = @contrasena)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@contrasena", contrasena);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entrenador = new EntrenadorDto
                            {
                                id_rol = Convert.ToInt32(reader["id_rol"]),
                                nombres = reader["nombre"].ToString(),
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
                            entrenadorResp.respuesta = 0;
                            entrenadorResp.mensaje = "Inicio Incorrecto";
                            return entrenadorResp;
                        }
                    }
                }
            }


            return null;
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


    