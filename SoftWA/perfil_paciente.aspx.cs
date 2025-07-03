using SoftBO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA.MA_Paciente
{
    public partial class perfil_paciente : System.Web.UI.Page
    {
        private readonly UsuarioBO _usuarioBO;
        public perfil_paciente()
        {
            _usuarioBO = new UsuarioBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int usuarioActualId = ObtenerUsuarioActualId();

                if (usuarioActualId > 0)
                {
                    ViewState["UsuarioId"] = usuarioActualId;
                    CargarDatosPerfil(usuarioActualId);
                }
                else
                {
                    MostrarMensaje("No se pudo identificar al usuario.", true);
                    pnlViewProfile.Visible = false;
                    Response.Redirect("~/indexLogin.aspx");
                }

                pnlEditProfile.Visible = false;
                pnlMensaje.Visible = false;
            }
        }

        #region --- Lógica de Carga y Visualización de Datos ---
        private void CargarDatosPerfil(int usuarioActualId)
        {
            try
            {
                SoftBO.SoftCitWS.usuarioDTO perfil = _usuarioBO.ObtenerPorIdUsuario(usuarioActualId);
                if(perfil != null)
                {
                    PopulateViewControls(perfil);
                    ViewState["PerfilCompleto"] = perfil;
                }
                else
                {
                    MostrarMensaje("No se pudo cargar el perfil del usuario.", true);
                    pnlViewProfile.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar el perfil. Por favor, inténtelo más tarde.", true);
                pnlViewProfile.Visible = false;
                System.Diagnostics.Debug.WriteLine($"Error al cargar perfil: {ex.Message}");
            }
        }
        private void PopulateViewControls(SoftBO.SoftCitWS.usuarioDTO perfil)
        {
            lblNombresView.Text = Server.HtmlEncode(CapitalizarNombre(perfil.nombres));
            lblApellidoPaternoView.Text = Server.HtmlEncode(CapitalizarNombre(perfil.apellidoPaterno));
            lblApellidoMaternoView.Text = Server.HtmlEncode(CapitalizarNombre(perfil.apellidoMaterno));
            if(DateTime.TryParse(perfil.fechaNacimiento, out DateTime fechaNac))
            {
                lblFechaNacimientoView.Text = fechaNac.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            }
            else
            {
                lblFechaNacimientoView.Text = "No especificada";
            }
            lblCorreoView.Text = Server.HtmlEncode(perfil.correoElectronico);
            lblCelularView.Text = Server.HtmlEncode(string.IsNullOrEmpty(perfil.numCelular) ? "No especificado" : perfil.numCelular);
            lblGeneroView.Text = Server.HtmlEncode(ObtenerNombreGenero(perfil.genero.ToString()));
        }
        private void PopulateEditControls(SoftBO.SoftCitWS.usuarioDTO perfil)
        {
            txtNombresEdit.Text = CapitalizarNombre(perfil.nombres);
            txtApellidoPaternoEdit.Text = CapitalizarNombre(perfil.apellidoPaterno);
            txtApellidoMaternoEdit.Text = CapitalizarNombre(perfil.apellidoMaterno);
            if (DateTime.TryParse(perfil.fechaNacimiento, out DateTime fechaNac))
            {
                txtFechaNacimientoEdit.Text = fechaNac.ToString("yyyy-MM-dd");
            }
            else
            {
                txtFechaNacimientoEdit.Text = string.Empty;
            }
            txtCorreoEdit.Text = perfil.correoElectronico;
            txtCelularEdit.Text = perfil.numCelular ?? string.Empty;
            string generoValue = perfil.genero.ToString().ToUpperInvariant();
            if(ddlGeneroEdit.Items.FindByValue(generoValue) != null)
            {
                ddlGeneroEdit.SelectedValue = generoValue;
            }
            ddlGeneroEdit.DataBind();
        }
        #endregion

        #region --- Lógica de Cambio de Modo y Guardado ---
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var perfil = ViewState["PerfilCompleto"] as SoftBO.SoftCitWS.usuarioDTO;
            if (perfil != null)
            {
                PopulateEditControls(perfil);
                SwitchMode(true);
            }
            else
            {
                MostrarMensaje("No se pudo cargar el perfil para editar.", true);
            }
        }
        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            SwitchMode(false); 
            pnlMensaje.Visible = false; 
        }
        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            Page.Validate("EditProfile");
            if (!Page.IsValid) return;
            var perfil = ViewState["PerfilCompleto"] as SoftBO.SoftCitWS.usuarioDTO;
            if (perfil == null)
            {
                MostrarMensaje("No se pudo cargar el perfil para guardar los cambios.", true);
                return;
            }
            var perfilActualizado = Session["UsuarioCompleto"] as SoftBO.SoftCitWS.usuarioDTO;
            perfil.nombres = txtNombresEdit.Text.Trim();
            perfil.apellidoPaterno = txtApellidoPaternoEdit.Text.Trim();
            perfil.apellidoMaterno = txtApellidoMaternoEdit.Text.Trim();
            perfil.correoElectronico = txtCorreoEdit.Text.Trim();
            perfil.numCelular = txtCelularEdit.Text.Trim();
            perfil.genero = perfilActualizado.genero;

            if (DateTime.TryParse(txtFechaNacimientoEdit.Text, out DateTime fechaNac))
            {
                perfil.fechaNacimiento = fechaNac.ToString("yyyy-MM-dd");
            }
            perfil.usuarioModificacion = perfilActualizado.idUsuario;
            perfil.usuarioModificacionSpecified = true;
            perfil.fechaModificacion = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                int resultado = _usuarioBO.ModificarUsuario(perfil);
                if (resultado > 0) 
                {
                    MostrarMensaje("Perfil actualizado correctamente.", false);
                    ViewState["PerfilCompleto"] = perfil; 
                    PopulateViewControls(perfil);
                    SwitchMode(false);
                }
                else
                {
                    MostrarMensaje("No se pudo guardar la información del perfil. Es posible que otro usuario haya modificado los datos.", true);
                }

            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error inesperado al guardar el perfil.", true);
                System.Diagnostics.Debug.WriteLine("Error al guardar perfil: " + ex.Message);
            }
        }
        #endregion

        #region --- Métodos Auxiliares ---
        private string CapitalizarNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return "";
            }
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(texto.ToLower());
        }
        private int ObtenerUsuarioActualId()
        {
            var usuario = Session["UsuarioCompleto"] as SoftBO.SoftCitWS.usuarioDTO;
            return usuario?.idUsuario ?? 0;
        }
        private void SwitchMode(bool editMode)
        {
            pnlViewProfile.Visible = !editMode;
            pnlEditProfile.Visible = editMode;
        }
        private void MostrarMensaje(string mensaje, bool esError)
        {
            lblMensaje.Text = mensaje;
            pnlMensaje.CssClass = esError ? "alert alert-danger alert-dismissible fade show mb-3" : "alert alert-success alert-dismissible fade show mb-3";
            pnlMensaje.Visible = true;
        }
        private string ObtenerNombreGenero(string generoChar)
        {
            switch (generoChar?.ToUpper())
            {
                case "MASCULINO": return "Masculino";
                case "FEMENINO": return "Femenino";
                case "OTRO": return "Otro";
                default: return "No especificado";
            }
        }
        #endregion
    }
}