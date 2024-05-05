using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class RutinaDto
    {
        public int id_rutina {  get; set; }
        public int id_nivel_rutina { get; set; }
        public string nombre_rutina { get; set; }
        public string dia {  get; set; }
        public string descripcion { get; set; }
        public int id_entrenador { get; set; }
        public int respuesta { get; set; }
        public string mensaje { get; set; }

    }
}