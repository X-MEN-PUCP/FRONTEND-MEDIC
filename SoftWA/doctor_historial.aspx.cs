using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftBO;
using SoftBO.medicoWS;

namespace SoftWA
{
     public class CitaHist
     {
         public int IdCita { get; set; }
         public DateTime FechaCita { get; set; } 
         public string NombrePaciente { get; set; }
         public string DescripcionHorario { get; set; }
         public string NombreEspecialidad { get; set; }
     }

    public partial class doctor_historial : System.Web.UI.Page
    {
        private readonly CitaBO _citaBO;
        private readonly MedicoBO _medicoBO;
        private readonly HistoriaClinicaPorCitaBO _historiaClinicaPorCitaBO;    

        public doctor_historial()
        {
            _citaBO = new CitaBO();
            _medicoBO = new MedicoBO();
            _historiaClinicaPorCitaBO = new HistoriaClinicaPorCitaBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idDoctorLogueado = ObtenerIdDoctorLogueado();
                if (idDoctorLogueado == 0)
                {
                    Response.Redirect("~/indexLogin.aspx");
                    return;
                }

                lblDoctorInfo.Text = $"CMP: {idDoctorLogueado}";
                lblDoctorInfo.Visible = false;

                AplicarFiltrosYOrdenamiento();
            }
        }
        private int ObtenerIdDoctorLogueado()
        {
            var usuario = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            return usuario?.idUsuario ?? 0;
        }
        private void AplicarFiltrosYOrdenamiento()
        {
            int idDoctorActual = ObtenerIdDoctorLogueado();
            if (idDoctorActual == 0) return;
            string fechaDesde = txtFechaDesdeHist.Text;
            string fechaHasta = txtFechaHastaHist.Text;

            List<CitaHist> historialDoctor = new List<CitaHist>();
            try
            {
                var historial = _medicoBO.ListarCitasMedicos(idDoctorActual, estadoCita.PAGADO); //cambiar si atendidooooo

                foreach (var cita in historial)
                {
                    var historiaClinicaPorCita = _historiaClinicaPorCitaBO.ObtenerHistoriaClinicaPorIdCita(cita.idCita);

                    string nombreCompletoPaciente = "Paciente no encontrado";
                    if (historiaClinicaPorCita?.historiaClinica?.paciente != null)
                    {
                        var paciente = historiaClinicaPorCita.historiaClinica.paciente;
                        nombreCompletoPaciente = $"{paciente.nombres} {paciente.apellidoPaterno}".Trim();
                    }

                    historialDoctor.Add(new CitaHist
                    {
                        IdCita = cita.idCita,
                        FechaCita = DateTime.Parse(cita.fechaCita),
                        NombrePaciente = nombreCompletoPaciente,
                        DescripcionHorario = cita.turno?.horaInicio?.Substring(0, 5) ?? "N/A",
                        NombreEspecialidad = cita.especialidad?.nombreEspecialidad ?? "Sin Especialidad"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR al obtener historial: {ex.Message}");
                phNoHistorial.Visible = true;
                (phNoHistorial.FindControl("ltlNoHistorialMsg") as Literal).Text = "Error al conectar con el servidor para obtener el historial.";
                rptHistDoctor.DataSource = null;
                rptHistDoctor.DataBind();
                return;
            }

            string ordenSeleccionado = ddlOrdenarPorHist.SelectedValue;
            switch (ordenSeleccionado)
            {
                case "FechaAsc":
                    historialDoctor = historialDoctor.OrderBy(c => c.FechaCita).ThenBy(c => c.DescripcionHorario).ToList();
                    break;
                case "NombrePacienteAsc":
                    historialDoctor = historialDoctor.OrderBy(c => c.NombrePaciente).ThenBy(c => c.FechaCita).ToList();
                    break;
                case "NombrePacienteDesc":
                    historialDoctor = historialDoctor.OrderByDescending(c => c.NombrePaciente).ThenBy(c => c.FechaCita).ToList();
                    break;
                case "FechaDesc":
                default:
                    historialDoctor = historialDoctor.OrderByDescending(c => c.FechaCita).ThenBy(c => c.DescripcionHorario).ToList();
                    break;
            }

            rptHistDoctor.DataSource = historialDoctor;
            rptHistDoctor.DataBind();

            phNoHistorial.Visible = !historialDoctor.Any();
        }
        protected void btnAplicarFiltrosHistDoc_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYOrdenamiento();
        }
        protected void btnLimpiarFiltrosHistDoc_Click(object sender, EventArgs e)
        {
            txtFechaDesdeHist.Text = string.Empty;
            txtFechaHastaHist.Text = string.Empty;
            ddlOrdenarPorHist.SelectedValue = "FechaDesc";
            AplicarFiltrosYOrdenamiento();
        }
        protected void rptHistDoctor_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idCita = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "VerResultados")
            {
                LinkButton btn = e.CommandSource as LinkButton;
                if (btn != null)
                {
                    Response.Redirect($"doctor_registrar_atencion.aspx?idCita={idCita}&modo=vista");
                }
            }
        }
    }
}