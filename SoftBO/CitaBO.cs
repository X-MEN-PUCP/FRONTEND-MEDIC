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
        public BindingList<citaDTO> ListarCitasMedicoWS(int codMedico, estadoCita estado)
        {
            citaDTO[] citas = this.citaCliente.listarCitasMedicoWS(codMedico, estado);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }

        public BindingList<citaDTO> BuscarCitasWSCitas(int idEspecialidad, int codMedico, string fecha,string hora_inicio,estadoCita estado)
        {
            citaDTO[] citas = this.citaCliente.buscarCitasWSCitas(idEspecialidad, codMedico, fecha,hora_inicio,estado);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }

        public BindingList<citaDTO> BuscarCitasoloCalendario(int idEspecialidad, int codMedico, string fecha, string hora_inicio, estadoCita estado)
        {
            citaDTO[] citas = this.citaCliente.buscarCitasoloCalendario(idEspecialidad, codMedico, fecha, hora_inicio, estado);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }

        public citaDTO ObtenerPorIdCitaCita(int id)
        {
            return this.citaCliente.obtenerPorIdCitaCita(id);
        }
    }
}
