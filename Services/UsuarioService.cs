using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Services
{
    public class UsuarioService
    {
        public UsuarioDto registroUsuario(UsuarioDto usuario)
        {
            UsuarioDto usuarioResp = new UsuarioDto();
            UsuarioDto usuarioAux = new UsuarioDto();

            usuarioResp.persona = new PersonaDto();
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            RutinaRepository rutinaRepository = new RutinaRepository();
            PlanAlimenticioRepository planAlimenticioRepository = new PlanAlimenticioRepository();

          
            if (usuarioRepository.buscarUsuario(usuario.persona.correo))
            {
                usuarioResp.persona.respuesta = 0;
                usuarioResp.persona.mensaje = "Ya existe el usuario";
            }
            else
            {
             
                usuario.persona.id_rol = 1;
                EncriptarContrasenaUtility encrip = new EncriptarContrasenaUtility();
                usuario.persona.contrasena = encrip.EncriptarContrasena(usuario.persona);
                
                usuarioResp = usuarioRepository.registroUsuario(usuario);

                if (usuarioResp.persona.respuesta != 0)
                {

                    usuarioAux = usuarioRepository.buscarPorId(usuarioResp.persona.id_usuario);
                    int resultado = rutinaRepository.asignarRutina(usuarioAux);
                    int resultadoAlimento = planAlimenticioRepository.asignarPlanAlimenticio(usuarioAux);
                    if(resultado != 0 && resultadoAlimento != 0)
                    {
                        usuarioResp.persona.respuesta = 1;
                        usuarioResp.persona.mensaje = "Se ha registrado el usuario correctamente";
                    }
                    else
                    {
                        usuarioResp.persona.respuesta = 0;
                        usuarioResp.persona.mensaje = "Error al momento de asignar rutina";
                    }


                }
                else
                {
                    usuarioResp.persona.respuesta = 0;
                    usuarioResp.persona.mensaje = "Error en el registro del usuario";
                }


            }
            return usuarioResp;
        }

        public (string,string,string) ConvertirIdText(UsuarioDto usuario)
        {
            string rehabilitacionText = "";
            string nivelEntrenamiento = "";
            string objetivo = "";

            switch(usuario.rehabilitacion)
            {
                case 0:
                    rehabilitacionText = "Si";
                    break;
                case 1:
                    rehabilitacionText = "No";
                    break;
            }

            switch (usuario.id_nivel_entrenamiento)
            {
                case 0:
                    nivelEntrenamiento = "No aplica";
                    break;
                case 1:
                    nivelEntrenamiento = "Basico";
                    break;
                case 2:
                    nivelEntrenamiento = "Medio";
                    break;
                case 3:
                    nivelEntrenamiento = "Avanzado";
                    break;
            }

            switch(usuario.id_objetivo)
            {
                case 0:
                    objetivo = "No aplica";
                    break;
                case 1:
                    objetivo = "Definicion";
                    break;
                case 2:
                    objetivo = "Mantenimiento";
                    break;
                case 3:
                    objetivo = "Hipertrofia";
                    break;
                case 4:
                    objetivo = "Fuerza";
                    break;
                case 5:
                    objetivo = "Rehabilitacion";
                    break;

            }

            return (rehabilitacionText, nivelEntrenamiento,objetivo);
        }
        public UsuarioDto logueo(UsuarioDto usuario,string contraNormal)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            UsuarioDto usuarioResp = usuarioRepository.IniciarSesion(usuario,contraNormal);

            if (usuarioResp.persona.respuesta  !=0)
            {
                usuarioResp.persona.mensaje = "Inicio de sesión correcto";
            }
            else
            {
                usuarioResp.persona.mensaje = "Inicio de sesión incorrecto";
            }

            return usuarioResp;
        }

        public UsuarioDto actualizarObjetivo(UsuarioDto usuario)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            RutinaRepository rutinaRepository = new RutinaRepository();
            PlanAlimenticioRepository planAlimenticioRepository = new PlanAlimenticioRepository();

            UsuarioDto usuarioResp = new UsuarioDto();
            usuarioResp.persona = new PersonaDto();
            int resultado = usuarioRepository.ActualizarObjetivoUsuario(usuario);
            

            if(resultado != 0)
            {
                int resultadoAux = rutinaRepository.asignarRutina(usuario);
                int resultadoAlimento = planAlimenticioRepository.asignarPlanAlimenticio(usuario);
                if (resultadoAux != 0 && resultadoAlimento != 0)
                {
                    usuarioResp.persona.respuesta = 1;
                    usuarioResp.persona.mensaje = "Actualización de objetivo y asignacion de rutina exitoso";
                }
                else
                {
                    usuarioResp.persona.respuesta = 0;
                    usuarioResp.persona.mensaje = "Actualización de objetivo y asignacion de rutina fallida";
                }
                

            }
            else
            {
                usuarioResp.persona.respuesta = 0;
                usuarioResp.persona.mensaje = "Error de Actualizacion";
            }

            return usuarioResp;

        }
        public UsuarioDto actualizarDatosUsuario(UsuarioDto usuario)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            UsuarioDto usuarioResp = new UsuarioDto();
            usuarioResp.persona = new PersonaDto();
            int resultado = usuarioRepository.ActualizarDatosUsuario(usuario);

            if (resultado != 0)
            {
                usuarioResp.persona.respuesta = 1;
                usuarioResp.persona.mensaje = "Actualización Exitosa";
            }
            else
            {
                usuarioResp.persona.respuesta = 0;
                usuarioResp.persona.mensaje = "Error de Actualizacion";
            }

            return usuarioResp;

        }
        public UsuarioDto eliminarUsuario(UsuarioDto usuario)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            UsuarioDto usuarioResp = new UsuarioDto();
            usuarioResp.persona = new PersonaDto();
            int resultado = usuarioRepository.EliminarUsuario(usuario);

            if (resultado != 0)
            {
                usuarioResp.persona.respuesta = 1;
                usuarioResp.persona.mensaje = "Actualización Exitosa";
            }
            else
            {
                usuarioResp.persona.respuesta = 0;
                usuarioResp.persona.mensaje = "Error de Actualizacion";
            }

            return usuarioResp;
        }
        
        public (RutinaDto,List<EjercicioDto>)  mostrarRutinaDia(UsuarioDto usuario)
        {
            List<EjercicioDto> ejerciciosDia = new List<EjercicioDto> ();
            RutinaDto rutinaResp = new RutinaDto();
            RutinaRepository rutinaRepository = new RutinaRepository();
            IdentificadorDiaUtility identDia = new IdentificadorDiaUtility();

            string dia = identDia.DiaActual();

            rutinaResp = rutinaRepository.buscarRutinaIdUsuario(usuario.persona.id_usuario, dia);
            ejerciciosDia = rutinaRepository.ObtenerEjerciciosDia(rutinaResp.id_rutina);

            return (rutinaResp,ejerciciosDia);
        }

        public (PlanAlimenticioDto,List<AlimentoDto>) mostrarPlanNutricionalDia(UsuarioDto usuario)
        {
            List<AlimentoDto> alimentosDia = new List<AlimentoDto>();
            PlanAlimenticioDto planAlimenticio = new PlanAlimenticioDto();
            PlanAlimenticioRepository planRepository = new PlanAlimenticioRepository();

            IdentificadorDiaUtility identDia = new IdentificadorDiaUtility ();
            string dia = identDia.DiaActual();
            planAlimenticio = planRepository.buscarPlanIdUsuario(usuario.persona.id_usuario, dia);

            alimentosDia = planRepository.obtenerAlimentosDia(planAlimenticio.id_plan_alimenticio);

            return(planAlimenticio, alimentosDia);
        }


    }
}