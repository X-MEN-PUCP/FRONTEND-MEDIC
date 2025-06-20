using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SoftBO;
using SoftBO.citaWS;

namespace SoftWA
{
    public partial class paciente_pago_cita : System.Web.UI.Page
    {
        private readonly CitaBO _citaBO;
        public paciente_pago_cita()
        {
            _citaBO = new CitaBO();
        }
        protected int CitaIdParaPagar
        {
            get { return ViewState["CitaId"] != null ? (int)ViewState["CitaId"] : 0; }
            set { ViewState["CitaId"] = value; }
        }
        protected decimal MontoDeCita
        {
            get { return ViewState["MontoCita"] != null ? (decimal)ViewState["MontoCita"] : 0; }
            set { ViewState["MontoCita"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["citaId"] != null && Request.QueryString["monto"] != null)
                {
                    if (int.TryParse(Request.QueryString["citaId"], out int citaId) &&
                        decimal.TryParse(Request.QueryString["monto"], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal monto))
                    {
                        CitaIdParaPagar = citaId;
                        MontoDeCita = monto;
                        CargarDetallesDeCita(citaId);
                    }
                    else
                    {
                        MostrarErrorFatal("Los datos de la cita para el pago son inválidos.");
                    }
                }
                else
                {
                    MostrarErrorFatal("No se especificaron los detalles de la cita para el pago.");
                }
            }
        }
        private void CargarDetallesDeCita(int citaId)
        {
            //try
            //{
            //    var cita = _citaBO.ObtenerPorIdCitaCita(citaId);
            //    if (cita != null)
            //    {
            //        DateTime fechaCita = DateTime.Parse(cita.fechaCita);
            //        TimeSpan horaInicio = cita.turno.horaInicio; // Asumimos que horaInicio es TimeSpan

            //        string descripcion = $"Cita de {cita.especialidad.nombreEspecialidad} con el Dr(a). {cita.medico.nombres} {cita.medico.apellidoPaterno} para el día {fechaCita:dd/MM/yyyy} a las {horaInicio:hh\\:mm}.";

            //        ltlDescripcionCita.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(descripcion, false);
            //        ltlMontoPagar.Text = MontoDeCita.ToString("N2", new CultureInfo("es-PE"));
            //        phDetallesPago.Visible = true;
            //    }
            //    else
            //    {
            //        MostrarErrorFatal("La cita que intenta pagar no fue encontrada o ya no está disponible.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MostrarErrorFatal("Ocurrió un error al cargar los detalles de la cita.");
            //    System.Diagnostics.Debug.WriteLine($"Error cargando cita para pago: {ex.ToString()}");
            //}
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            //if (!Page.IsValid) return;

            //bool pagoExitosoSimulado = (txtCVV.Text != "000");

            //if (pagoExitosoSimulado)
            //{
            //    try
            //    {
            //        var citaAPagar = _citaBO.ObtenerPorIdCitaCita(CitaIdParaPagar);
            //        if (citaAPagar != null)
            //        {
            //            // Asumimos que el estado "PAGADO" tiene ID = 2. Ajustar si es diferente.
            //            citaAPagar.estado = new estadoCita { idEstadoCita = 2, idEstadoCitaSpecified = true };

            //            // Usar la capa de negocio para modificar la cita
            //            int resultado = _citaBO.ModificarCita(citaAPagar);

            //            if (resultado > 0)
            //            {
            //                MostrarMensajeExito(citaAPagar);
            //            }
            //            else
            //            {
            //                MostrarMensajeFallo("Su pago fue procesado, pero hubo un problema al confirmar su cita. Por favor, contacte a soporte con el ID de cita: " + CitaIdParaPagar);
            //            }
            //        }
            //        else
            //        {
            //            MostrarMensajeFallo("Su pago no pudo ser procesado porque la cita ya no se encuentra disponible.");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MostrarMensajeFallo("Ocurrió un error de comunicación al intentar confirmar su cita. Por favor, contacte a soporte.");
            //        System.Diagnostics.Debug.WriteLine($"Error al modificar cita tras pago: {ex.ToString()}");
            //    }
            //}
            //else
            //{
            //    MostrarMensajeFallo("Hubo un problema al procesar su pago. Por favor, verifique los datos de su tarjeta o intente con otra.");
            //    phDetallesPago.Visible = true;
            //}
        }

        private void MostrarMensajeExito(citaDTO citaPagada)
        {
            phDetallesPago.Visible = false;
            phMensajeResultado.Visible = true;
            pnlResumenCita.Visible = true;
            divMensajeResultado.Attributes["class"] = "alert alert-success text-center";
            hdrMensajeResultado.InnerText = "¡Pago Exitoso!";
            pMensajeResultado.InnerText = "Su pago ha sido procesado correctamente. Su cita está confirmada.";
            hrMensajeResultado.Visible = true;
            btnVolverInicio.Visible = true;
            btnVerMisCitas.Visible = true;

            // <<< CORRECCIÓN >>>: Parsear el string de la fecha antes de formatearlo.
            DateTime fechaCita = DateTime.Parse(citaPagada.fechaCita);

            string numeroTarjeta = txtNumeroTarjeta.Text.Trim().Replace(" ", "");
            ltlResumenCodOperacion.Text = citaPagada.idCita.ToString("D6");
            ltlResumenTarjeta.Text = $"XXXX - XXXX - XXXX - {numeroTarjeta.Substring(numeroTarjeta.Length - 4)}";
            ltlResumenEspecialidad.Text = citaPagada.especialidad.nombreEspecialidad;
            ltlResumenMedico.Text = $"{citaPagada.medico.nombres} {citaPagada.medico.apellidoPaterno}";
            ltlResumenFecha.Text = fechaCita.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            ltlResumenHorario.Text = citaPagada.turno.horaInicio.ToString(@"hh\:mm");
            ltlResumenMonto.Text = MontoDeCita.ToString("N2", new CultureInfo("es-PE"));
            ltlResumenMensaje.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(txtCorreo.Text, false);
        }

        #region Métodos de Mensajes y Navegación
        private void MostrarMensajeFallo(string mensaje)
        {
            phDetallesPago.Visible = false;
            phMensajeResultado.Visible = true;
            pnlResumenCita.Visible = false;
            divMensajeResultado.Attributes["class"] = "alert alert-danger text-center";
            hdrMensajeResultado.InnerText = "Pago Fallido";
            pMensajeResultado.InnerText = mensaje;
            hrMensajeResultado.Visible = false;
            btnVolverInicio.Visible = true;
            btnVerMisCitas.Visible = false;
        }

        private void MostrarErrorFatal(string mensaje)
        {
            phDetallesPago.Visible = false;
            phMensajeResultado.Visible = true;
            pnlResumenCita.Visible = false;
            divMensajeResultado.Attributes["class"] = "alert alert-danger text-center";
            hdrMensajeResultado.InnerText = "Error en el Proceso de Pago";
            pMensajeResultado.InnerText = mensaje;
            hrMensajeResultado.Visible = true;
            btnVolverInicio.Visible = true;
            btnVerMisCitas.Visible = false;
        }

        protected void btnCancelarPago_Click(object sender, EventArgs e)
        {
            Response.Redirect("paciente_cita_reserva.aspx");
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("indexPaciente.aspx");
        }
        protected void btnVerMisCitas_Click(object sender, EventArgs e)
        {
            Response.Redirect("paciente_proximas_citas.aspx");
        }
        #endregion
    }
}