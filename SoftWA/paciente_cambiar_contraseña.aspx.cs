using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class paciente_cambiar_contraseña : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                MostrarMensaje("Por favor, corrija los errores indicados.", false);
                return;
            }

            int pacienteId;
            if (Session["PacienteID"] == null || !int.TryParse(Session["PacienteID"].ToString(), out pacienteId))
            {
                MostrarMensaje("Error: No se pudo identificar al usuario. Por favor, inicie sesión nuevamente.", false);
                return;
            }

            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            
            try
            {
                string storedPasswordHash = ObtenerHashContraseñaActual(pacienteId);

                if (string.IsNullOrEmpty(storedPasswordHash))
                {
                    MostrarMensaje("Error: No se pudo recuperar la información del usuario.", false);
                    return;
                }

                if (!VerificarContraseña(currentPassword, storedPasswordHash))
                {
                    MostrarMensaje("La contraseña actual ingresada es incorrecta.", false);
                    return;
                }

                string newPasswordHash = HashearContraseña(newPassword);

                if (ActualizarContraseñaUsuario(pacienteId, newPasswordHash))
                {
                    MostrarMensaje("¡Contraseña actualizada exitosamente!", true);
                    txtCurrentPassword.Text = string.Empty;
                    txtNewPassword.Text = string.Empty;
                    txtConfirmNewPassword.Text = string.Empty;
                }
                else
                {
                    MostrarMensaje("Error al actualizar la contraseña. Inténtelo de nuevo más tarde.", false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en btnChangePassword_Click: " + ex.Message);
                MostrarMensaje("Ocurrió un error inesperado. Por favor, contacte al administrador.", false);
            }
        }

        private string ObtenerHashContraseñaActual(int pacienteId)
        {
            if (pacienteId == 1)
            {
                return HashearContraseña("password123");
            }
            return "hash_simulado_de_la_bd_para_otro_usuario";
        }

        private bool ActualizarContraseñaUsuario(int pacienteId, string nuevoHashContraseña)
        {
            System.Diagnostics.Debug.WriteLine($"Actualizando contraseña para PacienteID: {pacienteId} con Hash: {nuevoHashContraseña}");
            return true;
        }

        private string HashearContraseña(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerificarContraseña(string contraseñaIngresada, string hashAlmacenado)
        {
            string hashDeContraseñaIngresada = HashearContraseña(contraseñaIngresada);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Equals(hashDeContraseñaIngresada, hashAlmacenado);
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            string cssClass = esExito ? "alert alert-success" : "alert alert-danger";
            ltlMensajeError.Text = $"<div class='{cssClass} mt-3' role='alert'>{Server.HtmlEncode(mensaje)}</div>";
        }
    }
}