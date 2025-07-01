using SoftBO;
using SoftBO.adminWS; 
using SoftBO.rolesporusuarioWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    [Serializable]
    public class UsuarioGestionInfo
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoDocumento { get; set; }
        public string NumDocumento { get; set; }
        public string Correo { get; set; }
        public List<SoftBO.adminWS.rolDTO> Roles { get; set; }
        public string RolesConcatenados => Roles != null && Roles.Any()
            ? string.Join(", ", Roles.Select(r => r.nombreRol))
            : "Sin roles";
        public SoftBO.adminWS.estadoGeneral EstadoGeneral { get; set; }
    }
    [Serializable]
    public class RolSimple
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
    }
    [Serializable]
    public class EspecialidadSimple
    {
        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; }
    }

    public partial class admin_gestionar_usuarios : System.Web.UI.Page
    {
        private readonly AdminBO _adminBO;
        private readonly RolesPorUsuarioBO _rolesBO;
        private readonly EspecialidadBO _especialidadBO;
        private readonly UsuarioBO _usuarioBO;
        private List<UsuarioGestionInfo> ListaCompletaUsuarios
        {
            get { return ViewState["ListaCompletaUsuarios"] as List<UsuarioGestionInfo>; }
            set { ViewState["ListaCompletaUsuarios"] = value; }
        }

        private List<RolSimple> ListaCompletaRoles
        {
            get { return ViewState["ListaCompletaRoles"] as List<RolSimple>; }
            set { ViewState["ListaCompletaRoles"] = value; }
        }

        private List<EspecialidadSimple> ListaCompletaEspecialidades
        {
            get { return ViewState["ListaCompletaEspecialidades"] as List<EspecialidadSimple>; }
            set { ViewState["ListaCompletaEspecialidades"] = value; }
        }
        public admin_gestionar_usuarios()
        {
            _adminBO = new AdminBO();
            _rolesBO = new RolesPorUsuarioBO();
            _especialidadBO = new EspecialidadBO();
            _usuarioBO = new UsuarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosMaestros();

                CargarDatosUsuariosDesdeServicio();

                AplicarFiltrosYEnlazarGrid();
            }
        }

        private void CargarDatosMaestros()
        {
            try
            {
                ListaCompletaRoles = new List<RolSimple>
                {
                    new RolSimple { IdRol = 1, NombreRol = "Administrador" },
                    new RolSimple { IdRol = 2, NombreRol = "Médico" },
                    new RolSimple { IdRol = 3, NombreRol = "Paciente" }
                };
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar la lista de roles maestros. " + ex.Message, true);
                ListaCompletaRoles = new List<RolSimple>();
            }

            try
            {
                var especialidadesWs = _especialidadBO.ListarEspecialidad();
                ListaCompletaEspecialidades = especialidadesWs.Select(e => new EspecialidadSimple
                {
                    IdEspecialidad = e.idEspecialidad,
                    NombreEspecialidad = e.nombreEspecialidad
                }).ToList();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar especialidades. " + ex.Message, true);
                ListaCompletaEspecialidades = new List<EspecialidadSimple>();
            }

            PoblarFiltroRoles();
            PoblarDropDownsModal();
        }

        private void CargarDatosUsuariosDesdeServicio()
        {
            try
            {
                var usuariosWs = _adminBO.ListarTodosUsuarios();
                if (usuariosWs == null) throw new Exception("El servicio no devolvió usuarios.");

                var listaUsuariosParaMostrar = new List<UsuarioGestionInfo>();

                foreach (var u in usuariosWs)
                {
                    var rolesPorUsuarioWs = _rolesBO.ListarPorUsuarioRolesPorUsuario(u.idUsuario);

                    var rolesDelUsuario = new List<SoftBO.adminWS.rolDTO>();
                    if (rolesPorUsuarioWs != null)
                    {
                        rolesDelUsuario = rolesPorUsuarioWs.Select(rpu =>
                        {
                            var rolInfo = this.ListaCompletaRoles.FirstOrDefault(r => r.IdRol == rpu.rol.idRol);
                            return new SoftBO.adminWS.rolDTO { idRol = rpu.rol.idRol, nombreRol = rolInfo?.NombreRol ?? "Desconocido" };
                        }).ToList();
                    }

                    listaUsuariosParaMostrar.Add(new UsuarioGestionInfo
                    {
                        IdUsuario = u.idUsuario,
                        NombreCompleto = $"{u.nombres} {u.apellidoPaterno} {u.apellidoMaterno}".Trim(),
                        TipoDocumento = u.tipoDocumento.ToString(),
                        NumDocumento = u.numDocumento,
                        Correo = u.correoElectronico,
                        EstadoGeneral = u.estadoGeneral,
                        Roles = rolesDelUsuario
                    });
                }

                this.ListaCompletaUsuarios = listaUsuariosParaMostrar;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error de conexión al obtener la lista de usuarios. " + ex.Message, true);
                this.ListaCompletaUsuarios = new List<UsuarioGestionInfo>();
            }
        }

        private void PoblarDropDownsModal()
        {
            try
            {
                var roles = this.ListaCompletaRoles;
                var especialidades = this.ListaCompletaEspecialidades;

                ddlRolNuevo.DataSource = roles.OrderBy(r => r.NombreRol);
                ddlRolNuevo.DataTextField = "NombreRol";
                ddlRolNuevo.DataValueField = "IdRol";
                ddlRolNuevo.DataBind();
                ddlRolNuevo.Items.Insert(0, new ListItem("-- Seleccione un rol --", "0"));

                ddlEspecialidadNuevo.DataSource = especialidades.OrderBy(e => e.NombreEspecialidad);
                ddlEspecialidadNuevo.DataTextField = "NombreEspecialidad";
                ddlEspecialidadNuevo.DataValueField = "IdEspecialidad";
                ddlEspecialidadNuevo.DataBind();
                ddlEspecialidadNuevo.Items.Insert(0, new ListItem("-- Seleccione especialidad --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al poblar dropdowns del modal: " + ex.Message, true);
            }
        }

        private void PoblarFiltroRoles()
        {
            try
            {
                var roles = this.ListaCompletaRoles;
                ddlFiltroRol.DataSource = roles.OrderBy(r => r.NombreRol);
                ddlFiltroRol.DataTextField = "NombreRol";
                ddlFiltroRol.DataValueField = "IdRol";
                ddlFiltroRol.DataBind();
                ddlFiltroRol.Items.Insert(0, new ListItem("-- Todos los Roles --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al poblar filtro de roles: " + ex.Message, true);
            }
        }

        private void AplicarFiltrosYEnlazarGrid()
        {
            var listaUsuariosParaMostrar = this.ListaCompletaUsuarios;

            if (listaUsuariosParaMostrar == null)
            {
                MostrarMensaje("No se pudieron cargar los datos de los usuarios.", true);
                lvUsuarios.DataSource = null;
                lvUsuarios.DataBind();
                return;
            }

            // Aplicar filtros
            int filtroRolId = 0;
            int.TryParse(ddlFiltroRol.SelectedValue, out filtroRolId);
            if (filtroRolId > 0)
            {
                listaUsuariosParaMostrar = listaUsuariosParaMostrar.Where(u => u.Roles.Any(r => r.idRol == filtroRolId)).ToList();
            }

            string filtroNombre = txtFiltroNombre.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(filtroNombre))
            {
                listaUsuariosParaMostrar = listaUsuariosParaMostrar.Where(u =>
                    u.NombreCompleto.ToLower().Contains(filtroNombre) ||
                    u.NumDocumento.Contains(filtroNombre)
                ).ToList();
            }

            // Aplicar ordenamiento
            switch (ddlOrdenarUsuarios.SelectedValue)
            {
                case "IdDesc": listaUsuariosParaMostrar = listaUsuariosParaMostrar.OrderByDescending(u => u.IdUsuario).ToList(); break;
                case "NombreAsc": listaUsuariosParaMostrar = listaUsuariosParaMostrar.OrderBy(u => u.NombreCompleto).ToList(); break;
                case "NombreDesc": listaUsuariosParaMostrar = listaUsuariosParaMostrar.OrderByDescending(u => u.NombreCompleto).ToList(); break;
                default: listaUsuariosParaMostrar = listaUsuariosParaMostrar.OrderBy(u => u.IdUsuario).ToList(); break;
            }

            lvUsuarios.DataSource = listaUsuariosParaMostrar;
            lvUsuarios.DataBind();
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYEnlazarGrid();
        }

        protected void lnkLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroNombre.Text = string.Empty;
            ddlFiltroRol.SelectedValue = "0";
            ddlOrdenarUsuarios.SelectedValue = "IdAsc";
            AplicarFiltrosYEnlazarGrid();
        }

        protected void lvUsuarios_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var usuario = (UsuarioGestionInfo)e.Item.DataItem;
                var ltlEstado = (Literal)e.Item.FindControl("ltlEstado");
                var btnToggleStatus = (LinkButton)e.Item.FindControl("btnToggleStatus");

                if (usuario.EstadoGeneral == SoftBO.adminWS.estadoGeneral.ACTIVO)
                {
                    ltlEstado.Text = "<span class='badge bg-success'>Activo</span>";
                    btnToggleStatus.ToolTip = "Desactivar Usuario";
                    var ltlIcono = (Literal)btnToggleStatus.FindControl("ltlIcono");
                    if (ltlIcono != null)
                        ltlIcono.Text = "<i class='fas fa-power-off text-danger'></i>";
                }
                else
                {
                    ltlEstado.Text = "<span class='badge bg-danger'>Inactivo</span>";
                    btnToggleStatus.ToolTip = "Activar Usuario";
                    var ltlIcono = (Literal)btnToggleStatus.FindControl("ltlIcono");
                    if (ltlIcono != null)
                        ltlIcono.Text = "<i class='fas fa-power-off text-success'></i>";
                }
            }
        }

        protected void lvUsuarios_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int usuarioId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "GestionarRoles")
            {
                var rolesPorUsuarioWs = _rolesBO.ListarPorUsuarioRolesPorUsuario(usuarioId);
                var usuarioWs = _adminBO.ListarTodosUsuarios().FirstOrDefault(u => u.idUsuario == usuarioId);

                if (usuarioWs != null)
                {
                    var rolesActuales = rolesPorUsuarioWs?.Select(rpu =>
                    {
                        var rolInfo = ListaCompletaRoles.FirstOrDefault(r => r.IdRol == rpu.rol.idRol);
                        return new RolSimple { IdRol = rpu.rol.idRol, NombreRol = rolInfo?.NombreRol ?? "Desconocido" };
                    }).ToList() ?? new List<RolSimple>();

                    hfUsuarioIdModal.Value = usuarioWs.idUsuario.ToString();
                    ltlNombreUsuarioModal.Text = $"{usuarioWs.nombres} {usuarioWs.apellidoPaterno}".Trim();

                    rptRolesActuales.DataSource = rolesActuales;
                    rptRolesActuales.DataBind();

                    var rolesDisponibles = ListaCompletaRoles.Where(r => !rolesActuales.Any(ur => ur.IdRol == r.IdRol)).ToList();
                    ddlRolesDisponibles.DataSource = rolesDisponibles;
                    ddlRolesDisponibles.DataTextField = "NombreRol";
                    ddlRolesDisponibles.DataValueField = "IdRol";
                    ddlRolesDisponibles.DataBind();
                    ddlRolesDisponibles.Items.Insert(0, new ListItem("-- Seleccione un rol --", "0"));

                    btnAsignarRol.Enabled = rolesDisponibles.Any();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalGestionarRoles').modal('show');", true);
                }
            }
            else if (e.CommandName == "ToggleStatus")
            {
                try
                {
                    var usuarioAmodificar = _usuarioBO.ObtenerPorIdUsuario(usuarioId);
                    var nuevoEstado = (usuarioAmodificar.estadoGeneral == SoftBO.usuarioWS.estadoGeneral.ACTIVO)
                    ? SoftBO.usuarioWS.estadoGeneral.INACTIVO
                    : SoftBO.usuarioWS.estadoGeneral.ACTIVO;
                    usuarioAmodificar.estadoGeneral = nuevoEstado;
                    usuarioAmodificar.estadoGeneralSpecified = true;
                    var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
                    if (adminLogueado != null)
                    {
                        usuarioAmodificar.usuarioModificacion = adminLogueado.idUsuario;
                        usuarioAmodificar.usuarioModificacionSpecified = true;
                        usuarioAmodificar.fechaModificacion = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    _usuarioBO.ModificarUsuario(usuarioAmodificar);
                    MostrarMensaje("Cambio de estado exitoso", false);
                    CargarDatosUsuariosDesdeServicio();
                    AplicarFiltrosYEnlazarGrid();
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error al intentar cambiar el estado del usuario: " + ex.Message, true);
                }
            }
        }

        protected void btnAsignarRol_Click(object sender, EventArgs e)
        {
            int usuarioId = Convert.ToInt32(hfUsuarioIdModal.Value);
            int rolId = Convert.ToInt32(ddlRolesDisponibles.SelectedValue);
            var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;

            if (rolId > 0 && adminLogueado != null)
            {
                var usuario = new SoftBO.adminWS.usuarioDTO { idUsuario = usuarioId, idUsuarioSpecified = true };
                var rol = new SoftBO.adminWS.rolDTO { idRol = rolId, idRolSpecified = true };
                var usuarioPorRol = new SoftBO.adminWS.usuarioPorRolDTO
                {
                    usuarioDTO = usuario,
                    rol = rol,
                    usuarioCreacion = adminLogueado.idUsuario,
                    usuarioCreacionSpecified = true,
                    fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd")
                };

                try
                {
                    int resultado = _adminBO.AsignarNuevoRolParaUsuario(usuarioPorRol);
                    if (resultado > 0)
                    {
                        MostrarMensaje("Rol asignado correctamente.", false);
                        CargarDatosUsuariosDesdeServicio();
                        AplicarFiltrosYEnlazarGrid();
                    }
                    else
                    {
                        MostrarMensaje("No se pudo asignar el rol.", true);
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error al asignar rol: " + ex.Message, true);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideModal", "$('#modalGestionarRoles').modal('hide');", true);
        }

        protected void rptRoles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "QuitarRol")
            {
                int usuarioId = Convert.ToInt32(hfUsuarioIdModal.Value);
                int rolId = Convert.ToInt32(e.CommandArgument);

                var usuarioPorRol = new SoftBO.adminWS.usuarioPorRolDTO
                {
                    usuarioDTO = new SoftBO.adminWS.usuarioDTO { idUsuario = usuarioId, idUsuarioSpecified = true },
                    rol = new SoftBO.adminWS.rolDTO { idRol = rolId, idRolSpecified = true }
                };

                try
                {
                    int resultado = _adminBO.EliminarRolDeUsuario(usuarioPorRol);
                    if (resultado > 0)
                    {
                        MostrarMensaje("Rol eliminado correctamente.", false);
                        CargarDatosUsuariosDesdeServicio();
                        AplicarFiltrosYEnlazarGrid();
                    }
                    else
                    {
                        MostrarMensaje("No se pudo eliminar el rol.", true);
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error al eliminar rol: " + ex.Message, true);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideModal", "$('#modalGestionarRoles').modal('hide');", true);
            }
        }

        private void MostrarMensaje(string mensaje, bool esError)
        {
            phMensaje.Visible = true;
            ltlMensaje.Text = mensaje;
            divAlert.Attributes["class"] = esError ? "alert alert-danger alert-dismissible fade show" : "alert alert-success alert-dismissible fade show";
        }

        protected void ddlRolNuevo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool esMedico = ddlRolNuevo.SelectedValue == "2";
            pnlEspecialidadNuevo.Visible = esMedico;
        }

        protected void btnGuardarNuevoUsuario_Click(object sender, EventArgs e)
        {
            Page.Validate("NuevoUsuario");
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpen", "$('#modalAgregarUsuario').modal('show');", true);
                return;
            }

            var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (adminLogueado == null)
            {
                MostrarMensaje("Su sesión ha expirado. Por favor, inicie sesión de nuevo.", true);
                return;
            }

            var nuevoUsuario = new SoftBO.adminWS.usuarioDTO
            {
                nombres = txtNombresNuevo.Text.Trim(),
                apellidoPaterno = txtApellidoPaternoNuevo.Text.Trim(),
                apellidoMaterno = "",
                tipoDocumento = (SoftBO.adminWS.tipoDocumento)Enum.Parse(typeof(SoftBO.adminWS.tipoDocumento), ddlTipoDocumentoNuevo.SelectedValue),
                tipoDocumentoSpecified = true,
                numDocumento = txtNumDocumentoNuevo.Text.Trim(),
                contrasenha = txtContrasenhaNuevo.Text,
                correoElectronico = txtCorreoNuevo.Text.Trim(),
                usuarioCreacion = adminLogueado.idUsuario,
                usuarioCreacionSpecified = true,
                fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd")
            };

            try
            {
                int rolId = Convert.ToInt32(ddlRolNuevo.SelectedValue);
                if (rolId == 2)
                {
                    int especialidadId = Convert.ToInt32(ddlEspecialidadNuevo.SelectedValue);
                    if (especialidadId == 0)
                    {
                        MostrarMensaje("Debe seleccionar una especialidad para el rol de Médico.", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpenOnError", "$('#modalAgregarUsuario').modal('show');", true);
                        return;
                    }
                    BindingList<SoftBO.adminWS.especialidadDTO> especialidades = new BindingList<SoftBO.adminWS.especialidadDTO>();
                    var especialidad = new especialidadDTO { idEspecialidad = especialidadId, idEspecialidadSpecified = true };
                    especialidades.Add(especialidad);
                    bool resultado = _adminBO.InsertarNuevoMedico(nuevoUsuario, especialidades);

                    if (resultado)
                    {
                        MostrarMensaje("Médico creado y asignado correctamente.", false);
                        LimpiarFormularioNuevoUsuario();
                    }
                    else
                    {
                        MostrarMensaje("Error: No se pudo crear el médico.", true);
                    }
                }
                else
                {
                    //_usuarioBO.InsertarUsuario(nuevoUsuario); faltaimplementar
                    LimpiarFormularioNuevoUsuario();
                }

                CargarDatosUsuariosDesdeServicio();
                AplicarFiltrosYEnlazarGrid();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAddModal", "$('#modalAgregarUsuario').modal('hide');", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar el nuevo usuario: {ex.Message}", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpenOnError", "$('#modalAgregarUsuario').modal('show');", true);
            }
        }

        private void LimpiarFormularioNuevoUsuario()
        {
            txtNombresNuevo.Text = string.Empty;
            txtApellidoPaternoNuevo.Text = string.Empty;
            txtNumDocumentoNuevo.Text = string.Empty;
            txtCorreoNuevo.Text = string.Empty;
            txtContrasenhaNuevo.Text = string.Empty;
            ddlRolNuevo.SelectedValue = "0";
            ddlEspecialidadNuevo.SelectedValue = "0";
            pnlEspecialidadNuevo.Visible = false;
        }
    }
}