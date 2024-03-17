using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class UsuarioDto
    {
        public int idUsuario { get; set; }
        public int id_rol { get; set; }
        public String nombre { get; set; }
        public String apellidos { get; set; }
        public String usuario { get; set; }
        public String contraseña { get; set; }

        public String correo { get; set; }
        public String fecha_nacimiento { get; set; }
        public double estatura { get; set; }
        public double peso { get; set; }
        public String genero { get; set; }
        public int id_nivel_entrenamiento { get; set; }
        public int id_objetivo { get; set; }
        public bool rehabilitacion { get; set; }
        public int respuesta { get; set; }
        public string mensaje { get; set; }
    }
}