using SoftBO;
using SoftBO.adminWS; // Cambiar si el WS se renombra o es diferente
using SoftBO.rolesporusuarioWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
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

    public class RolSimple
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
    }

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
        private static List<RolSimple> _listaCompletaRoles;
        private List<EspecialidadSimple> _listaCompletaEspecialidades;

        public admin_gestionar_usuarios()
        {
            _adminBO = new AdminBO();
            _rolesBO = new RolesPorUsuarioBO();
            _especialidadBO = new EspecialidadBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar datos maestros solo una vez
                if (_listaCompletaRoles == null)
                {
                    CargarRolesMaestros();
                }
                PoblarFiltroRoles();
                BindUsuariosGrid();
            }
        }

        private void CargarRolesMaestros()
        {
            try
            {
                // Simulación de roles estáticos (idealmente vendrían de un WS, por ejemplo, RolWS.ListarTodos())
                _listaCompletaRoles = new List<RolSimple>
            {
                new RolSimple { IdRol = 1, NombreRol = "Administrador" },
                new RolSimple { IdRol = 2, NombreRol = "Médico" },
                new RolSimple { IdRol = 3, NombreRol = "Paciente" }
            };
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar la lista de roles maestros. " + ex.Message, true);
                _listaCompletaRoles = new List<RolSimple>();
            }

            try
            {
                var especialidadesWs = _especialidadBO.ListarEspecialidad();
                _listaCompletaEspecialidades = especialidadesWs.Select(e => new EspecialidadSimple
                {
                    IdEspecialidad = e.idEspecialidad,
                    NombreEspecialidad = e.nombreEspecialidad
                }).ToList();

                ddlRolNuevo.DataSource = _listaCompletaRoles.OrderBy(r => r.NombreRol);
                ddlRolNuevo.DataTextField = "NombreRol";
                ddlRolNuevo.DataValueField = "IdRol";
                ddlRolNuevo.DataBind();
                ddlRolNuevo.Items.Insert(0, new ListItem("-- Seleccione un rol --", "0"));

                ddlEspecialidadNuevo.DataSource = _listaCompletaEspecialidades.OrderBy(e => e.NombreEspecialidad);
                ddlEspecialidadNuevo.DataTextField = "NombreEspecialidad";
                ddlEspecialidadNuevo.DataValueField = "IdEspecialidad";
                ddlEspecialidadNuevo.DataBind();
                ddlEspecialidadNuevo.Items.Insert(0, new ListItem("-- Seleccione especialidad --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar datos maestros para la creación de usuarios. " + ex.Message, true);
                _listaCompletaEspecialidades = new List<EspecialidadSimple>();
            }


        }

        private void PoblarFiltroRoles()
        {
            ddlFiltroRol.DataSource = _listaCompletaRoles.OrderBy(r => r.NombreRol);
            ddlFiltroRol.DataTextField = "NombreRol";
            ddlFiltroRol.DataValueField = "IdRol";
            ddlFiltroRol.DataBind();
            ddlFiltroRol.Items.Insert(0, new ListItem("-- Todos los Roles --", "0"));
        }

        private void BindUsuariosGrid()
        {
            List<UsuarioGestionInfo> listaUsuariosParaMostrar;

            try
            {
                var usuariosWs = _adminBO.ListarTodosUsuarios();
                if (usuariosWs == null) throw new Exception("El servicio no devolvió usuarios.");

                listaUsuariosParaMostrar = new List<UsuarioGestionInfo>();

                foreach (var u in usuariosWs)
                {
                    var rolesPorUsuarioWs = _rolesBO.ListarPorUsuarioRolesPorUsuario(u.idUsuario);

                    var rolesDelUsuario = new List<SoftBO.adminWS.rolDTO>();
                    if (rolesPorUsuarioWs != null)
                    {
                        rolesDelUsuario = rolesPorUsuarioWs.Select(rpu =>
                        {
                            var rolInfo = _listaCompletaRoles.FirstOrDefault(r => r.IdRol == rpu.rol.idRol);
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
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error de conexión al obtener la lista de usuarios. " + ex.Message, true);
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
            BindUsuariosGrid();
        }

        protected void lnkLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroNombre.Text = string.Empty;
            ddlFiltroRol.SelectedValue = "0";
            ddlOrdenarUsuarios.SelectedValue = "IdAsc";
            BindUsuariosGrid();
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
                    ltlIcono.Text = "<i class='fas fa-power-off text-danger'></i>"; 

                }
                else
                {
                    ltlEstado.Text = "<span class='badge bg-danger'>Inactivo</span>";
                    btnToggleStatus.ToolTip = "Activar Usuario";
                    var ltlIcono = (Literal)btnToggleStatus.FindControl("ltlIcono");
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
                        var rolInfo = _listaCompletaRoles.FirstOrDefault(r => r.IdRol == rpu.rol.idRol);
                        return new RolSimple { IdRol = rpu.rol.idRol, NombreRol = rolInfo?.NombreRol ?? "Desconocido" };
                    }).ToList() ?? new List<RolSimple>();

                    hfUsuarioIdModal.Value = usuarioWs.idUsuario.ToString();
                    ltlNombreUsuarioModal.Text = $"{usuarioWs.nombres} {usuarioWs.apellidoPaterno}".Trim();

                    rptRolesActuales.DataSource = rolesActuales;
                    rptRolesActuales.DataBind();

                    var rolesDisponibles = _listaCompletaRoles.Where(r => !rolesActuales.Any(ur => ur.IdRol == r.IdRol)).ToList();
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
                 MostrarMensaje("Cambio de estado exitoso", false);
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
                        BindUsuariosGrid(); 
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
                        BindUsuariosGrid();
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
            // ID Rol "Médico" = 2 (según la simulación de datos)
            bool esMedico = ddlRolNuevo.SelectedValue == "2";
            pnlEspecialidadNuevo.Visible = esMedico;

            // Actualizar el panel del modal para que el cambio sea visible
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateModal", "$('#modalAgregarUsuario').modal('handleUpdate');", true);
        }

        protected void btnGuardarNuevoUsuario_Click(object sender, EventArgs e)
        {
            Page.Validate("NuevoUsuario");
            if (!Page.IsValid)
            {
                // Forzar al modal a mantenerse abierto si la validación falla
                ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpen", "$('#modalAgregarUsuario').modal('show');", true);
                return;
            }

            var adminLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (adminLogueado == null)
            {
                MostrarMensaje("Su sesión ha expirado. Por favor, inicie sesión de nuevo.", true);
                return;
            }

            // Crear el objeto usuario a partir del formulario
            var nuevoUsuario = new SoftBO.adminWS.usuarioDTO
            {
                nombres = txtNombresNuevo.Text.Trim(),
                apellidoPaterno = txtApellidoPaternoNuevo.Text.Trim(),
                apellidoMaterno = "", // Asumimos que no es obligatorio por ahora
                tipoDocumento = (SoftBO.adminWS.tipoDocumento)Enum.Parse(typeof(SoftBO.adminWS.tipoDocumento), ddlTipoDocumentoNuevo.SelectedValue),
                tipoDocumentoSpecified = true,
                numDocumento = txtNumDocumentoNuevo.Text.Trim(),
                contrasenha = txtContrasenhaNuevo.Text, // La contraseña se debe cifrar en el backend
                correoElectronico = txtCorreoNuevo.Text.Trim(),
                // Campos de auditoría
                usuarioCreacion = adminLogueado.idUsuario,
                usuarioCreacionSpecified = true,
                fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd")
            };

            try
            {
                // Determinar si es un médico
                int rolId = Convert.ToInt32(ddlRolNuevo.SelectedValue);
                if (rolId == 2) // ID del rol Médico
                {
                    int especialidadId = Convert.ToInt32(ddlEspecialidadNuevo.SelectedValue);
                    if (especialidadId == 0)
                    {
                        MostrarMensaje("Debe seleccionar una especialidad para el rol de Médico.", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpenOnError", "$('#modalAgregarUsuario').modal('show');", true);
                        return;
                    }
                    var especialidad = new especialidadDTO { idEspecialidad = especialidadId, idEspecialidadSpecified = true };

                    // Llamar al método específico para insertar médico
                    int resultado = _adminBO.InsertarNuevoMedico(nuevoUsuario, especialidad);

                    if (resultado > 0)
                    {
                        MostrarMensaje("Médico creado y asignado correctamente.", false);
                    }
                    else
                    {
                        MostrarMensaje("Error: No se pudo crear el médico.", true);
                    }
                }
                else
                {
                    // Lógica para insertar un usuario general (no médico)
                    // Esto requeriría un nuevo método en el backend: AdminWS.insertarUsuarioGeneral
                    // Por ahora, simularemos que solo se pueden crear médicos desde aquí.
                    // int resultado = _adminBO.InsertarUsuarioGeneral(nuevoUsuario, rolId);
                    MostrarMensaje("Funcionalidad para crear usuarios no médicos pendiente de implementación en backend.", false);
                }

                // Si el guardado fue exitoso, recargar la grilla
                CargarRolesMaestros(); // Recargar todo por si hay nuevos datos
                BindUsuariosGrid();

                // Cerrar el modal desde el servidor
                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAddModal", "$('#modalAgregarUsuario').modal('hide');", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar el nuevo usuario: {ex.Message}", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpenOnError", "$('#modalAgregarUsuario').modal('show');", true);
            }
        }

    }
}