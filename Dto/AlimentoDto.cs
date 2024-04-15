using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Dto
{
    public class AlimentoDto
    {
        public string NombreAlimento { get; set; }
        public string CategoriaAlimento { get; set; }
        public decimal CaloriasPorGramo { get; set; }
        public decimal Grasa { get; set; }
        public decimal Carbohidrato { get; set; }
        public decimal Proteina { get; set; }
        public decimal Fibra { get; set; }
    }
}