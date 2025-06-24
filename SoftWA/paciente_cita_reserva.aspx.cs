using SoftBO.citaWS;
using SoftBO.especialidadWS;
using SoftBO.loginWS;
using SoftBO.pacienteWS;
using SoftBO.usuarioporespecialidadWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SoftWA
{
    public partial class paciente_cita_reserva : System.Web.UI.Page
    {
        private Dictionary<DateTime, List<TimeSpan>> HorariosDisponiblesPorFecha
        {
            get { return ViewState["HorariosDisponibles"] as Dictionary<DateTime, List<TimeSpan>> ?? new Dictionary<DateTime, List<TimeSpan>>(); }
            set { ViewState["HorariosDisponibles"] = value; }
        }
        private List<DateTime> FechasDisponibles
        {
            get { return HorariosDisponiblesPorFecha.Keys.ToList(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
                if (Session["PreloadEspecialidadId"] != null || Session["PreloadMedicoId"] != null)
                {
                    string especialidadId = Session["PreloadEspecialidadId"].ToString();
                    string medicoId = Session["PreloadMedicoId"].ToString();
                    Session.Remove("PreloadEspecialidadId");
                    Session.Remove("PreloadMedicoId");
                    PrecargarFiltros(especialidadId, medicoId);
                }
                else
                {
                    ddlMedico.Enabled = false;
                    divHorarios.Visible = false;
                    pnlResultados.Visible = false;
                }
            }
        }
        private void PrecargarFiltros(string especialidadId, string medicoId)
        {
            try
            {
                if (ddlEspecialidad.Items.FindByValue(especialidadId) != null)
                {
                    ddlEspecialidad.SelectedValue = especialidadId;
                    CargarMedicosPorEspecialidad();
                    if (ddlMedico.Items.FindByValue(medicoId) != null)
                    {
                        ddlMedico.SelectedValue = medicoId;
                    }
                    ActualizarDisponibilidadCompleta();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error precargando filtros: {ex}");
            }
        }
        private void CargarEspecialidades()
        {
            try
            {
                using (var servicioEspecialidad = new EspecialidadWSClient())
                {
                    var especialidades = servicioEspecialidad.listarEspecialidad();
                    ddlEspecialidad.DataSource = especialidades;
                    ddlEspecialidad.DataTextField = "nombreEspecialidad";
                    ddlEspecialidad.DataValueField = "idEspecialidad";
                    ddlEspecialidad.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargando especialidades: " + ex.Message);
            }
            ddlEspecialidad.Items.Insert(0, new ListItem("-- Seleccione Especialidad --", ""));
        }
        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMedicosPorEspecialidad();
            LimpiarFiltrosDependientes(limpiarMedico: false);
            ActualizarDisponibilidadCompleta();
        }
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarFiltrosDependientes(limpiarMedico: false);
            ActualizarDisponibilidadCompleta();
        }
        private void ActualizarDisponibilidadCompleta()
        {
            HorariosDisponiblesPorFecha = new Dictionary<DateTime, List<TimeSpan>>();
            lblCalendarioStatus.Visible = true;
            updFiltrosCitas.Update();

            if (!int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) || idEspecialidad == 0)
            {
                lblCalendarioStatus.Visible = false;
                calFechaCita.DataBind();
                return;
            }

            int.TryParse(ddlMedico.SelectedValue, out int idMedico);

            try
            {
                using (var servicioCita = new CitaWSClient())
                {
                    var citasDisponibles = servicioCita.buscarCitasoloCalendario(idEspecialidad, idMedico, null, null, SoftBO.citaWS.estadoCita.DISPONIBLE);

                    if (citasDisponibles != null && citasDisponibles.Any())
                    {
                        var disponibilidad = citasDisponibles
                            .Where(c => c != null && !string.IsNullOrEmpty(c.fechaCita) && !string.IsNullOrEmpty(c.horaInicio))
                            .Select(c => new
                            {
                                Fecha = DateTime.TryParse(c.fechaCita, out var dt) ? dt.Date : DateTime.MinValue,
                                Hora = TimeSpan.TryParse(c.horaInicio, out var ts) ? ts : TimeSpan.MinValue
                            })
                            .Where(c => c.Fecha != DateTime.MinValue && c.Hora != TimeSpan.MinValue)
                            .GroupBy(c => c.Fecha)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(c => c.Hora)
                                      .Distinct()
                                      .OrderBy(t => t)
                                      .ToList()
                            );
                        if (disponibilidad.Any())
                        {
                            HorariosDisponiblesPorFecha = disponibilidad;
                            lblCalendarioStatus.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error actualizando disponibilidad completa: " + ex.Message);
            }
            calFechaCita.DataBind();
        }
        protected void calFechaCita_SelectionChanged(object sender, EventArgs e)
        {
            divHorarios.Visible = false;
            rblHorarios.Items.Clear();
            LimpiarResultadosBusqueda();

            if (calFechaCita.SelectedDate.Date >= DateTime.Today)
            {
                lblFechaSeleccionadaInfo.Text = "Fecha seleccionada: " + calFechaCita.SelectedDate.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
                lblFechaSeleccionadaInfo.Visible = true;
                var disponibilidad = HorariosDisponiblesPorFecha;
                if (disponibilidad.ContainsKey(calFechaCita.SelectedDate.Date))
                {
                    var horariosParaFecha = disponibilidad[calFechaCita.SelectedDate.Date];

                    rblHorarios.DataSource = horariosParaFecha;
                    rblHorarios.DataBind();
                    divHorarios.Visible = true;
                    lblErrorHorario.Visible = false;
                }
                else
                {
                    lblErrorHorario.Text = "No se encontraron horarios para este día.";
                    lblErrorHorario.Visible = true;
                }
            }
        }
        protected void calFechaCita_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.CssClass += " day-disabled";
                return;
            }
            if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                e.Day.IsSelectable = false;
                e.Cell.ToolTip = "Seleccione una especialidad para ver disponibilidad";
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
            pnlResultados.Visible = true;
            if (!int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) || idEspecialidad == 0) return;
            if (calFechaCita.SelectedDate.Year == 1) return;
            lblErrorBusqueda.Visible = false;
            int.TryParse(ddlMedico.SelectedValue, out int idMedico);
            DateTime fecha = calFechaCita.SelectedDate;
            string horaSeleccionada = rblHorarios.SelectedValue;

            try
            {
                using (var servicioCita = new CitaWSClient())
                {
                    var resultados = servicioCita.buscarCitasWSCitas(idEspecialidad, idMedico, fecha.ToString("yyyy-MM-dd"),
                                                                     string.IsNullOrEmpty(horaSeleccionada) ? null : horaSeleccionada,
                                                                     SoftBO.citaWS.estadoCita.DISPONIBLE);

                    if (resultados != null && resultados.Any())
                    {
                        var citasParaMostrar = resultados
                            .Where(c => c.especialidad != null && c.medico != null && c.turno != null)
                            .Select(c => new
                            {
                                IdCitaDisponible = c.idCita,
                                NombreEspecialidad = c.especialidad.nombreEspecialidad,
                                NombreMedico = $"{c.medico.nombres} {c.medico.apellidoPaterno}",
                                FechaCita = DateTime.Parse(c.fechaCita),
                                DescripcionHorario = c.horaInicio.Substring(0, 5),
                                Precio = c.especialidad.precioConsulta
                            }).ToList();

                        rptResultadosCitas.DataSource = citasParaMostrar;
                        rptResultadosCitas.DataBind();
                        phNoResultados.Visible = !citasParaMostrar.Any();
                    }
                    else
                    {
                        LimpiarResultadosBusqueda();
                        phNoResultados.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error buscando citas: " + ex.Message);
                LimpiarResultadosBusqueda();
                phNoResultados.Visible = true;
            }
        }
        #region Metodos de Limpieza y UI
        private void LimpiarFiltrosDependientes(bool limpiarMedico = true)
        {
            if (limpiarMedico)
            {
                ddlMedico.Items.Clear();
                ddlMedico.Items.Add(new ListItem("-- Cualquier Médico --", ""));
                ddlMedico.Enabled = false;
            }
            calFechaCita.SelectedDates.Clear();
            lblFechaSeleccionadaInfo.Visible = false;
            divHorarios.Visible = false;
            rblHorarios.Items.Clear();
            lblErrorHorario.Visible = false;
            HorariosDisponiblesPorFecha = new Dictionary<DateTime, List<TimeSpan>>();
            LimpiarResultadosBusqueda();
        }
        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            ddlEspecialidad.ClearSelection();
            LimpiarFiltrosDependientes(limpiarMedico: true);
            calFechaCita.DataBind();
        }
        private void LimpiarResultadosBusqueda()
        {
            rptResultadosCitas.DataSource = null;
            rptResultadosCitas.DataBind();
            phNoResultados.Visible = true;
            ltlMensajeReserva.Text = "";
            pnlResultados.Visible = false;
        }
        private void CargarMedicosPorEspecialidad()
        {
            ddlMedico.Items.Clear();
            if (int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) && idEspecialidad > 0)
            {
                try
                {
                    using (var servicioMedicos = new UsuarioPorEspecialidadWSClient())
                    {
                        var medicos = servicioMedicos.listarPorEspecialidadUsuarioPorEspecialidad(idEspecialidad);
                        var listaMedicos = medicos.Select(m => new ListItem(
                            $"{m.usuario.nombres} {m.usuario.apellidoPaterno}",
                            m.usuario.idUsuario.ToString()
                        )).ToList();
                        ddlMedico.DataSource = listaMedicos;
                        ddlMedico.DataTextField = "Text";
                        ddlMedico.DataValueField = "Value";
                        ddlMedico.DataBind();
                    }
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
        private void ProcesarReserva()
        {
            if (!int.TryParse(hfModalCitaId.Value, out int idCita) || idCita == 0)
            {
                ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Error: No se pudo identificar la cita a reservar.</div>";
                CerrarModalDesdeServidor();
                return;
            }
            var usuarioLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (usuarioLogueado == null)
            {
                ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Error: Su sesión ha expirado. Por favor, inicie sesión de nuevo.</div>";
                CerrarModalDesdeServidor();
                return;
            }
            try
            {
                SoftBO.citaWS.citaDTO citaCompleta;
                using (var servicioCita = new CitaWSClient())
                {
                    citaCompleta = servicioCita.obtenerPorIdCitaCita(idCita);
                }
                if (citaCompleta == null)
                {
                    ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Error: La cita ya no existe.</div>";
                    CerrarModalDesdeServidor();
                    return;
                }
                if (citaCompleta.medico == null || citaCompleta.medico.idUsuario == 0 ||
                    citaCompleta.especialidad == null || citaCompleta.especialidad.idEspecialidad == 0 ||
                    citaCompleta.consultorio == null || citaCompleta.consultorio.idConsultorio == 0)
                {
                    ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Error: Los detalles de la cita son incompletos.</div>";
                    CerrarModalDesdeServidor();
                    return;
                }

                var citaParaApi = MapearCitaParaPacienteWS(citaCompleta);
                var pacienteParaApi = new SoftBO.pacienteWS.usuarioDTO
                {
                    idUsuario = usuarioLogueado.idUsuario,
                    idUsuarioSpecified = true
                };
                int resultadoReserva;
                using (var servicioPaciente = new PacienteWSClient())
                {
                    resultadoReserva = servicioPaciente.reservarCitaPaciente(citaParaApi, pacienteParaApi);
                }

                if (resultadoReserva > 0)
                {
                    LimpiarResultadosBusqueda();
                    pnlResultados.Visible = true;
                    ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>¡Su cita ha sido reservada con éxito!</div>";
                    Session["CitaIdParaPago"] = idCita;
                    string scriptRedireccion = "setTimeout(function() { window.location.href = 'paciente_pago_cita.aspx'; }, 3000);";
                    ScriptManager.RegisterStartupScript(updResultadosCitas, updResultadosCitas.GetType(), "RedirectAfterReserve", scriptRedireccion, true);
                    Response.Redirect("paciente_pago_cita.aspx", false);
                }
                else
                {
                    ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>No se pudo completar la reserva, la cita ya no esté disponible.</div>";
                    btnBuscarCitas_Click(this, EventArgs.Empty);
                    CerrarModalDesdeServidor();
                }
            }
            catch (Exception ex)
            {
                ltlMensajeReserva.Text = "<div class='alert alert-danger mt-3'>Ocurrió un error inesperado al procesar su reserva.</div>";
                System.Diagnostics.Debug.WriteLine("Error al reservar cita: " + ex.ToString());
                CerrarModalDesdeServidor();
            }
        }
        private SoftBO.pacienteWS.citaDTO MapearCitaParaPacienteWS(SoftBO.citaWS.citaDTO citaOriginal)
        {
            if (citaOriginal == null) return null;

            var citaMapeada = new SoftBO.pacienteWS.citaDTO();

            citaMapeada.idCita = citaOriginal.idCita;
            citaMapeada.idCitaSpecified = true;
            citaMapeada.fechaCita = citaOriginal.fechaCita;
            citaMapeada.horaInicio = citaOriginal.horaInicio;
            citaMapeada.horaFin = citaOriginal.horaFin;
            if (citaOriginal.estadoSpecified)
            {
                string nombreEstado = citaOriginal.estado.ToString().ToUpperInvariant();
                SoftBO.pacienteWS.estadoCita estadoDestino;
                if (Enum.TryParse<SoftBO.pacienteWS.estadoCita>(nombreEstado, true, out estadoDestino))
                {
                    citaMapeada.estado = estadoDestino;
                    citaMapeada.estadoSpecified = true;
                }
            }
            citaMapeada.usuarioCreacion = citaOriginal.usuarioCreacion;
            citaMapeada.usuarioCreacionSpecified = true;
            citaMapeada.fechaCreacion = citaOriginal.fechaCreacion;
            if (citaOriginal.medico != null)
            {
                citaMapeada.medico = new SoftBO.pacienteWS.usuarioDTO
                {
                    idUsuario = citaOriginal.medico.idUsuario,
                    idUsuarioSpecified = true
                };
            }
            if (citaOriginal.especialidad != null)
            {
                citaMapeada.especialidad = new SoftBO.pacienteWS.especialidadDTO
                {
                    idEspecialidad = citaOriginal.especialidad.idEspecialidad,
                    idEspecialidadSpecified = true
                };
            }
            if (citaOriginal.consultorio != null)
            {
                citaMapeada.consultorio = new SoftBO.pacienteWS.consultorioDTO
                {
                    idConsultorio = citaOriginal.consultorio.idConsultorio,
                    idConsultorioSpecified = true
                };
            }
            if (citaOriginal.turno != null)
            {
                citaMapeada.turno = new SoftBO.pacienteWS.turnoDTO
                {
                    idTurno = citaOriginal.turno.idTurno,
                    idTurnoSpecified = true
                };
            }
            return citaMapeada;
        }
        protected void btnModalPagarDespues_Click(object sender, EventArgs e)
        {
            ProcesarReserva();
        }
        protected void btnModalPagarAhora_Click(object sender, EventArgs e)
        {
            ProcesarReserva();
        }
        private void RedirigirAPago(int idCita)
        {
            Session["CitaIdParaPago"] = idCita;
            Response.Redirect("paciente_pago_cita.aspx", false);
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
        private void CerrarModalDesdeServidor()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModalScript", "cerrarModalConfirmacion();", true);
        }
        #endregion
    }
}