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

           
            doc.Add(new Paragraph("Lista de Entrenadores"));
            doc.Add(new Paragraph(" ")); 

           
            PdfPTable table = new PdfPTable(6); 

          
            table.AddCell("ID");
            table.AddCell("Nombres");
            table.AddCell("Apellidos");
            table.AddCell("Correo");
            table.AddCell("Genero");
            table.AddCell("Fecha de Nacimiento");


            
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
       

    }
}