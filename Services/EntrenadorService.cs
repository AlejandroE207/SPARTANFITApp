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
    }

    //public PersonaDto logueo(PersonaDto entrenador)
    //{
    //    EntrenadorRepository entrenadorRepository = new EntrenadorRepository();
    //    PersonaDto entrenadorResp = entrenadorRepository.IniciarSesion(entrenador.correo, entrenador.contrasena);

    //    if (entrenadorResp.respuesta == 1)
    //    {
    //        entrenadorResp.mensaje = "Inicio de sesión correcto";
    //    }
    //    else
    //    {
    //        entrenadorResp.mensaje = "Inicio de sesión incorrecto";
    //    }

    //    return entrenadorResp;
    //}
}
