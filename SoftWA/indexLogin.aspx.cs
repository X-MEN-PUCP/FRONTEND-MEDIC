using SoftBO.rolesporusuarioWS;
using SoftBO.loginWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftBO;

namespace SoftWA
{
    public partial class indexLogin : System.Web.UI.Page
    {
        private readonly LoginBO _loginBO;
        private readonly RolesPorUsuarioBO _rolesBO;
        public indexLogin()
        {
            _loginBO = new LoginBO();
            _rolesBO = new RolesPorUsuarioBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltlMensajeError.Text = string.Empty;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            
            string numeroDocumento = txtDNI.Text.Trim();
            string contrasenha = txtPassword.Text;
            string tipoDocumento = hdnLoginDocType.Value;
            SoftBO.loginWS.usuarioDTO usuarioAutenticadoLogin = null;
            try
            {
                usuarioAutenticadoLogin = _loginBO.IniciarSesion(numeroDocumento, tipoDocumento, contrasenha);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error en el servicio de autenticación.");
                System.Diagnostics.Debug.WriteLine($"Error General en Login: {ex.ToString()}");
                return;
            }

            if (usuarioAutenticadoLogin != null && usuarioAutenticadoLogin.idUsuario > 0)
            {
                IEnumerable<usuarioPorRolDTO> listaRoles = null;
                try
                {
                    listaRoles = _rolesBO.ListarPorUsuarioRolesPorUsuario(usuarioAutenticadoLogin.idUsuario);
                }
                catch (Exception ex)
                {
                    MostrarError("Error al obtener los roles del usuario.");
                    System.Diagnostics.Debug.WriteLine($"Error en CompletarRolesUsuario: {ex.ToString()}");
                    return;
                }

                var rolPrincipal = ObtenerRolPrincipal(listaRoles);

                if (rolPrincipal == null)
                {
                    MostrarError("Usuario autenticado pero sin un rol válido asignado. Contacte al administrador.");
                    return;
                }

                Session["UsuarioCompleto"] = usuarioAutenticadoLogin;
                Session["ListaRolesUsuario"] = listaRoles.ToList();
                Session["RolActual"] = rolPrincipal.rol;

                string nombreCompleto = $"{usuarioAutenticadoLogin.nombres} {usuarioAutenticadoLogin.apellidoPaterno} {usuarioAutenticadoLogin.apellidoMaterno}".Trim();
                Session["UsuarioLogueado_NombreCompleto"] = nombreCompleto;
                string targetUrl = ObtenerUrlRedireccion(rolPrincipal);
                if (string.IsNullOrEmpty(targetUrl))
                {
                    MostrarError("Su rol no tiene una página de inicio configurada. Contacte al administrador.");
                    return;
                }

                Response.Redirect(targetUrl, false);
            }
            else
            {
                MostrarError("Usuario o contraseña incorrectos. Por favor, intente de nuevo.");
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
            }
        }
        private usuarioPorRolDTO ObtenerRolPrincipal(IEnumerable<usuarioPorRolDTO> roles)
        {
            if (roles == null || !roles.Any())
            {
                return null;
            }
            return roles.OrderBy(r => r.rol.idRol).FirstOrDefault();
        }

        private string ObtenerUrlRedireccion(usuarioPorRolDTO rolPrincipal)
        {
            if (rolPrincipal?.rol == null) return null;

            switch (rolPrincipal.rol.idRol)
            {
                case 1: return "~/indexAdmin.aspx";
                case 2: return "~/indexMedico.aspx";
                case 3: return "~/indexPaciente.aspx";
                default: return null;
            }
        }

        private void MostrarError(string mensaje)
        {
            ltlMensajeError.Text = $"<div class='alert alert-danger alert-dismissible fade show' role='alert'>{HttpUtility.HtmlEncode(mensaje)}<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
        }
    }
}