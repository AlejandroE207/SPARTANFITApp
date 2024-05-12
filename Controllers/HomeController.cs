using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using SPARTANFITApp.Dto;
using SPARTANFITApp.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;
using X.PagedList;
using System.IO;
using System.Drawing.Printing;
using SPARTANFITApp.Controllers;
using Org.BouncyCastle.Bcpg.Sig;


namespace SPARTANFIT_App.Controllers
{
    public class HomeController : BaseController
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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CerrarSesion()
        {
            Session["UserLogged"] = null;
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PrincipalEntrenador()
        {
            return View("PrincipalEntrenador");
        }

        [HttpPost]
        public ActionResult ControladorLogin(PersonaDto persona)
        {
            PersonaService personaService = new PersonaService();
            PersonaDto personaLogeo = personaService.logueo(persona);
            if (personaLogeo.id_rol == 1)
            {
                UsuarioDto usuario = personaService.mapeoPersona_Usuario(personaLogeo);
                string contraNormal = persona.contrasena;

                if (usuario.persona.respuesta != 0)
                {
                    UsuarioService usuarioService = new UsuarioService();
                    usuario = usuarioService.logueo(usuario, contraNormal);
                    Session["UserLogged"] = usuario;
                    return RedirectToAction("PrincipalUsuario");
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
                        Session["entrenadorLogged"] = personaLogeo;
                        return RedirectToAction("PrincipalEntrenador");
                    }
                    else
                    {
                        return View("Index");
                    }

                }
                else if (personaLogeo.id_rol == 3)
                {
                    if (personaLogeo.respuesta != 0)
                    {
                        Session["UserLogged"] = personaLogeo;
                        return MostrarUsuarios();
                    }
                    else
                    {
                        return View("Index");
                    }
                }
            }
            return View("Index");
        }
        public ActionResult BuscarCorreo() { return View("BuscarCorreo"); }
        public ActionResult CambiarContrasena() { return View("CambiarContrasena"); }
        public ActionResult Perfil() 
        { 
            UsuarioService usuarioService = new UsuarioService();
            UsuarioDto usuarioResp = new UsuarioDto();
            usuarioResp = (UsuarioDto)Session["UserLogged"];
            string rehabilitacionText = "";
            string nivelEntrenamiento = "";
            string objetivo = "";
            (rehabilitacionText,nivelEntrenamiento,objetivo) = usuarioService.ConvertirIdText(usuarioResp);

            ViewData["rehabilitacion"] = rehabilitacionText;
            ViewData["nivelEntrenamiento"] = nivelEntrenamiento;
            ViewData["objetivo"] = objetivo;


            return View("Perfil"); 
        }

        public ActionResult ActualizarObjetivo() { return View("ActualizarObjetivo"); }

        [HttpPost]
        public ActionResult ActualizarObjetivo(UsuarioDto usuObjetivo)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            UsuarioService usuarioService = new UsuarioService();
            usuario = (UsuarioDto)Session["UserLogged"];

            usuario.rehabilitacion = usuObjetivo.rehabilitacion;

            if (usuario.rehabilitacion == 1)
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

