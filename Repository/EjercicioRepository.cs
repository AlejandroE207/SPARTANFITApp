using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
       
        public class EjercicioRepository
        {
        public EjercicioRepository() { }    
        public void AgregarEjercicio(EjercicioDto ejercicio, HttpPostedFileBase imagen_ejercicio)
        {
            DBContextUtility conexion = new DBContextUtility();
            byte[] imagenBytes = ConvertImageToBytes(imagen_ejercicio);

            try
            {
                conexion.Connect();

                // Obtener el ID del grupo muscular de la tabla Musculo
                int idGrupoMuscular = ObtenerIdGrupoMuscular(ejercicio.GrupoMuscular, conexion);

                // Consulta SQL para insertar el ejercicio en la base de datos
                string SQLInsertExercise = "INSERT INTO EJERCICIO (Id_grupo_muscular, nombre_ejercicio, apoyo_visual) VALUES (@Id_grupo_Muscular, @NombreEjercicio, @ApoyoVisual)";
                using (var commandInsertExercise = new SqlCommand(SQLInsertExercise, conexion.Conexion()))
                {
                    commandInsertExercise.Parameters.AddWithValue("@Id_grupo_Muscular", idGrupoMuscular);
                    commandInsertExercise.Parameters.AddWithValue("@NombreEjercicio", ejercicio.NombreEjercicio);
                    commandInsertExercise.Parameters.AddWithValue("@ApoyoVisual", imagenBytes);
                    commandInsertExercise.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conexion.Disconnect();
            }
        }

        private int ObtenerIdGrupoMuscular(string grupoMuscular, DBContextUtility conexion)
        {
            // Consulta SQL para obtener el ID del grupo muscular
            string SQLGetMuscleId = "SELECT Id_grupo_muscular FROM MUSCULO WHERE nombre_grupo = @GrupoMuscular";
            using (var command = new SqlCommand(SQLGetMuscleId, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@GrupoMuscular", grupoMuscular);
                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("El grupo muscular proporcionado no existe en la base de datos.");
                }
            }
        }

        private byte[] ConvertImageToBytes(HttpPostedFileBase imagen_ejercicio)
            {
                // Convertir la imagen a bytes
                if (imagen_ejercicio != null && imagen_ejercicio.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imagen_ejercicio.InputStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
                return null;
            }
        }
 }



        

 