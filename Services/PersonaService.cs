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
            PersonaRepository personaRepository = new PersonaRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            persona.correo = sintetizarFormularios.Sintetizar(persona.correo);
            persona.contrasena = sintetizarFormularios.Sintetizar(persona.contrasena);
            PersonaDto personaResp = personaRepository.IniciarSesion(persona.correo, persona.contrasena);

            //PRUEBA DE SOLUCION
            if (personaResp == null)
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
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            CorreoUtility correoUtility = new CorreoUtility();
            Recuperacion_contrasenaRepository recuperacion_ContrasenaRepository = new Recuperacion_contrasenaRepository();
            PersonaRepository personaRepository = new PersonaRepository();
            GeneradorCodigoUtility generadorCodigo = new GeneradorCodigoUtility();
            correo = sintetizarFormularios.Sintetizar(correo);

            if (personaRepository.buscarPersona(correo))
            {
               
                persona = personaRepository.SeleccionarPersona(correo);
                recuperacion_ContrasenaRepository.EliminarCodigo(persona.id_usuario);
                string codigo = generadorCodigo.NumeroAleatorio().ToString();
                correoUtility.enviarCorreoContrasena(correo,codigo);
                
                recuperacion_ContrasenaRepository.registroRecuperacion(persona.id_usuario,codigo);

                return persona;
            }
            else
            {
            return persona;
            }   
        }

        public int ActualizarContrasena(String correo, String contrasena, String codigo)
        {

            int filasAfectadas = 0;
            PersonaRepository personaRepository = new PersonaRepository();
            PersonaDto persona = new PersonaDto();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            Recuperacion_ContrasenaDto recuperacion = new Recuperacion_ContrasenaDto();
            correo = sintetizarFormularios.Sintetizar(correo);
            persona = personaRepository.SeleccionarPersona(correo);
            Recuperacion_contrasenaRepository recuperacion_ContrasenaRepository = new Recuperacion_contrasenaRepository();
            recuperacion = recuperacion_ContrasenaRepository.SeleccionarCodigo(persona.id_usuario);
            codigo = sintetizarFormularios.Sintetizar(codigo);
            if (codigo == recuperacion.codigo)
            {
                EncriptarContrasenaUtility encriptarContrasenaUtility = new EncriptarContrasenaUtility();
                contrasena = sintetizarFormularios.Sintetizar(contrasena);
                string contraEn = encriptarContrasenaUtility.EncripContraRec(contrasena);
                personaRepository.ActualizarContrasena(correo, contraEn);
                recuperacion_ContrasenaRepository.EliminarCodigo(persona.id_usuario);
            }
            return filasAfectadas;
        }

    }
}