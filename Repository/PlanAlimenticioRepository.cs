using Org.BouncyCastle.Asn1.Mozilla;
using SPARTANFITApp.Dto;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Repository
{
    public class PlanAlimenticioRepository
    {
        public int registrarPlan(PlanAlimenticioDto planAlimenticio)
        {
            int comando = 0;
            try
            {
                DBContextUtility conexion = new DBContextUtility();
                conexion.Connect();

                string SQL = "INSERT INTO PLAN_ALIMENTICIO (nombre, dia, id_entrenador)"
                            + "VALUES (@nombre, @dia, @id_entrenador)";


                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {

                    command.Parameters.AddWithValue("@nombre", planAlimenticio.nombre);
                    command.Parameters.AddWithValue("@dia", planAlimenticio.dia);
                    command.Parameters.AddWithValue("@id_entrenador", planAlimenticio.id_entrenador);


                    command.ExecuteNonQuery();
                    comando = 1;
                }
                conexion.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            PlanAlimenticioDto planResp = new PlanAlimenticioDto();

            planResp = obtenerUltimoPlan();
            if (planResp.respuesta != 0)
            {
                comando = planResp.id_plan_alimenticio;
            }
            else
            {
                comando = -1;
            }


            return comando;
        }

        public PlanAlimenticioDto obtenerUltimoPlan()
        {
            PlanAlimenticioDto planAlimenticio = new PlanAlimenticioDto();
            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "SELECT MAX(id_plan_alimenticio) as id_max FROM PLAN_ALIMENTICIO";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            planAlimenticio.id_plan_alimenticio = Convert.ToInt32(reader["id_max"]);
                            planAlimenticio.respuesta = 1;
                            planAlimenticio.mensaje = "Obtencion con exito";
                            return planAlimenticio;
                        }
                        else
                        {
                            planAlimenticio.respuesta = 0;
                            planAlimenticio.mensaje = "Error al momento de obtener id";
                            return planAlimenticio;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                planAlimenticio.respuesta = -1;
                planAlimenticio.mensaje = "Error al obtener id: " + ex.Message;

            }
            finally
            {
                conexion.Disconnect();
            }
            return planAlimenticio;
        }

        public int registrarAlimentoPlan(List<int> idAlimentos, int id_plan_alimenticio)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            try
            {

                conexion.Connect();

                foreach (int alimento in idAlimentos)
                {
                    string sql = "INSERT INTO PLAN_ALIMENTO (id_plan_alimenticio, id_alimento)" +
                                "VALUES (@id_plan_alimenticio, @id_alimento)";

                    using (SqlCommand command = new SqlCommand(sql, conexion.Conexion()))
                    {
                        command.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                        command.Parameters.AddWithValue("@id_alimento", alimento);

                        command.ExecuteNonQuery();
                    }
                    comando = 1;
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

            return comando;
        }

        public int asignarPlanAlimenticio(UsuarioDto usuario)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();
            double TMB = 0;
            try
            {
                DateTime fechaActual = DateTime.Now;
                int añoActual = fechaActual.Year;
                DateTime fechaNacimiento = DateTime.Parse(usuario.persona.fecha_nacimiento);
                int añoNacimiento = fechaNacimiento.Year;
                double calorias = 0;
                double edad = añoActual - añoNacimiento;
                if (usuario.persona.genero == "masculino")
                {

                    TMB = 66 + (13.7 * usuario.peso) + (5 * usuario.estatura) - (6.8 * edad);
                    switch (usuario.id_objetivo)
                    {
                        case 1:
                            calorias = TMB + 734;
                            break;
                        case 2:
                            calorias = TMB + 1270;
                            break;
                        case 3:
                            calorias = TMB + 1810;
                            break;
                        case 4:
                            calorias = TMB + 1810;
                            break;
                    }
                }
                else
                {
                    TMB = 665 + (9.6 * usuario.peso) + (1.8 * usuario.estatura) - (4.7 * edad);
                    switch (usuario.id_objetivo)
                    {
                        case 1:
                            calorias = TMB + 606;
                            break;
                        case 2:
                            calorias = TMB + 1050;
                            break;
                        case 3:
                            calorias = TMB + 1270;
                            break;
                        case 4:
                            calorias = TMB + 1270;
                            break;
                    }
                }

                double carbohidratos = calorias * 0.55;
                double proteinas = calorias * 0.25;
                double grasas = calorias * 0.20;

                string descripcion = "Ten encuenta " + usuario.persona.nombres + " que tienes que consumir la cantidad de " + calorias + " diarias, distribuidas en las siguientes cantidades calorias de macronutrientes: \n" +
                                    "Carbohidratos: " + carbohidratos + "\nProteinas: " + proteinas + "\nGrasas: " + grasas;
                List<String> dias = new List<String> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };
                conexion.Connect();
                foreach (string dia in dias)
                {
                    string SQL = "SELECT TOP (1) id_plan_alimenticio FROM PLAN_ALIMENTICIO WHERE dia = @dia";
                    using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                    {
                        command.Parameters.AddWithValue("@dia", dia);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int id_plan_alimenticio = Convert.ToInt32(reader["id_plan_alimenticio"]);
                                comando = asignarPlanAlimenticioDia(usuario.persona.id_usuario, id_plan_alimenticio, descripcion);
                            }
                            else
                            {
                                comando = -1;
                            }
                        }
                    }
                }
                return comando;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

        public int asignarPlanAlimenticioDia(int id_usuario, int id_plan_alimenticio, string descripcion)
        {
            int comando = 0;
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "INSERT INTO USUARIO_PLAN_ALIMENTICIO (id_usuario,id_plan_alimenticio,descripcion) VALUES (@id_usuario,@id_plan_alimenticio,@descripcion)";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                    command.Parameters.AddWithValue("@descripcion", descripcion);
                    comando = command.ExecuteNonQuery();
                    comando = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

        public PlanAlimenticioDto buscarPlanIdUsuario(int id_usuario, string dia)
        {
            PlanAlimenticioDto planAlimenticio = new PlanAlimenticioDto();
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT pa.id_plan_alimenticio, pa.nombre, pa.dia, usp.descripcion " +
                    "FROM PLAN_ALIMENTICIO AS pa " +
                    "INNER JOIN USUARIO_PLAN_ALIMENTICIO AS usp ON pa.id_plan_alimenticio = usp.id_plan_alimenticio " +
                    "WHERE usp.id_usuario = @id_usuario AND pa.dia = @dia";

                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@dia", dia);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            planAlimenticio.id_plan_alimenticio = Convert.ToInt32(reader["id_plan_alimenticio"]);
                            planAlimenticio.nombre = reader["nombre"].ToString();
                            planAlimenticio.dia = reader["dia"].ToString();
                            planAlimenticio.descripcion = reader["descripcion"].ToString();
                            planAlimenticio.respuesta = 1;
                            planAlimenticio.mensaje = "Busqueda exitosa";
                            return planAlimenticio;
                        }
                        else
                        {
                            planAlimenticio.respuesta = 0;
                            planAlimenticio.mensaje = "No se encontro el plan nutricional";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conexion.Disconnect();
            }
            return planAlimenticio;
        }

        public List<AlimentoDto> obtenerAlimentosDia(int id_plan_alimenticio)
        {
            List<AlimentoDto> alimentosDia = new List<AlimentoDto>();
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT a.id_alimento, a.id_categoria_alimento, a.nombre,a.calorias_x_gramo, a.grasa, a.carbohidrato, a.proteina " +
                    "FROM ALIMENTO AS a " +
                    "INNER JOIN PLAN_ALIMENTO AS pa ON pa.id_alimento = a.id_alimento " +
                    "WHERE pa.id_plan_alimenticio = @id_plan_alimenticio";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AlimentoDto alimento = new AlimentoDto()
                            {
                                id_alimento = Convert.ToInt32(reader["id_alimento"]),
                                id_categoria_alimento = Convert.ToInt32(reader["id_categoria_alimento"]),
                                nombre = reader["nombre"].ToString(),
                                calorias_x_gramo = Convert.ToDouble(reader["calorias_x_gramo"]),
                                grasa = Convert.ToDouble(reader["grasa"]),
                                carbohidrato = Convert.ToDouble(reader["carbohidrato"]),
                                proteina = Convert.ToDouble(reader["proteina"])
                            };
                            alimentosDia.Add(alimento);
                        }
                    }
                    return alimentosDia;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conexion.Disconnect();
            }
            return alimentosDia;
        }

        public List<PlanAlimenticioDto> MostrarPlanes()
        {
            List<PlanAlimenticioDto> planes = new List<PlanAlimenticioDto>();
            DBContextUtility conexion = new DBContextUtility();

            try
            {
                conexion.Connect();
                string SQL = "SELECT id_plan_alimenticio, nombre, dia FROM PLAN_ALIMENTICIO";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlanAlimenticioDto plan = new PlanAlimenticioDto()
                            {
                                id_plan_alimenticio = Convert.ToInt32(reader["id_plan_alimenticio"]),
                                nombre = reader["nombre"].ToString(),
                                dia = reader["dia"].ToString()
                            };
                            planes.Add(plan);
                        }
                    }
                    return planes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return planes;
        }


        public PlanAlimenticioDto EliminarPlan(int id_plan_alimenticio)
        {
            PlanAlimenticioDto planResp = new PlanAlimenticioDto();
            DBContextUtility conexion = new DBContextUtility();
            int comando = 0;
            try
            {
                comando = EliminarUsuarioPlanAlimenticio(id_plan_alimenticio);
                if (comando != 0)
                {
                    comando = EliminarPlanAlimento(id_plan_alimenticio);
                    if (comando != 0)
                    {
                        conexion.Connect();
                        string SQL = "DELETE FROM PLAN_ALIMENTICIO WHERE id_plan_alimenticio = @id_plan_alimenticio";
                        using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                        {
                            command.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                            comando = command.ExecuteNonQuery();
                            planResp.respuesta = 1;
                            planResp.mensaje = "Rutina eliminada con exito";
                            return planResp;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return planResp;
        }


        public int EliminarUsuarioPlanAlimenticio(int id_plan_alimenticio)
        {
            int comando = 0;

            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM USUARIO_PLAN_ALIMENTICIO WHERE id_plan_alimenticio = @id_plan_alimenticio";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                    comando = command.ExecuteNonQuery();
                    comando = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

        public int EliminarPlanAlimento(int id_plan_alimenticio)
        {
            int comando = 0;

            DBContextUtility conexion = new DBContextUtility();
            try
            {
                conexion.Connect();
                string SQL = "DELETE FROM PLAN_ALIMENTO WHERE id_plan_alimenticio = @id_plan_alimenticio";
                using (SqlCommand command = new SqlCommand(SQL, conexion.Conexion()))
                {
                    command.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                    comando = command.ExecuteNonQuery();
                    comando = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { conexion.Disconnect(); }
            return comando;
        }

    }
}