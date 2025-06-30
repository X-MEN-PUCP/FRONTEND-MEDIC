using SoftBO;
using SoftBO.especialidadWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class admin_gestionar_especialidades : System.Web.UI.Page
    {
        [Serializable]
        public class EspecialidadConteo
        {
            public int ID { get; set; }
            public string NombreEspecialidad { get; set; }
            public double PrecioConsulta { get; set; }
            public int CantMedicos { get; set; }
            public estadoGeneral Estado { get; set; }
        }

        private readonly EspecialidadBO _especialidadBO;
        private readonly UsuarioPorEspecialidadBO _usuarioPorEspecialidadBO;
        private List<EspecialidadConteo> ListaCompletaEspecialidades
        {
            get
            {
                return ViewState["ListaEspecialidades"] as List<EspecialidadConteo>;
            }
            set
            {
                ViewState["ListaEspecialidades"] = value;
            }
        }
        public admin_gestionar_especialidades()
        {
            _especialidadBO = new EspecialidadBO();
            _usuarioPorEspecialidadBO = new UsuarioPorEspecialidadBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosDesdeServicio();
                // Y luego los muestra en la grilla
                AplicarFiltrosYEnlazarGrid();
            }
        }
        private void CargarDatosDesdeServicio()
        {
            try
            {
                var especialidadesWs = _especialidadBO.ListarEspecialidad();
                var listaViewModel = new List<EspecialidadConteo>();

                foreach (var esp in especialidadesWs)
                {
                    // Por cada especialidad, consultamos cuántos médicos tiene
                    var medicos = _usuarioPorEspecialidadBO.ListarPorEspecialidadUsuarioPorEspecialidad(esp.idEspecialidad);
                    listaViewModel.Add(new EspecialidadConteo
                    {
                        ID = esp.idEspecialidad,
                        NombreEspecialidad = esp.nombreEspecialidad,
                        PrecioConsulta = esp.precioConsulta,
                        CantMedicos = medicos?.Count ?? 0,
                        Estado = esp.estadoGeneral
                    });
                }

                // Almacenamos la lista completa en nuestra propiedad de ViewState
                this.ListaCompletaEspecialidades = listaViewModel;
            }
            catch (Exception ex)
            {
                // Manejar error de conexión
                phNoEspecialidad.Visible = true;
                // Podríamos mostrar un mensaje más explícito al usuario
                System.Diagnostics.Debug.WriteLine($"Error al cargar especialidades desde el servicio: {ex.Message}");
                // Si hay error, la lista se queda vacía.
                this.ListaCompletaEspecialidades = new List<EspecialidadConteo>();
            }
        }
        private void AplicarFiltrosYEnlazarGrid()
        {
            // Obtenemos la lista completa desde la propiedad (que usa ViewState)
            var listaViewModel = this.ListaCompletaEspecialidades;

            // Si por alguna razón la lista es nula, la inicializamos
            if (listaViewModel == null)
            {
                phNoEspecialidad.Visible = true;
                rptEspecialidades.DataSource = null;
                rptEspecialidades.DataBind();
                updGestionEspecialidades.Update();
                return;
            }

            // Aplicar filtros de la UI (esto ahora es súper rápido)
            string nombreFiltro = txtFiltrarNombre.Text.Trim();
            if (!string.IsNullOrEmpty(nombreFiltro))
            {
                listaViewModel = listaViewModel.Where(esp =>
                    esp.NombreEspecialidad.ToLower().Contains(nombreFiltro.ToLower())).ToList();
            }

            // Aplicar ordenamiento (también súper rápido)
            string ordenSeleccionado = ddlOrdenarPor.SelectedValue;
            switch (ordenSeleccionado)
            {
                case "NombreDesc":
                    listaViewModel = listaViewModel.OrderByDescending(esp => esp.NombreEspecialidad).ToList();
                    break;
                case "PrecioAsc":
                    listaViewModel = listaViewModel.OrderBy(esp => esp.PrecioConsulta).ToList();
                    break;
                case "PrecioDesc":
                    listaViewModel = listaViewModel.OrderByDescending(esp => esp.PrecioConsulta).ToList();
                    break;
                case "MedicosAsc":
                    listaViewModel = listaViewModel.OrderBy(esp => esp.CantMedicos).ToList();
                    break;
                case "MedicosDesc":
                    listaViewModel = listaViewModel.OrderByDescending(esp => esp.CantMedicos).ToList();
                    break;
                default: // NombreAsc
                    listaViewModel = listaViewModel.OrderBy(esp => esp.NombreEspecialidad).ToList();
                    break;
            }

            rptEspecialidades.DataSource = listaViewModel;
            rptEspecialidades.DataBind();

            phNoEspecialidad.Visible = !listaViewModel.Any();
            updGestionEspecialidades.Update();
        }

        protected void rptEspecialidades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var especialidad = (EspecialidadConteo)e.Item.DataItem;
                var ltlEstado = (Literal)e.Item.FindControl("ltlEstado");
                var btnToggleStatus = (LinkButton)e.Item.FindControl("btnToggleStatus");

                if (especialidad.Estado == estadoGeneral.ACTIVO)
                {
                    ltlEstado.Text = "<span class='badge bg-success'>Activo</span>";
                    btnToggleStatus.ToolTip = "Desactivar";
                    var iconPowerOff = btnToggleStatus.Controls.OfType<System.Web.UI.LiteralControl>().FirstOrDefault();
                    if (iconPowerOff != null)
                    {
                        iconPowerOff.Text = "<i class=\"fa-solid fa-power-off\" style=\"color: #dc3545; font-size: 1.2em;\"></i>";
                    }
                }
                else
                {
                    ltlEstado.Text = "<span class='badge bg-danger'>Inactivo</span>";
                    btnToggleStatus.ToolTip = "Activar";
                    var iconPowerOff = btnToggleStatus.Controls.OfType<System.Web.UI.LiteralControl>().FirstOrDefault();
                    if (iconPowerOff != null)
                    {
                        iconPowerOff.Text = "<i class=\"fa-solid fa-power-off\" style=\"color: #28a745; font-size: 1.2em;\"></i>";
                    }
                }
            }
        }

        protected void rptEspecialidades_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int especialidadId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditEspecialidad")
            {
                var especialidad = _especialidadBO.ObtenerPorIdTablaEspecialidad(especialidadId);
                if (especialidad != null)
                {
                    ResetForm();
                    hfEspecialidadId.Value = especialidad.idEspecialidad.ToString();
                    txtNombreAddEdit.Text = especialidad.nombreEspecialidad;
                    txtNombreAddEdit.ReadOnly = true;
                    txtNombreAddEdit.CssClass = "form-control-plaintext";
                    txtPrecioAddEdit.Text = especialidad.precioConsulta.ToString("F2");
                    lblFormTitle.Text = "Editar Especialidad";
                    pnlAddEditEspecialidad.Visible = true;
                    btnShowAddPanel.Visible = false;
                }
            }
            else if (e.CommandName == "ToggleStatus")
            {
                var especialidad = _especialidadBO.ObtenerPorIdTablaEspecialidad(especialidadId);
                if (especialidad != null)
                {
                    especialidad.estadoGeneral = (especialidad.estadoGeneral == estadoGeneral.ACTIVO) ? estadoGeneral.INACTIVO : estadoGeneral.ACTIVO;
                    especialidad.estadoGeneralSpecified = true;

                    var usuarioLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
                    especialidad.usuarioModificacion = usuarioLogueado?.idUsuario ?? 0;
                    especialidad.usuarioModificacionSpecified = true;
                    especialidad.fechaModificacion = DateTime.Now.ToString("yyyy-MM-dd");

                    _especialidadBO.ModificarEspecialidad(especialidad);
                    CargarDatosDesdeServicio();
                    AplicarFiltrosYEnlazarGrid();
                }
            }
            updGestionEspecialidades.Update();
        }

        protected void btnShowAddPanel_Click(object sender, EventArgs e)
        {
            ResetForm();
            pnlAddEditEspecialidad.Visible = true;
            btnShowAddPanel.Visible = false;
            updGestionEspecialidades.Update();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAddEditEspecialidad.Visible = false;
            btnShowAddPanel.Visible = true;
            ResetForm();
            updGestionEspecialidades.Update();
        }

        private void ResetForm()
        {
            hfEspecialidadId.Value = "0";
            txtNombreAddEdit.Text = string.Empty;
            txtNombreAddEdit.ReadOnly = false;
            txtNombreAddEdit.CssClass = "form-control";
            txtPrecioAddEdit.Text = string.Empty;
            lblFormTitle.Text = "Agregar Nueva Especialidad";
        }

        protected void btnGuardarEspecialidad_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEditEspecialidad");
            if (!Page.IsValid)
            {
                updGestionEspecialidades.Update();
                return;
            }

            var usuarioLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            int idUsuarioAuditoria = usuarioLogueado?.idUsuario ?? 0;

            int especialidadId = Convert.ToInt32(hfEspecialidadId.Value);

            especialidadDTO especialidad = new especialidadDTO
            {
                nombreEspecialidad = txtNombreAddEdit.Text.Trim(),
                precioConsulta = Convert.ToDouble(txtPrecioAddEdit.Text),
                precioConsultaSpecified = true,
                fechaModificacion = DateTime.Now.ToString("yyyy-MM-dd"),
                usuarioModificacion = idUsuarioAuditoria,
                usuarioModificacionSpecified = true
            };

            if (especialidadId == 0) 
            {
                especialidad.usuarioCreacion = idUsuarioAuditoria;
                especialidad.usuarioCreacionSpecified = true;
                especialidad.fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd");
                especialidad.estadoGeneral = estadoGeneral.ACTIVO; 
                especialidad.estadoGeneralSpecified = true;
                _especialidadBO.InsertarEspecialidad(especialidad);
            }
            else 
            {
                especialidad.idEspecialidad = especialidadId;
                var espOriginal = _especialidadBO.ObtenerPorIdTablaEspecialidad(especialidadId);
                especialidad.fechaCreacion = espOriginal.fechaCreacion;
                especialidad.usuarioCreacion = espOriginal.usuarioCreacion;
                especialidad.usuarioCreacionSpecified = espOriginal.usuarioCreacionSpecified;
                especialidad.estadoGeneral = espOriginal.estadoGeneral; 
                especialidad.estadoGeneralSpecified = true;
                especialidad.idEspecialidadSpecified = true;
                _especialidadBO.ModificarEspecialidad(especialidad);
            }

            pnlAddEditEspecialidad.Visible = false;
            btnShowAddPanel.Visible = true;
            ResetForm();
            CargarDatosDesdeServicio();
            AplicarFiltrosYEnlazarGrid();
        }

        protected void btnAplicarFiltrosEsp_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYEnlazarGrid();
        }

        protected void btnLimpiarFiltrosEsp_Click(object sender, EventArgs e)
        {
            txtFiltrarNombre.Text = string.Empty;
            ddlOrdenarPor.SelectedIndex = 0;
            AplicarFiltrosYEnlazarGrid();
        }
    }
}