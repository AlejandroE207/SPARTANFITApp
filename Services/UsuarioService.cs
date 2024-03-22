using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
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
            UsuarioRepository usuarioRepository = new UsuarioRepository();

          
            if (usuarioRepository.buscarUsuario(usuario.persona.correo))
            {
                usuarioResp.persona.respuesta = 0;
                usuarioResp.persona.mensaje = "Ya existe el usuario";
            }
            else
            {
             
                usuario.persona.id_rol = 1;

                
                int resultadoRegistro = usuarioRepository.registroUsuario(usuario);

                if (resultadoRegistro != 0)
                {
                    usuarioResp.persona.respuesta = 1;
                    usuarioResp.persona.mensaje = "Se ha registrado el usuario correctamente";
                }
                else
                {
                    usuarioResp.persona.respuesta = 0;
                    usuarioResp.persona.mensaje = "Error en el registro del usuario";
                }
            }
            return usuarioResp;
        }


        public UsuarioDto logueo(UsuarioDto usuario)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            UsuarioDto usuarioResp = usuarioRepository.IniciarSesion(usuario.persona.correo, usuario.persona.contrasena);

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

    }
}