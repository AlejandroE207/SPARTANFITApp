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

        public ActionResult IniciarSesion()
        {
            ViewBag.Message = "INICIAR SESION.";

            return View();
        }
    }
}