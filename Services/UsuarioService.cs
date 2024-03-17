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
        public void registroUsuario(UsuarioDto usuario)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            if (usuarioRepository.buscarUsuario(usuario.correo))
            {
                usuario.respuesta = 0;
                usuario.mensaje = "Ya existe el usuario";
            }
            else
            {
                usuario.id_rol = 1;
                if (usuarioRepository.registroUsuario(usuario) != 0)
                {
                    usuario.respuesta = 1;
                    usuario.mensaje = "Se ha registrado el usuario correctamente";
                }
                else
                {
                    usuario.respuesta = 0;
                    usuario.mensaje = "Error en el registro del usuario";
                }
            }
        }
    }
}