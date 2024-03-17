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
    public class UsuarioRepository
    {
        public int registroUsuario(UsuarioDto usuario)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            //string SQL = "INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion)"
            //            + "VALUES(" + usuario.id_rol + "," + usuario.nombre + "," + usuario.apellidos + "," + usuario.correo + "," + usuario.contraseña + "," + usuario.contraseña + "," + usuario.fecha_nacimiento +
            //            "," + usuario.estatura + "," + usuario.peso + "," + usuario.genero + "," + usuario.id_nivel_entrenamiento + "," + usuario.id_objetivo + "," + usuario.rehabilitacion + ");";

            string SQL = "INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion)"
                        + "VALUES (@id_rol, @nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento, @estatura, @peso, @genero, @id_nivel_entrenamiento, @id_objetivo, @rehabilitacion)";


            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@id_rol",usuario.id_rol);
                command.Parameters.AddWithValue("@nombres",usuario.nombres);
                command.Parameters.AddWithValue("@apellidos",usuario.apellidos);
                command.Parameters.AddWithValue("@correo",usuario.correo );
                command.Parameters.AddWithValue("@contrasena", usuario.contrasena);
                command.Parameters.AddWithValue("@fecha_nacimiento", usuario.fecha_nacimiento);
                command.Parameters.AddWithValue("@estatura", usuario.estatura );
                command.Parameters.AddWithValue("@peso",usuario.peso );
                command.Parameters.AddWithValue("@genero", usuario.genero );
                command.Parameters.AddWithValue("@id_nivel_entrenamiento",usuario.id_nivel_entrenamiento );
                command.Parameters.AddWithValue("@id_objetivo", usuario.id_objetivo);
                command.Parameters.AddWithValue("@rehabilitacion", usuario.rehabilitacion);
                command.ExecuteNonQuery();
            }
            conexion.Disconnect();
            return comando;
        }

        public bool buscarUsuario(string correo)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";
            int usuarioEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@correo", correo);

                usuarioEncontrado = (int)command.ExecuteScalar();
            }
            conexion.Disconnect();
            if (usuarioEncontrado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UsuarioDto IniciarSesion(string correo, string contrasena)
        {
            DBContextUtility conexion = new DBContextUtility();
            UsuarioDto usuario = null;
            UsuarioDto usuarioResp = new UsuarioDto();
            conexion.Connect();
            //if (VerificarCredenciales(correo, contrasena))
            //{
                string SQL = "SELECT id_usuario, nombres, apellidos, correo FROM USUARIO WHERE (correo = @correo AND contrasena = @contrasena)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@contrasena", contrasena);
                    
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new UsuarioDto
                            {
                                id_rol = Convert.ToInt32(reader["id_rol"]),
                                nombres = reader["nombre"].ToString(),
                                apellidos = reader["apellidos"].ToString(),
                                correo = reader["correo"].ToString(),
                                contrasena = reader["contrasena"].ToString(),
                                fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                                estatura = Convert.ToInt32(reader["estatura"]),
                                peso = Convert.ToDouble(reader["peso"]),
                                genero = reader["genero"].ToString(),
                                id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]),
                                id_objetivo = Convert.ToInt32(reader["id_objetivo"]),
                                rehabilitacion = Convert.ToInt32(reader["rehabilitacion"])

                            };
                            conexion.Disconnect();
                            usuario.respuesta = 1;
                            usuario.mensaje = "Inicio correcto";
                            return usuario;
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
        // private bool VerificarCredenciales(string correo, string contrasena)
        //{
        //    DBContextUtility conexion = new DBContextUtility();
        //    conexion.Connect();
        //    string SQL = "SELECT COUNT(*) FROM USUARIO WHERE (correo = @correo AND contrasena = @contrasena)";
        //    using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
        //    {
        //        command.Parameters.AddWithValue("@correo", correo);
        //        command.Parameters.AddWithValue("@contrasena", contrasena);

        //        var count = (int)command.ExecuteScalar();
        //        return count > 0;
        //    }
        //}
    }
}