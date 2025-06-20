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
        public BindingList<citaDTO> ListarCitasPaciente(int idEspecialidad,DateTime fecha, int idMedico)
        {
            string fecha_convertida = fecha.ToString("yyyy-MM-dd");
            citaDTO[] citas = this.pacienteCliente.listarCitasPaciente(idEspecialidad, fecha_convertida, idMedico);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }
        public int ReservarCitaPaciente(citaDTO cita, usuarioDTO paciente)
        {
            return this.pacienteCliente.reservarCitaPaciente(cita, paciente);
        }
        public int CancelarCitaPaciente(citaDTO cita, historiaClinicaPorCitaDTO historia_por_cita)
        {
            return this.pacienteCliente.cancelarCitaPaciente(cita, historia_por_cita);
        }
        public int ReprogramarCitaPaciente(citaDTO citaAntigua, citaDTO citaNueva, historiaClinicaPorCitaDTO historia_por_cita)
        {
            return this.pacienteCliente.reprogramarCitaPaciente(citaAntigua, citaNueva, historia_por_cita);
        }
        public BindingList<historiaClinicaPorCitaDTO> ListarCitasPorPersonaPaciente(historiaClinicaDTO historia)
        {
            historiaClinicaPorCitaDTO[] citas = this.pacienteCliente.listarCitasPorPersonaPaciente(historia);
            return new BindingList<historiaClinicaPorCitaDTO>(citas);
        }
    }
}
