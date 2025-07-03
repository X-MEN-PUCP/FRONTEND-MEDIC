using SoftBO;
using SoftBO.adminWS; 
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
                var especialidadesWs = _adminBO.ListarEspecialidades();
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
                    var rolesPorUsuarioWs = _adminBO.ListarRolesDeUsuario(u.idUsuario);

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
                ddlRolNuevo.DataSource = roles.OrderBy(r => r.NombreRol);
                ddlRolNuevo.DataTextField = "NombreRol";
                ddlRolNuevo.DataValueField = "IdRol";
                ddlRolNuevo.DataBind();
                ddlRolNuevo.Items.Insert(0, new ListItem("-- Seleccione un rol --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al poblar dropdown de roles del modal: " + ex.Message, true);
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
                var usuarioWs = _adminBO.ObtenerUsuarioPorId(usuarioId);

                if (usuarioWs != null)
                {
                    var rolesPorUsuarioWs = _adminBO.ListarRolesDeUsuario(usuarioId);
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
                    var usuarioAmodificar = _adminBO.ObtenerUsuarioPorId(usuarioId);
                    if (usuarioAmodificar != null)
                    {
                        var nuevoEstado = (usuarioAmodificar.estadoGeneral == SoftBO.adminWS.estadoGeneral.ACTIVO)
                           ? SoftBO.adminWS.estadoGeneral.INACTIVO
                           : SoftBO.adminWS.estadoGeneral.ACTIVO;

                        usuarioAmodificar.estadoGeneral = nuevoEstado;
                        usuarioAmodificar.estadoGeneralSpecified = true;

                        var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
                        if (adminLogueado != null)
                        {
                            usuarioAmodificar.usuarioModificacion = adminLogueado.idUsuario;
                            usuarioAmodificar.usuarioModificacionSpecified = true;
                            usuarioAmodificar.fechaModificacion = DateTime.Now.ToString("yyyy-MM-dd");
                        }

                        _adminBO.ModificarUsuario(usuarioAmodificar);
                        MostrarMensaje("Cambio de estado exitoso.", false);

                        var usuarioEnLista = ListaCompletaUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);
                        if (usuarioEnLista != null)
                        {
                            usuarioEnLista.EstadoGeneral = nuevoEstado;
                        }
                        AplicarFiltrosYEnlazarGrid();
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error al intentar cambiar el estado del usuario: " + ex.Message, true);
                }
            }
            else if (e.CommandName == "ResetPassword")
            {
                try
                {
                    var usuarioParaReset = _adminBO.ObtenerUsuarioPorId(usuarioId);

                    var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
                    if (adminLogueado != null)
                    {
                        usuarioParaReset.usuarioModificacion = adminLogueado.idUsuario;
                        usuarioParaReset.usuarioModificacionSpecified = true;
                    }

                    int resultado = _adminBO.modificarUsuarioPoneContraDefault(usuarioParaReset);

                    if (resultado > 0)
                    {
                        MostrarMensaje("La contraseña se ha restablecido correctamente a su valor por defecto.", false);
                    }
                    else
                    {
                        MostrarMensaje("No se pudo restablecer la contraseña. (password)", true);
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Ocurrió un error al restablecer la contraseña: {ex.Message}", true);
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
                        CerrarModalRolesDesdeServidor();
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
            pnlCamposMedico.Visible = esMedico;
    
            rfvCodMedico.Enabled = esMedico;
    
            if (esMedico)
            {
                CargarEspecialidadesEnModal();
            }

            string script = @"
                setTimeout(function() {
                    if (!$('#modalAgregarUsuario').hasClass('show')) {
                        $('#modalAgregarUsuario').modal('show');
                    }
                }, 100);";
    
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MantenerModalAbierto", script, true);
        }

        private void CargarEspecialidadesEnModal()
        {
            try
            {
                chkEspecialidadesNuevo.Items.Clear();
                if (ListaCompletaEspecialidades != null && ListaCompletaEspecialidades.Any())
                {
                    foreach (var esp in ListaCompletaEspecialidades.OrderBy(e => e.NombreEspecialidad))
                    {
                        chkEspecialidadesNuevo.Items.Add(new ListItem(esp.NombreEspecialidad, esp.IdEspecialidad.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar especialidades: " + ex.Message, true);
            }
        }

        protected void btnGuardarNuevoUsuario_Click(object sender, EventArgs e)
        {
            Page.Validate("NuevoUsuario");
            if (!Page.IsValid)
            {
                return;
            }

            var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (adminLogueado == null)
            {
                MostrarMensaje("Su sesión ha expirado. Por favor, inicie sesión de nuevo.", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CerrarModalError", "$('#modalAgregarUsuario').modal('hide');", true);
                return;
            }

            var nuevoUsuario = new SoftBO.adminWS.usuarioDTO
            {
                nombres = txtNombresNuevo.Text.Trim(),
                apellidoPaterno = txtApellidoPaternoNuevo.Text.Trim(),
                apellidoMaterno = txtApellidoMaternoNuevo.Text.Trim(),
                tipoDocumento = (SoftBO.adminWS.tipoDocumento)Enum.Parse(typeof(SoftBO.adminWS.tipoDocumento), ddlTipoDocumentoNuevo.SelectedValue),
                tipoDocumentoSpecified = true,
                numDocumento = txtNumDocumentoNuevo.Text.Trim(),
                correoElectronico = txtCorreoNuevo.Text.Trim(),
                numCelular = txtCelularNuevo.Text.Trim(),
                fechaNacimiento = txtFechaNacimientoNuevo.Text,
                genero = (SoftBO.adminWS.genero)Enum.Parse(typeof(SoftBO.adminWS.genero), ddlGeneroNuevo.SelectedValue),
                generoSpecified = true,
                usuarioCreacion = adminLogueado.idUsuario,
                usuarioCreacionSpecified = true,
                fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd"),
                estadoGeneral = SoftBO.adminWS.estadoGeneral.ACTIVO  
            };

            int resultado = 0;
            int rolId = Convert.ToInt32(ddlRolNuevo.SelectedValue);
            string nombreRolSeleccionado = ddlRolNuevo.SelectedItem.Text;

            try
            {
                switch (rolId)
                {
                    case 1: 
                        resultado = _adminBO.InsertarNuevoAdministrador(nuevoUsuario);
                        break;
                    case 2:
                        nuevoUsuario.codMedico = txtCodMedicoNuevo.Text.Trim();
                        var especialidadesSeleccionadas = new BindingList<especialidadDTO>();
                        foreach (ListItem item in chkEspecialidadesNuevo.Items)
                        {
                            if (item.Selected)
                            {
                                especialidadesSeleccionadas.Add(new especialidadDTO
                                {
                                    idEspecialidad = Convert.ToInt32(item.Value),
                                    idEspecialidadSpecified = true
                                });
                            }
                        }

                        if (!especialidadesSeleccionadas.Any())
                        {
                            vsNuevoUsuario.HeaderText = "Por favor, corrija los siguientes errores:";
                            var customValidator = new CustomValidator
                            {
                                IsValid = false,
                                ErrorMessage = "Debe seleccionar al menos una especialidad para el rol de Médico.",
                                ValidationGroup = "NuevoUsuario"
                            };
                            Page.Validators.Add(customValidator);

                            string script = @"
                                setTimeout(function() {
                                    if (!$('#modalAgregarUsuario').hasClass('show')) {
                                        $('#modalAgregarUsuario').modal('show');
                                    }
                                }, 100);";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MantenerModalAbierto", script, true);
                            return;
                        }

                        resultado = _adminBO.InsertarNuevoMedico(nuevoUsuario, especialidadesSeleccionadas);
                        break;
                    case 3: // Paciente
                        resultado = _adminBO.InsertarNuevoPaciente(nuevoUsuario);
                        break;
                    default:
                        MostrarMensaje("Debe seleccionar un rol válido.", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpenOnError", "$('#modalAgregarUsuario').modal('show');", true);
                        return;
                }

                if (resultado > 0)
                {
                    string contrasenhaPorDefecto = nuevoUsuario.numDocumento +
                                                   nuevoUsuario.apellidoPaterno.Substring(0, 1).ToUpper() +
                                                   nuevoUsuario.apellidoMaterno.Substring(0, 1).ToUpper();

                    MostrarMensajeExitoConContrasenha(nombreRolSeleccionado, contrasenhaPorDefecto);

                    LimpiarFormularioNuevoUsuario();
                    CargarDatosUsuariosDesdeServicio();
                    AplicarFiltrosYEnlazarGrid();
                    CerrarModalDesdeServidor();
                    string scriptCerrar = @"
                        setTimeout(function() {
                            $('#modalAgregarUsuario').modal('hide');
                            // Limpiar el backdrop si queda
                            $('.modal-backdrop').remove();
                            $('body').removeClass('modal-open');
                        }, 500);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAddModal", "$('#modalAgregarUsuario').modal('hide');", true);
                }
                else
                {
                    MostrarMensaje($"Error: No se pudo crear el nuevo {nombreRolSeleccionado.ToLower()}.", true);
                    CerrarModalDesdeServidor();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar el nuevo usuario: {ex.Message}", true);
            }
        }

        private void LimpiarFormularioNuevoUsuario()
        {
            txtNombresNuevo.Text = string.Empty;
            txtApellidoPaternoNuevo.Text = string.Empty;
            txtApellidoMaternoNuevo.Text = string.Empty;
            ddlTipoDocumentoNuevo.SelectedIndex = 0;
            txtNumDocumentoNuevo.Text = string.Empty;
            txtCorreoNuevo.Text = string.Empty;
            txtCelularNuevo.Text = string.Empty;
            txtFechaNacimientoNuevo.Text = string.Empty;
            ddlGeneroNuevo.SelectedIndex = 0;
            ddlRolNuevo.SelectedValue = "0";

            pnlCamposMedico.Visible = false;
            txtCodMedicoNuevo.Text = string.Empty;
            foreach (ListItem item in chkEspecialidadesNuevo.Items)
            {
                item.Selected = false;
            }
        }

        private void MostrarMensajeExitoConContrasenha(string rol, string contrasenha)
        {
            phMensaje.Visible = true;
            string mensajeHtml = $@"
                <strong>¡Éxito!</strong> Se ha creado el nuevo {rol.ToLower()} correctamente.
                <hr>
                La contraseña temporal es: <strong class='fs-5'>{contrasenha}</strong>
                <br>
                <small>Se recomienda al usuario cambiarla en su primer inicio de sesión.</small>";
            ltlMensaje.Text = mensajeHtml;
            divAlert.Attributes["class"] = "alert alert-success alert-dismissible fade show";
        }

        private void CerrarModalDesdeServidor()
        {
            string script = @"
                var modalElement = document.getElementById('modalAgregarUsuario');
                if (modalElement) {
                    var modalInstance = bootstrap.Modal.getInstance(modalElement);
                    if (modalInstance) {
                        modalInstance.hide();
                    }
                }
                var backdrops = document.getElementsByClassName('modal-backdrop');
                while (backdrops[0]) {
                    backdrops[0].parentNode.removeChild(backdrops[0]);
                }
                document.body.classList.remove('modal-open');
                document.body.style.overflow = 'auto';
                ";

            ScriptManager.RegisterStartupScript(updGestionUsuarios, updGestionUsuarios.GetType(), "CloseModalScript", script, true);
        }

        private void CerrarModalRolesDesdeServidor()
        {
            string script = @"
                var modalElement = document.getElementById('modalLabel');
                if (modalElement) {
                    var modalInstance = bootstrap.Modal.getInstance(modalElement);
                    if (modalInstance) {
                        modalInstance.hide();
                    }
                }
                var backdrops = document.getElementsByClassName('modal-backdrop');
                while (backdrops[0]) {
                    backdrops[0].parentNode.removeChild(backdrops[0]);
                }
                document.body.classList.remove('modal-open');
                document.body.style.overflow = 'auto';
                ";

            ScriptManager.RegisterStartupScript(updGestionUsuarios, updGestionUsuarios.GetType(), "CloseModalScript", script, true);
        }
    }
}