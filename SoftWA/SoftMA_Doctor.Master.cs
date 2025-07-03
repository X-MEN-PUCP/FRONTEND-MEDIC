using System;
using SoftBO.SoftCitWS;

namespace SoftWA
{
    public partial class SoftMA_Doctor : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioCompleto"] as usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/indexLogin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarNombreDeUsuario(usuario);
            }
        }
        private void CargarNombreDeUsuario(usuarioDTO usuario)
        {
            string nombreParaMostrar = "Usuario";

            if (usuario != null && !string.IsNullOrEmpty(usuario.nombres) && !string.IsNullOrEmpty(usuario.apellidoPaterno))
            {
                string primerNombre = usuario.nombres.Split(' ')[0];
                string inicialApellido = usuario.apellidoPaterno.Substring(0, 1).ToUpper();
                nombreParaMostrar = $"{primerNombre} {inicialApellido}.";
            }
            ltlNombreUsuario.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(nombreParaMostrar, false);
        }
        protected void lbtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/indexLogin.aspx");
        }
    }
}