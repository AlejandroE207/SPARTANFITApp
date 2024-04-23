using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class Recuperacion_ContrasenaDto
    {
        public int id_recuperacion { get; set; }
        public int id_usuario { get; set; } 
        public string codigo { get; set;}
        public string fecha { get; set;}    
    }
}