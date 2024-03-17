using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Utilities
{
    public class DBContextUtility
    {

        static string SERVER = "AER-PC2";
        static string DB_NAME = "SpartanFit";
        static string DB_USER = "userSPARTANFIT";
        static string DB_PASSWORD = "123";

        static string Conn = "server=" + SERVER + ";database=" + DB_NAME + ";user id=" + DB_USER + ";password=" + DB_PASSWORD + ";MultipleActiveResultSets=true";
        SqlConnection Con = new SqlConnection(Conn);
        public void Connect()
        {
            try
            {
                Con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Disconnect()
        {
            Con.Close();
        }

        public SqlConnection Conexion()
        {
            return Con;
        }
    }
}