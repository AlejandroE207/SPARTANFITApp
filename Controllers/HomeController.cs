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
        public ActionResult Formulario_Registro(UsuarioDto usuario)
        {
            UsuarioService usuarioServices = new UsuarioService();
            UsuarioDto resultado = usuarioServices.registroUsuario(usuario);

            if(resultado.respuesta != 0)
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
        public ActionResult IniciarSesion(UsuarioDto usuario)
        {
            UsuarioService usuarioService = new UsuarioService();
            UsuarioDto usuarioLogeo = usuarioService.logueo(usuario);

            if(usuarioLogeo.respuesta != 0)
            {
                Session["UserLogged"] = usuarioLogeo;
                return View("Principal");
        
            }
            return View("Index");

            

        }
        [HttpPost]
        public ActionResult IniciarSesionEntrenador(EntrenadorDto entrenador)
        {
            EntrenadorService entrenadorService = new EntrenadorService();
            EntrenadorDto entrenadorLogeo = entrenadorService.logueo(entrenador);

            if (entrenadorLogeo.respuesta != 0)
            {
                Session["UserLogged"] = entrenadorLogeo;
                return View("PrincipalEntrenador");

            }
            return View("Index");
        }
            public ActionResult CerrarSesion()
        {
            Session["UserLogged"] = null;
            return Redirect("Index");
        }
    }
}