using SoftBO.citaWS;
using System;
using System.ComponentModel;

namespace SoftBO
{
    public class CitaBO
    {
        private CitaWSClient citaCliente;
        public CitaBO()
        {
            this.citaCliente = new CitaWSClient();
        }
        public int ModificarCita(citaDTO cita)
        {
            return this.citaCliente.modificarCita(cita);
        }
        public BindingList<citaDTO> ListarTodosCita()
        {
            citaDTO[] citas = this.citaCliente.listarTodosCita();
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }
        public BindingList<citaDTO> ListarCitasProgramadas(int codMedico)
        {
            citaDTO[] citas = this.citaCliente.listarCitasProgramadas(codMedico);
            return new BindingList<citaDTO>(citas);
        }
        public BindingList<citaDTO> BuscarCitasDisponibles(int idEspecialidad, int codMedico, DateTime fecha)
        {
            string fecha_convertida = fecha.ToString("yyyy-MM-dd");
            citaDTO[] citas = this.citaCliente.buscarCitasDisponibles(idEspecialidad, codMedico, fecha_convertida);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }
        public citaDTO ObtenerPorIdCitaCita(int id)
        {
            return this.citaCliente.obtenerPorIdCitaCita(id);
        }
    }
}
