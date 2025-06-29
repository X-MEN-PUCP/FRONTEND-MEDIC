using SoftBO.loginWS;
using SoftBO.pacienteWS;
using SoftBO.especialidadWS;
using SoftBO.usuarioporespecialidadWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using citaDTO = SoftBO.pacienteWS.citaDTO;
using SoftBO;

namespace SoftWA
{
    public partial class paciente_historial_citas : System.Web.UI.Page
    {
        private List<historiaClinicaPorCitaDTO> CitasCompletasPaciente
        {
            get { return ViewState["CitasCompletasPaciente"] as List<historiaClinicaPorCitaDTO> ?? new List<historiaClinicaPorCitaDTO>(); }
            set { ViewState["CitasCompletasPaciente"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosIniciales();
            }
        }
        private void CargarDatosIniciales()
        {
            var usuario = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/indexLogin.aspx");
                return;
            }

            try
            {
                var servicioPaciente = new PacienteBO();
                var paciente = new SoftBO.pacienteWS.usuarioDTO 
                { 
                    idUsuario = usuario.idUsuario,
                    idUsuarioSpecified = true
                };
                var historialCompleto = servicioPaciente.ListarCitasPaciente(paciente);
                if (historialCompleto != null)
                {
                    CitasCompletasPaciente = historialCompleto.ToList();
                }
                CargarFiltroEspecialidades();
                CargarFiltroMedicos();
                AplicarFiltrosYRecargarHistorial();
            }
            catch (Exception ex) 
            {
                phNoHistorial.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error al modificar cita tras pago: {ex.ToString()}");
            }
        }
        private void CargarFiltroEspecialidades()
        {
            try
            {
                var especialidades = CitasCompletasPaciente
                    .Where(h=>h.cita?.especialidad != null)
                    .Select(h=>h.cita.especialidad)
                    .GroupBy(e=>e.idEspecialidad)
                    .Select(g=>g.First())
                    .OrderBy(e=>e.nombreEspecialidad);
                ddlEspecialidadHistorial.DataSource = especialidades;
                ddlEspecialidadHistorial.DataTextField = "nombreEspecialidad";
                ddlEspecialidadHistorial.DataValueField = "idEspecialidad";
                ddlEspecialidadHistorial.DataBind();
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar especialidades: {ex.ToString()}");
            }
            ddlEspecialidadHistorial.Items.Insert(0, new ListItem("-- Todas --", "0"));
        }
        private void CargarFiltroMedicos(int idEspecialidadFiltro = 0)
        {
            ddlMedicoHistorial.Items.Clear();
            ddlMedicoHistorial.Items.Add(new ListItem("-- Todos --", "0"));
            IEnumerable<historiaClinicaPorCitaDTO> listaMedicos = CitasCompletasPaciente;
            if (idEspecialidadFiltro > 0)
                listaMedicos = listaMedicos.Where(h => h.cita?.especialidad != null && h.cita.especialidad.idEspecialidad == idEspecialidadFiltro);
            var medicos = listaMedicos
                .Where(h => h.cita?.medico != null)
                .Select(h => h.cita.medico)
                .GroupBy(m => m.idUsuario)
                .Select(g => g.First())
                .OrderBy(m => m.nombres)
                .ToList();
            ddlMedicoHistorial.DataSource = medicos.Select(m=>new ListItem($"{m.nombres} {m.apellidoPaterno}",m.idUsuario.ToString()));
            ddlMedicoHistorial.DataTextField = "Text";
            ddlMedicoHistorial.DataValueField = "Value";
            ddlMedicoHistorial.DataBind();
            ddlMedicoHistorial.Enabled = medicos.Any();
        }
        private void AplicarFiltrosYRecargarHistorial()
        {
            IEnumerable<historiaClinicaPorCitaDTO> historialFiltrado = CitasCompletasPaciente
                                                            .Where(h => h.cita != null &&
                            (DateTime.TryParse(h.cita.fechaCita, out var fecha) && fecha.Date < DateTime.Today));

            if (DateTime.TryParse(txtFechaDesde.Text, out DateTime fechaDesde))
                historialFiltrado = historialFiltrado.Where(h => DateTime.Parse(h.cita.fechaCita).Date >= fechaDesde.Date);

            if (DateTime.TryParse(txtFechaHasta.Text, out DateTime fechaHasta))
                historialFiltrado = historialFiltrado.Where(h => DateTime.Parse(h.cita.fechaCita).Date <= fechaHasta.Date);

            if (int.TryParse(ddlEspecialidadHistorial.SelectedValue, out int idEspecialidad) && idEspecialidad > 0)
                historialFiltrado = historialFiltrado.Where(h => h.cita.especialidad != null && h.cita.especialidad.idEspecialidad == idEspecialidad);

            if (int.TryParse(ddlMedicoHistorial.SelectedValue, out int idMedico) && idMedico > 0)
                historialFiltrado = historialFiltrado.Where(h => h.cita.medico != null && h.cita.medico.idUsuario == idMedico);

            var listaFinal = historialFiltrado
                .OrderByDescending(h => DateTime.Parse(h.cita.fechaCita))
                .Select(h => new {
                    NombreEspecialidad = h.cita.especialidad?.nombreEspecialidad ?? "N/A",
                    NombreMedico = h.cita.medico != null ? $"{h.cita.medico.nombres} {h.cita.medico.apellidoPaterno}" : "N/A",
                    FechaCita = DateTime.Parse(h.cita.fechaCita),
                    DescripcionHorario = h.cita.horaInicio.Substring(0, 5),
                    Estado = h.cita.estado.ToString(),
                    MotivoConsulta = h.motivoConsulta ?? "",
                    PresionArterial = h.presionArterial ?? "N/A",
                    Temperatura = h.temperatura,
                    Peso = h.peso,
                    Talla = h.talla,
                    Evolucion = h.evolucion ?? "",
                    Tratamiento = h.tratamiento ?? "",
                    Receta = h.receta ?? "",
                    Recomendacion = h.recomendacion ?? ""
                }).ToList();

            rptHistorial.DataSource = listaFinal;
            rptHistorial.DataBind();
            phNoHistorial.Visible = !listaFinal.Any();
        }
        protected void ddlEspecialidadHistorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEspecialidad = 0;
            int.TryParse(ddlEspecialidadHistorial.SelectedValue, out idEspecialidad);
            CargarFiltroMedicos(idEspecialidad);
        }
        protected void btnAplicarFiltrosHistorial_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYRecargarHistorial();
        }
        protected void btnLimpiarFiltrosHistorial_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = string.Empty;
            txtFechaHasta.Text = string.Empty;
            ddlEspecialidadHistorial.SelectedValue = "0";
            CargarFiltroMedicos();
            ddlMedicoHistorial.SelectedValue = "0";
            ddlMedicoHistorial.Enabled = false;
            AplicarFiltrosYRecargarHistorial();
        }
    }
}