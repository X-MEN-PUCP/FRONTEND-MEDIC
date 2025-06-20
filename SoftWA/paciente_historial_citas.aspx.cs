using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public class CitaHistInfo
    {
        public int IdCita { get; set; }
        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; }
        public int IdMedico { get; set; }
        public string NombreMedico { get; set; }
        public DateTime FechaCita { get; set; }
        public string DescripcionHorario { get; set; }
        public string Estado { get; set; }
        public string ObsMedicas { get; set; }
    }

    public partial class paciente_historial_citas : System.Web.UI.Page
    {
        private static List<CitaHistInfo> _listaGlobalHistorialPaciente;

        static paciente_historial_citas()
        {
            InicializarHistorialDeEjemplo();
        }

        private static void InicializarHistorialDeEjemplo()
        {
            _listaGlobalHistorialPaciente = new List<CitaHistInfo>
            {
                new CitaHistInfo {
                    IdCita = 1, IdEspecialidad = 4, NombreEspecialidad = "Traumatología", IdMedico = 401, NombreMedico = "Dr. Pedro Castillo",
                    FechaCita = DateTime.Today.AddDays(-4), DescripcionHorario = "09:00", Estado = "Atendida", ObsMedicas = "Fractura leve, reposo por 1 semana."
                },
                new CitaHistInfo {
                    IdCita = 2, IdEspecialidad = 5, NombreEspecialidad = "Odontología General", IdMedico = 501, NombreMedico = "Dra. Laura Pausini",
                    FechaCita = DateTime.Today.AddDays(-10), DescripcionHorario = "15:30", Estado = "Atendida", ObsMedicas = "Limpieza dental realizada. Próxima revisión en 6 meses."
                },
                new CitaHistInfo {
                    IdCita = 3, IdEspecialidad = 3, NombreEspecialidad = "Pediatría", IdMedico = 301, NombreMedico = "Dra. Sofía Gómez",
                    FechaCita = DateTime.Today.AddDays(-20), DescripcionHorario = "10:30", Estado = "Atendida", ObsMedicas = "Control de niño sano. Todo normal."
                },
                new CitaHistInfo {
                    IdCita = 4, IdEspecialidad = 1, NombreEspecialidad = "Cardiología", IdMedico = 102, NombreMedico = "Dra. Ana López",
                    FechaCita = DateTime.Today.AddDays(-35), DescripcionHorario = "14:00", Estado = "Atendida", ObsMedicas = "Electrocardiograma normal. Se recomienda ejercicio moderado."
                },
                new CitaHistInfo {
                    IdCita = 5, IdEspecialidad = 3, NombreEspecialidad = "Pediatría", IdMedico = 301, NombreMedico = "Dra. Sofía Gómez",
                    FechaCita = DateTime.Today.AddDays(-60), DescripcionHorario = "16:00", Estado = "Atendida", ObsMedicas = "Vacunación completada según esquema."
                },
                 new CitaHistInfo {
                    IdCita = 6, IdEspecialidad = 4, NombreEspecialidad = "Traumatología", IdMedico = 401, NombreMedico = "Dr. Pedro Castillo",
                    FechaCita = DateTime.Today.AddDays(-90), DescripcionHorario = "10:00", Estado = "Atendida", ObsMedicas = "Seguimiento de fractura. Buena recuperación."
                },
            };
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFiltroEspecialidades();
                CargarFiltroMedicos();
                AplicarFiltrosYRecargarHistorial();
            }
        }

        private void CargarFiltroEspecialidades()
        {
            var especialidades = _listaGlobalHistorialPaciente
                                .Where(c => c.Estado == "Atendida")
                                .Select(c => new { c.IdEspecialidad, c.NombreEspecialidad })
                                .Distinct()
                                .OrderBy(e => e.NombreEspecialidad)
                                .ToList();

            ddlEspecialidadHistorial.DataSource = especialidades;
            ddlEspecialidadHistorial.DataTextField = "NombreEspecialidad";
            ddlEspecialidadHistorial.DataValueField = "IdEspecialidad";
            ddlEspecialidadHistorial.DataBind();
            ddlEspecialidadHistorial.Items.Insert(0, new ListItem("-- Todas --", "0"));
        }

        private void CargarFiltroMedicos(int idEspecialidadFiltro = 0)
        {
            ddlMedicoHistorial.Items.Clear();
            var medicosQuery = _listaGlobalHistorialPaciente
                                .Where(c => c.Estado == "Atendida");

            if (idEspecialidadFiltro > 0)
            {
                medicosQuery = medicosQuery.Where(c => c.IdEspecialidad == idEspecialidadFiltro);
            }

            var medicos = medicosQuery
                          .Select(c => new { c.IdMedico, c.NombreMedico })
                          .Distinct()
                          .OrderBy(m => m.NombreMedico)
                          .ToList();

            ddlMedicoHistorial.DataSource = medicos;
            ddlMedicoHistorial.DataTextField = "NombreMedico";
            ddlMedicoHistorial.DataValueField = "IdMedico";
            ddlMedicoHistorial.DataBind();
            ddlMedicoHistorial.Items.Insert(0, new ListItem("-- Todos --", "0"));
            ddlMedicoHistorial.Enabled = (idEspecialidadFiltro > 0 && medicos.Any()) || idEspecialidadFiltro == 0;
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

        private void AplicarFiltrosYRecargarHistorial()
        {
            IEnumerable<CitaHistInfo> historialFiltrado = _listaGlobalHistorialPaciente
                                                            .Where(c => c.Estado == "Atendida");

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                DateTime fechaDesde;
                if (DateTime.TryParse(txtFechaDesde.Text, out fechaDesde))
                {
                    historialFiltrado = historialFiltrado.Where(c => c.FechaCita.Date >= fechaDesde.Date);
                }
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                DateTime fechaHasta;
                if (DateTime.TryParse(txtFechaHasta.Text, out fechaHasta))
                {
                    historialFiltrado = historialFiltrado.Where(c => c.FechaCita.Date <= fechaHasta.Date);
                }
            }

            int idEspecialidad = 0;
            int.TryParse(ddlEspecialidadHistorial.SelectedValue, out idEspecialidad);
            if (idEspecialidad > 0)
            {
                historialFiltrado = historialFiltrado.Where(c => c.IdEspecialidad == idEspecialidad);
            }

            int idMedico = 0;
            int.TryParse(ddlMedicoHistorial.SelectedValue, out idMedico);
            if (idMedico > 0)
            {
                historialFiltrado = historialFiltrado.Where(c => c.IdMedico == idMedico);
            }

            var listaFinal = historialFiltrado.OrderByDescending(c => c.FechaCita).ThenBy(c => c.DescripcionHorario).ToList();
            rptHistorial.DataSource = listaFinal;
            rptHistorial.DataBind();

            phNoHistorial.Visible = !listaFinal.Any();
        }
    }
}