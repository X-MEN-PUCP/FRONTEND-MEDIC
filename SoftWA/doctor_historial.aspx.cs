using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        private static List<CitaHist> _listaGlobalHistorialDoctor;
        private int _idDoctorLogueadoSimulado = 101;

        static doctor_historial()
        {
            InicializarHistorialSimulado();
        }

        private static void InicializarHistorialSimulado()
        {
            _listaGlobalHistorialDoctor = new List<CitaHist>
            {
                new CitaHist { IdCita = 1, NombrePaciente = "Ana Martínez", FechaCita = DateTime.Today.AddDays(-2), DescripcionHorario = "10:00", NombreEspecialidad = "Cardiología" },
                new CitaHist { IdCita = 5, NombrePaciente = "Luis Rodríguez", FechaCita = DateTime.Today.AddDays(-8), DescripcionHorario = "12:30", NombreEspecialidad = "Cardiología" },
                new CitaHist { IdCita = 6, NombrePaciente = "Elena Gómez", FechaCita = DateTime.Today.AddDays(-11), DescripcionHorario = "08:00", NombreEspecialidad = "Cardiología" },
                new CitaHist { IdCita = 8, NombrePaciente = "Juan Viejo", FechaCita = DateTime.Today.AddDays(-1), DescripcionHorario = "16:30", NombreEspecialidad = "Cardiología"},
                new CitaHist { IdCita = 10, NombrePaciente = "Zoe Ceballos", FechaCita = DateTime.Today.AddDays(-5), DescripcionHorario = "09:00", NombreEspecialidad = "Cardiología" },

                new CitaHist { IdCita = 2, NombrePaciente = "Pedro Ramírez", FechaCita = DateTime.Today.AddDays(-5), DescripcionHorario = "14:30", NombreEspecialidad = "Cardiología"},
                new CitaHist { IdCita = 7, NombrePaciente = "Sofía Castillo", FechaCita = DateTime.Today.AddDays(-30), DescripcionHorario = "15:00", NombreEspecialidad = "Cardiología"},
                new CitaHist { IdCita = 11, NombrePaciente = "Mario Luna", FechaCita = DateTime.Today.AddDays(-45), DescripcionHorario = "11:30", NombreEspecialidad = "Cardiología"},
            };
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

                string nombreDoctorSimulado = ObtenerNombreDoctorSimulado(idDoctorLogueado);
                lblDoctorInfo.Text = $"CMP: {idDoctorLogueado}";
                lblDoctorInfo.Visible = false;

                AplicarFiltrosYOrdenamiento();
            }
        }

        private int ObtenerIdDoctorLogueado()
        {
            return _idDoctorLogueadoSimulado;
        }
        private string ObtenerNombreDoctorSimulado(int idDoctor)
        {
            if (idDoctor == 101) return "Simulado Pérez";
            if (idDoctor == 201) return "Ejemplo García";
            return "Desconocido";
        }

        private void AplicarFiltrosYOrdenamiento()
        {
            int idDoctorActual = ObtenerIdDoctorLogueado();
            IEnumerable<CitaHist> historialDoctor = _listaGlobalHistorialDoctor;
            if (!string.IsNullOrEmpty(txtFechaDesdeHist.Text))
            {
                DateTime fechaDesde;
                if (DateTime.TryParse(txtFechaDesdeHist.Text, out fechaDesde))
                {
                    historialDoctor = historialDoctor.Where(c => c.FechaCita.Date >= fechaDesde.Date);
                }
            }
            if (!string.IsNullOrEmpty(txtFechaHastaHist.Text))
            {
                DateTime fechaHasta;
                if (DateTime.TryParse(txtFechaHastaHist.Text, out fechaHasta))
                {
                    historialDoctor = historialDoctor.Where(c => c.FechaCita.Date <= fechaHasta.Date);
                }
            }

            string ordenSeleccionado = ddlOrdenarPorHist.SelectedValue;
            switch (ordenSeleccionado)
            {
                case "FechaAsc":
                    historialDoctor = historialDoctor.OrderBy(c => c.FechaCita).ThenBy(c => c.DescripcionHorario);
                    break;
                case "NombrePacienteAsc":
                    historialDoctor = historialDoctor.OrderBy(c => c.NombrePaciente).ThenBy(c => c.FechaCita);
                    break;
                case "NombrePacienteDesc":
                    historialDoctor = historialDoctor.OrderByDescending(c => c.NombrePaciente).ThenBy(c => c.FechaCita);
                    break;
                case "FechaDesc":
                default:
                    historialDoctor = historialDoctor.OrderByDescending(c => c.FechaCita).ThenBy(c => c.DescripcionHorario);
                    break;
            }

            List<CitaHist> listaFinal = historialDoctor.ToList();

            rptHistDoctor.DataSource = listaFinal;
            rptHistDoctor.DataBind();

            phNoHistorial.Visible = !listaFinal.Any();
            Literal ltlNoResultados = rptHistDoctor.Controls[rptHistDoctor.Controls.Count - 1].FindControl("ltlNoResultadosMsg") as Literal;
            if (ltlNoResultados != null)
            {
                ltlNoResultados.Visible = !listaFinal.Any();
            }
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
                    btn.Text = "<i class='fa-solid fa-check'></i> Resultados Vistos";
                }
            }
        }
    }
}