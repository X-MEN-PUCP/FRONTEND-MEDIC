using SoftBO.examenporcitaWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class ExamenPorCitaBO
    {
        private ExamenPorCitaWSClient examenPorCitaCliente;
        public ExamenPorCitaBO()
        {
            this.examenPorCitaCliente = new ExamenPorCitaWSClient();
        }
        public int InsertarExamenPorCita(examenPorCitaDTO examenxCita)
        {
            return this.examenPorCitaCliente.InsertarExamenPorCita(examenxCita);
        }
        public BindingList<examenPorCitaDTO> ListarExamenesPorIdCita(int idCita)
        {
            examenPorCitaDTO[] examenes = this.examenPorCitaCliente.ListarExamenesPorIdCita(idCita);
            return new BindingList<examenPorCitaDTO>(examenes ?? Array.Empty<examenPorCitaDTO>());
        }
        public BindingList<examenPorCitaDTO> ListarTodosLosExamenesPorCita()
        {
            examenPorCitaDTO[] examenes = this.examenPorCitaCliente.ListarTodosLosExamenesPorCita();
            return new BindingList<examenPorCitaDTO>(examenes);
        }
    }
}
