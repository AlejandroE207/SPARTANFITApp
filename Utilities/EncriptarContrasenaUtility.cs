using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using SPARTANFITApp.Dto;

namespace SPARTANFITApp.Utilities
{
    public class EncriptarContrasenaUtility
    {
        public string EncriptarContrasena(PersonaDto persona)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(persona.contrasena);

                // Calcular el hash MD5 de la contraseña
                byte[] hashBytes = md5.ComputeHash(passwordBytes);

                // Convertir el hash en una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public bool ValidarContrasena(string contraNormal, string contrasenaEncriptadaAlmacenada)
        {

            string contrasenaEncriptadaIngresada = EncriptarContrasena(new PersonaDto { contrasena = contraNormal });


            return contrasenaEncriptadaIngresada == contrasenaEncriptadaAlmacenada;
        }

    }
}