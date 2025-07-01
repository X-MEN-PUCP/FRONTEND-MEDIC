using SoftBO;
using SoftBO.pacienteWS;
using SoftBO.historiaclinicaporcitaWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class paciente_proximas_citas : System.Web.UI.Page
    {
        private List<SoftBO.pacienteWS.historiaClinicaPorCitaDTO> CitasCompletasPaciente
        {
            get { return ViewState["CitasCompletasPaciente"] as List<SoftBO.pacienteWS.historiaClinicaPorCitaDTO> ?? new List<SoftBO.pacienteWS.historiaClinicaPorCitaDTO>(); }
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
            if (usuario == null || usuario.idUsuario == 0)
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
                if(historialCompleto != null)
                {
                    CitasCompletasPaciente = historialCompleto.ToList();
                }
                CargarFiltroEspecialidades();
                FiltrarYMostrarCitas();
            }
            catch (Exception ex)
            {
                ltlMensajeAccion.Text = "<div class='alert alert-danger'>Error al cargar sus citas.</div>";
                System.Diagnostics.Debug.WriteLine($"Error cargando citas del paciente: {ex}");
            }
        }
        private void CargarFiltroEspecialidades()
        {
            var especialidades = CitasCompletasPaciente
                .Where(h => h.cita?.especialidad != null)
                .Select(h => h.cita.especialidad)
                .GroupBy(e => e.idEspecialidad)
                .Select(g => g.First())
                .OrderBy(e => e.nombreEspecialidad)
                .ToList();

            ddlFiltroEspecialidad.DataSource = especialidades;
            ddlFiltroEspecialidad.DataTextField = "nombreEspecialidad";
            ddlFiltroEspecialidad.DataValueField = "idEspecialidad";
            ddlFiltroEspecialidad.DataBind();
            ddlFiltroEspecialidad.Items.Insert(0, new ListItem("-- Todas --", "0"));
        }
        private void FiltrarYMostrarCitas()
        {
            ltlMensajeAccion.Text = "";
            int.TryParse(ddlFiltroEspecialidad.SelectedValue, out int filtroEspecialidadId);
            IEnumerable<SoftBO.pacienteWS.historiaClinicaPorCitaDTO> citasFiltradas = CitasCompletasPaciente
                    .Where(h => h.cita != null && (h.cita.estado == SoftBO.pacienteWS.estadoCita.RESERVADO || h.cita.estado == SoftBO.pacienteWS.estadoCita.PAGADO) &&
                    DateTime.TryParse(h.cita.fechaCita, out var fecha) && fecha.Date >= DateTime.Today);
            if (filtroEspecialidadId > 0)
            {
                citasFiltradas = citasFiltradas.Where(h => h.cita.especialidad != null && h.cita.especialidad.idEspecialidad == filtroEspecialidadId);
            }

            var listaParaMostrar = citasFiltradas
                .OrderBy(h => DateTime.Parse(h.cita.fechaCita))
                .ThenBy(h => TimeSpan.Parse(h.cita.horaInicio))
                .Select(h => new {
                    IdCita=h.cita.idCita,
                    NombreEspecialidad = h.cita.especialidad?.nombreEspecialidad ?? "N/A",
                    NombreMedico = h.cita.medico != null ? $"{h.cita.medico.nombres} {h.cita.medico.apellidoPaterno}" : "N/A",
                    FechaCita = DateTime.Parse(h.cita.fechaCita),
                    DescripcionHorario = h.cita.horaInicio.Substring(0, 5),
                    EstadoCita = FormatearNombreEstado(h.cita.estado),
                    Precio = (h.cita.especialidad?.precioConsulta ?? 0).ToString("F2",CultureInfo.InvariantCulture),
                    EsCancelable = DateTime.Parse(h.cita.fechaCita).Date > DateTime.Today.AddDays(1)
                }).ToList();

            rptProximasCitas.DataSource = listaParaMostrar;
            rptProximasCitas.DataBind();
            phNoCitas.Visible = !listaParaMostrar.Any();
        }
        protected void ddlFiltroEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarYMostrarCitas();
        }
        protected void rptProximasCitas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic cita = e.Item.DataItem;
                if (cita == null) return;

                var btnPagar = (LinkButton)e.Item.FindControl("btnPagarAhora");
                var btnCancelar = (LinkButton)e.Item.FindControl("btnCancelarCita");
                string estadoActual = cita.EstadoCita;
                btnPagar.Visible = (estadoActual == "Reservado");
                btnCancelar.Visible = (estadoActual == "Reservado" || estadoActual == "Pagado") && cita.EsCancelable;
            }
        }
        protected void rptProximasCitas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument.ToString(), out int idCita)) return;
            var usuario = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (usuario == null || usuario.idUsuario == 0)
            {
                MostrarMensaje("Error: Usuario no encontrado.", esExito: false);
                Response.Redirect("~/indexLogin.aspx");
                return;
            }
            if (e.CommandName == "Cancelar")
            {
                try
                {
                    var historiaOriginal = CitasCompletasPaciente.FirstOrDefault(h => h.cita?.idCita == idCita);
                    if (historiaOriginal == null || historiaOriginal.historiaClinica == null)
                    {
                        MostrarMensaje("No se encontró la cita a cancelar.", esExito: false);
                        return;
                    }
                    var historiaParaCancelar = new SoftBO.pacienteWS.historiaClinicaPorCitaDTO
                    {
                        cita = new SoftBO.pacienteWS.citaDTO
                        {
                            idCita = idCita,
                            idCitaSpecified = true
                        },
                        historiaClinica = new SoftBO.pacienteWS.historiaClinicaDTO
                        {
                            idHistoriaClinica = historiaOriginal.historiaClinica.idHistoriaClinica,
                            idHistoriaClinicaSpecified = true,
                            paciente = new SoftBO.pacienteWS.usuarioDTO
                            {
                                idUsuario = usuario.idUsuario,
                                idUsuarioSpecified = true
                            }
                        }
                    };
                    var servicioPaciente = new PacienteBO();
                    int resultado = servicioPaciente.CancelarCitaPaciente(historiaParaCancelar);

                    if (resultado > 0)
                    {
                        MostrarMensaje("Cita cancelada exitosamente.", esExito: true);
                        CargarDatosIniciales();
                    }
                    else
                    {
                        MostrarMensaje("No se pudo cancelar la cita.", esExito: false);
                    }
                }
                catch (Exception ex)
                {
                    ltlMensajeAccion.Text = "<div class='alert alert-danger'>Ocurrió un error al cancelar la cita.</div>";
                    System.Diagnostics.Debug.WriteLine($"Error cancelando cita: {ex}");
                }
            }
            else if (e.CommandName == "Pagar")
            {
                Session["CitaIdParaPago"] = idCita;
                Response.Redirect("paciente_pago_cita.aspx", false);
            }
        }
        #region Métodos de Ayuda para la UI

        public string GetEstadoBadgeClass(string estado)
        {
            switch (estado)
            {
                case "Pagado": return "bg-success";
                case "Reservado": return "bg-warning text-dark";
                default: return "bg-secondary";
            }
        }
        public string GetCardBorderClass(string estado)
        {
            switch (estado)
            {
                case "Pagado": return "cita-card-confirmada";
                case "Reservado": return "cita-card-pendiente";
                default: return "";
            }
        }
        private string FormatearNombreEstado(SoftBO.pacienteWS.estadoCita estadoEnum)
        {
            switch (estadoEnum)
            {
                case SoftBO.pacienteWS.estadoCita.RESERVADO: return "Reservado";
                case SoftBO.pacienteWS.estadoCita.PAGADO: return "Pagado";
                case SoftBO.pacienteWS.estadoCita.DISPONIBLE: return "Disponible";
                case SoftBO.pacienteWS.estadoCita.ATENDIDO: return "Atendido";
                default: return "Desconocido";
            }
        }
        private void MostrarMensaje(string mensaje, bool esExito)
        {
            string cssClass = esExito ? "alert alert-success" : "alert alert-danger";
            ltlMensajeAccion.Text = $"<div class='{cssClass} mt-3'>{Server.HtmlEncode(mensaje)}</div>";
        }

        #endregion
    }
}