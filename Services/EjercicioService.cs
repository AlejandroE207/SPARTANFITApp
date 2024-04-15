using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;

namespace SPARTANFITApp.Services
{
    public class EjercicioService
    {
        private readonly EjercicioRepository _ejercicioRepository= new EjercicioRepository();

        public EjercicioService() { }

        public EjercicioService(EjercicioRepository ejercicioRepository)
        {
            _ejercicioRepository = ejercicioRepository;
        }

        public void AgregarEjercicio(EjercicioDto ejercicio, HttpPostedFileBase imagen_ejercicio)
        {
            _ejercicioRepository.AgregarEjercicio(ejercicio, imagen_ejercicio);
        }
    }
}
