using Microsoft.Ajax.Utilities;
using SPARTANFITApp.Dto;
using SPARTANFITApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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

        public ActionResult CerrarSesion()
        {
            Session["UserLogged"] = null;
            return View("Index");
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
                    usuario = usuarioService.logueo(usuario,contraNormal);
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
                else if(personaLogeo.id_rol == 3)
                {
                    if (personaLogeo.respuesta != 0)
                    {
                        Session["UserLogged"] = personaLogeo;
                        return View("MostrarUsuarios");
                    }
                    else
                    {
                        return View("Index");
                    }
                }
            }
            return View();
        }
        public ActionResult BuscarCorreo() { return View("BuscarCorreo"); }
        public ActionResult CambiarContrasena() { return View("CambiarContrasena"); }
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
        public ActionResult MostrarEntrenadores()
        {
            AdministradorService servicio=new AdministradorService();
            List<PersonaDto>entrenadores=servicio.Mostrar_Entrenadores();
            ViewData["entrenadores"] = entrenadores;
            return View("MostrarEntrenadores");
        }
        public ActionResult MostrarUsuarios(UsuarioDto usuario)
        {
            AdministradorService servicio= new AdministradorService();
            List<UsuarioDto>usuarios=servicio.Mostrar_Usuarios(usuario);
            ViewData["usuarios"] = usuarios;
            return View("MostrarUsuarios",usuarios);
        }
        [HttpPost]
        public ActionResult EliminarEntrenador(String correo)
        {
            AdministradorService servicio= new AdministradorService();
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

            resultado=servicio.registrarEntrenador(entrenador);
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
            return View("FormActualizarEntrenador",entrenador);
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
            List<EjercicioDto> Ejercicios = servicio.Mostrar_Ejercicio();
            ViewData["Ejercicios"] = Ejercicios;
            return View("MostrarEjercicios", Ejercicios);
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
        public ActionResult AgregarAlimento(AlimentoDto alimento)
        {
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
            return View("CambiarContrasena", correo);
        }

        [HttpPost]
        public ActionResult CambiarContraseña(string correo,string codigo, string contrasena)
        {
            PersonaService personaService = new PersonaService();
            personaService.ActualizarContrasena(correo,contrasena, codigo);
            return View("IniciarSesion");
        }
    }
}