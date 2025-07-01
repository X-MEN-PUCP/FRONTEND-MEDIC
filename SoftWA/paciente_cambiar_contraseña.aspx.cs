using SoftBO;
using SoftBO.loginWS;
using SoftBO.usuarioWS;
using System;
using System.Linq;
using System.Web.UI;

namespace SoftWA
{
    public partial class paciente_cambiar_contraseña : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
            string confirmacionContraseña = txtConfirmNewPassword.Text;
            
            if (!EsValido(contraseñaActual, nuevaContraseña, confirmacionContraseña)) return;
            if (!EsContrasenaSegura(nuevaContraseña))
            {
                MostrarMensaje("La nueva contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y símbolos.", false);
                return;
            }
            try
            {
                var usuarioCompleto = new UsuarioBO().ObtenerPorIdUsuario(usuarioLogueado.idUsuario);
                
                int resultado;
                var usuarioService = new UsuarioBO();
                resultado = usuarioService.CambiarContraseñaUsuario(usuarioCompleto,nuevaContraseña);
                
                if(resultado > 0)
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
        private bool EsValido(string contraseñaActual, string nuevaContraseña, string confirmacionContraseña)
        {
            if (string.IsNullOrWhiteSpace(contraseñaActual) || string.IsNullOrWhiteSpace(nuevaContraseña) || string.IsNullOrWhiteSpace(confirmacionContraseña))
            {
                MostrarMensaje("Por favor, complete todos los campos requeridos.", false);
                return false;
            }
            if (nuevaContraseña != confirmacionContraseña)
            {
                MostrarMensaje("La nueva contraseña y su confirmación no coinciden. Por favor, inténtelo de nuevo.", false);
                return false;
            }
            if (contraseñaActual == nuevaContraseña)
            {
                MostrarMensaje("La nueva contraseña no puede ser la misma que la actual. Por favor, elija una contraseña diferente.", false);
                return false;
            }

            return true;
        }
        private bool EsContrasenaSegura(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8) return false;

            bool tieneMayuscula = password.Any(char.IsUpper);
            bool tieneMinuscula = password.Any(char.IsLower);
            bool tieneNumero = password.Any(char.IsDigit);
            bool tieneSimbolo = password.Any(c => !char.IsLetterOrDigit(c));

            return tieneMayuscula && tieneMinuscula && tieneNumero && tieneSimbolo;
        }
        private void MostrarMensaje(string mensaje, bool esExito)
        {
            string cssClass = esExito ? "alert alert-success" : "alert alert-danger";
            ltlMensajeError.Text = $"<div class='{cssClass} mt-3' role='alert'>{Server.HtmlEncode(mensaje)}</div>";
        }
    }
}