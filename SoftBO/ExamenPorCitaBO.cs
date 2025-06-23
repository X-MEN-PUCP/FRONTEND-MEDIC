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
        public int InsertarExamenPorCita(examenPorCita examenxCita)
        {
            return this.examenPorCitaCliente.InsertarExamenPorCita(examenxCita);
        }
        public BindingList<examenPorCita> ListarExamenesPorIdCita(int idCita)
        {
            examenPorCita[] examenes = this.examenPorCitaCliente.ListarExamenesPorIdCita(idCita);
            return new BindingList<examenPorCita>(examenes ?? Array.Empty<examenPorCita>());
        }
        public BindingList<examenPorCita> ListarTodosLosExamenesPorCita()
        {
            examenPorCita[] examenes = this.examenPorCitaCliente.ListarTodosLosExamenesPorCita();
            return new BindingList<examenPorCita>(examenes);
        }
    }
}
