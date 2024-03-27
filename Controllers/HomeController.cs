using SPARTANFITApp.Dto;
using SPARTANFITApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPARTANFIT_App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Formulario_Registro()
        {
            ViewBag.Message = "REGISTRO.";

            return View();
        }

        [HttpPost]
        public ActionResult Formulario_Registro(UsuarioDto usuario, string nombre)
        {
            string nombreUsuario = nombre;
            UsuarioService usuarioServices = new UsuarioService();
            UsuarioDto resultado = usuarioServices.registroUsuario(usuario);

            if(resultado.persona.respuesta != 0)
            {
                return View("Index",resultado);
            }
            else
            {
                return View(resultado);
            }
        }

        public ActionResult IniciarSesion()
        {
            ViewBag.Message = "INICIAR SESION.";

            return View();
        }
        [HttpPost]
        public ActionResult ControladorLogin(PersonaDto persona)
        {
            PersonaService personaService = new PersonaService();
            PersonaDto personaLogeo = personaService.logueo(persona);
            if (personaLogeo.id_rol == 1)
            {
                UsuarioDto usuario = personaService.mapeoPersona_Usuario(personaLogeo);
                
                if (usuario.persona.respuesta != 0)
                {
                    UsuarioService usuarioService = new UsuarioService();
                    usuario = usuarioService.logueo(usuario);
                    Session["UserLogged"] = usuario;
                    return View("Perfil");
                }
                else
                {
                    return View("Index");
                }
            }
            else
            {
                if (personaLogeo.id_rol == 2)
                {
                    if (personaLogeo.respuesta != 0)
                    {
                        Session["UserLogged"] = personaLogeo;
                        return View("PrincipalEntrenador");
                    }
                    else
                    {
                        return View("Index");
                    }

                }
            }
            return View();
        }

        public ActionResult Perfil() { return View("Perfil"); }

        public ActionResult ActualizarObjetivo() { return View("ActualizarObjetivo"); }

        [HttpPost]
        public ActionResult ActualizarObjetivo(UsuarioDto usuObjetivo)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            UsuarioService usuarioService = new UsuarioService();
            usuario = (UsuarioDto) Session["UserLogged"];

            usuario.rehabilitacion = usuObjetivo.rehabilitacion;

            if(usuario.rehabilitacion == 1)
            {
                usuario.id_objetivo = 0;
                usuario.id_nivel_entrenamiento = 0;
            }
            else
            {
                usuario.id_objetivo = usuObjetivo.id_objetivo;
                usuario.id_nivel_entrenamiento = usuObjetivo.id_nivel_entrenamiento;
            }

            usuario = usuarioService.actualizarObjetivo(usuario);

            if(usuario.persona.respuesta != 0)
            {
                return View("Perfil");
            }
            else
            {
                return View("ActualizarObjetivo");
            }
        }

        [HttpPost]
        public ActionResult EliminarCuenta()
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            UsuarioService usuarioService = new UsuarioService();
            usuario = (UsuarioDto)Session["UserLogged"];

            usuario = usuarioService.eliminarUsuario(usuario);

            if (usuario.persona.respuesta != 0)
            {
                return View("Index");
            }
            else
            {
                return View("ActualizarObjetivo");
            }
        }
    }
}