using SoftBO.loginWS;
using SoftBO.usuarioWS;
using System;
using System.Web.UI;

namespace SoftWA
{
    public partial class doctor_cambiar_contraseña : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltlMensajeError.Text = "";
            }
        }
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            var usuarioLogueado = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            if (usuarioLogueado == null || usuarioLogueado.idUsuario == 0)
            {
                MostrarMensaje("Error: No se pudo identificar al usuario. Por favor, inicie sesión nuevamente.", false);
                return;
            }
            string contraseñaActual = txtCurrentPassword.Text;
            string nuevaContraseña = txtNewPassword.Text;

            try
            {
                var usuarioVerificar = new SoftBO.usuarioWS.usuarioDTO
                {
                    idUsuario = usuarioLogueado.idUsuario,
                    idUsuarioSpecified = true,
                    contrasenha = contraseñaActual
                };

                int resultado;
                using (var usuarioService = new SoftBO.usuarioWS.UsuarioWSClient())
                {
                    resultado = usuarioService.CambiarContrasenhaUsuario(usuarioVerificar, nuevaContraseña);
                }
                if (resultado > 0)
                {
                    MostrarMensaje("Contraseña modificada correctamente.", true);
                    txtCurrentPassword.Text = string.Empty;
                    txtNewPassword.Text = string.Empty;
                    txtConfirmNewPassword.Text = string.Empty;
                }
                else if (resultado == -1)
                {
                    MostrarMensaje("La contraseña actual es incorrecta. Por favor, intente nuevamente.", false);
                }
                else
                {
                    MostrarMensaje("Error al modificar la contraseña. Por favor, intente nuevamente.", false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en btnChangePassword_Click: " + ex.Message);
                MostrarMensaje("Ocurrió un error inesperado. Por favor, contacte al administrador.", false);
            }
        }
        private void MostrarMensaje(string mensaje, bool esExito)
        {
            string cssClass = esExito ? "alert alert-success" : "alert alert-danger";
            ltlMensajeError.Text = $"<div class='{cssClass} mt-3' role='alert'>{Server.HtmlEncode(mensaje)}</div>";
        }
    }
}