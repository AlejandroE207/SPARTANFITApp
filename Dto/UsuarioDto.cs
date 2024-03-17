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
        public String nombres { get; set; }
        public String apellidos { get; set; }
        public String usuario { get; set; }
        public String contrasena { get; set; }

        public String correo { get; set; }
        public string fecha_nacimiento { get; set; }
        public double estatura { get; set; }
        public double peso { get; set; }
        public String genero { get; set; }
        public int id_nivel_entrenamiento { get; set; }
        public int id_objetivo { get; set; }
        public int rehabilitacion { get; set; }
        public int respuesta { get; set; }
        public string mensaje { get; set; }
    }
}