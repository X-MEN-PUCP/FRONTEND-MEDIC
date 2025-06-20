using SoftBO.historiaclinicaporcitaWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class HistoriaClinicaPorCitaBO
    {
        private HistoriaClinicaPorCitaWSClient historiaClinicaCliente;
        public HistoriaClinicaPorCitaBO()
        {
            this.historiaClinicaCliente = new HistoriaClinicaPorCitaWSClient();
        }
        public int InsertarHistoriaClinicaPorCita(historiaClinicaPorCitaDTO historiaClinicaPorCita)
        {
            return this.historiaClinicaCliente.InsertarHistoriaClinicaPorCita(historiaClinicaPorCita);
        }
        public int ModificarHistoriaClinicaPorCita(historiaClinicaPorCitaDTO historiaClinicaPorCita)
        {
            return this.historiaClinicaCliente.ModificarHistoriaClinicaPorCita(historiaClinicaPorCita);
        }
        public BindingList<historiaClinicaPorCitaDTO> ListarTodasLasHistoriasClinicasPorCita()
        {
            historiaClinicaPorCitaDTO[] historias = this.historiaClinicaCliente.ListarTodasLasHistoriasClinicasPorCita();
            return new BindingList<historiaClinicaPorCitaDTO>(historias);
        }
        public BindingList<historiaClinicaPorCitaDTO> ListarHistoriasClinicasPorIdHistoria(int idHistoria)
        {
            historiaClinicaPorCitaDTO[] historias = this.historiaClinicaCliente.ListarHistoriasClinicasPorIdHistoria(idHistoria);
            return new BindingList<historiaClinicaPorCitaDTO>(historias);
        }
        public historiaClinicaPorCitaDTO ObtenerHistoriaClinicaPorIdCita(int idCita)
        {
            return this.historiaClinicaCliente.ObtenerHistoriaClinicaPorIdCita(idCita);
        }
    }
}
