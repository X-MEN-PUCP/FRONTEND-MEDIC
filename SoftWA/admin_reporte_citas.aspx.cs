using SoftBO;
using SoftBO.adminWS;
using SoftBO.reporteCitasWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class admin_reporte_citas : System.Web.UI.Page
    {
        private readonly ReporteCitasBO _reporteCitasBO;
        private readonly EspecialidadBO _especialidadBO;
        private readonly UsuarioBO _usuarioBO;

        public admin_reporte_citas()
        {
            _reporteCitasBO = new ReporteCitasBO();
            _especialidadBO = new EspecialidadBO();
            _usuarioBO = new UsuarioBO();
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
            try
            {
                // Poblar especialidades
                var especialidades = _especialidadBO.ListarEspecialidad();
                ddlEspecialidadReporte.DataSource = especialidades.OrderBy(e => e.nombreEspecialidad);
                ddlEspecialidadReporte.DataTextField = "nombreEspecialidad";
                ddlEspecialidadReporte.DataValueField = "idEspecialidad";
                ddlEspecialidadReporte.DataBind();
                ddlEspecialidadReporte.Items.Insert(0, new ListItem("-- Todas --", "0"));

                // Poblar doctores
                PoblarFiltroDoctores();
            }
            catch (Exception ex)
            {
                // Manejar error de conexión
                System.Diagnostics.Debug.WriteLine("Error poblando filtros: " + ex.Message);
            }
        }

        private void PoblarFiltroDoctores()
        {
            try
            {
                ddlDoctorReporte.Items.Clear();
                //var todosLosUsuarios = _usuarioBO.ListarTodos();
                var todosLosUsuarios = new List<usuarioDTO>();

                // Filtrar solo los que tienen el rol de Médico (asumiendo ID de Rol 2)
                //var doctores = todosLosUsuarios
                //    .Where(u => u.roles != null && u.roles.Contains(2))
                //    .OrderBy(d => d.apellidoPaterno)
                //    .ThenBy(d => d.nombres)
                //    .Select(d => new ListItem($"{d.apellidoPaterno} {d.apellidoMaterno}, {d.nombres}", d.idUsuario.ToString()))
                //    .ToList();

                //ddlDoctorReporte.Items.AddRange(doctores.ToArray());
                ddlDoctorReporte.Items.Insert(0, new ListItem("-- Todos --", "0"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error poblando doctores: " + ex.Message);
                ddlDoctorReporte.Items.Insert(0, new ListItem("-- Error al cargar --", "0"));
            }
        }

        protected void ddlEspecialidadReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            // En el futuro, se podría refinar la lista de doctores según la especialidad seleccionada.
            // Por ahora, la lista de doctores es general.
            updReporteCitas.Update();
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
            ddlDoctorReporte.SelectedValue = "0";
            ddlOrdenarReporte.SelectedValue = "FechaDesc";
            AplicarFiltrosYActualizarReporte();
        }

        private void AplicarFiltrosYActualizarReporte()
        {
            try
            {
                var listaCitas = ObtenerDatosReporte();

                // Ordenamiento se realiza en el lado del cliente (C#)
                string orden = ddlOrdenarReporte.SelectedValue;
                switch (orden)
                {
                    case "FechaAsc": listaCitas = listaCitas.OrderBy(c => Convert.ToDateTime(c.fechaCita)).ThenBy(c => c.hora).ToList(); break;
                    case "PacienteAsc": listaCitas = listaCitas.OrderBy(c => c.paciente).ToList(); break;
                    case "PacienteDesc": listaCitas = listaCitas.OrderByDescending(c => c.paciente).ToList(); break;
                    case "DoctorAsc": listaCitas = listaCitas.OrderBy(c => c.doctor).ToList(); break;
                    case "DoctorDesc": listaCitas = listaCitas.OrderByDescending(c => c.doctor).ToList(); break;
                    case "EspecialidadAsc": listaCitas = listaCitas.OrderBy(c => c.especialidad).ToList(); break;
                    case "EspecialidadDesc": listaCitas = listaCitas.OrderByDescending(c => c.especialidad).ToList(); break;
                    case "FechaDesc": default: listaCitas = listaCitas.OrderByDescending(c => Convert.ToDateTime(c.fechaCita)).ThenByDescending(c => c.hora).ToList(); break;
                }

                lvCitas.DataSource = listaCitas;
                lvCitas.DataBind();

                var phNoReporte = lvCitas.FindControl("phNoReporte") as PlaceHolder;
                if (phNoReporte != null)
                {
                    phNoReporte.Visible = !listaCitas.Any();
                }

                CalcularYMostrarEstadisticas(listaCitas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al generar reporte: " + ex.Message);
                var phNoReporte = lvCitas.FindControl("phNoReporte") as PlaceHolder;
                if (phNoReporte != null)
                {
                    var lit = new Literal();
                    lit.Text = "<div class='alert alert-danger'>Ocurrió un error al conectar con el servicio de reportes.</div>";
                    phNoReporte.Controls.Add(lit);
                    phNoReporte.Visible = true;
                }
                lvCitas.DataSource = null;
                lvCitas.DataBind();
            }
        }
        private List<reporteCitaDTO> ObtenerDatosReporte()
        {
            // Recoger valores de los filtros
            string fechaDesde = string.IsNullOrEmpty(txtFechaDesdeReporte.Text) ? null : txtFechaDesdeReporte.Text;
            string fechaHasta = string.IsNullOrEmpty(txtFechaHastaReporte.Text) ? null : txtFechaHastaReporte.Text;

            int.TryParse(ddlEspecialidadReporte.SelectedValue, out int idEspecialidad);

            int.TryParse(ddlDoctorReporte.SelectedValue, out int idDoctor);

            // Llamar al servicio web
            var reporte = _reporteCitasBO.ReporteCitasGeneral(
                idEspecialidad,
                idDoctor,
                fechaDesde,
                fechaHasta
            );

            return reporte.ToList();
        }

        protected void btnExportarCSV_Click(object sender, EventArgs e)
        {
            try
            {
                var listaParaExportar = ObtenerDatosReporte();

                if (!listaParaExportar.Any())
                {
                    // Opcional: mostrar un mensaje si no hay datos para exportar
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No hay datos para exportar con los filtros actuales.');", true);
                    return;
                }

                // Usamos StringBuilder para construir el contenido del CSV
                StringBuilder sb = new StringBuilder();

                // Encabezados
                sb.AppendLine("ID Cita,Paciente,Especialidad,CMP Doctor,Doctor,Fecha Cita,Hora");

                // Filas de datos
                foreach (var cita in listaParaExportar)
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
                        cita.idCita,
                        SanitizeCsvField(cita.paciente),
                        SanitizeCsvField(cita.especialidad),
                        SanitizeCsvField(cita.codMedico),
                        SanitizeCsvField(cita.doctor),
                        Convert.ToDateTime(cita.fechaCita).ToString("yyyy-MM-dd"),
                        cita.hora
                    );
                    sb.AppendLine();
                }

                // Configurar la respuesta HTTP para la descarga del archivo
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", $"attachment;filename=ReporteCitas_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                Response.Charset = "utf-8";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "text/csv";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al exportar CSV: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Ocurrió un error al generar el archivo de exportación.');", true);
            }
        }

        // Función auxiliar para limpiar los campos del CSV
        private string SanitizeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }
            // Reemplaza las comillas dobles con dos comillas dobles para escaparlas en CSV
            return field.Replace("\"", "\"\"");
        }

        private void CalcularYMostrarEstadisticas(List<reporteCitaDTO> listaCitas)
        {
            if (listaCitas == null || !listaCitas.Any())
            {
                lblMasSolicitadaEspecialidad.Text = "No hay datos para los filtros aplicados.";
                lblMasSolicitadoDoctor.Text = "No hay datos para los filtros aplicados.";
                return;
            }

            var topEspecialidad = listaCitas
                .GroupBy(c => c.especialidad)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            var topDoctor = listaCitas
                .GroupBy(c => c.doctor)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            lblMasSolicitadaEspecialidad.Text = topEspecialidad != null ? $"{topEspecialidad.Name} ({topEspecialidad.Count} {(topEspecialidad.Count == 1 ? "cita" : "citas")})" : "N/A";
            lblMasSolicitadoDoctor.Text = topDoctor != null ? $"{topDoctor.Name} ({topDoctor.Count} {(topDoctor.Count == 1 ? "cita" : "citas")})" : "N/A";
        }
    }
}