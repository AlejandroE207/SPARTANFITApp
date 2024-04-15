using SPARTANFITApp.Dto;
using SPARTANFITApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARTANFITApp.Services
{
    public class AlimentoService
    {
        private readonly AlimentoRepository _alimentoRepository = new AlimentoRepository();

        public AlimentoService() { }

        // Constructor del servicio, recibe una instancia del repositorio de alimento
        public AlimentoService(AlimentoRepository alimentoRepository)
        {
            // Asignamos la instancia del repositorio al campo privado _alimentoRepository
            _alimentoRepository = alimentoRepository;
        }

        public void AgregarAlimento(AlimentoDto alimento)
        {
            // Aquí se puede implementar la lógica necesaria antes de llamar al método del repositorio para agregar el alimento
            // con la instancia de alimentorepository llamamos el metodo agregaralimento del repository y le mandandamos el objeto alimento
            _alimentoRepository.AgregarAlimento(alimento);
        }
        // aqui agregare los otros metodos que necesita el capera
    }
}