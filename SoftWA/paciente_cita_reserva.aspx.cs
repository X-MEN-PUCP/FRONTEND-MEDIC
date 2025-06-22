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
                ddlMedico.Enabled = false;
                phNoResultados.Visible = true;
                divHorarios.Visible = false;
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
            LimpiarFiltrosDependientes();
            ActualizarDisponibilidadCompleta();
        }
        private void ActualizarDisponibilidadCompleta()
        {
            HorariosDisponiblesPorFecha = new Dictionary<DateTime, List<TimeSpan>>();

            if (!int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) || idEspecialidad == 0)
            {
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
                            .GroupBy(c => c.fechaCita)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(c => c.turno.horaInicio)
                                      .Distinct()
                                      .OrderBy(t => t)
                                      .ToList()
                            );

                        HorariosDisponiblesPorFecha = disponibilidad;
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

                    rblHorarios.DataSource = horariosParaFecha.Select(h => h.ToString(@"hh\:mm"));
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
            if (!int.TryParse(ddlEspecialidad.SelectedValue, out int idEspecialidad) || idEspecialidad == 0)
            {
                return;
            }
            if (calFechaCita.SelectedDate.Year == 1)
            {
                return;
            }

            int.TryParse(ddlMedico.SelectedValue, out int idMedico);
            DateTime fecha = calFechaCita.SelectedDate;
            string horaSeleccionada = rblHorarios.SelectedValue;

            try
            {
                using (var servicioCita = new CitaWSClient())
                {
                    var resultados = servicioCita.buscarCitasDisponibles(idEspecialidad, idMedico, fecha);

                    if (!string.IsNullOrEmpty(horaSeleccionada))
                    {
                        resultados = resultados.Where(c => c.turno.horaInicio.ToString(@"hh\:mm") == horaSeleccionada).ToArray();
                    }

                    if (resultados.Any())
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
                        LimpiarResultadosBusqueda();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error buscando citas: " + ex.Message);
                phNoResultados.Visible = true;
            }
        }

        #region Metodos de Limpieza y UI
        private void LimpiarFiltrosDependientes(bool limpiarMedico = true)
        {
            if (limpiarMedico)
            {
                CargarMedicosPorEspecialidad();
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
            LimpiarFiltrosDependientes();
            calFechaCita.DataBind();
        }

        private void LimpiarResultadosBusqueda()
        {
            rptResultadosCitas.DataSource = null;
            rptResultadosCitas.DataBind();
            phNoResultados.Visible = true;
            ltlMensajeReserva.Text = "";
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