using SoftBO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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
                var servicioEspecialidad = new EspecialidadBO();
                var especialidades = servicioEspecialidad.ListarEspecialidad();
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
                var servicioCita = new CitaBO();
                var citasDisponibles = servicioCita.BuscarCitasWSCitas(idEspecialidad, idMedico, null, null, SoftBO.citaWS.estadoCita.DISPONIBLE);
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
                var servicioCita = new CitaBO();
                var resultados = servicioCita.BuscarCitasWSCitas(idEspecialidad, idMedico, fecha.ToString("yyyy-MM-dd"),
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
                    var servicioMedicos = new UsuarioPorEspecialidadBO();
                    var medicos = servicioMedicos.ListarPorEspecialidadUsuarioPorEspecialidad(idEspecialidad);
                    if(medicos != null && medicos.Any())
                    {
                        var listaMedicos = medicos.Select(m => new ListItem(
                            $"{m.usuario.nombres} {m.usuario.apellidoPaterno}",
                            m.usuario.idUsuario.ToString()
                        )).ToList();
                        ddlMedico.DataSource = listaMedicos;
                        ddlMedico.DataTextField = "Text";
                        ddlMedico.DataValueField = "Value";
                        ddlMedico.DataBind();
                        ddlMedico.Enabled = true;
                    }
                    else
                    {
                        ddlMedico.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error cargando médicos: " + ex.Message);
                    ddlMedico.Enabled = false;
                }
                ddlMedico.Enabled = true;
            }
            else
            {
                ddlMedico.Enabled = false;
            }
            ddlMedico.Items.Insert(0, new ListItem("-- Cualquier Médico --", ""));
        }
        private SoftBO.pacienteWS.citaDTO PrepararCitaReserva(int idCita)
        {
            SoftBO.citaWS.citaDTO citaCompleta;
            var servicioCita = new CitaBO();
            citaCompleta = servicioCita.ObtenerPorIdCitaCita(idCita);
            
            if (citaCompleta == null)
            {
                MostrarMensaje(ltlMensajeReserva, "Error: La cita ya no existe.", esError: true);
                CerrarModalDesdeServidor();
                return null;
            }
            if (citaCompleta.medico == null || citaCompleta.medico.idUsuario == 0 ||
                citaCompleta.especialidad == null || citaCompleta.especialidad.idEspecialidad == 0 ||
                citaCompleta.consultorio == null || citaCompleta.consultorio.idConsultorio == 0)
            {
                MostrarMensaje(ltlMensajeReserva, "Error: Los detalles de la cita son incompletos.", esError: true);
                CerrarModalDesdeServidor();
                return null;
            }
            var cita = MapearCitaParaPacienteWS(citaCompleta);
            return cita;
        }
        private SoftBO.pacienteWS.usuarioDTO PrepararPacienteReserva(int idusuario)
        {
            var paciente = new SoftBO.pacienteWS.usuarioDTO
            {
                idUsuario = idusuario,
                idUsuarioSpecified = true
            };
            return paciente;
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
        protected void btnConfirmarReserva_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfModalCitaId.Value, out int idCita) || idCita == 0)
            {
                MostrarMensaje(ltlMensajeReserva, "Error: No se pudo identificar la cita a reservar.", esError: true);
                CerrarModalDesdeServidor();
                return;
            }
            var usuarioLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (usuarioLogueado == null)
            {
                MostrarMensaje(ltlMensajeReserva, "Error: Su sesión ha expirado. Por favor, inicie sesión de nuevo.", esError: true);
                CerrarModalDesdeServidor();
                return;
            }
            try
            {
                int resultadoReserva;
                var servicioPaciente = new PacienteBO();
                var citaParaReserva = PrepararCitaReserva(idCita);
                var pacienteParaReserva = PrepararPacienteReserva(usuarioLogueado.idUsuario);
                resultadoReserva = servicioPaciente.ReservarCitaPaciente(citaParaReserva, pacienteParaReserva);
                if (resultadoReserva > 0)
                {
                    LimpiarResultadosBusqueda();
                    pnlResultados.Visible = true;
                    MostrarMensaje(ltlMensajeReserva, "¡Su cita ha sido reservada con éxito!", esError: false);
                    ActualizarDisponibilidadCompleta();
                }
                else
                {
                    MostrarMensaje(ltlMensajeReserva, "No se pudo completar la reserva, la cita ya no esté disponible.", esError: true);
                    btnBuscarCitas_Click(this, EventArgs.Empty);
                    //CerrarModalDesdeServidor();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ltlMensajeReserva, "Ocurrió un error inesperado al procesar su reserva.", esError: true);
                System.Diagnostics.Debug.WriteLine("Error al reservar cita: " + ex.ToString());
            }
            finally
            {
                CerrarModalDesdeServidor();
            }
        }
        private void MostrarMensaje(Literal lit, string mensaje, bool esError)
        {
            string cssClass = esError ? "alert alert-danger" : "alert alert-success";
            lit.Text = $"<div class='{cssClass} mt-3'>{Server.HtmlEncode(mensaje)}</div>";
        }
        protected void rptResultadosCitas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic cita = e.Item.DataItem;
                if (cita == null) return;
                LinkButton btnAccion = (LinkButton)e.Item.FindControl("btnAccionReserva");
                if (btnAccion == null) return;

                string especialidad = System.Web.HttpUtility.JavaScriptStringEncode(cita.NombreEspecialidad);
                string medico = System.Web.HttpUtility.JavaScriptStringEncode(cita.NombreMedico);
                string fecha = System.Web.HttpUtility.JavaScriptStringEncode(cita.FechaCita.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES")));
                string hora = System.Web.HttpUtility.JavaScriptStringEncode(cita.DescripcionHorario);
                string precio = cita.Precio.ToString("N2", CultureInfo.InvariantCulture);
                string hiddenFieldId = hfModalCitaId.ClientID;
                btnAccion.OnClientClick = $"return mostrarModalConfirmacionReserva({cita.IdCitaDisponible}, '{especialidad}', '{medico}', '{fecha}', '{hora}', '{precio}', '{hiddenFieldId}');";
            }
        }
        private void CerrarModalDesdeServidor()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModalScript", "cerrarModalConfirmacion();", true);
        }
        #endregion
    }
}