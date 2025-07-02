using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftBO;
using SoftBO.usuarioWS;
using SoftBO.usuarioporespecialidadWS;

namespace SoftWA.MA_Medico
{
    public partial class perfil_medico : System.Web.UI.Page
    {
        private readonly UsuarioBO _usuarioBO;
        //private readonly UsuarioPorEspecialidadBO _usuarioPorEspecialidadBO;

        public perfil_medico()
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
                    ViewState["MedicoId"] = usuarioActualId;
                    CargarDatosPerfil(usuarioActualId);
                }
                else
                {
                    MostrarMensaje("No se pudo identificar al médico. Por favor, inicie sesión nuevamente.", true);
                    pnlViewProfileMed.Visible = false;
                    Response.Redirect("~/indexLogin.aspx"); 
                }

                pnlEditProfileMed.Visible = false;
                pnlMensaje.Visible = false;
            }
        }

        #region --- Lógica de Carga y Visualización de Datos ---

        private void CargarDatosPerfil(int medicoId)
        {
            try
            {
                SoftBO.usuarioWS.usuarioDTO perfil = _usuarioBO.ObtenerPorIdUsuario(medicoId);
                if (perfil != null)
                {
                    PopulateViewControls(perfil);

                    ViewState["PerfilCompleto"] = perfil;
                }
                else
                {
                    MostrarMensaje("No se pudo cargar la información del perfil.", true);
                    pnlViewProfileMed.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error de conexión al cargar el perfil. Inténtelo más tarde.", true);
                System.Diagnostics.Debug.WriteLine($"Error en CargarDatosPerfil: {ex}");
                pnlViewProfileMed.Visible = false;
            }
        }

        private void PopulateViewControls(SoftBO.usuarioWS.usuarioDTO perfilMed)
        {
            lblNombresMedView.Text = Server.HtmlEncode(perfilMed.nombres);
            lblApellidoPaternoMedView.Text = Server.HtmlEncode(perfilMed.apellidoPaterno);
            lblApellidoMaternoMedView.Text = Server.HtmlEncode(perfilMed.apellidoMaterno);

            if (DateTime.TryParse(perfilMed.fechaNacimiento, out DateTime fechaNac))
            {
                lblFechaNacimientoMedView.Text = fechaNac.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            }
            else
            {
                lblFechaNacimientoMedView.Text = "No especificada";
            }

            lblCorreoMedView.Text = Server.HtmlEncode(perfilMed.correoElectronico);
            lblCelularMedView.Text = Server.HtmlEncode(string.IsNullOrEmpty(perfilMed.numCelular) ? "No especificado" : perfilMed.numCelular);
            lblCodigocmpMed.Text = Server.HtmlEncode(string.IsNullOrEmpty(perfilMed.codMedico) ? "No asignado" : perfilMed.codMedico);
            lblGeneroMedView.Text = Server.HtmlEncode(ObtenerNombreGenero(perfilMed.genero.ToString()));

            try
            {
                var especialidades = _usuarioPorEspecialidadBO.ListarPorUsuarioUsuarioPorEspecialidad(perfilMed.idUsuario);
                if (especialidades != null && especialidades.Any())
                {
                    lblEspecialidadMed.Text = Server.HtmlEncode(especialidades.First().especialidad.nombreEspecialidad);
                }
                else
                {
                    lblEspecialidadMed.Text = "No asignada";
                }
            }
            catch (Exception ex)
            {
                lblEspecialidadMed.Text = "Error al cargar";
                System.Diagnostics.Debug.WriteLine($"Error cargando especialidad: {ex}");
            }
        }

        private void PopulateEditControls(SoftBO.usuarioWS.usuarioDTO perfil)
        {
            txtNombresEdit.Text = perfil.nombres;
            txtApellidoPaternoEdit.Text = perfil.apellidoPaterno;
            txtApellidoMaternoEdit.Text = perfil.apellidoMaterno;

            if (DateTime.TryParse(perfil.fechaNacimiento, out DateTime fechaNac))
            {
                txtFechaNacimientoEdit.Text = fechaNac.ToString("yyyy-MM-dd");
            }

            txtCorreoEdit.Text = perfil.correoElectronico;
            txtCelularEdit.Text = perfil.numCelular;
            txtGeneroEdit.Text = perfil.genero.ToString();



            txtEspecialidadEdit.Text = lblEspecialidadMed.Text;
            txtCodigocmpEdit.Text = perfil.codMedico;
        }

        #endregion

        #region --- Lógica de Cambio de Modo y Guardado ---

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var perfil = ViewState["PerfilCompleto"] as SoftBO.usuarioWS.usuarioDTO;
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
            if (!Page.IsValid)
            {
                return;
            }

            var perfilParaActualizar = ViewState["PerfilCompleto"] as SoftBO.usuarioWS.usuarioDTO;
            if (perfilParaActualizar == null)
            {
                MostrarMensaje("La sesión de edición ha expirado. Por favor, intente de nuevo.", true);
                SwitchMode(false);
                return;
            }
            var usuarioLogueado = Session["UsuarioCompleto"] as SoftBO.usuarioWS.usuarioDTO;
            perfilParaActualizar.nombres = txtNombresEdit.Text.Trim();
            perfilParaActualizar.apellidoPaterno = txtApellidoPaternoEdit.Text.Trim();
            perfilParaActualizar.apellidoMaterno = txtApellidoMaternoEdit.Text.Trim();
            perfilParaActualizar.correoElectronico = txtCorreoEdit.Text.Trim();
            perfilParaActualizar.numCelular = txtCelularEdit.Text.Trim();
            perfilParaActualizar.genero = usuarioLogueado.genero;
             

            if (DateTime.TryParse(txtFechaNacimientoEdit.Text, out DateTime fechaNac))
            {
                perfilParaActualizar.fechaNacimiento = fechaNac.ToString("yyyy-MM-dd");
            }

            perfilParaActualizar.usuarioModificacion = usuarioLogueado.idUsuario;
            perfilParaActualizar.usuarioModificacionSpecified = true;
            perfilParaActualizar.fechaModificacion = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                int resultado = _usuarioBO.ModificarUsuario(perfilParaActualizar);

                if (resultado > 0)
                {
                    MostrarMensaje("Perfil actualizado correctamente.", false);
                    ViewState["PerfilCompleto"] = perfilParaActualizar; 
                    PopulateViewControls(perfilParaActualizar);
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
                System.Diagnostics.Debug.WriteLine($"Error en btnSaveChanges_Click: {ex}");
            }
        }

        #endregion

        #region --- Métodos Auxiliares ---

        private int ObtenerUsuarioActualId()
        {
            var usuario = Session["UsuarioCompleto"] as SoftBO.loginWS.usuarioDTO;
            return usuario?.idUsuario ?? 0;
        }

        private void SwitchMode(bool editMode)
        {
            pnlViewProfileMed.Visible = !editMode;
            pnlEditProfileMed.Visible = editMode;
        }

        private void MostrarMensaje(string mensaje, bool esError)
        {
            lblMensaje.Text = mensaje;
            pnlMensaje.CssClass = esError ? "alert alert-danger" : "alert alert-success";
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