using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA.MA_Paciente
{
    #region --- Estructura de Datos Perfil ---
    public class UsuarioPerfilInfo
    {
        public int UsuarioId { get; set; } // Identificador del usuario
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; } // Nullable por si no es obligatoria
        public string CorreoElectronico { get; set; }
        public string NumCelular { get; set; }
        public string Genero { get; set; } // Podría ser char 'M', 'F', 'O', 'N' o string
        public string NombreGeneroCompleto // Propiedad calculada para mostrar
        {
            get
            {
                switch (Genero?.ToUpper())
                {
                    case "M": return "Masculino";
                    case "F": return "Femenino";
                    case "O": return "Otro";
                    case "N": return "Prefiero no decir";
                    default: return "No especificado";
                }
            }
        }
    }
    #endregion

    public partial class perfil_paciente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int usuarioActualId = ObtenerUsuarioActualId_Simulado();

                if (usuarioActualId > 0)
                {
                    UsuarioPerfilInfo perfil = CargarPerfilUsuario_LlamadaBackend(usuarioActualId);

                    if (perfil != null)
                    {
                        PopulateViewControls(perfil);
                        ViewState["UsuarioId"] = usuarioActualId;
                    }
                    else
                    {
                        MostrarMensaje("No se pudo cargar la información del perfil.", true);
                        pnlViewProfile.Visible = false;
                    }
                }
                else
                {
                    MostrarMensaje("No se pudo identificar al usuario.", true);
                    Response.Redirect("~/indexLogin.aspx");
                    pnlViewProfile.Visible = false;
                }

                pnlEditProfile.Visible = false;
                pnlMensaje.Visible = false;
            }
        }

        #region --- Lógica de Cambio de Modo y Población de Controles ---

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int usuarioActualId = (int)(ViewState["UsuarioId"] ?? 0);
            if (usuarioActualId > 0)
            {
                UsuarioPerfilInfo perfil = CargarPerfilUsuario_LlamadaBackend(usuarioActualId);
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
            else
            {
                MostrarMensaje("No se pudo identificar al usuario para editar.", true);
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            SwitchMode(false); 
            pnlMensaje.Visible = false; 
        }

        private void SwitchMode(bool editMode)
        {
            pnlViewProfile.Visible = !editMode;
            pnlEditProfile.Visible = editMode;
        }

        private void PopulateViewControls(UsuarioPerfilInfo perfil)
        {
            lblNombresView.Text = Server.HtmlEncode(perfil.Nombres);
            lblApellidoPaternoView.Text = Server.HtmlEncode(perfil.ApellidoPaterno);
            lblApellidoMaternoView.Text = Server.HtmlEncode(perfil.ApellidoMaterno);
            lblFechaNacimientoView.Text = perfil.FechaNacimiento?.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES")) ?? "No especificada";
            lblCorreoView.Text = Server.HtmlEncode(perfil.CorreoElectronico);
            lblCelularView.Text = Server.HtmlEncode(perfil.NumCelular) ?? "No especificado";
            lblGeneroView.Text = perfil.NombreGeneroCompleto;
        }

        private void PopulateEditControls(UsuarioPerfilInfo perfil)
        {
            txtNombresEdit.Text = perfil.Nombres;
            txtApellidoPaternoEdit.Text = perfil.ApellidoPaterno;
            txtApellidoMaternoEdit.Text = perfil.ApellidoMaterno;
            txtFechaNacimientoEdit.Text = perfil.FechaNacimiento?.ToString("yyyy-MM-dd") ?? "";
            txtCorreoEdit.Text = perfil.CorreoElectronico;
            txtCelularEdit.Text = perfil.NumCelular;
            ddlGeneroEdit.ClearSelection();
            ListItem itemGenero = ddlGeneroEdit.Items.FindByValue(perfil.Genero?.ToUpper());
            if (itemGenero != null)
            {
                itemGenero.Selected = true;
            }
            else if (ddlGeneroEdit.Items.FindByValue("") != null)
            {
                ddlGeneroEdit.SelectedValue = ""; 
            }
        }

        #endregion

        #region --- Lógica de Guardado ---

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            int usuarioId = (int)(ViewState["UsuarioId"] ?? 0);
            if (usuarioId == 0)
            {
                MostrarMensaje("No se puede guardar, no se identifica al usuario.", true);
                return;
            }

            UsuarioPerfilInfo perfilActualizado = new UsuarioPerfilInfo();
            perfilActualizado.UsuarioId = usuarioId;
            perfilActualizado.Nombres = txtNombresEdit.Text.Trim();
            perfilActualizado.ApellidoPaterno = txtApellidoPaternoEdit.Text.Trim();
            perfilActualizado.ApellidoMaterno = txtApellidoMaternoEdit.Text.Trim();
            perfilActualizado.CorreoElectronico = txtCorreoEdit.Text.Trim();
            perfilActualizado.NumCelular = txtCelularEdit.Text.Trim();
            perfilActualizado.Genero = ddlGeneroEdit.SelectedValue;

            if (DateTime.TryParse(txtFechaNacimientoEdit.Text, out DateTime fechaNac))
            {
                perfilActualizado.FechaNacimiento = fechaNac;
            }
            else
            {
                perfilActualizado.FechaNacimiento = null;
            }

            if (string.IsNullOrWhiteSpace(perfilActualizado.Nombres) || string.IsNullOrWhiteSpace(perfilActualizado.CorreoElectronico))
            {
                MostrarMensaje("Los nombres y el correo electrónico son obligatorios.", true);
            }

            try
            {
                bool exito_simulado = GuardarPerfilUsuario_LlamadaBackend(perfilActualizado);

                if (exito_simulado)
                {
                    MostrarMensaje("Perfil actualizado correctamente.", false);
                    PopulateViewControls(perfilActualizado);
                    SwitchMode(false);
                }
                else
                {
                    MostrarMensaje("No se pudo guardar la información del perfil.", true);
                    SwitchMode(true);
                }
            }
            catch (Exception)
            {
                MostrarMensaje("Ocurrió un error inesperado al guardar el perfil.", true);
                SwitchMode(true);
            }
        }

        #endregion

        #region --- Funciones Simuladas Backend / Lógica ---

        private int ObtenerUsuarioActualId_Simulado()
        {
            return 1;
        }

        private UsuarioPerfilInfo CargarPerfilUsuario_LlamadaBackend(int usuarioId)
        {
            System.Diagnostics.Debug.WriteLine($"Simulando carga de perfil para Usuario ID: {usuarioId}");
            if (usuarioId == 1)
            {
                return new UsuarioPerfilInfo
                {
                    UsuarioId = 1,
                    Nombres = "Ana María",
                    ApellidoPaterno = "García",
                    ApellidoMaterno = "López",
                    FechaNacimiento = new DateTime(1990, 5, 15),
                    CorreoElectronico = "ana.garcia@email.com",
                    NumCelular = "987654321",
                    Genero = "F"
                };
            }
            return null; 
        }

        private bool GuardarPerfilUsuario_LlamadaBackend(UsuarioPerfilInfo perfil)
        {

            if (perfil.CorreoElectronico.Contains("error"))
            {
                return false;
            }
            return true; 
        }

        #endregion

        #region --- Métodos Auxiliares ---
        private void MostrarMensaje(string mensaje, bool esError)
        {
            lblMensaje.Text = mensaje;
            pnlMensaje.CssClass = esError ? "alert alert-danger alert-dismissible fade show mb-3" : "alert alert-success alert-dismissible fade show mb-3";
            pnlMensaje.Visible = true;
        }
        #endregion
    }
}