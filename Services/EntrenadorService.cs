using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
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
}