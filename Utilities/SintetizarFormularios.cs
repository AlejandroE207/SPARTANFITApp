using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Utilities
{
    public class SintetizarFormularios
    {
        public string Sintetizar(string input)
        {
            if (input == null) return string.Empty;
            input = input.Replace("'", " ");
            return input;
        }
    }
}