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
        public int registroAlimento(AlimentoDto alimento)
        {
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO Alimento (id_categoria_alimento,nombre,calorias_x_gramo,grasa,carbohidrato,proteina,fibra)"
                            + "VALUES (@id_categoria_alimento,@nombre,@calorias_x_gramo,@grasa,@carbohidrato,@proteina,@fibra)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_categoria_alimento", alimento.id_categoria_alimento);
                    command.Parameters.AddWithValue("@nombre", alimento.nombre);
                    command.Parameters.AddWithValue("@calorias_x_gramo", alimento.calorias_x_gramo);
                    command.Parameters.AddWithValue("@grasa", alimento.grasa);
                    command.Parameters.AddWithValue("@carbohidrato", alimento.carbohidrato);
                    command.Parameters.AddWithValue("@proteina", alimento.proteina);
                    command.Parameters.AddWithValue("@fibra", alimento.fibra);

                    command.ExecuteNonQuery();
                }
                conexion.Disconnect();
                comando = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return comando;
        }
        public List<AlimentoDto> MostrarAlimento()
        {
            List<AlimentoDto> Alimentos = new List<AlimentoDto>();
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();

            string SQL = "SELECT id_categoria_alimento,nombre,calorias_x_gramo,grasa,carbohidrato,proteina,fibra FROM ALIMENTO";

            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlimentoDto alimento = new AlimentoDto();   
                        {
                            alimento.id_categoria_alimento = Convert.ToInt32(reader["id_categoria_alimento"]);
                            alimento.nombre = reader["nombre"].ToString();
                            alimento.calorias_x_gramo = Convert.ToDouble(reader["calorias_x_gramo"]);
                            alimento.grasa = Convert.ToDouble(reader["grasa"]);
                            alimento.carbohidrato = Convert.ToDouble(reader["carbohidrato"]);
                            alimento.proteina = Convert.ToDouble(reader["proteina"]);
                            alimento.fibra = Convert.ToDouble(reader["fibra"]);
                        }

                        Alimentos.Add(alimento);
                    }
                }
            }
            conexion.Disconnect();
            return Alimentos;
        }
        public int EliminarAlimento(string nombre)
        {
            int filasAfectadas = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM ALIMENTO WHERE nombre = @nombre";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))

                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.ExecuteNonQuery();
                }
                filasAfectadas = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conexion.Disconnect();
            }

            return filasAfectadas;
        }
        public int ActualizarAlimento(AlimentoDto alimento)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();
                string SQL = "UPDATE ALIMENTO SET nombre= @nombre,calorias_x_gramo = @calorias_x_gramo, grasa=@grasa, carbohidrato=@carbohidrato, proteina=@proteina,fibra=@fibra  " + "WHERE nombre = @nombre";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@nombre", alimento.nombre);
                    command.Parameters.AddWithValue("@calorias_x_gramo",alimento.calorias_x_gramo);
                    command.Parameters.AddWithValue("@grasa", alimento.grasa);
                    command.Parameters.AddWithValue("@carbohidrato", alimento.carbohidrato);
                    command.Parameters.AddWithValue("@proteina", alimento.proteina);
                    command.Parameters.AddWithValue("@fibra", alimento.fibra);
                    command.ExecuteNonQuery();
                }
                comando = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conexion.Disconnect();
            }



            return comando;
        }
        public bool buscarAlimento(string nombre)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();
            string SQL = "SELECT COUNT(*) FROM ALIMENTO WHERE nombre = @nombre";
            int AlimentoEncontrado = 0;
            using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
            {
                command.Parameters.AddWithValue("@nombre", nombre);
                AlimentoEncontrado = (int)command.ExecuteScalar();
            }
            conexion.Disconnect();
            if (AlimentoEncontrado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
}
}