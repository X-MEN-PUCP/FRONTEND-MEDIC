using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class admin_reporte_citas : System.Web.UI.Page
    {
        protected class CitaAtendida
        {
            public int IdCita { get; set; }
            public string NombrePaciente { get; set; }
            public string Especialidad { get; set; }
            public int IdDoctor { get; set; }
            public string CmpDoctor { get; set; }
            public string NombreDoctor { get; set; }
            public DateTime FechaCita { get; set; }
            public string Horario { get; set; }
        }

        private static List<CitaAtendida> _listaGlobalCitasAtendidas;

        static admin_reporte_citas()
        {
            _listaGlobalCitasAtendidas = GenerarDatosMuestraCompletos(100);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PoblarFiltrosDropDown();
                AplicarFiltrosYActualizarReporte();
            }
        }

        private void PoblarFiltrosDropDown()
        {
            var especialidades = _listaGlobalCitasAtendidas
                                .Select(c => c.Especialidad)
                                .Distinct()
                                .OrderBy(e => e)
                                .ToList();
            ddlEspecialidadReporte.DataSource = especialidades;
            ddlEspecialidadReporte.DataBind();
            ddlEspecialidadReporte.Items.Insert(0, new ListItem("-- Todas --", "0"));
            PoblarFiltroDoctores();
        }

        private void PoblarFiltroDoctores(string especialidadFiltro = "0")
        {
            ddlDoctorReporte.Items.Clear();
            IEnumerable<CitaAtendida> citasParaDoctores = _listaGlobalCitasAtendidas;

            if (especialidadFiltro != "0" && !string.IsNullOrEmpty(especialidadFiltro))
            {
                citasParaDoctores = citasParaDoctores.Where(c => c.Especialidad == especialidadFiltro);
            }

            var doctores = citasParaDoctores
                            .Select(c => new { Id = c.IdDoctor, Nombre = c.NombreDoctor })
                            .Distinct()
                            .OrderBy(d => d.Nombre)
                            .ToList();

            ddlDoctorReporte.DataSource = doctores;
            ddlDoctorReporte.DataTextField = "Nombre";
            ddlDoctorReporte.DataValueField = "Id";
            ddlDoctorReporte.DataBind();
            ddlDoctorReporte.Items.Insert(0, new ListItem("-- Todos --", "0"));
            ddlDoctorReporte.Enabled = doctores.Any();
        }


        protected void ddlEspecialidadReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            PoblarFiltroDoctores(ddlEspecialidadReporte.SelectedValue);
        }

        protected void btnAplicarFiltrosReporte_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYActualizarReporte();
        }

        protected void btnLimpiarFiltrosReporte_Click(object sender, EventArgs e)
        {
            txtFechaDesdeReporte.Text = string.Empty;
            txtFechaHastaReporte.Text = string.Empty;
            ddlEspecialidadReporte.SelectedValue = "0";
            PoblarFiltroDoctores();
            ddlDoctorReporte.SelectedValue = "0";
            ddlOrdenarReporte.SelectedValue = "FechaDesc";
            AplicarFiltrosYActualizarReporte();
        }

        private void AplicarFiltrosYActualizarReporte()
        {
            IEnumerable<CitaAtendida> citasFiltradas = _listaGlobalCitasAtendidas;

            if (!string.IsNullOrEmpty(txtFechaDesdeReporte.Text))
            {
                DateTime fechaDesde;
                if (DateTime.TryParse(txtFechaDesdeReporte.Text, out fechaDesde))
                {
                    citasFiltradas = citasFiltradas.Where(c => c.FechaCita.Date >= fechaDesde.Date);
                }
            }

            if (!string.IsNullOrEmpty(txtFechaHastaReporte.Text))
            {
                DateTime fechaHasta;
                if (DateTime.TryParse(txtFechaHastaReporte.Text, out fechaHasta))
                {
                    citasFiltradas = citasFiltradas.Where(c => c.FechaCita.Date <= fechaHasta.Date);
                }
            }

            if (ddlEspecialidadReporte.SelectedValue != "0" && !string.IsNullOrEmpty(ddlEspecialidadReporte.SelectedValue))
            {
                citasFiltradas = citasFiltradas.Where(c => c.Especialidad == ddlEspecialidadReporte.SelectedValue);
            }

            if (ddlDoctorReporte.SelectedValue != "0" && !string.IsNullOrEmpty(ddlDoctorReporte.SelectedValue))
            {
                int idDoctorFiltro = 0;
                if (int.TryParse(ddlDoctorReporte.SelectedValue, out idDoctorFiltro))
                {
                    citasFiltradas = citasFiltradas.Where(c => c.IdDoctor == idDoctorFiltro);
                }
            }

            string orden = ddlOrdenarReporte.SelectedValue;
            switch (orden)
            {
                case "FechaAsc": citasFiltradas = citasFiltradas.OrderBy(c => c.FechaCita).ThenBy(c => c.Horario); break;
                case "PacienteAsc": citasFiltradas = citasFiltradas.OrderBy(c => c.NombrePaciente); break;
                case "PacienteDesc": citasFiltradas = citasFiltradas.OrderByDescending(c => c.NombrePaciente); break;
                case "DoctorAsc": citasFiltradas = citasFiltradas.OrderBy(c => c.NombreDoctor); break;
                case "DoctorDesc": citasFiltradas = citasFiltradas.OrderByDescending(c => c.NombreDoctor); break;
                case "EspecialidadAsc": citasFiltradas = citasFiltradas.OrderBy(c => c.Especialidad); break;
                case "EspecialidadDesc": citasFiltradas = citasFiltradas.OrderByDescending(c => c.Especialidad); break;
                case "FechaDesc": default: citasFiltradas = citasFiltradas.OrderByDescending(c => c.FechaCita).ThenByDescending(c => c.Horario); break;
            }

            List<CitaAtendida> listaFinal = citasFiltradas.ToList();
            lvCitas.DataSource = listaFinal;
            lvCitas.DataBind();

            CalcularYMostrarEstadisticas(listaFinal);
        }

        private static List<CitaAtendida> GenerarDatosMuestraCompletos(int cantidad)
        {
            var lista = new List<CitaAtendida>();
            var rnd = new Random();
            var nombresPacientes = new[] { "Ana García", "Luis Martínez", "María Rodríguez", "José Hernández", "Carmen López", "Javier Pérez", "Isabel Gómez", "Manuel Sánchez", "Laura Díaz", "Pedro Moreno", "Elena Fernández", "David Ruiz", "Sara Jiménez", "Daniel González" };
            var especialidadesSource = new[] { "Cardiología", "Dermatología", "Pediatría", "Ginecología", "Medicina General", "Oftalmología", "Traumatología" };
            var doctoresSource = new Dictionary<string, List<(int, string, string)>> {
                { "Cardiología", new List<(int, string, string)> { (101, "Dr. Carlos Ruiz", "ASDB126"), (102, "Dra. Elena Castillo", "NIV128") } },
                { "Dermatología", new List<(int, string, string)> { (201, "Dra. Sofia Vargas", "TLK896"), (202, "Dr. Mario Bross", "BYG374") } },
                { "Pediatría", new List<(int, string, string)> {(301, "Dr. Andrés Molina", "MKO789"), (302, "Dra. Paula Navarro", "JOF897") } },
                { "Ginecología", new List<(int, string, string)> {(401, "Dra. Irene Gil", "FVT453") } },
                { "Medicina General", new List<(int, string, string)> { (501, "Dr. Ricardo Soler", "CUN286"), (502, "Dra. Beatriz Alonso", "IOU876") } },
                { "Oftalmología", new List<(int, string, string)> { (601, "Dr. Fernando Sáez", "VBI123") } },
                { "Traumatología", new List<(int, string, string)> {(701, "Dra. Lucia Méndez", "OIU675") } }
            };
            var horariosBase = new[] { "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "14:00", "14:30", "15:00", "15:30", "16:00" };

            for (int i = 1; i <= cantidad; i++)
            {
                string esp = especialidadesSource[rnd.Next(especialidadesSource.Length)];
                var doctoresEnEspecialidad = doctoresSource[esp];
                var doctorSeleccionado = doctoresEnEspecialidad[rnd.Next(doctoresEnEspecialidad.Count)];
                DateTime fecha = DateTime.Today.AddDays(-rnd.Next(1, 90));
                string hora = horariosBase[rnd.Next(horariosBase.Length)];

                lista.Add(new CitaAtendida
                {
                    IdCita = 1000 + i,
                    NombrePaciente = nombresPacientes[rnd.Next(nombresPacientes.Length)],
                    Especialidad = esp,
                    IdDoctor = doctorSeleccionado.Item1,
                    CmpDoctor = doctorSeleccionado.Item3,
                    NombreDoctor = doctorSeleccionado.Item2,
                    FechaCita = fecha,
                    Horario = hora
                });
            }
            return lista;
        }

        private void CalcularYMostrarEstadisticas(List<CitaAtendida> listaCitas)
        {
            if (listaCitas == null || !listaCitas.Any())
            {
                lblMasSolicitadaEspecialidad.Text = "No hay datos para los filtros aplicados.";
                lblMasSolicitadoDoctor.Text = "No hay datos para los filtros aplicados.";
                return;
            }

            var topEspecialidad = listaCitas
                .GroupBy(c => c.Especialidad)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            var topDoctor = listaCitas
                .GroupBy(c => c.NombreDoctor)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            lblMasSolicitadaEspecialidad.Text = topEspecialidad != null ? $"{topEspecialidad.Name} ({topEspecialidad.Count} {(topEspecialidad.Count == 1 ? "cita" : "citas")})" : "N/A";
            lblMasSolicitadoDoctor.Text = topDoctor != null ? $"{topDoctor.Name} ({topDoctor.Count} {(topDoctor.Count == 1 ? "cita" : "citas")})" : "N/A";
        }
    }
}