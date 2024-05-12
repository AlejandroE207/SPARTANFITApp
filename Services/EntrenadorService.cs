
using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Services
{
    public class EntrenadorService
    {
        public PersonaDto registrarEntrenador(PersonaDto entrenador)
        {
            PersonaDto EntrenadorResp = new PersonaDto();
            EntrenadorRepository entrenadorRepository = new EntrenadorRepository();


            if (entrenadorRepository.buscarEntrenador(entrenador.correo))
            {
                EntrenadorResp.respuesta = 0;
                EntrenadorResp.mensaje = "Ya existe el usuario";
            }
            else
            {

                entrenador.id_rol = 2;


                int resultadoRegistro = entrenadorRepository.registroEntrenador(entrenador);

                if (resultadoRegistro != 0)
                {
                    EntrenadorResp.respuesta = 1;
                    EntrenadorResp.mensaje = "Se ha registrado el usuario correctamente";
                }
                else
                {
                    EntrenadorResp.respuesta = 0;
                    EntrenadorResp.mensaje = "Error en el registro del usuario";
                }
            }
            return EntrenadorResp;
        }
        public List<EjercicioDto> Mostrar_Ejercicio()
        {
            EjercicioRepository ejercicioRepository = new EjercicioRepository();
            var lista_Ejercicios = ejercicioRepository.MostrarEjercicio();
            return lista_Ejercicios;
        }

        public int EliminarEjercicio(int id_ejercicio)
        {
            int filasAfectadas = 0;
            EjercicioRepository ejercicioRepository = new EjercicioRepository();

            try
            {
                filasAfectadas = ejercicioRepository.EliminarEjercicio(id_ejercicio);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return filasAfectadas;
        }

        public int ActualizarEjercicio(EjercicioDto ejercicio)
        {
            int filasAfectadas = 0;
            EjercicioRepository ejercicioRepository = new EjercicioRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            ejercicio.nombre_ejercicio = sintetizarFormularios.Sintetizar(ejercicio.nombre_ejercicio);
            ejercicio.apoyo_visual = sintetizarFormularios.Sintetizar(ejercicio.apoyo_visual);

            try { 
            
                filasAfectadas = ejercicioRepository.ActualizarEjercicio(ejercicio);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return filasAfectadas;
        }
        public EjercicioDto registrarEjercicio(EjercicioDto ejercicio)
        {
            EjercicioDto EjercicioResp = new EjercicioDto();
            EjercicioRepository ejercicioRepository = new EjercicioRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            ejercicio.nombre_ejercicio = sintetizarFormularios.Sintetizar(ejercicio.nombre_ejercicio);
            ejercicio.apoyo_visual = sintetizarFormularios.Sintetizar(ejercicio.apoyo_visual);

            if (ejercicioRepository.buscarEjercicio(ejercicio.nombre_ejercicio))
            {
                EjercicioResp.respuesta = 0;
                EjercicioResp.mensaje = "Ya existe el ejercicio";
            }
            else
            {    
                int resultadoRegistro = ejercicioRepository.registroEjercicio(ejercicio);

               
            }
            return EjercicioResp;
        }
        public List<AlimentoDto> Mostrar_Alimento()
        {
            AlimentoRepository alimentoRepository = new AlimentoRepository();
            var lista_Alimentos = alimentoRepository.MostrarAlimento();
            return lista_Alimentos;
        }

        public int EliminarAlimento(int id_alimento)
        {
            int filasAfectadas = 0;
            AlimentoRepository alimentoRepository = new AlimentoRepository();

            try
            {
                filasAfectadas = alimentoRepository.EliminarAlimento(id_alimento);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return filasAfectadas;
        }

        public int ActualizarAlimento(AlimentoDto alimento)
        {
            int filasAfectadas = 0;
            AlimentoRepository alimentoRepository = new AlimentoRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            alimento.nombre = sintetizarFormularios.Sintetizar(alimento.nombre);

            try
            {
                filasAfectadas = alimentoRepository.ActualizarAlimento(alimento);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return filasAfectadas;
        }
        public AlimentoDto registrarAlimento(AlimentoDto alimento)
        {
            AlimentoDto AlimentoResp = new AlimentoDto();
            AlimentoRepository alimentoRepository = new AlimentoRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            alimento.nombre = sintetizarFormularios.Sintetizar(alimento.nombre);

            if (alimentoRepository.buscarAlimento(alimento.nombre))
            {
                AlimentoResp.respuesta = 0;
                AlimentoResp.mensaje = "Ya existe el alimento";
            }
            else
            {
                int resultadoRegistro = alimentoRepository.registroAlimento(alimento);


            }
            return AlimentoResp;
        }

        public int registrarRutina(RutinaDto rutina, List<EjercicioDto> ejerciciosRutina)
        {
            RutinaDto rutinaResp = new RutinaDto(); ;
            RutinaRepository rutinaRepository = new RutinaRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            rutina.nombre_rutina = sintetizarFormularios.Sintetizar(rutina.nombre_rutina);
            rutina.descripcion = sintetizarFormularios.Sintetizar(rutina.descripcion);



            int id_rutina = rutinaRepository.regitrarRutina(rutina);

            int registroEjerciciosRutina = rutinaRepository.registrarEjerciciosRutina(ejerciciosRutina, id_rutina);


            return registroEjerciciosRutina;
        }
        
        public int registrarPlanNutricional(PlanAlimenticioDto planAlimenticio, List<int> idAlimentos) 
        {
            PlanAlimenticioDto planResp = new PlanAlimenticioDto();
            PlanAlimenticioRepository planRepository = new PlanAlimenticioRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            planAlimenticio.nombre = sintetizarFormularios.Sintetizar(planAlimenticio.nombre);
            planAlimenticio.descripcion = sintetizarFormularios.Sintetizar(planAlimenticio.descripcion);

            int id_plan_alimenticio = planRepository.registrarPlan(planAlimenticio);

            int registroAlimentoPlan = planRepository.registrarAlimentoPlan(idAlimentos, id_plan_alimenticio);

            return registroAlimentoPlan;
        }
    
        public List<RutinaDto> MostrarRutinas() 
        {
            List<RutinaDto> rutinas = new List<RutinaDto>();
            RutinaRepository rutinaRepository = new RutinaRepository();

            rutinas = rutinaRepository.MostrasRutinas();

            return rutinas;
        }

        public RutinaDto EliminarRutina(int id_rutina)
        {
            RutinaDto rutinaResp = new RutinaDto();
            RutinaRepository rutinaRepository = new RutinaRepository();

            rutinaResp = rutinaRepository.EliminarRutina(id_rutina);

            return rutinaResp;
        }

        public List<PlanAlimenticioDto> MostrarPlanes()
        {
            List<PlanAlimenticioDto> planes = new List<PlanAlimenticioDto>();
            PlanAlimenticioRepository planAlimenticioRepository = new PlanAlimenticioRepository();

            planes = planAlimenticioRepository.MostrarPlanes();

            return planes;
        }

        public PlanAlimenticioDto EliminarPlan(int id_plan_alimenticio)
        {
            PlanAlimenticioDto rutinaResp = new PlanAlimenticioDto();
            PlanAlimenticioRepository planAlimenticioRepository = new PlanAlimenticioRepository();

            rutinaResp = planAlimenticioRepository.EliminarPlan(id_plan_alimenticio);

            return rutinaResp;
        }
    }
    
}