            if (usuario.persona.respuesta != 0)
            {
                return View("Perfil");
            }
            else
            {
                return View("ActualizarObjetivo");
            }
        }
        public ActionResult MostrarEntrenadores()
        {
            AdministradorService servicio = new AdministradorService();
            List<PersonaDto> entrenadores = servicio.Mostrar_Entrenadores();
            ViewData["entrenadores"] = entrenadores;
            return View("MostrarEntrenadores");
        }
        public ActionResult MostrarUsuarios()
        {
            AdministradorService servicio = new AdministradorService();
            List<UsuarioDto> usuarios = servicio.Mostrar_Usuarios();
            ViewData["usuarios"] = usuarios;
            return View("MostrarUsuarios", usuarios);
        }
        [HttpPost]
        public ActionResult EliminarEntrenador(String correo)
        {
            AdministradorService servicio = new AdministradorService();
            servicio.EliminarEntrenador(correo);
            return MostrarEntrenadores();
        }

        public ActionResult AgregarEntrenador()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarEntrenador(PersonaDto entrenador)
        {
            AdministradorService servicio = new AdministradorService();
            PersonaDto resultado = new PersonaDto();

            resultado = servicio.registrarEntrenador(entrenador);
            if (resultado.respuesta != 0)
            {
                return MostrarEntrenadores();
            }
            else
            {
                return View(resultado);
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
                Session["UserLogged"] = null;
                return View("Index");
            }
            else
            {
                return View("ActualizarObjetivo");
            }
        }
        [HttpPost]
        public ActionResult ActualizarEntrenador(PersonaDto entrenador)
        {
            return View("FormActualizarEntrenador", entrenador);
        }

        [HttpPost]
        public ActionResult FormActualizarEntrenador(PersonaDto entrenador)
        {
            AdministradorService administradorService = new AdministradorService();
            int resultado = administradorService.ActualizarEntrenador(entrenador);
            if (resultado != 0)
            {
                return MostrarEntrenadores();
            }
            else
            {
                return View();
            }
        }

        public ActionResult MostrarEjercicios()
        {
            EntrenadorService servicio = new EntrenadorService();
            List<EjercicioDto> ejercicios = servicio.Mostrar_Ejercicio();
            ViewData["ejercicios"] = ejercicios;
            return View("MostrarEjercicios", ejercicios);
        }

        [HttpPost]
        public ActionResult EliminarEjercicio(int id_ejercicio)
        {
            EntrenadorService entrenadorService = new EntrenadorService();
            entrenadorService.EliminarEjercicio(id_ejercicio);
            return MostrarEjercicios();
        }

        public ActionResult AgregarEjercicio()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarEjercicio(EjercicioDto ejercicio)
        {
            EntrenadorService servicio = new EntrenadorService();
            EjercicioDto resultado = new EjercicioDto();

            resultado = servicio.registrarEjercicio(ejercicio);
            if (resultado.respuesta != 0)
            {
                return MostrarEjercicios();
            }
            else
            {
                return View(resultado);
            }
        }

        [HttpPost]
        public ActionResult ActualizarEjercicio(EjercicioDto ejercicio)
        {
            return View("FormActualizarEjercicio", ejercicio);
        }

        [HttpPost]
        public ActionResult FormActualizarEjercicio(EjercicioDto ejercicio)
        {
            EntrenadorService entrenadorService = new EntrenadorService();
            int resultado = entrenadorService.ActualizarEjercicio(ejercicio);
            if (resultado != 0)
            {
                return MostrarEjercicios();
            }
            else
            {
                return View();
            }
        }
        public ActionResult MostrarAlimentos()
        {
            EntrenadorService servicio = new EntrenadorService();
            List<AlimentoDto> Alimentos = servicio.Mostrar_Alimento();
            ViewData["Alimentos"] = Alimentos;
            return View("MostrarAlimentos", Alimentos);
        }

        [HttpPost]
        public ActionResult EliminarAlimento(int id_alimento)
        {
            EntrenadorService servicio = new EntrenadorService();
            servicio.EliminarAlimento(id_alimento);
            return MostrarAlimentos();
        }

        public ActionResult AgregarAlimento()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarAlimento(string nombre, int id_categoria_alimento, string calorias_x_gramo, string grasa, string carbohidrato, string proteina, string fibra)
        {
            double calorias_x_gramoValue = double.Parse(calorias_x_gramo);
            double grasaValue = double.Parse(grasa);
            double carbohidratoValue = double.Parse(carbohidrato);
            double proteinaValue = double.Parse(proteina);
            double fibraValue = double.Parse(fibra);
            AlimentoDto alimento = new AlimentoDto()
            {
                nombre = nombre,
                id_categoria_alimento = id_categoria_alimento,
                calorias_x_gramo = calorias_x_gramoValue,
                grasa = grasaValue,
                carbohidrato = carbohidratoValue,
                proteina = proteinaValue,
                fibra = fibraValue

            };
            EntrenadorService entrenadorServicio = new EntrenadorService();
            AlimentoDto resultado = new AlimentoDto();

            resultado = entrenadorServicio.registrarAlimento(alimento);
            if (resultado.respuesta != 0)
            {
                return MostrarAlimentos();
            }
            else
            {
                return View(resultado);
            }
        }



        [HttpPost]
        public ActionResult ActualizarAlimento(AlimentoDto alimento)
        {
            return View("FormActualizarAlimento", alimento);
        }

        [HttpPost]
        public ActionResult FormActualizarAlimento(AlimentoDto alimento)
        {
            EntrenadorService entrenadorService = new EntrenadorService();
            int resultado = entrenadorService.ActualizarAlimento(alimento);
            if (resultado != 0)
            {
                return MostrarAlimentos();
            }
            else
            {
                return View();
            }
        }



        [HttpPost]
        public ActionResult BuscarCorreo(string correo)
        {
            PersonaService personaService = new PersonaService();
            personaService.enviarCodigo(correo);
            ViewData["correo"] = correo;
            return View("CambiarContrasena");

        }

        [HttpPost]
        public ActionResult FormCambiarContrasena(string correo, string codigo, string contrasena)
        {
            PersonaService personaService = new PersonaService();
            personaService.ActualizarContrasena(correo, contrasena, codigo);
            return View("IniciarSesion");
        }

        //--------------------------------------------------------------------------------------------------
        public ActionResult CrearRutina()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormCrearRutina(RutinaDto rutina)
        {
            ViewData["Rutina"] = rutina;
            return AgregarRutina();
        }

        public ActionResult AgregarRutina()
        {
            EntrenadorService servicio = new EntrenadorService();
            List<EjercicioDto> ejercicios = servicio.Mostrar_Ejercicio();
            ViewData["Ejercicios"] = ejercicios;
            return View("AgregarRutina");
        }

        [HttpPost]
        public ActionResult FormAgregarRutina(int[] selectedCheckboxIds, int[] listadoSeries, int[] listadoRepeticiones, RutinaDto rutina)
        {
            EntrenadorService entrenadorService = new EntrenadorService();
            List<EjercicioDto> ejerciciosRutina = new List<EjercicioDto>();
            if (selectedCheckboxIds != null)
            {

                for (int i = 0; i < selectedCheckboxIds.Length; i++)
                {
                    int checkboxId = selectedCheckboxIds[i];
                    int series = listadoSeries[i];   
                    int repeticiones = listadoRepeticiones[i];

                    EjercicioDto ejercicio = new EjercicioDto
                    {
                        id_ejercicio = checkboxId,
                        num_series = series,
                        repeticiones = repeticiones
                    };

                    ejerciciosRutina.Add(ejercicio);
                }
            }
            int resultado = entrenadorService.registrarRutina(rutina, ejerciciosRutina);
            if (resultado != 0)
            {
                return View("CrearRutina");
            }
            else
            {
                return View("CrearRutina", resultado);
            }
        }

        [HttpPost]
        public ActionResult CrearRutina(RutinaDto rutina)
        {
            return View("AgregarRutina", rutina);
        }

        [HttpPost]
        public ActionResult FormCrearPlanAlimenticio(PlanAlimenticioDto planAlimenticio)
        {
            ViewData["planAlimenticio"] = planAlimenticio;
            return AgregarPlanAlimenticio();
        }

        [HttpPost]
        public ActionResult FormAgregarPlanAlimenticio(PlanAlimenticioDto planAlimenticio, int[] selectedCheckboxIds)
        {
            EntrenadorService entrenadorService = new EntrenadorService();

            List<int> idAlimentos = new List<int>();
            for(int i = 0; i < selectedCheckboxIds.Length; i++)
            {
                idAlimentos.Add(selectedCheckboxIds[i]);
            }

            int resultado = entrenadorService.registrarPlanNutricional(planAlimenticio, idAlimentos);
            if(resultado != 0)
            {
                return RedirectToAction("CrearPlanAlimenticio");
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult AgregarPlanAlimenticio()
        {
            EntrenadorService servicio = new EntrenadorService();
            List<AlimentoDto> alimentos = servicio.Mostrar_Alimento();
            ViewData["alimentos"] = alimentos;
            return View("AgregarPlanAlimenticio");
        }
        public ActionResult CrearPlanAlimenticio()
        {
            return View();
        }



        public ActionResult ActualizarDatos() { return View("ActualizarDatos"); }

        [HttpPost]
        public ActionResult ActualizarDatos(UsuarioDto usuDatos)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            UsuarioService usuarioService = new UsuarioService();
            usuario = (UsuarioDto)Session["UserLogged"];

            usuario.peso = usuDatos.peso;
            usuario.estatura = usuDatos.estatura;


            usuario = usuarioService.actualizarDatosUsuario(usuario);

            if (usuario.persona.respuesta != 0)
            {
                return View("Perfil");
            }
            else
            {
                return View("ActualizarDatos");
            }
        }


        [HttpPost]
        public ActionResult DescargarPdfUsuarios()
        {

            AdministradorService administradorService = new AdministradorService();


            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Usuarios.pdf");
            administradorService.CrearPdfUsuarios();
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Lista_Usuarios.pdf");
            Response.WriteFile(tempFilePath);
            Response.Flush();

            return RedirectToAction("MostrarUsuarios");
        }
        [HttpPost]
        public ActionResult DescargarPdfEntrenadores()
        {

            AdministradorService administradorService = new AdministradorService();


            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Entrenadores.pdf");
            administradorService.CrearPdfEntrenadores();
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Lista_Entrenadores.pdf");
            Response.WriteFile(tempFilePath);
            Response.Flush();

            return RedirectToAction("MostrarEntrenadores");
        }

        public ActionResult PrincipalUsuario()
        {
            UsuarioDto usuario = new UsuarioDto();
            UsuarioService usuarioService = new UsuarioService();
            usuario = (UsuarioDto)Session["UserLogged"];

            List<EjercicioDto> ejerciciosDia = new List<EjercicioDto>();
            RutinaDto rutinaDia = new RutinaDto();

            (rutinaDia,ejerciciosDia) = usuarioService.mostrarRutinaDia(usuario);

            ViewData["ejerciciosDia"] = ejerciciosDia;
            ViewData["rutinaDia"] = rutinaDia;
            return View();
        }

        public ActionResult PlanNutricionalUsuario()
        {
            UsuarioDto usuario = new UsuarioDto();
            UsuarioService usuarioService = new UsuarioService();
            PlanAlimenticioDto planAlimenticio = new PlanAlimenticioDto();
            usuario = (UsuarioDto)Session["UserLogged"];

            List<AlimentoDto> dietaDia = new List<AlimentoDto>();

            (planAlimenticio,dietaDia) = usuarioService.mostrarPlanNutricionalDia(usuario);

            ViewData["planAlimenticio"] = planAlimenticio;
            ViewData["dietaDia"] = dietaDia;
            return View();

        }
        
        public ActionResult MostrarRutinas()
        {
            EntrenadorService entrenadorService = new EntrenadorService();

            List<RutinaDto> rutinas = new List<RutinaDto>();

            rutinas = entrenadorService.MostrarRutinas();

            ViewData["rutinas"] = rutinas;
            return View();
        }

        [HttpPost]
        public ActionResult EliminarRutina(int id_rutina)
        {
            RutinaDto rutina = new RutinaDto();
            EntrenadorService entrenadorService = new EntrenadorService();

            rutina = entrenadorService.EliminarRutina(id_rutina);

            if(rutina.respuesta != 0)
            {
                return RedirectToAction("MostrarRutinas");
            }
            return View();
        }

        public ActionResult MostrarPlanesAlimenticios()
        {
            PlanAlimenticioDto plan = new PlanAlimenticioDto();
            EntrenadorService entrenadorService = new EntrenadorService();

            List<PlanAlimenticioDto> planes = new List<PlanAlimenticioDto>();

            planes = entrenadorService.MostrarPlanes();

            ViewData["planes"] = planes;
            return View();
        }

        [HttpPost]
        public ActionResult EliminarPlanAlimenticio(int id_plan_alimenticio)
        {
            PlanAlimenticioDto plan = new PlanAlimenticioDto();
            EntrenadorService entrenadorService = new EntrenadorService();

            plan = entrenadorService.EliminarPlan(id_plan_alimenticio);

            if (plan.respuesta != 0)
            {
                return RedirectToAction("MostrarPlanesAlimenticios");
            }
            return View();
        }
    }
}