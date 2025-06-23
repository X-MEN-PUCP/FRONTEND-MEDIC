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
using SoftBO.examenporcitaWS;
using SoftBO.interconsultaWS;
using System.Web.UI.WebControls;
using SoftBO.diagnosticoporcitaWS;
using SoftBO.examenporcitaWS;
using SoftBO.interconsultaWS;
using System.ComponentModel;
using System.Globalization;

namespace SoftWA
{
    public partial class doctor_registrar_atencion : System.Web.UI.Page
    {
        private readonly CitaBO _citaBO;
        private readonly HistoriaBO _historiaBO;
        private readonly HistoriaClinicaPorCitaBO _historiaClinicaPorCitaBO;
        private readonly DiagnosticoBO _diagnosticoBO;
        private readonly DiagnosticoPorCitaBO _diagnosticoPorCitaBO;


        private readonly ExamenBO _examenBO;
        private readonly ExamenPorCitaBO _examenPorCitaBO;
        private readonly InterconsultaBO _interconsultaBO;
        private readonly EspecialidadBO _especialidadBO;




        private List<diagnosticoPorCita> DiagnosticosDeCita
        {
            get { return ViewState["DiagnosticosDeCita"] as List<diagnosticoPorCita> ?? new List<diagnosticoPorCita>(); }
            set { ViewState["DiagnosticosDeCita"] = value; }
        }

        private List<examenPorCita> ExamenesDeCita
        {
            get { return ViewState["ExamenesDeCita"] as List<examenPorCita> ?? new List<examenPorCita>(); }
            set { ViewState["ExamenesDeCita"] = value; }
        }

        private List<interconsultaDTO> InterconsultasDeCita
        {
            get { return ViewState["InterconsultasDeCita"] as List<interconsultaDTO> ?? new List<interconsultaDTO>(); }
            set { ViewState["InterconsultasDeCita"] = value; }
        }

