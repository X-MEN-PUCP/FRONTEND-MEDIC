using SoftBO.pacienteWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class PacienteBO
    {
        private PacienteWSClient pacienteCliente;
        public PacienteBO()
        {
            this.pacienteCliente = new PacienteWSClient();
        }
        public BindingList<citaDTO> BuscarCitasPaciente(int idEspecialidad,string fecha, int idMedico, string horaInicio, estadoCita estado)
        {
            citaDTO[] citas = this.pacienteCliente.buscarCitasPaciente(idEspecialidad,fecha, idMedico, horaInicio, estado);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }
        public int ReservarCitaPaciente(citaDTO cita, usuarioDTO paciente)
        {
            return this.pacienteCliente.reservarCitaPaciente(cita, paciente);
        }
        public int CancelarCitaPaciente(historiaClinicaPorCitaDTO historia_por_cita)
        {
            return this.pacienteCliente.cancelarCitaPaciente(historia_por_cita);
        }
        public int ReprogramarCitaPaciente(citaDTO cita, historiaClinicaPorCitaDTO historia_por_cita)
        {
            return this.pacienteCliente.reprogramarCitaPaciente(cita, historia_por_cita);
        }
        public BindingList<historiaClinicaPorCitaDTO> ListarCitasPaciente(usuarioDTO persona)
        {
            historiaClinicaPorCitaDTO[] citas = this.pacienteCliente.listarCitasPorPaciente(persona);
            return new BindingList<historiaClinicaPorCitaDTO>(citas ?? new historiaClinicaPorCitaDTO[0]);
        }
    }
}
