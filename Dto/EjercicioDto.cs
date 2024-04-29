using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class EjercicioDto
    {
        public int id_ejercicio { get; set; }
        public String nombre_ejercicio { get; set; }
        public int id_grupo_muscular { get; set; }  
        public string apoyo_visual {  get; set; }
        public int num_series {  get; set; }
        public int repeticiones { get; set; }
        public int respuesta { get; set; }
        public string mensaje { get; set; }
    }
}