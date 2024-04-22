using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Utilities
{
    public class GeneradorCodigoUtility
    {
        public int NumeroAleatorio()
        {
            Random r = new Random();
            return r.Next(0, 9999 + 1);
        }
    }
}