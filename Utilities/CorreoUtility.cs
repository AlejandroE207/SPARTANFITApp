using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using SPARTANFITApp.Dto;
using System.Runtime.Remoting.Messaging;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;

namespace SPARTANFITApp.Utilities
{
    public class CorreoUtility
    {
        private SmtpClient cliente;
        private MailMessage email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "spartanfitsoporte@gmail.com";
        private string Password = "ufzosujaxkkbxord";
        private bool EnabledSSL = true;
        public CorreoUtility()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensajeCorreo, bool esHtlm = true)
        {
            email = new MailMessage(User, destinatario, asunto, mensajeCorreo);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);

        }
        public void enviarCorreoMensual()
        {
            FechaUtility fechaUtility = new FechaUtility();
            if (fechaUtility.ValidarDia())
            {
                UsuarioDto usuario = new UsuarioDto();
                string mensajeCorreo = mensaje(usuario.persona.nombres);

                EnviarCorreo(usuario.persona.correo, "Actualizacion Datos", mensajeCorreo, true);
            }

        }
        public string mensaje(string nombreUsuario)
        {
            string mensaje = "<html>" +
                "<head>" +
                "<style>" +
                "body {" +
                "margin: 0;" +
                "padding: 0;" +
                "font-family: Arial, sans-serif;" +
                "background-color: #ffffff;" +
                "}" +
                ".container {" +
                "max-width: 600px;" +
                "margin: 0 auto;" +
                "padding: 20px;" +
                "background-color: #171717;" +
                "border-radius: 10px;" +
                "box-shadow: 0 0 20px #2c2c2c;" +
                "color: #ffffff;" +
                "}" +

                ".header {" +
                "text-align: center;" +
                "margin-bottom: 30px;" +
                "}" +
                ".header h1 {" +
                "color: #f3c623;" +
                "}" +

                ".content {" +
                "padding: 20px;" +
                "background-color: #2c2c2c;" +
                "border-radius: 8px;" +
                "box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);" +
                "}" +
                ".content h2 {" +
                "color: #f3c623;" +
                "margin-bottom: 15px;" +
                "}" +
                ".content p {" +
                "color: #ffffff;" +
                "font-size: 16px;" +
                "line-height: 1.6;" +
                "}" +

                ".footer {" +
                "text-align: center;" +
                "margin-top: 30px;" +
                "padding-top: 20px;" +
                "border-top: 1px solid #f3c623;" +
                "}" +
                ".footer p {" +
                "color: #f3c623;" +
                "font-size: 14px;" +
                "}" +
                "</style>" +
                "</head>" +
                "<body>" +

                "<div class='container'>" +
                "<div class='header'>" +
                "<h1>¡Recordatorio de registro!</h1>" +
                "</div>" +

                "<div class='content'>" +
                "<h2>Hola " + nombreUsuario + ",</h2>" +
                "<p>Esperamos que este correo te encuentre bien.</p>" +
                "<p>No olvides visitar nuestra página web para realizar tu actualización de datos personales.</p>" +
                "<p>¡Gracias por ser parte de nuestra comunidad!</p>" +
                "</div>" +

                "<div class='footer'>" +
                "<p>Este correo electrónico fue enviado por SPARTANFIT. Si tienes alguna pregunta, por favor contáctanos a través de spartanfitsoporte@gmail.com.</p>" +
                "</div>" +
                "</div>" +

                "</body>" +
                "</html>";
            return mensaje;
        }

    }
}