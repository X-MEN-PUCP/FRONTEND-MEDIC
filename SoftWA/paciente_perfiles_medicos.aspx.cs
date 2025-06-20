using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SoftBO.usuarioporespecialidadWS;
using SoftBO.especialidadWS;

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
            var listaPerfiles = new List<usuarioPorEspecialidadDTO>();
            try
            {
                using (var servicioEspecialidad = new EspecialidadWSClient())
                {
                    var todasEspecialidades = servicioEspecialidad.listarEspecialidad();
                    using (var servicioUsuarioEsp = new UsuarioPorEspecialidadWSClient())
                    {
                        foreach (var esp in todasEspecialidades)
                        {
                            var medicosDeEspecialidad = servicioUsuarioEsp.listarPorEspecialidadUsuarioPorEspecialidad(esp.idEspecialidad);
                            if(medicosDeEspecialidad != null)
                            {
                                listaPerfiles.AddRange(medicosDeEspecialidad);
                            }
                        }
                    }
                }
                ListaCompletaMedicos = listaPerfiles;
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
            if (medicosFiltrados == null || !medicosFiltrados.Any())
            {
                phNoMedicos.Visible = true;
                rptPerfilesMedicos.Visible = false;
                return;
            }
            string filtro = txtFiltroNombre.Text.Trim().ToUpper();
            if (!string.IsNullOrEmpty(filtro))
            {
                medicosFiltrados = medicosFiltrados.Where(m =>
                    (m.usuario.nombres.ToUpper() + " " + m.usuario.apellidoPaterno.ToUpper()).Contains(filtro) ||
                    m.especialidad.nombreEspecialidad.ToUpper().Contains(filtro)
                ).ToList();
            }
            string orden = ddlOrdenarPor.SelectedValue;
            if (orden == "Nombre")
            {
                medicosFiltrados = medicosFiltrados.OrderBy(m => m.usuario.nombres).ThenBy(m => m.usuario.apellidoPaterno).ToList();
            }
            else
            {
                medicosFiltrados = medicosFiltrados.OrderBy(m => m.especialidad.nombreEspecialidad).ThenBy(m => m.usuario.nombres).ToList();
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