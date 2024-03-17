using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            string SQL = "INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contraseña, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion)"
                        + "VALUES(" + usuario.id_rol + "," + usuario.nombre + "," + usuario.apellidos + "," + usuario.correo + "," + usuario.contraseña + "," + usuario.contraseña + "," + usuario.fecha_nacimiento +
                        "," + usuario.estatura + "," + usuario.peso + "," + usuario.genero + "," + usuario.id_nivel_entrenamiento + "," + usuario.id_objetivo + "," + usuario.rehabilitacion + ");";
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                comando = command.ExecuteNonQuery();
            }
            conexion.Disconnect();
            return comando;
        }

        public bool buscarUsuario(string correo)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM usuario WHERE correo = @correoUsuario";
            int usuarioEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@correoUsuario", correo);

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
    }
}