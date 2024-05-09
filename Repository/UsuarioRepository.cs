using Antlr.Runtime.Tree;
using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Web.UI;

namespace SPARTANFITApp.Repository
{
    public class UsuarioRepository
    {
        public UsuarioDto registroUsuario(UsuarioDto usuario)
        {
            UsuarioDto usuarioResp = new UsuarioDto();
            PersonaDto persona = new PersonaDto();
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion)"
                            + "VALUES (@id_rol, @nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento, @estatura, @peso, @genero, @id_nivel_entrenamiento, @id_objetivo, @rehabilitacion)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_rol", usuario.persona.id_rol);
                    command.Parameters.AddWithValue("@nombres", usuario.persona.nombres);
                    command.Parameters.AddWithValue("@apellidos", usuario.persona.apellidos);
                    command.Parameters.AddWithValue("@correo", usuario.persona.correo);
                    command.Parameters.AddWithValue("@contrasena", usuario.persona.contrasena);
                    command.Parameters.AddWithValue("@fecha_nacimiento", usuario.persona.fecha_nacimiento);
                    command.Parameters.AddWithValue("@estatura", usuario.estatura);
                    command.Parameters.AddWithValue("@peso", usuario.peso);
                    command.Parameters.AddWithValue("@genero", usuario.persona.genero);
                    command.Parameters.AddWithValue("@id_nivel_entrenamiento", usuario.id_nivel_entrenamiento);
                    command.Parameters.AddWithValue("@id_objetivo", usuario.id_objetivo);
                    command.Parameters.AddWithValue("@rehabilitacion", usuario.rehabilitacion);
                    command.ExecuteNonQuery();
                }
                conexion.Disconnect();
                comando = 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (comando != 0)
            {
                usuarioResp = ObtenerUltimoUsuario();
                if (usuarioResp.persona.id_usuario != 0)
                {
                    usuarioResp.persona.respuesta = 1;
                    usuarioResp.persona.mensaje = "Registro exitoso";
                }
                else
                {
                    usuarioResp.persona.respuesta = 0;
                    usuarioResp.persona.mensaje = "Registro fallido";
                }
            }
            return usuarioResp;
            

        }

        public UsuarioDto ObtenerUltimoUsuario()
        {
            UsuarioDto usuario = new UsuarioDto();
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT MAX(id_usuario) as id_max FROM USUARIO";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario.persona = new PersonaDto(); 
                            usuario.persona.id_usuario = Convert.ToInt32(reader["id_max"]);
                            usuario.persona.respuesta = 1;
                            usuario.persona.mensaje = "Obtencion con exito";
                        }
                        else
                        {
                            usuario.persona = new PersonaDto();
                            usuario.persona.respuesta = 0;
                            usuario.persona.mensaje = "Error al momento de obtener id";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                usuario.persona = new PersonaDto(); 
                usuario.persona.respuesta = -1;
                usuario.persona.mensaje = "Error al obtener id: " + ex.Message;
            }
            finally
            {
                conexion.Disconnect();
            }

            return usuario;
        }



        public List<UsuarioDto> MostrarUsuarios()
        {
            List<UsuarioDto> usuarios = new List<UsuarioDto>();
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();

            string SQL = "SELECT  id_usuario,nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE id_rol = 1";

            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PersonaDto persona;

                        persona = new PersonaDto
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombres = reader["nombres"].ToString(),
                            apellidos = reader["apellidos"].ToString(),
                            correo = reader["correo"].ToString(),
                            contrasena = reader["contrasena"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            genero = reader["genero"].ToString()
                        };
                        UsuarioDto usuario = new UsuarioDto
                        {
                            
                            estatura = Convert.ToDouble(reader["estatura"]),
                            peso = Convert.ToDouble(reader["peso"]),
                            id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]),
                            id_objetivo = Convert.ToInt32(reader["id_objetivo"]),
                            rehabilitacion = Convert.ToInt32(reader["rehabilitacion"])
                        };

                        usuario.persona = persona;
                        usuarios.Add(usuario);
                    }
                }
            }
            conexion.Disconnect();
            return usuarios;
        }

        public UsuarioDto buscarPorId(int id)
        {
            UsuarioDto usuario = new UsuarioDto();
            PersonaDto persona = new PersonaDto();
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "SELECT  id_usuario, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE id_usuario = @id_usuario ";
                using(SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            persona.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                            persona.fecha_nacimiento = reader["fecha_nacimiento"].ToString();

                            usuario.estatura = Convert.ToDouble(reader["estatura"]);
                            usuario.peso = Convert.ToDouble(reader["peso"]);
                            usuario.id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]);
                            usuario.id_objetivo = Convert.ToInt32(reader["id_objetivo"]);
                            usuario.rehabilitacion = Convert.ToInt32(reader["rehabilitacion"]);

                            persona.respuesta = 1;
                            persona.mensaje = "Usuario encontrado";

                            usuario.persona = persona;
                            conexion.Disconnect();
                            return usuario;
                        }
                        else
                        {
                            usuario.persona.respuesta = 0;
                            usuario.persona.mensaje = "Usuario no encontrado";
                            return usuario;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                usuario.persona.respuesta = -1;
                usuario.persona.mensaje = "Error en proceso";
            }
            return usuario;
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

        public UsuarioDto IniciarSesion(UsuarioDto usuario,string contraNormal)
        {
            DBContextUtility conexion = new DBContextUtility();
            UsuarioDto usuarioResp = new UsuarioDto();
            EncriptarContrasenaUtility encr = new EncriptarContrasenaUtility();
            
            try
            {
                conexion.Connect();
                string SQL = "SELECT  estatura, peso, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE (correo = @correo)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@correo", usuario.persona.correo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string contrasenaAlmacenada = usuario.persona.contrasena;
                            if (encr.ValidarContrasena(contraNormal, contrasenaAlmacenada))
                            {
                                usuario.estatura = Convert.ToInt32(reader["estatura"]);
                                usuario.peso = Convert.ToDouble(reader["peso"]);
                                usuario.id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]);
                                usuario.id_objetivo = Convert.ToInt32(reader["id_objetivo"]);
                                usuario.rehabilitacion = Convert.ToInt32(reader["rehabilitacion"]);

                                conexion.Disconnect();
                                usuario.persona.respuesta = 1;
                                usuario.persona.mensaje = "Inicio correcto";
                                return usuario;
                            }

                        }
                        else
                        {

                            usuarioResp.persona = new PersonaDto(); 
                            usuarioResp.persona.respuesta = 0;
                            usuarioResp.persona.mensaje = "Inicio Incorrecto";
                            return usuarioResp;
                        }
                    }
                }

            }catch(Exception ex)
            {
                usuarioResp.persona = new PersonaDto(); 
                usuarioResp.persona.respuesta = -1;
                usuarioResp.persona.mensaje = "Error al inicio sesión: " + ex.Message;
            }
            finally
            {
                conexion.Disconnect();
            }
            return usuario;

        }


        public int ActualizarObjetivoUsuario(UsuarioDto usuario)
        {

            int id = usuario.persona.id_usuario;
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();
                string SQL = "UPDATE USUARIO SET id_nivel_entrenamiento = @id_nivel_entrenamiento, id_objetivo = @id_objetivo, rehabilitacion = @rehabilitacion " + "WHERE correo = @correo";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_nivel_entrenamiento", usuario.id_nivel_entrenamiento);
                    command.Parameters.AddWithValue("@id_objetivo", usuario.id_objetivo);
                    command.Parameters.AddWithValue("@rehabilitacion", usuario.rehabilitacion);
                    command.Parameters.AddWithValue("@correo", usuario.persona.correo);
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
        public int EliminarUsuario(UsuarioDto usuario)
        {
            int filasAfectadas = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM USUARIO WHERE correo = @correo";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))

                {
                    command.Parameters.AddWithValue("@correo", usuario.persona.correo);
                    command.ExecuteNonQuery();
                }
                filasAfectadas = 1;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conexion.Disconnect();
            }

            return filasAfectadas;
        }
        public int ActualizarDatosUsuario(UsuarioDto usuario)
        {

            int id = usuario.persona.id_usuario;
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();
                string SQL = "UPDATE USUARIO SET estatura=@estatura , peso=@peso" + "WHERE correo = @correo";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@estatura", usuario.estatura);
                    command.Parameters.AddWithValue("@peso", usuario.peso);                
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
    
