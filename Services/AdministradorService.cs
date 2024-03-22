using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Services
{
    public class AdministradorService
    {
        public PersonaDto registrarAdministrador(PersonaDto administrador)
        {
            PersonaDto administradorResp = new PersonaDto();
            AdministradorRepository administradorRepository = new AdministradorRepository();


            if (administradorRepository.buscarAdministrador(administrador.correo))
            {
                administradorResp.respuesta = 0;
                administradorResp.mensaje = "Ya existe el usuario";
            }
            else
            {

                administrador.id_rol = 3;


                int resultadoRegistro = administradorRepository.registrarAdministrador(administrador);

                if (resultadoRegistro != 0)
                {
                    administradorResp.respuesta = 1;
                    administradorResp.mensaje = "Se ha registrado el usuario correctamente";
                }
                else
                {
                    administradorResp.respuesta = 0;
                    administradorResp.mensaje = "Error en el registro del usuario";
                }
            }
            return administradorResp;
        }


        //public PersonaDto logueo(PersonaDto administrador)
        //{
        //    AdministradorRepository administradorRepository = new AdministradorRepository();
        //    PersonaDto administradorResp = administradorRepository.IniciarSesion(administrador.correo, administrador.contrasena);

        //    if (administradorResp.respuesta == 1)
        //    {
        //        administradorResp.mensaje = "Inicio de sesión correcto";
        //    }
        //    else
        //    {
        //        administradorResp.mensaje = "Inicio de sesión incorrecto";
        //    }

        //    return administradorResp;
        //}
    }
}
