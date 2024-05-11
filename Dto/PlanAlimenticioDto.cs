using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class PlanAlimenticioDto
    {
        public int id_plan_alimenticio { get; set; }
        public string nombre { get; set; }
        public string dia {  get; set; }
        public int id_entrenador { get; set; }
        public string descripcion { get; set; }
        public int respuesta { get; set; }
        public string mensaje { get; set; }

    }
}