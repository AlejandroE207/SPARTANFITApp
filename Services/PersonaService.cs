using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Services
{
    public class PersonaService
    {

        public UsuarioDto mapeoPersona_Usuario(PersonaDto pers)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            usuario.persona.id_usuario = pers.id_usuario;
            usuario.persona.id_rol = pers.id_rol;
            usuario.persona.nombres = pers.nombres;
            usuario.persona.apellidos = pers.apellidos;
            usuario.persona.fecha_nacimiento = pers.fecha_nacimiento;
            usuario.persona.correo = pers.correo;
            usuario.persona.contrasena = pers.contrasena;
            usuario.persona.genero = pers.genero;
            usuario.persona.respuesta = pers.respuesta;
            usuario.persona.mensaje = pers.mensaje;

            return usuario;
        }
        public PersonaDto logueo(PersonaDto persona)
        {
            //EntrenadorRepository PersonaRepository = new EntrenadorRepository();
            PersonaRepository personaRepository = new PersonaRepository();
            PersonaDto personaResp = personaRepository.IniciarSesion(persona.correo, persona.contrasena);

            if (personaResp.respuesta == 1)
            {
                personaResp.mensaje = "Inicio de sesión correcto";
            }
            else
            {
                personaResp.mensaje = "Inicio de sesión incorrecto";
            }
            return personaResp;
        }
    }
}