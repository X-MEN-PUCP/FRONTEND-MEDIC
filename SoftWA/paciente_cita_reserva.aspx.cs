using SoftBO;
using SoftBO.citaWS;
using SoftBO.especialidadWS;
using SoftBO.loginWS;
using SoftBO.pacienteWS;
using SoftBO.usuarioporespecialidadWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class paciente_cita_reserva : System.Web.UI.Page
    {
        private readonly CitaBO _citaBO;
        private readonly EspecialidadBO _especialidadBO;
        private readonly UsuarioPorEspecialidadBO _usuarioPorEspecialidadBO;
        private readonly PacienteBO _pacienteBO;
        private List<DateTime> FechasDisponibles 
        {
            get {  return ViewState["FechasDisponibles"] as List<DateTime> ?? new List<DateTime>(); }
            set { ViewState["FechasDisponibles"] = value; }
        }
        public paciente_cita_reserva()
        {
            _citaBO = new CitaBO();
            _especialidadBO = new EspecialidadBO();
            _usuarioPorEspecialidadBO = new UsuarioPorEspecialidadBO();
            _pacienteBO = new PacienteBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
                ddlMedico.Enabled = false;
                phNoResultados.Visible = true;
            }
        }
        private void CargarEspecialidades()
        {
            try
            {
                var especialidades = _especialidadBO.ListarEspecialidad();
                ddlEspecialidad.DataSource = especialidades;
                ddlEspecialidad.DataTextField = "nombreEspecialidad";
                ddlEspecialidad.DataValueField = "idEspecialidad";
                ddlEspecialidad.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargando especialidades: " + ex.Message);
            }
            ddlEspecialidad.Items.Insert(0, new ListItem("-- Seleccione Especialidad --", ""));
        }
        private void CargarMedicosPorEspecialidad()
        {
            ddlMedico.Items.Clear();
            if (int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) && idEspecialidad > 0)
            {
                try
                {
                    var medicos = _usuarioPorEspecialidadBO.ListarPorEspecialidadUsuarioPorEspecialidad(idEspecialidad);
                    var listaMedicos = medicos.Select(m => new ListItem(
                        $"{m.usuario.nombres} {m.usuario.apellidoPaterno}",
                        m.usuario.idUsuario.ToString()
                    )).ToList();
                    ddlMedico.DataSource = listaMedicos;
                    ddlMedico.DataTextField = "Text";
                    ddlMedico.DataValueField = "Value";
                    ddlMedico.DataBind();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error cargando médicos: " + ex.Message);
                }
                ddlMedico.Enabled = true;
            }
            else
            {
                ddlMedico.Enabled = false;
            }
            ddlMedico.Items.Insert(0, new ListItem("-- Cualquier Médico --", ""));
        }
        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMedicosPorEspecialidad();
            LimpiarResultados();
            ActualizarFechasDisponibles();
        }
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarResultados();
            ActualizarFechasDisponibles();
        }
        private void ActualizarFechasDisponibles()
        {
            //FechasDisponibles = new List<DateTime>();
            //if (!int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) || idEspecialidad == 0)
            //{
            //    calFechaCita.DataBind();
            //    return;
            //}
            //int.TryParse(ddlMedico.SelectedValue, out int idMedico);
            //try
            //{
            //    var fechasComoString = _citaBO.ListarFechasDisponibles(idEspecialidad, idMedico);

            //    if (fechasComoString != null)
            //    {
            //        var fechasParseadas = new List<DateTime>();
            //        foreach (var fechaStr in fechasComoString)
            //        {
            //            if (DateTime.TryParse(fechaStr, out DateTime fecha))
            //            {
            //                fechasParseadas.Add(fecha);
            //            }
            //        }
            //        FechasDisponibles = fechasParseadas;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("Error actualizando fechas disponibles: " + ex.Message);
            //}
            //calFechaCita.DataBind();
        }
        protected void calFechaCita_SelectionChanged(object sender, EventArgs e)
        {
            if (calFechaCita.SelectedDate >= DateTime.Today)
            {
                lblFechaSeleccionadaInfo.Text = "Fecha seleccionada: " + calFechaCita.SelectedDate.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
                lblFechaSeleccionadaInfo.Visible = true;
                lblErrorBusqueda.Visible = false;
            }
            LimpiarResultados();
        }
        protected void calFechaCita_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.CssClass += "day-disabled";
                return;
            }
            if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                e.Day.IsSelectable = false;
                e.Cell.ToolTip = "Seleccione una especialidad para ver citas disponibles";
            }
            else if (FechasDisponibles.Contains(e.Day.Date))
            {
                e.Cell.CssClass += " day-available";
                e.Cell.ToolTip = "Hay horarios disponibles este día";
            }
            else
            {
                e.Day.IsSelectable = false;
                e.Cell.ToolTip = "No hay citas disponibles este día";
            }
        }
        protected void btnBuscarCitas_Click(object sender, EventArgs e)
        {
            lblErrorBusqueda.Visible = false;
            if (!int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) || idEspecialidad == 0)
            {
                lblErrorBusqueda.Text = "Seleccione una especialidad para buscar citas.";
                lblErrorBusqueda.Visible = true;
                return;
            }
            if (calFechaCita.SelectedDate.Year == 1)
            {
                lblErrorBusqueda.Text = "Seleccione una fecha válida para buscar citas.";
                lblErrorBusqueda.Visible = true;
                return;
            }
            int.TryParse(ddlMedico.SelectedValue, out int idMedico);
            DateTime fecha = calFechaCita.SelectedDate;
            try
            {
                var resultados = _citaBO.BuscarCitasDisponibles(idEspecialidad, idMedico, fecha);

                if (resultados != null && resultados.Any())
                {
                    var citasParaMostrar = resultados.Select(c => new
                        {
                            IdCitaDisponible = c.idCita,
                            NombreEspecialidad = c.especialidad.nombreEspecialidad,
                            NombreMedico = $"{c.medico.nombres} {c.medico.apellidoPaterno}",
                            FechaCita = c.fechaCita,
                            DescripcionHorario = c.turno.horaInicio.ToString(@"hh\:mm"),
                            Precio = c.especialidad.precioConsulta
                    }).ToList();

                    rptResultadosCitas.DataSource = citasParaMostrar;
                    rptResultadosCitas.DataBind();
                    phNoResultados.Visible = false;
                }
                else
                {
                    LimpiarResultados();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error buscando citas: " + ex.Message);
                phNoResultados.Visible = true;
            }
        }
        #region Metodos de Limpieza y UI
        private void LimpiarResultados()
        {
            rptResultadosCitas.DataSource = null;
            rptResultadosCitas.DataBind();
            phNoResultados.Visible = true;
            ltlMensajeReserva.Text = "";
            lblErrorBusqueda.Visible = false;
        }
        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            ddlEspecialidad.ClearSelection();
            ddlMedico.Items.Clear();
            ddlMedico.Items.Add(new ListItem("-- Cualquier Médico --", ""));
            ddlMedico.Enabled = false;
            calFechaCita.SelectedDates.Clear();
            lblFechaSeleccionadaInfo.Visible = false;
            FechasDisponibles = new List<DateTime>();
            LimpiarResultados();
        }
        protected void btnModalPagarDespues_Click(object sender, EventArgs e)
        {
            int.TryParse(hfModalCitaId.Value, out int idCita);
            var usuario = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;

            if (idCita == 0 || usuario == null)
            {
                ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Error: No se pudo identificar la cita o el usuario.</div>";
                return;
            }

            try
            {
                SoftBO.citaWS.citaDTO citaAReservar;
                using (var servicioCita = new CitaWSClient())
                {
                    citaAReservar = servicioCita.obtenerPorIdCitaCita(idCita);
                }

                if (citaAReservar == null)
                {
                    ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Error: La cita seleccionada ya no está disponible.</div>";
                    return;
                }

                var paciente = new SoftBO.pacienteWS.usuarioDTO { idUsuario = usuario.idUsuario };

                using (var servicioPaciente = new PacienteWSClient())
                {
                    var citaParaReservarApi = new SoftBO.pacienteWS.citaDTO { idCita = citaAReservar.idCita };
                    int resultado = servicioPaciente.reservarCitaPaciente(citaParaReservarApi, paciente);

                    if (resultado > 0)
                    {
                        ltlMensajeReserva.Text = "<div class='alert alert-info mt-3'>Su cita ha sido reservada con éxito. Tiene 24 horas para completar el pago.</div>";
                        btnBuscarCitas_Click(sender, e);
                    }
                    else
                    {
                        ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>No se pudo completar la reserva. La cita puede haber sido tomada por otro usuario.</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Ocurrió un error al procesar la reserva.</div>";
                System.Diagnostics.Debug.WriteLine("Error al reservar: " + ex.ToString());
            }
            CerrarModalDesdeServidor();
        }
        protected void rptResultadosCitas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic cita = e.Item.DataItem;
                LinkButton btnAccion = (LinkButton)e.Item.FindControl("btnAccionReserva");

                btnAccion.Text = "<i class='fa-solid fa-check-circle me-1'></i>Reservar";
                btnAccion.CssClass = "btn btn-success btn-sm";
                btnAccion.Enabled = true;
                btnAccion.OnClientClick = string.Format("return mostrarModalConfirmacionReserva({0});", cita.IdCitaDisponible);
            }
        }
        protected void btnModalPagarAhora_Click(object sender, EventArgs e)
        {
            int.TryParse(hfModalCitaId.Value, out int idCitaModal);
            CerrarModalDesdeServidor();
        }
        private void CerrarModalDesdeServidor()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModalScript", "cerrarModalConfirmacion();", true);
        }
        #endregion
    }
}