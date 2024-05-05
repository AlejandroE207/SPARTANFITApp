using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using SPARTANFITApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec.wmf;
using System.IO;


namespace SPARTANFITApp.Services
{
    public class AdministradorService
    {
        public PersonaDto registroAdministrador(PersonaDto administrador)
        {
            PersonaDto administradorResp = new PersonaDto();
            AdministradorRepository administradorRepository = new AdministradorRepository();


            if (administradorRepository.buscarAdministrador(administrador.correo))
            {
                administradorResp.respuesta = 0;
                administradorResp.mensaje = "Ya existe el usuario";
            }
            else
            {

                administrador.id_rol = 3;


                int resultadoRegistro = administradorRepository.registrarAdministrador(administrador);

                if (resultadoRegistro != 0)
                {
                    administradorResp.respuesta = 1;
                    administradorResp.mensaje = "Se ha registrado el usuario correctamente";
                }
                else
                {
                    administradorResp.respuesta = 0;
                    administradorResp.mensaje = "Error en el registro del usuario";
                }
            }
            return administradorResp;
        }
        public PersonaDto registrarEntrenador(PersonaDto entrenador)
        {
            PersonaDto entrenadorResp = new PersonaDto();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            entrenador.nombres = sintetizarFormularios.Sintetizar(entrenador.nombres);
            entrenador.apellidos = sintetizarFormularios.Sintetizar(entrenador.apellidos);
            entrenador.correo = sintetizarFormularios.Sintetizar(entrenador.correo);
            entrenador.contrasena = sintetizarFormularios.Sintetizar(entrenador.contrasena);
            EntrenadorRepository entrenadorRepository = new EntrenadorRepository();


            if (entrenadorRepository.buscarEntrenador(entrenador.correo))
            {
                entrenadorResp.respuesta = 0;
                entrenadorResp.mensaje = "Ya existe el entrenador";
            }
            else
            {

                entrenador.id_rol = 2;

                EncriptarContrasenaUtility encrip = new EncriptarContrasenaUtility();
                entrenador.contrasena = encrip.EncriptarContrasena(entrenador);
                int resultadoRegistro = entrenadorRepository.registroEntrenador(entrenador);

                if (resultadoRegistro != 0)
                {
                    entrenadorResp.respuesta = 1;
                    entrenadorResp.mensaje = "Se ha registrado el entrenador correctamente";
                }
                else
                {
                    entrenadorResp.respuesta = 0;
                    entrenadorResp.mensaje = "Error en el registro del usuario";
                }
            }
            return entrenadorResp;
        }
        public List<PersonaDto> Mostrar_Entrenadores()
        {
            EntrenadorRepository entrenadorRepository = new EntrenadorRepository();
            var Lista_entrenadores = entrenadorRepository.MostrarEntrenadores();
            return Lista_entrenadores;
        }
        public List<UsuarioDto> Mostrar_Usuarios()
        {
           UsuarioRepository usuarioRepository = new UsuarioRepository();
            var lista_Usuarios = usuarioRepository.MostrarUsuarios();
            return lista_Usuarios;
        }
        public string CrearPdfUsuarios()
        {
            ReporteUtility reporte = new ReporteUtility();
            var lista = Mostrar_Usuarios();

            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Usuarios.pdf");
            reporte.CrearPdfUsuarios(lista, tempFilePath);

            return tempFilePath; 
        }
        public string CrearPdfEntrenadores()
        {
            ReporteUtility reporte = new ReporteUtility();
            var lista = Mostrar_Entrenadores();

            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Entrenadores.pdf");
            reporte.CrearPdfDeEntrenadores (lista, tempFilePath);

            return tempFilePath;
        }

        public int EliminarEntrenador(String correo)
        {
            int filasAfectadas = 0;
            EntrenadorRepository entrenadorRepository = new EntrenadorRepository();

            try
            {
                filasAfectadas = entrenadorRepository.EliminarEntrenador(correo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return filasAfectadas;
        }
        
         public int ActualizarEntrenador(PersonaDto entrenador)
            {
            int filasAfectadas = 0;
            EntrenadorRepository entrenadorRepository = new EntrenadorRepository();
            SintetizarFormularios sintetizarFormularios = new SintetizarFormularios();
            entrenador.nombres = sintetizarFormularios.Sintetizar(entrenador.nombres);
            entrenador.apellidos = sintetizarFormularios.Sintetizar(entrenador.apellidos);
            entrenador.correo = sintetizarFormularios.Sintetizar(entrenador.correo);
            entrenador.contrasena = sintetizarFormularios.Sintetizar(entrenador.contrasena);

            try
                {
                EncriptarContrasenaUtility encrip = new EncriptarContrasenaUtility();
                entrenador.contrasena = encrip.EncriptarContrasena(entrenador);

                filasAfectadas = entrenadorRepository.ActualizarEntrenador(entrenador);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                return filasAfectadas;
            }
        }


    }


        //public PersonaDto logueo(PersonaDto administrador)
        //{
        //    AdministradorRepository administradorRepository = new AdministradorRepository();
        //    PersonaDto administradorResp = administradorRepository.IniciarSesion(administrador.correo, administrador.contrasena);

        //    if (administradorResp.respuesta == 1)
        //    {
        //        administradorResp.mensaje = "Inicio de sesión correcto";
        //    }
        //    else
        //    {
        //        administradorResp.mensaje = "Inicio de sesión incorrecto";
        //    }

        //    return administradorResp;
        //}
    

