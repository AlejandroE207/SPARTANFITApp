﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class UsuarioDto
    {
        public PersonaDto persona { get; set; }
        public double estatura { get; set; }
        public double peso { get; set; }
        public int id_nivel_entrenamiento { get; set; }
        public int id_objetivo { get; set; }
        public int rehabilitacion { get; set; }
    }
}