        public doctor_registrar_atencion()
        {
            _citaBO = new CitaBO();
            _historiaBO = new HistoriaBO();
            _historiaClinicaPorCitaBO = new HistoriaClinicaPorCitaBO();
            _diagnosticoBO = new DiagnosticoBO();
            _diagnosticoPorCitaBO = new DiagnosticoPorCitaBO();

            _examenBO = new ExamenBO();
            _examenPorCitaBO = new ExamenPorCitaBO();
            _interconsultaBO = new InterconsultaBO();
            _especialidadBO = new EspecialidadBO();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["idCita"] != null && int.TryParse(Request.QueryString["idCita"], out int idCita))
                {
                    hfIdCita.Value = idCita.ToString();
                    if (Request.QueryString["modo"] == "vista")
                    {
                        CargarDatosDeLaCita(idCita); // Carga datos del paciente/cita
                        CargarDatosAtencionGuardada(idCita); // Carga epicrisis, diagnósticos, etc.
                        ConfigurarModoLectura();
                    }
                    else
                    {
                        // Modo normal de registro
                        CargarDatosDeLaCita(idCita);
                        CargarMaestros(); // Cargar DDLs para agregar
                    }
                }
                else
                {
                    MostrarMensaje("Error: No se ha especificado una cita para atender.", true);
                    btnFinalizarAtencion.Enabled = false;
                }
            }
        }

        private void CargarDatosAtencionGuardada(int idCita)
        {
            try
            {
                // 1. Cargar Epicrisis
                var epicrisis = _historiaClinicaPorCitaBO.ObtenerHistoriaClinicaPorIdCita(idCita);
                if (epicrisis != null)
                {
                    txtPeso.Text = epicrisis.peso.ToString() ;
                    txtTalla.Text = epicrisis.talla.ToString("0.##",CultureInfo.InvariantCulture);
                    txtPresion.Text = epicrisis.presionArterial;
                    txtTemperatura.Text = epicrisis.temperatura.ToString("0.##", CultureInfo.InvariantCulture);
                    txtMotivoConsulta.Text = epicrisis.motivoConsulta;
                    txtTratamiento.Text = epicrisis.tratamiento;
                    txtRecomendaciones.Text = epicrisis.recomendacion;
                }

                // 2. Cargar Diagnósticos
                var diagnosticosGuardados = _diagnosticoPorCitaBO.ListarDiagnosticoPorIdCita(idCita);
                rptDiagnosticosAgregados.DataSource =
                    (diagnosticosGuardados?.Select(d => new {
                        diagnostico = d.diagnostico,
                        observacion = d.observacion
                    })?.ToList<object>()) ?? new List<object>();

                rptDiagnosticosAgregados.DataBind();

                // 3. Cargar Exámenes Solicitados
                var examenesGuardados = _examenPorCitaBO.ListarExamenesPorIdCita(idCita);
                rptExamenesAgregados.DataSource =
                    (examenesGuardados?.Select(ex => new {
                        examen = new { nombreExamen = ex.examen.nombreExamen },
                        ex.observaciones
                    })?.ToList<object>()) ?? new List<object>();
                rptExamenesAgregados.DataBind();

                // 4. Cargar Interconsultas
                // Nota: El backend no tenía un "listarPorIdCita" para interconsultas. Lo simularemos aquí.
                // LO IDEAL sería agregarlo al backend para eficiencia.
                var interconsultasGuardadas = (_interconsultaBO.ListarTodosInterconuslta() ?? new BindingList<interconsultaDTO>())
                .Where(i => i?.cita?.idCita == idCita)
                .ToList();
                rptInterconsultasAgregadas.DataSource =
                    (interconsultasGuardadas?.Select(i => new {
                        especialidadInterconsulta = new { nombreEspecialidad = i.especialidadInterconsulta.nombreEspecialidad },
                        i.razonInterconsulta
                    })?.ToList<object>()) ?? new List<object>();
                rptInterconsultasAgregadas.DataBind();

            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar los detalles de la atención: {ex.Message}", true);
            }
        }

        private void ConfigurarModoLectura()
        {
            // Deshabilitar campos de Epicrisis
            txtPeso.ReadOnly = true;
            txtTalla.ReadOnly = true;
            txtPresion.ReadOnly = true;
            txtTemperatura.ReadOnly = true;
            txtMotivoConsulta.ReadOnly = true;
            txtTratamiento.ReadOnly = true;
            txtRecomendaciones.ReadOnly = true;

            // Ocultar paneles para agregar nuevos elementos
            panelAgregarDiagnostico.Visible = false;
            panelAgregarExamen.Visible = false;
            panelAgregarInterconsulta.Visible = false;

            // Ocultar botones de acción
            btnFinalizarAtencion.Visible = false;

            // Cambiar el Repeater para que no muestre el botón "Quitar"
            // Esto se hace en el HTML del ASPX con una condición.
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
                    ltlFechaCita.Text = DateTime.Parse(cita.fechaCita).ToString("dd/MM/yyyy") + " " + TimeSpan.Parse(cita.turno.horaInicio).ToString(@"hh\:mm"); ;
                    
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

            try
            {
                var todosExamenes = _examenBO.ListarTodosTablaExamen();
                ddlExamenes.DataSource = todosExamenes;
                ddlExamenes.DataTextField = "nombreExamen";
                ddlExamenes.DataValueField = "idExamen";
                ddlExamenes.DataBind();
                ddlExamenes.Items.Insert(0, new ListItem("-- Seleccione un examen --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar la lista de exámenes: {ex.Message}", true);
            }

            try
            {
                var todasEspecialidades = _especialidadBO.ListarEspecialidad();
                ddlEspecialidadInterconsulta.DataSource = todasEspecialidades;
                ddlEspecialidadInterconsulta.DataTextField = "nombreEspecialidad";
                ddlEspecialidadInterconsulta.DataValueField = "idEspecialidad";
                ddlEspecialidadInterconsulta.DataBind();
                ddlEspecialidadInterconsulta.Items.Insert(0, new ListItem("-- Seleccione especialidad --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar la lista de especialidades para interconsulta: {ex.Message}", true);
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

        protected void btnAgregarExamen_Click(object sender, EventArgs e)
        {
            if (int.TryParse(ddlExamenes.SelectedValue, out int idExamen) && idExamen > 0)
            {
                var listaActual = ExamenesDeCita;
                if (listaActual.Any(ex => ex.examen.idExamen == idExamen))
                {
                    MostrarMensaje("Este examen ya ha sido solicitado.", false, true);
                    return;
                }

                var examenSeleccionado = _examenBO.ObtenerPorIdTablaExamen(idExamen);

                listaActual.Add(new examenPorCita
                {
                    examen = new SoftBO.examenporcitaWS.examenDTO
                    {
                        idExamen = examenSeleccionado.idExamen,
                        nombreExamen = examenSeleccionado.nombreExamen
                    },
                    observaciones = txtObservacionExamen.Text.Trim()
                });

                ExamenesDeCita = listaActual;
                RefrescarListaExamenes();
                txtObservacionExamen.Text = string.Empty;
                ddlExamenes.SelectedIndex = 0;
            }
        }

        protected void rptExamenesAgregados_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int idExamenQuitar = int.Parse(e.CommandArgument.ToString());
                var listaActual = ExamenesDeCita;
                listaActual.RemoveAll(ex => ex.examen.idExamen == idExamenQuitar);
                ExamenesDeCita = listaActual;
                RefrescarListaExamenes();
            }
        }

        private void RefrescarListaExamenes()
        {
            rptExamenesAgregados.DataSource = ExamenesDeCita;
            rptExamenesAgregados.DataBind();
        }


        protected void btnAgregarInterconsulta_Click(object sender, EventArgs e)
        {
            if (int.TryParse(ddlEspecialidadInterconsulta.SelectedValue, out int idEspecialidad) && idEspecialidad > 0)
            {
                var listaActual = InterconsultasDeCita;
                if (listaActual.Any(i => i.especialidadInterconsulta.idEspecialidad == idEspecialidad))
                {
                    MostrarMensaje("Ya se ha solicitado una interconsulta para esta especialidad.", false, true);
                    return;
                }

                var especialidadSeleccionada = _especialidadBO.ObtenerPorIdTablaEspecialidad(idEspecialidad);

                listaActual.Add(new interconsultaDTO
                {
                    especialidadInterconsulta = new SoftBO.interconsultaWS.especialidadDTO
                    {
                        idEspecialidad = especialidadSeleccionada.idEspecialidad,
                        nombreEspecialidad = especialidadSeleccionada.nombreEspecialidad
                    },
                    razonInterconsulta = txtRazonInterconsulta.Text.Trim()
                });

                InterconsultasDeCita = listaActual;
                RefrescarListaInterconsultas();
                txtRazonInterconsulta.Text = string.Empty;
                ddlEspecialidadInterconsulta.SelectedIndex = 0;
            }
        }

        protected void rptInterconsultasAgregadas_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int idEspecialidadQuitar = int.Parse(e.CommandArgument.ToString());
                var listaActual = InterconsultasDeCita;
                listaActual.RemoveAll(i => i.especialidadInterconsulta.idEspecialidad == idEspecialidadQuitar);
                InterconsultasDeCita = listaActual;
                RefrescarListaInterconsultas();
            }
        }

        private void RefrescarListaInterconsultas()
        {
            rptInterconsultasAgregadas.DataSource = InterconsultasDeCita;
            rptInterconsultasAgregadas.DataBind();
        }



        protected void btnFinalizarAtencion_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
                if (usuario == null) throw new Exception("La sesión ha expirado.");
                string fechaHoy = DateTime.Today.ToString("yyyy-MM-dd");

                // guardar Epicrisis
                if (Request.QueryString["idCita"] != null && int.TryParse(Request.QueryString["idCita"], out int idCita))
                {
                    hfIdCita.Value = idCita.ToString();
                    var epicrisis =_historiaClinicaPorCitaBO.ObtenerHistoriaClinicaPorIdCita(idCita);

                    //if (double.TryParse(txtPeso.Text, out double peso)) { epicrisis.peso = peso; epicrisis.pesoSpecified = true; }
                    //if (double.TryParse(txtTalla.Text, out double talla)) { epicrisis.talla = talla; epicrisis.tallaSpecified = true; }
                    //if (double.TryParse(txtTemperatura.Text, out double temp)) { epicrisis.temperatura = temp; epicrisis.temperaturaSpecified = true; }
                    //epicrisis.presionArterial = txtPresion.Text;
                    //epicrisis.motivoConsulta = txtMotivoConsulta.Text;
                    //epicrisis.tratamiento = txtTratamiento.Text;
                    //epicrisis.recomendacion = txtRecomendaciones.Text;
                    //epicrisis.estadoGeneral = SoftBO.historiaclinicaporcitaWS.estadoGeneral.ACTIVO;
                    //epicrisis.estadoGeneralSpecified = true;
                    epicrisis.usuarioModificacion= usuario.idUsuario;
                    epicrisis.usuarioModificacionSpecified = true;
                    epicrisis.fechaModificacion = fechaHoy;
                    //epicrisis.evolucion = " ";
                    //epicrisis.frecuenciaCardiaca = 0;
                    //epicrisis.receta = " ";
                    _historiaClinicaPorCitaBO.ModificarHistoriaClinicaPorCita(epicrisis);
                }



                

                // guardar Diagnósticos
                foreach (var diag in DiagnosticosDeCita)
                {
                    diag.cita = new SoftBO.diagnosticoporcitaWS.citaDTO { idCita = int.Parse(hfIdCita.Value) };
                    _diagnosticoPorCitaBO.InsertarDiagnosticoPorCita(diag);
                }

                //guardar Exámenes 
                foreach (var exam in ExamenesDeCita)
                {
                    exam.cita = new SoftBO.examenporcitaWS.citaDTO { idCita = int.Parse(hfIdCita.Value) };
                    exam.usuarioCreacion = usuario.idUsuario;
                    exam.usuarioCreacionSpecified = true;
                    _examenPorCitaBO.InsertarExamenPorCita(exam);
                }

                //Guardar Interconsultas
                foreach (var inter in InterconsultasDeCita)
                {
                    inter.cita = new SoftBO.interconsultaWS.citaDTO { idCita = int.Parse(hfIdCita.Value) };
                    _interconsultaBO.InsertarInterconuslta(inter);
                }

                // cambiar estado de cita
                var citaParaActualizar = _citaBO.ObtenerPorIdCitaCita(int.Parse(hfIdCita.Value));
                citaParaActualizar.estado = SoftBO.citaWS.estadoCita.PAGADO; //cambiar si estado atendidooooooooo
                citaParaActualizar.fechaModificacion = fechaHoy;
                citaParaActualizar.usuarioModificacion = usuario.idUsuario;
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