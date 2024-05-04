using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec.wmf;
using SPARTANFITApp.Dto;
using SPARTANFITApp.Services;
namespace SPARTANFITApp.Utilities
{
    public class ReporteUtility
    {
        public void CrearPdfDeEntrenadores(List<PersonaDto> entrenadores, string filePath)
        {
            Document doc = new Document(PageSize.A4); 
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open(); 

          
            BaseColor goldColor = new BaseColor(255, 215, 0); 

         
            BaseColor goldBackground = new BaseColor(255, 223, 0); 

         
            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, goldColor);

           
            Paragraph title = new Paragraph("Lista de Entrenadores", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title); 

            doc.Add(new Paragraph(" ")); 

          
            PdfPTable table = new PdfPTable(6); 

            
            var headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor); 
            table.AddCell(new PdfPCell(new Phrase("ID", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Fecha de Nacimiento", headerFont)) { BackgroundColor = goldBackground });

            
            foreach (var entrenador in entrenadores)
            {
                table.AddCell(entrenador.id_usuario.ToString());
                table.AddCell(entrenador.nombres);
                table.AddCell(entrenador.apellidos);
                table.AddCell(entrenador.correo);
                table.AddCell(entrenador.genero);
                table.AddCell(entrenador.fecha_nacimiento);
            }

            doc.Add(table); 

            doc.Close(); 
        }
        public void CrearPdfUsuarios(List<UsuarioDto> usuarios, string filePath)
        {
            Document doc = new Document(PageSize.A4); 
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open(); 

           
            BaseColor goldColor = new BaseColor(255, 215, 0); 

            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, goldColor);

          
            Paragraph title = new Paragraph("Lista de Usuarios", titleFont)
            {
                Alignment = Element.ALIGN_CENTER 
            };
            doc.Add(title); 

            doc.Add(new Paragraph(" ")); 

           
            BaseColor goldBackground = new BaseColor(255, 223, 0); 

            PdfPTable table = new PdfPTable(6); 

            Font headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor); 

           
            table.AddCell(new PdfPCell(new Phrase("ID", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = goldBackground });
            table.AddCell(new PdfPCell(new Phrase("Fecha de Nacimiento", headerFont)) { BackgroundColor = goldBackground });

       
            Font contentFont = FontFactory.GetFont("Arial", 12); 

            
            foreach (var usuario in usuarios)
            {
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.id_usuario.ToString(), contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.nombres, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.apellidos, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.correo, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.genero, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.fecha_nacimiento, contentFont)));
            }

            doc.Add(table); 

            doc.Close(); 
        }
    }

}
}