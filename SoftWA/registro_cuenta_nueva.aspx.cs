using Newtonsoft.Json;
using SoftBO.registroWS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using SoftBO;

namespace SoftWA
{
    public class FactilizaResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonProperty("data")]
        public PersonaData Data { get; set; }
    }
    public class PersonaData
    {
        [JsonProperty("numero")]
        public string Numero { get; set; }
        [JsonProperty("nombre_completo")]
        public string NombreCompleto { get; set; }
        [JsonProperty("nombres")]
        public string Nombres { get; set; }
        [JsonProperty("apellido_paterno")]
        public string ApellidoPaterno { get; set; }
        [JsonProperty("apellido_materno")]
        public string ApellidoMaterno { get; set; }
    }
    public partial class registro_cuenta_nueva : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();
        private const string FactilizaToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzODk1OSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImNvbnN1bHRvciJ9.XrDaAeSivMeulRvfQf3ee4n1owZ9I7I8YEXQqx01vKc";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnValidarDocumento_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(ValidarDocumento));
        }
        private async Task ValidarDocumento()
        {
            string tipoDoc = hdnSelectedDocumentType.Value;
            string numeroDoc = (tipoDoc == "DNI") ? txtDNI.Text.Trim() : txtCE.Text.Trim();

            if (string.IsNullOrEmpty(numeroDoc))
            {
                MostrarMensaje("Por favor, ingrese un número de documento.", esExito: false);
                return;
            }

            string apiEndpoint = (tipoDoc == "DNI") ? "dni" : "cee";
            string apiUrl = $"https://api.factiliza.com/v1/{apiEndpoint}/info/{numeroDoc}";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", FactilizaToken);

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<FactilizaResponse>(jsonResponse);

                    if (apiResponse != null && apiResponse.Success && apiResponse.Data != null)
                    {
                        txtNombres.Text = apiResponse.Data.Nombres;
                        txtApellidoPaterno.Text = apiResponse.Data.ApellidoPaterno;
                        txtApellidoMaterno.Text = apiResponse.Data.ApellidoMaterno;

                        pnlBusquedaDocumento.Enabled = false;
                        pnlDatosRegistro.Visible = true;
                        MostrarMensaje("Documento validado correctamente. Por favor, complete su registro.", esExito: true);
                    }
                    else
                    {
                        MostrarMensaje(apiResponse?.Message ?? "El documento no pudo ser validado o no existe.", esExito: false);
                    }
                }
                else
                {
                    MostrarMensaje("Error de comunicación con el servicio de validación de documentos.", esExito: false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error inesperado al validar el documento.", esExito: false);
                System.Diagnostics.Debug.WriteLine($"Error API Factiliza: {ex}");
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Page.Validate("RegisterValidation");
            if (!Page.IsValid)
            {
                MostrarMensaje("Por favor, corrija los errores indicados.", esExito: false);
                return;
            }
            string correo = txtCorreo.Text.Trim();
            if (!EsDominioDeCorreoValido(correo))
            {
                MostrarMensaje("El dominio del correo electrónico no es válido. Por favor, use un proveedor conocido (ej: Gmail, Outlook, etc.).", esExito: false);
                return;
            }
            DateTime fechaNac;
            if (DateTime.TryParse(txtFechaNacimiento.Text, out fechaNac))
            {
                if (!EsMayorDeEdad(fechaNac))
                {
                    MostrarMensaje("Debe ser mayor de 18 años para registrarse.", esExito: false);
                    return;
                }
            }
            else
            {
                MostrarMensaje("El formato de la fecha de nacimiento no es válido.", esExito: false);
                return;
            }

            string password = txtPassword.Text;
            if (!EsContrasenaSegura(password))
            {
                MostrarMensaje("La contraseña no es segura. Debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un símbolo.", esExito: false);
                return;
            }
            try
            {
                string tipoDocumento = hdnSelectedDocumentType.Value;
                string genero = ddlGenero.SelectedValue;
                DateTime fechaNacParseada = DateTime.Parse(txtFechaNacimiento.Text);

                System.Diagnostics.Debug.WriteLine($"Valor para tipoDocumento: {tipoDocumento}");
                System.Diagnostics.Debug.WriteLine($"Valor para genero: {genero}");

                Enum.TryParse<tipoDocumento>(tipoDocumento, true, out tipoDocumento tipoEnum);
                Enum.TryParse<genero>(genero, true, out genero genEnum);

                System.Diagnostics.Debug.WriteLine($"Enum parseado para tipoDocumento: {tipoEnum.ToString()}");
                System.Diagnostics.Debug.WriteLine($"Enum parseado para genero: {genEnum.ToString()}");

                var nuevoUsuario = new usuarioDTO();
                nuevoUsuario.numDocumento = (tipoDocumento == "DNI") ? txtDNI.Text.Trim() : txtCE.Text.Trim();
                nuevoUsuario.nombres = txtNombres.Text.Trim();
                nuevoUsuario.apellidoPaterno = txtApellidoPaterno.Text.Trim();
                nuevoUsuario.apellidoMaterno = txtApellidoMaterno.Text.Trim();
                nuevoUsuario.correoElectronico = correo;
                nuevoUsuario.numCelular = txtCelular.Text.Trim();
                nuevoUsuario.fechaNacimiento = fechaNacParseada.ToString("yyyy-MM-dd");
                nuevoUsuario.contrasenha = password;
                nuevoUsuario.codMedico = "";
                if (Enum.TryParse<tipoDocumento>(tipoDocumento, true, out var tipoDocEnum))
                {
                    nuevoUsuario.tipoDocumento = tipoDocEnum;
                    nuevoUsuario.tipoDocumentoSpecified = true;
                }
                if (Enum.TryParse<genero>(genero, true, out var generoEnum))
                {
                    nuevoUsuario.genero = generoEnum;
                    nuevoUsuario.generoSpecified = true;
                }

                var servicioRegistro = new RegistroBO();
                bool resultadoRegistro = servicioRegistro.Registrarse(nuevoUsuario);
                
                if (resultadoRegistro)
                {
                    Session["CorreoVerificacion"] = nuevoUsuario.correoElectronico;
                    Response.Redirect("verificar_correo.aspx", false);
                }
                else
                {
                    MostrarMensaje("No se pudo completar el registro. Es posible que el número de documento ya esté en uso.", esExito: false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error inesperado durante el registro.", esExito: false);
                System.Diagnostics.Debug.WriteLine($"Error en registro: {ex}");
            }
        }
        private void MostrarMensaje(string mensaje, bool esExito)
        {
            string cssClass = esExito ? "alert alert-success" : "alert alert-danger";
            ltlMensaje.Text = $"<div class='{cssClass} mt-3'>{Server.HtmlEncode(mensaje)}</div>";
        }
        #region Funciones de Validación
        private bool EsDominioDeCorreoValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                return false;
            }

            string dominio = email.Split('@')[1].ToLower();
            var dominiosValidos = new HashSet<string>
        {
            "gmail.com", "outlook.com", "hotmail.com", "yahoo.com", "icloud.com",
            "pucp.edu.pe"
        };

            return dominiosValidos.Contains(dominio);
        }
        private bool EsMayorDeEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad))
            {
                edad--;
            }
            return edad >= 18;
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
        #endregion
    }
}