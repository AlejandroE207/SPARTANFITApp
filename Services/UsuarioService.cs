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

          
            if (usuarioRepository.buscarUsuario(usuario.correo))
            {
                usuarioResp.respuesta = 0;
                usuarioResp.mensaje = "Ya existe el usuario";
            }
            else
            {
             
                usuario.id_rol = 1;

                
                int resultadoRegistro = usuarioRepository.registroUsuario(usuario);

                if (resultadoRegistro != 0)
                {
                    usuarioResp.respuesta = 1;
                    usuarioResp.mensaje = "Se ha registrado el usuario correctamente";
                }
                else
                {
                    usuarioResp.respuesta = 0;
                    usuarioResp.mensaje = "Error en el registro del usuario";
                }
            }
            return usuarioResp;
        }


        public UsuarioDto logueo(UsuarioDto usuario)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            UsuarioDto usuarioResp = usuarioRepository.IniciarSesion(usuario.correo, usuario.contrasena);

            if (usuarioResp.respuesta == 1)
            {
                usuarioResp.mensaje = "Inicio de sesión correcto";
            }
            else
            {
                usuarioResp.mensaje = "Inicio de sesión incorrecto";
            }

            return usuarioResp;
        }

    }
}