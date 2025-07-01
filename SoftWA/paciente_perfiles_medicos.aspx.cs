using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SoftBO;
using SoftBO.pacienteWS;

namespace SoftWA
{
    public partial class paciente_perfiles_medicos : System.Web.UI.Page
    {
        private List<usuarioPorEspecialidadDTO> ListaCompletaMedicos
        {
            get { return ViewState["ListaCompletaMedicos"] as List<usuarioPorEspecialidadDTO>; }
            set { ViewState["ListaCompletaMedicos"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarListaCompletaDeMedicos();
                AplicarFiltrosYOrdenar();
            }
        }
        private void CargarListaCompletaDeMedicos()
        {
            try
            { 
                var servicioUsuarioEsp = new PacienteBO();
                var perfiles = servicioUsuarioEsp.ListarTodosPerfilesMedicosParaPaciente();
                if (perfiles != null)
                {
                    ListaCompletaMedicos = perfiles.ToList();
                }
                else
                {
                    ListaCompletaMedicos = new List<usuarioPorEspecialidadDTO>();
                }
                System.Diagnostics.Debug.WriteLine($"Número de médicos cargados: {ListaCompletaMedicos?.Count ?? 0}");
            }

            catch (Exception ex)
            {
                phNoMedicos.Visible = true;
                rptPerfilesMedicos.Visible = false;
                System.Diagnostics.Debug.WriteLine($"Error al cargar médicos: {ex.ToString()}");
                ListaCompletaMedicos = new List<usuarioPorEspecialidadDTO>();
            }
        }
        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYOrdenar();
        }
        private void AplicarFiltrosYOrdenar()
        {
            var medicosFiltrados = ListaCompletaMedicos;
            if (medicosFiltrados != null)
            {
                medicosFiltrados = medicosFiltrados
                    .Where(m => m.usuario != null && m.especialidad != null)
                    .ToList();
            }
            if(medicosFiltrados == null || !medicosFiltrados.Any())
            {
                phNoMedicos.Visible = true;
                rptPerfilesMedicos.DataSource = null;
                rptPerfilesMedicos.DataBind();
                rptPerfilesMedicos.Visible = false;
                return;
            }
            string filtro = txtFiltroNombre.Text.Trim().ToUpper();
            if (!string.IsNullOrEmpty(filtro))
            {
                medicosFiltrados = medicosFiltrados.Where(m =>
                    (!string.IsNullOrEmpty(m.usuario.nombres) && !string.IsNullOrEmpty(m.usuario.apellidoPaterno) &&
                     (m.usuario.nombres.ToUpper() + " " + m.usuario.apellidoPaterno.ToUpper()).Contains(filtro)) ||
                    (!string.IsNullOrEmpty(m.especialidad.nombreEspecialidad) &&
                     m.especialidad.nombreEspecialidad.ToUpper().Contains(filtro))
                ).ToList();
            }
            string orden = ddlOrdenarPor.SelectedValue;
            if (orden == "Nombre")
            {
                medicosFiltrados = medicosFiltrados
                    .OrderBy(m => m.usuario.nombres)
                    .ThenBy(m => m.usuario.apellidoPaterno)
                    .ToList();
            }
            else
            {
                medicosFiltrados = medicosFiltrados
                    .OrderBy(m => m.especialidad.nombreEspecialidad)
                    .ThenBy(m => m.usuario.nombres)
                    .ToList();
            }

            rptPerfilesMedicos.DataSource = medicosFiltrados;
            rptPerfilesMedicos.DataBind();
            phNoMedicos.Visible = !medicosFiltrados.Any();
            rptPerfilesMedicos.Visible = medicosFiltrados.Any();
        }
        protected void rptPerfilesMedicos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Reservar")
            {
                Console.WriteLine("Redirigiendo a la página de reserva de citas...");
                string commandArgument = e.CommandArgument.ToString();
                string[] arguments = commandArgument.Split(';');

                if (arguments.Length == 2)
                {
                    string idMedico = arguments[0];
                    string idEspecialidad = arguments[1];

                    Session["PreloadMedicoId"] = idMedico;
                    Session["PreloadEspecialidadId"] = idEspecialidad;

                    Response.Redirect("paciente_cita_reserva.aspx");
                }
            }
        }
    }
}