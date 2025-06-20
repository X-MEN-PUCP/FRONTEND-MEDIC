using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public class CitaProximaInfo
    {
        public int IdCita { get; set; }
        public string NombreEspecialidad { get; set; }
        public int IdEspecialidad { get; set; }
        public string NombreMedico { get; set; }
        public DateTime FechaCita { get; set; }
        public string DescripcionHorario { get; set; }
        public string EstadoCita { get; set; }
        public decimal Precio { get; set; }
        public bool EsCancelable { get; set; }
    }

    public partial class paciente_proximas_citas : System.Web.UI.Page
    {
        private static List<CitaProximaInfo> _listaGlobalCitasPaciente;

        static paciente_proximas_citas()
        {
            InicializarCitasDeEjemplo();
        }

        private static void InicializarCitasDeEjemplo()
        {
            _listaGlobalCitasPaciente = new List<CitaProximaInfo>
            {
                new CitaProximaInfo {
                    IdCita = 1, IdEspecialidad = 1, NombreEspecialidad = "Cardiología", NombreMedico = "Dr. Juan Pérez",
                    FechaCita = DateTime.Today.AddDays(3), DescripcionHorario = "10:00", Precio = 150.00m,
                    EstadoCita = "Confirmada", EsCancelable = true
                },
                new CitaProximaInfo {
                    IdCita = 2, IdEspecialidad = 2, NombreEspecialidad = "Dermatología", NombreMedico = "Dr. Carlos García",
                    FechaCita = DateTime.Today.AddDays(1), DescripcionHorario = "19:00", Precio = 120.00m,
                    EstadoCita = "Confirmada", EsCancelable = true
                },
                 new CitaProximaInfo {
                    IdCita = 3, IdEspecialidad = 3, NombreEspecialidad = "Pediatría", NombreMedico = "Dra. Sofía Gómez",
                    FechaCita = DateTime.Today.AddDays(7), DescripcionHorario = "08:30", Precio = 100.00m,
                    EstadoCita = "Pendiente de Pago", EsCancelable = true
                },
                 new CitaProximaInfo {
                     IdCita = 4, IdEspecialidad = 1, NombreEspecialidad = "Cardiología", NombreMedico = "Dra. Ana López",
                     FechaCita = DateTime.Today.AddDays(-2), DescripcionHorario = "14:00", Precio = 150.00m,
                     EstadoCita = "Atendida", EsCancelable = false
                 },
                 new CitaProximaInfo {
                    IdCita = 5, IdEspecialidad = 1, NombreEspecialidad = "Cardiología", NombreMedico = "Dr. Juan Pérez",
                    FechaCita = DateTime.Today.AddDays(5), DescripcionHorario = "11:00", Precio = 150.00m,
                    EstadoCita = "Pendiente de Pago", EsCancelable = true
                },
            };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFiltroEspecialidades();
                CargarProximasCitas();
            }
        }

        private void CargarFiltroEspecialidades()
        {
            var especialidades = _listaGlobalCitasPaciente
                                .Where(c => c.FechaCita >= DateTime.Today)
                                .Select(c => new { c.IdEspecialidad, c.NombreEspecialidad })
                                .Distinct()
                                .OrderBy(e => e.NombreEspecialidad)
                                .ToList();

            ddlFiltroEspecialidad.DataSource = especialidades;
            ddlFiltroEspecialidad.DataTextField = "NombreEspecialidad";
            ddlFiltroEspecialidad.DataValueField = "IdEspecialidad";
            ddlFiltroEspecialidad.DataBind();
            ddlFiltroEspecialidad.Items.Insert(0, new ListItem("-- Todas --", "0"));
        }

        private void CargarProximasCitas()
        {
            ltlMensajeAccion.Text = "";
            int filtroEspecialidadId = 0;
            int.TryParse(ddlFiltroEspecialidad.SelectedValue, out filtroEspecialidadId);

            var citasFiltradas = _listaGlobalCitasPaciente
                .Where(c => c.FechaCita >= DateTime.Today && c.EstadoCita != "Atendida" && c.EstadoCita != "Cancelada");

            if (filtroEspecialidadId > 0)
            {
                citasFiltradas = citasFiltradas.Where(c => c.IdEspecialidad == filtroEspecialidadId);
            }

            var listaFinal = citasFiltradas.OrderBy(c => c.FechaCita).ThenBy(c => c.DescripcionHorario).ToList();

            rptProximasCitas.DataSource = listaFinal;
            rptProximasCitas.DataBind();

            phNoCitas.Visible = !listaFinal.Any();
        }

        protected void ddlFiltroEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProximasCitas();
        }

        protected void rptProximasCitas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CitaProximaInfo cita = (CitaProximaInfo)e.Item.DataItem;
                LinkButton btnPagar = (LinkButton)e.Item.FindControl("btnPagarAhora");
                LinkButton btnCancelar = (LinkButton)e.Item.FindControl("btnCancelarCita");

                if (cita.EstadoCita == "Pendiente de Pago")
                {
                    btnPagar.Visible = true;
                    btnCancelar.Visible = true;
                }
                else if (cita.EstadoCita == "Confirmada")
                {
                    btnPagar.Visible = false;
                    btnCancelar.Visible = cita.EsCancelable;
                }
                else
                {
                    btnPagar.Visible = false;
                    btnCancelar.Visible = false;
                }
            }
        }

        protected void rptProximasCitas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idCita = Convert.ToInt32(e.CommandArgument);
            var citaSeleccionada = _listaGlobalCitasPaciente.FirstOrDefault(c => c.IdCita == idCita);

            if (citaSeleccionada == null)
            {
                ltlMensajeAccion.Text = "<div class='alert alert-danger'>Error: No se encontró la cita seleccionada.</div>";
                return;
            }

            if (e.CommandName == "Cancelar")
            {
                _listaGlobalCitasPaciente.Remove(citaSeleccionada);
                ltlMensajeAccion.Text = "<div class='alert alert-success'>Cita cancelada exitosamente.</div>";
                CargarProximasCitas();
            }
            else if (e.CommandName == "Pagar")
            {
                if (citaSeleccionada.EstadoCita == "Pendiente de Pago")
                {
                    string descripcion = $"{citaSeleccionada.NombreEspecialidad} con {citaSeleccionada.NombreMedico}";
                    Response.Redirect($"paciente_pago_cita.aspx?citaId={idCita}&monto={citaSeleccionada.Precio.ToString("F2", CultureInfo.InvariantCulture)}&desc={HttpUtility.UrlEncode(descripcion)}", false);
                }
            }
        }
        public string GetEstadoBadgeClass(string estado)
        {
            switch (estado)
            {
                case "Confirmada":
                    return "bg-success";
                case "Pendiente de Pago":
                    return "bg-warning text-dark";
                default:
                    return "bg-secondary";
            }
        }
        public string GetCardBorderClass(string estado)
        {
            switch (estado)
            {
                case "Confirmada":
                    return "cita-card-confirmada";
                case "Pendiente de Pago":
                    return "cita-card-pendiente";
                default:
                    return "";
            }
        }
    }
}