using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using SPARTANFITApp.Services;
using System;
using System.IO;
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

        private readonly EjercicioService _ejercicioService = new EjercicioService(new EjercicioRepository());

        private readonly AlimentoService _alimentoService = new AlimentoService(new AlimentoRepository());

        public HomeController() { }
        // Muestra el formulario de agregar ejercicio
        public ActionResult AgregarEjercicio()
        {
            return View(); // Muestra el formulario de agregar ejercicio
        }
        // Método para mostrar el formulario de agregar alimento
        public ActionResult AgregarAlimento()
        {
            return View();
        }

        public ActionResult PrincipalEntrenador()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Formulario_Registro(UsuarioDto usuario)
        {
            UsuarioService usuarioServices = new UsuarioService();
            UsuarioDto resultado = usuarioServices.registroUsuario(usuario);

            if (resultado.persona.respuesta != 0)
            {
                return View("Index", resultado);
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
                    Session["UserLogged"] = usuario;
                    return View("Principal", usuario);
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


        [HttpPost]
        public ActionResult AgregarEjercicio(EjercicioDto ejercicio, HttpPostedFileBase imagen_ejercicio)
        {
            // Validar si el modelo recibido es válido
            if (ModelState.IsValid)
            {
                // Validar si se ha seleccionado una imagen válida
                if (imagen_ejercicio != null && imagen_ejercicio.ContentLength > 0)
                {
                    // Verificar la extensión del archivo
                    var extension = Path.GetExtension(imagen_ejercicio.FileName);
                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("imagen_ejercicio", "La imagen debe ser de tipo JPG o PNG.");
                        return View();
                    }

                    // Lógica para agregar el ejercicio a la base de datos
                    try
                    {
                        // se llama al servicio para agregar el ejercicio a la base de datos
                        _ejercicioService.AgregarEjercicio(ejercicio, imagen_ejercicio);

                        // Redirigir a la  página de éxito
                        return RedirectToAction("PrincipalEntrenador", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Ocurrió un error al agregar el ejercicio: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("imagen_ejercicio", "Debe seleccionar una imagen.");
                }
            }

            // Si llegamos aquí, significa que ha ocurrido un error, así que volvemos a mostrar el formulario con los mensajes de error
            return View();
        }

        [HttpPost]
        public ActionResult AgregarAlimento(AlimentoDto alimento)
        {
            try
            {
                // Llamar al servicio para agregar el alimento
                _alimentoService.AgregarAlimento(alimento);

                // Redirigir a la página de éxito
                return RedirectToAction("PrincipalEntrenador", "Home");
            }
            catch (Exception ex)
            {
                // Manejar el error y mostrar un mensaje al usuario
                ViewBag.ErrorMessage = "Error al agregar el alimento: " + ex.Message;
                return View(alimento);
            }
        }
    }


}









