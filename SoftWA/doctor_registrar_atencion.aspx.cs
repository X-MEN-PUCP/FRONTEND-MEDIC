// --- Archivo: doctor_registrar_atencion.aspx.cs ---
using SoftBO;
using SoftBO.citaWS;
using SoftBO.diagnosticoWS;
using SoftBO.diagnosticoporcitaWS;
using SoftBO.historiaWS;
using SoftBO.historiaclinicaporcitaWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace SoftWA
{
    public partial class doctor_registrar_atencion : System.Web.UI.Page
    {
        private readonly CitaBO _citaBO;
        private readonly HistoriaBO _historiaBO;
        private readonly HistoriaClinicaPorCitaBO _historiaClinicaPorCitaBO;
        private readonly DiagnosticoBO _diagnosticoBO;
        private readonly DiagnosticoPorCitaBO _diagnosticoPorCitaBO;


        // Propiedad para manejar la lista de diagnósticos en ViewState
        private List<diagnosticoPorCita> DiagnosticosDeCita
        {
            get { return ViewState["DiagnosticosDeCita"] as List<diagnosticoPorCita> ?? new List<diagnosticoPorCita>(); }
            set { ViewState["DiagnosticosDeCita"] = value; }
        }

        public doctor_registrar_atencion()
        {
            _citaBO = new CitaBO();
            _historiaBO = new HistoriaBO();
            _historiaClinicaPorCitaBO = new HistoriaClinicaPorCitaBO();
            _diagnosticoBO = new DiagnosticoBO();
            _diagnosticoPorCitaBO = new DiagnosticoPorCitaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["idCita"] != null && int.TryParse(Request.QueryString["idCita"], out int idCita))
                {
                    hfIdCita.Value = idCita.ToString();
                    CargarDatosDeLaCita(idCita);
                    CargarMaestros(); // Cargar DDLs
                }
                else
                {
                    MostrarMensaje("Error: No se ha especificado una cita para atender.", true);
                    btnFinalizarAtencion.Enabled = false;
                }
            }
        }

        private void CargarDatosDeLaCita(int idCita)
        {
            try
            {
                var cita = _citaBO.ObtenerPorIdCitaCita(idCita);
                if (cita != null)
                {
                    var historiaClinicaPorCita = _historiaClinicaPorCitaBO.ObtenerHistoriaClinicaPorIdCita(cita.idCita);
                    var historiaClinica = historiaClinicaPorCita.historiaClinica;
                    var paciente = historiaClinica.paciente;
                    hfIdPaciente.Value = paciente.idUsuario.ToString();
                    ltlNombrePaciente.Text = $"{paciente.nombres} {paciente.apellidoPaterno}";
                    ltlEspecialidad.Text = cita.especialidad.nombreEspecialidad;
                    ltlFechaCita.Text = DateTime.Parse(cita.fechaCita).ToString("dd/MM/yyyy") + " " + cita.turno.horaInicio.ToString(@"hh\:mm");
                    
                    //var historiaClinica = _historiaBO.ObtenerHistoriaPorIdPaciente(cita.paciente.idUsuario);
                    if (historiaClinica != null)
                    {
                        hfIdHistoria.Value = historiaClinica.idHistoriaClinica.ToString();
                    }
                }
                else
                {
                    throw new Exception("Cita no encontrada.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar los datos de la cita: {ex.Message}", true);
                btnFinalizarAtencion.Enabled = false;
            }
        }
        
        private void CargarMaestros()
        {
            try
            {
                var todosDiagnosticos = _diagnosticoBO.ListarTodosDiagnostico();
                ddlDiagnosticos.DataSource = todosDiagnosticos;
                ddlDiagnosticos.DataTextField = "nombreDiagnostico";
                ddlDiagnosticos.DataValueField = "idDiagnostico";
                ddlDiagnosticos.DataBind();
            }
            catch (Exception ex)
            {
                 MostrarMensaje($"Error al cargar la lista de diagnósticos: {ex.Message}", true);
            }
        }
        
        protected void btnAgregarDiagnostico_Click(object sender, EventArgs e)
        {
            int idDiagnostico;
            if (int.TryParse(ddlDiagnosticos.SelectedValue, out idDiagnostico) && idDiagnostico > 0)
            {
                var listaActual = DiagnosticosDeCita;
                if (listaActual.Any(d => d.diagnostico.idDiagnostico == idDiagnostico))
                {
                    MostrarMensaje("Este diagnóstico ya ha sido agregado.", false, true);
                    return;
                }

                var diagnosticoSeleccionado = _diagnosticoBO.ObtenerPorIdDiagnostico(idDiagnostico);
                
                listaActual.Add(new diagnosticoPorCita
                {
                    diagnostico = new SoftBO.diagnosticoporcitaWS.diagnosticoDTO { 
                        idDiagnostico = diagnosticoSeleccionado.idDiagnostico,
                        nombreDiagnostico = diagnosticoSeleccionado.nombreDiagnostico
                    },
                    observacion = txtObservacionDiagnostico.Text.Trim()
                });
                
                DiagnosticosDeCita = listaActual;
                RefrescarListaDiagnosticos();
                txtObservacionDiagnostico.Text = string.Empty;
            }
        }
        
        protected void rptDiagnosticosAgregados_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int idDiagnosticoQuitar = int.Parse(e.CommandArgument.ToString());
                var listaActual = DiagnosticosDeCita;
                listaActual.RemoveAll(d => d.diagnostico.idDiagnostico == idDiagnosticoQuitar);
                DiagnosticosDeCita = listaActual;
                RefrescarListaDiagnosticos();
            }
        }

        private void RefrescarListaDiagnosticos()
        {
            rptDiagnosticosAgregados.DataSource = DiagnosticosDeCita;
            rptDiagnosticosAgregados.DataBind();
        }

        protected void btnFinalizarAtencion_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Guardar Epicrisis (Historia Clinica Por Cita)
                var epicrisis = new historiaClinicaPorCitaDTO
                {
                    historiaClinica = new SoftBO.historiaclinicaporcitaWS.historiaClinicaDTO { idHistoriaClinica = int.Parse(hfIdHistoria.Value) },
                    cita = new SoftBO.historiaclinicaporcitaWS.citaDTO { idCita = int.Parse(hfIdCita.Value) }
                };
                
                // Asignar valores, controlando nulos
                if (double.TryParse(txtPeso.Text, out double peso)) { epicrisis.peso = peso; epicrisis.pesoSpecified = true; }
                if (double.TryParse(txtTalla.Text, out double talla)) { epicrisis.talla = talla; epicrisis.tallaSpecified = true; }
                if (double.TryParse(txtTemperatura.Text, out double temp)) { epicrisis.temperatura = temp; epicrisis.temperaturaSpecified = true; }
                epicrisis.presionArterial = txtPresion.Text;
                epicrisis.motivoConsulta = txtMotivoConsulta.Text;
                epicrisis.tratamiento = txtTratamiento.Text;
                epicrisis.recomendacion = txtRecomendaciones.Text;

                // En el backend, el DAO debe hacer un INSERT o un UPDATE.
                // Por simplicidad, aquí asumimos que siempre modifica.
                _historiaClinicaPorCitaBO.ModificarHistoriaClinicaPorCita(epicrisis);

                // 2. Guardar Diagnósticos
                foreach (var diag in DiagnosticosDeCita)
                {
                    diag.cita = new SoftBO.diagnosticoporcitaWS.citaDTO { idCita = int.Parse(hfIdCita.Value) };
                    _diagnosticoPorCitaBO.InsertarDiagnosticoPorCita(diag);
                }

                // 3. Marcar la cita como "Atendida"
                var citaParaActualizar = _citaBO.ObtenerPorIdCitaCita(int.Parse(hfIdCita.Value));
                //citaParaActualizar.estado = estadoCita.ATENDIDA; // Suponiendo que el enum se genera así
                _citaBO.ModificarCita(citaParaActualizar);

                MostrarMensaje("Atención guardada exitosamente. Puede cerrar esta pestaña.", false);
                btnFinalizarAtencion.Enabled = false;
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar la atención: {ex.Message}", true);
            }
        }
        
        private void MostrarMensaje(string mensaje, bool esError, bool esWarning = false)
        {
            phMensaje.Visible = true;
            ltlMensaje.Text = Server.HtmlEncode(mensaje);
            string cssClass = esError ? "alert alert-danger" : (esWarning ? "alert alert-warning" : "alert alert-success");
            divAlert.Attributes["class"] = cssClass + " alert-dismissible fade show";
        }
    }
}