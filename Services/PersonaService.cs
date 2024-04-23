using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

            //PRUEBA DE SOLUCION
            if(personaResp == null)
            {
                personaResp.mensaje = "Inicio de sesión incorrecto";
            }
            //FIN PRUEBA

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
        public PersonaDto enviarCodigo(String correo)
        {
            PersonaDto persona = new PersonaDto(); 
            CorreoUtility correoUtility = new CorreoUtility();
            Recuperacion_contrasenaRepository recuperacion_ContrasenaRepository= new Recuperacion_contrasenaRepository();
            PersonaRepository personaRepository = new PersonaRepository();
            GeneradorCodigoUtility generadorCodigo=new GeneradorCodigoUtility();
            if (personaRepository.buscarPersona(correo))
            {
                persona = personaRepository.SeleccionarPersona(correo);
                correoUtility.enviarCorreoContrasena(correo);
                String codigo = generadorCodigo.NumeroAleatorio().ToString();
                recuperacion_ContrasenaRepository.registroRecuperacion(persona.id_usuario,codigo);

                return persona;
            }
            else
            {
            return persona;
            }   
        }

        public int ActualizarContrasena (String correo, String contrasena,String codigo)
        {
            
            int filasAfectadas = 0;
            PersonaRepository personaRepository = new PersonaRepository();
            Recuperacion_ContrasenaDto recuperacion=new Recuperacion_ContrasenaDto();
            if(codigo==recuperacion.codigo)
            {
              personaRepository.ActualizarContrasena(correo,contrasena);
            }
            return filasAfectadas;
        }
      
    }
}