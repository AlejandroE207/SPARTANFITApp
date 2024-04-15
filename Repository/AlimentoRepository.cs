using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class AlimentoRepository
    {
        public AlimentoRepository() { }
        public void AgregarAlimento(AlimentoDto alimento)
        {
            
           
                // Creamos una instancia de DBContextUtility para manejar la conexión a la base de datos
                DBContextUtility conexion = new DBContextUtility();

                try
                {
                    // Conectamos con la base de datos
                    conexion.Connect();

                    // Obtenemos el ID de la categoría del alimento
                    int idCategoria = ObtenerIdCategoriaAlimento(alimento.CategoriaAlimento, conexion);

                    // Consulta SQL para insertar el alimento en la base de datos
                    string SQLInsertAlimento = "INSERT INTO ALIMENTO (id_categoria_alimento, nombre, calorias_x_gramo, grasa, carbohidrato, proteina, fibra) " +
                                                "VALUES (@IdCategoria, @Nombre, @CaloriasPorGramo, @Grasa, @Carbohidrato, @Proteina, @Fibra)";
                    using (var commandInsertAlimento = new SqlCommand(SQLInsertAlimento, conexion.Conexion()))
                    {
                        // Agregamos los parámetros necesarios para la consulta SQL
                        commandInsertAlimento.Parameters.AddWithValue("@IdCategoria", idCategoria); // Asignamos el ID de la categoría obtenido
                        commandInsertAlimento.Parameters.AddWithValue("@Nombre", alimento.NombreAlimento); // Asignamos el nombre del alimento
                        commandInsertAlimento.Parameters.AddWithValue("@CaloriasPorGramo", alimento.CaloriasPorGramo); // Asignamos las calorías por gramo
                        commandInsertAlimento.Parameters.AddWithValue("@Grasa", alimento.Grasa); // Asignamos la cantidad de grasa
                        commandInsertAlimento.Parameters.AddWithValue("@Carbohidrato", alimento.Carbohidrato); // Asignamos la cantidad de carbohidratos
                        commandInsertAlimento.Parameters.AddWithValue("@Proteina", alimento.Proteina); // Asignamos la cantidad de proteínas
                        commandInsertAlimento.Parameters.AddWithValue("@Fibra", alimento.Fibra); // Asignamos la cantidad de fibra

                        // Ejecutamos la consulta SQL
                        commandInsertAlimento.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Manejamos cualquier excepción y mostramos un mensaje de error
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    // Desconectamos de la base de datos
                    conexion.Disconnect();
                }
         }

      
        // Método para obtener el ID de la categoría del alimento
        private int ObtenerIdCategoriaAlimento(string categoriaAlimento, DBContextUtility conexion)
        {
            // Consulta SQL para obtener el ID de la categoría del alimento
            string SQLGetCategoryId = "SELECT id_categoria_alimento FROM CATEGORIA_ALIMENTO WHERE categoria = @CategoriaAlimento";

            // Creamos un nuevo comando SQL utilizando la consulta y la conexión a la base de datos
            using (var command = new SqlCommand(SQLGetCategoryId, conexion.Conexion()))
            {
                // Agregamos el parámetro para la categoría del alimento a la consulta SQL
                command.Parameters.AddWithValue("@CategoriaAlimento", categoriaAlimento);

                // Ejecutamos la consulta y obtenemos el resultado (el ID de la categoría)
                var result = command.ExecuteScalar();

                // Verificamos si el resultado es válido y no es nulo
                if (result != null && result != DBNull.Value)
                {
                    // Convertimos el resultado en un entero y lo devolvemos como el ID de la categoría
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Si la categoría no existe, llamamos al método para insertarla en la base de datos
                    return InsertarCategoriaAlimento(categoriaAlimento, conexion);
                }
            }
        }

        private int InsertarCategoriaAlimento(string categoriaAlimento, DBContextUtility conexion)
        {
            // Consulta SQL para insertar la nueva categoría del alimento
            string SQLInsertCategory = "INSERT INTO CATEGORIA_ALIMENTO (categoria) VALUES (@CategoriaAlimento); SELECT SCOPE_IDENTITY();";

            // Creamos un nuevo comando SQL utilizando la consulta y la conexión a la base de datos
            using (var commandInsert = new SqlCommand(SQLInsertCategory, conexion.Conexion()))
            {
                // Agregamos el parámetro para la categoría del alimento a la consulta SQL
                commandInsert.Parameters.AddWithValue("@CategoriaAlimento", categoriaAlimento);

                // Ejecutamos la consulta y obtenemos el resultado (el ID de la nueva categoría)
                var result = commandInsert.ExecuteScalar();

                // Verificamos si el resultado es válido y no es nulo
                if (result != null && result != DBNull.Value)
                {
                    // Convertimos el resultado en un entero y lo devolvemos como el ID de la nueva categoría
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Si el resultado es nulo o inválido, lanzamos una excepción indicando un error al insertar la categoría
                    throw new Exception("Error al insertar la categoría del alimento en la base de datos.");
                }
            }
        }
    }
}
