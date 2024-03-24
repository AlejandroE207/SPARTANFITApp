using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace SPARTANFITApp.Utilities
{
    public class FechaUtility
    {
        
        public bool ValidarDia()
        {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;

            if (day == 1)
            {
             return true;   
            }
            return false;
        }
    }
}