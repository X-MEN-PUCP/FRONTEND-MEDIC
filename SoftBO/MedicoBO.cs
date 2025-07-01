using SoftBO.medicoWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class MedicoBO
    {
        private MedicoWSClient medicoCliente;
        public MedicoBO()
        {
            this.medicoCliente = new MedicoWSClient();
        }
        public BindingList<citaDTO> ListarCitasMedicos(int codMedico,estadoCita estadoCita)
        {
            citaDTO[] citas = this.medicoCliente.listarCitasMedico(codMedico, estadoCita);
            return new BindingList<citaDTO>(citas);
        }
        public int LlenarEpicrisisMedico(historiaClinicaPorCitaDTO epicrisis)
        {
            return this.medicoCliente.llenarEpicrisisMedico(epicrisis);
        }

        public historiaClinicaPorCitaDTO ObtenerHistoriaClinicaPorCita(int idCita)
        {
            return this.medicoCliente.obtenerHistoriaClinicaPorCita(idCita);
        }

        public BindingList<especialidadDTO> ListarEspecialidadesParaInterconsulta()
        {
            especialidadDTO[] especialidades = this.medicoCliente.listarEspecialidadesParaInterconsulta();
            return new BindingList<especialidadDTO>(especialidades ?? new especialidadDTO[0]);
        }
        public int InsertarInterconsultasDeCita(interconsultaDTO interconsulta)
        {
            return this.medicoCliente.insertarInterconsultasDeCita(interconsulta);
        }
        public int ModificarInteronsultaDeCita(interconsultaDTO interconsulta)
        {
            return this.medicoCliente.modificarInteronsultaDeCita(interconsulta);
        }
        public int EliminarInterconsultaDeCita(interconsultaDTO interconsulta)
        {
            return this.medicoCliente.eliminarInterconsultaDeCita(interconsulta);
        }
        public BindingList<tipoExamenDTO> ListarTiposDeExamen()
        {
            tipoExamenDTO[] tipos = this.medicoCliente.listarTiposDeExamen();
            return new BindingList<tipoExamenDTO>(tipos ?? new tipoExamenDTO[0]);
        }
        public BindingList<examenDTO> ListarExamenesPorTipo(int idTipoExamen)
        {
            examenDTO[] examenes = this.medicoCliente.listarExamenesPorTipo(idTipoExamen);
            return new BindingList<examenDTO>(examenes ?? new examenDTO[0]);
        }
        public int AgregarExamenPorCita(examenPorCitaDTO examenPorCita)
        {
            return this.medicoCliente.agregarExamenPorCita(examenPorCita);
        }
        ///////////////////////////////////////////////////////////////////////////////////
        //////////////////////////NUEVOS METODOS MEDICO////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////
        public citaDTO ObtenerCitaPorIdCitaParaMedico(int idCita)
        {
            return this.medicoCliente.obtenerCitaPorIdCitaParaMedico(idCita);
        }
        public int ModificarCitaParaMedico(citaDTO cita)
        {
            return this.medicoCliente.modificarCitaParaMedico(cita);
        }
        public BindingList<interconsultaDTO> ListarInterconsultasPorIdCitaParaMedico(int idCita)
        {
            interconsultaDTO[] interconsultas = this.medicoCliente.listarInterconsultasPorIdCitaParMedico(idCita);
            return new BindingList<interconsultaDTO>(interconsultas ?? new interconsultaDTO[0]);
        }
        public BindingList<interconsultaDTO> ListarTodasLasInterconsultasParaMedico()
        {
            interconsultaDTO[] interconsultas = this.medicoCliente.listarTodasLasInterconsultasParaMedico();
            return new BindingList<interconsultaDTO>(interconsultas ?? new interconsultaDTO[0]);
        }
        public BindingList<examenDTO> ListarTodosLosExamenesParaMedico()
        {
            examenDTO[] examenes = this.medicoCliente.listarTodosLosExamenesParaMedico();
            return new BindingList<examenDTO>(examenes ?? new examenDTO[0]);
        }
        public examenDTO ObtenerExamenPorIdParaMedico(int idExamen)
        {
            return this.medicoCliente.obtenerExamenPorIdParaMedico(idExamen);
        }
        public BindingList<examenPorCitaDTO> ListarExamenesPorIdCitaParaMedico(int idCita)
        {
            examenPorCitaDTO[] examenes = this.medicoCliente.listarExamenesPorIdCitaParaMedico(idCita);
            return new BindingList<examenPorCitaDTO>(examenes ?? new examenPorCitaDTO[0]);
        }
        public diagnosticoDTO ObtenerDiagnosticoPorIdParaMedico(int idDiagnostico)
        {
            return this.medicoCliente.obtenerDiagnosticoPorIdParaMedico(idDiagnostico);
        }
    }
}
