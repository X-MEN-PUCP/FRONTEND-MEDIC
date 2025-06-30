using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftBO;
using SoftBO.registroWS;

namespace SoftWA
{
    public partial class verificar_correo : System.Web.UI.Page
    {
        private string CorreoVerificacion => Session["CorreoVerificacion"] as string;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(CorreoVerificacion))
            {
                Response.Redirect("registro_cuenta_nueva.aspx", true);
                return;
            }
            if (!IsPostBack)
            { 
                ltlCorreoUsuario.Text = Server.HtmlEncode(CorreoVerificacion);
            }
        }
        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string codigoIngresado = txtCodigo.Text.Trim();
            if(codigoIngresado.Length != 6)
            {
                MostrarMensaje("El código debe contener 6 dígitos.", esExito: false);
                return;
            }
            try
            {
                var servicioRegistro = new RegistroBO();
                bool resultado = servicioRegistro.VerificarCodigo(CorreoVerificacion,codigoIngresado);
                if (resultado)
                {
                    Session.Remove("CorreoVerificacion");
                    Response.Redirect("indexLogin.aspx?reg=verified", false);
                }
                else
                {
                    MostrarMensaje("El código ingresado es incorrecto. Por favor, inténtelo de nuevo.", esExito: false);
                }
            }catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al verificar el código. Por favor, inténtelo de nuevo más tarde.", esExito: false);
                System.Diagnostics.Debug.WriteLine($"Error al verificar código: {ex}");
            }
        }
        protected void lbtnReenviar_Click(object sender, EventArgs e)
        {
            try
            {
                var servicioRegistro = new RegistroBO();
                bool resultado = servicioRegistro.ReenviarCodigo(CorreoVerificacion);
                if (resultado)
                {
                    MostrarMensaje("El código de verificación ha sido reenviado a su correo electrónico.", esExito: true);
                }
                else
                {
                    MostrarMensaje("No se pudo reenviar el código de verificación. Por favor, inténtelo de nuevo más tarde.", esExito: false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al reenviar el código. Por favor, inténtelo de nuevo más tarde.", esExito: false);
                System.Diagnostics.Debug.WriteLine($"Error al reenviar código: {ex}");
            }
        }
        private void MostrarMensaje(string mensaje, bool esExito)
        {
            string cssClass = esExito ? "alert alert-success" : "alert alert-danger";
            ltlMensaje.Text = $"<div class='{cssClass} mt-3'>{Server.HtmlEncode(mensaje)}</div>";
        }
    }
}