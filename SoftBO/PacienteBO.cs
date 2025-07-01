using SoftBO.pacienteWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
        //////////////////////NUEVOS METODOS PACIENTE//////////////////////
        public void ActualizarEstadoDeCita(int idCita, int estado, int idModificacion)
        {
            this.pacienteCliente.ActualizarEStadoDeCita(idCita, estado, idModificacion);
        }
        public BindingList<usuarioPorEspecialidadDTO> ListarMedicosPorEspecialidadPaciente(int idEspecialidad)
        {
            usuarioPorEspecialidadDTO[] medicos = this.pacienteCliente.listarMedicosPorEspecialidadPaciente(idEspecialidad);
            return new BindingList<usuarioPorEspecialidadDTO>(medicos ?? new usuarioPorEspecialidadDTO[0]);
        }
        public BindingList<usuarioPorEspecialidadDTO> ListarTodosPerfilesMedicosParaPaciente()
        {
            usuarioPorEspecialidadDTO[] medicos = this.pacienteCliente.listarTodosPerfilesMedicosParaPaciente();
            return new BindingList<usuarioPorEspecialidadDTO>(medicos ?? new usuarioPorEspecialidadDTO[0]);
        }
        public BindingList<especialidadDTO> ListarTodasLasEspecialidadesParaPaciente()
        {
            especialidadDTO[] especialidades = this.pacienteCliente.listarTodasLasEspecialidadesParaElPaceinte();
            return new BindingList<especialidadDTO>(especialidades ?? new especialidadDTO[0]);
        }
        public BindingList<citaDTO> BuscarCitasParaPaciente(int idEspecialidad, int idMedico, string fecha, string horaInicio, estadoCita estado)
        {
            citaDTO[] citas = this.pacienteCliente.buscarCitasParaPaciente(idEspecialidad, idMedico, fecha, horaInicio, estado);
            return new BindingList<citaDTO>(citas ?? new citaDTO[0]);
        }
        public citaDTO ObtenerPorIdCitaParaPaciente(int idCita)
        {
            return this.pacienteCliente.obtenerPorIdCitaParaPaciente(idCita);
        }
        public int ModificarCitaParaPaciente(citaDTO cita)
        {
            return this.pacienteCliente.modificarCitaParaPaciente(cita);
        }
        public BindingList<usuarioPorRolDTO> ListarRolesPorUsuarioVistaPaciente(int id)
        {
            usuarioPorRolDTO[] roles = this.pacienteCliente.listarRolesPorUsuarioVistaPaciente(id);
            return new BindingList<usuarioPorRolDTO>(roles ?? new usuarioPorRolDTO[0]);
        }
    }
}
