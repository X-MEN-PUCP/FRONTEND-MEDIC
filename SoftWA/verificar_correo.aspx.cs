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
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["CorreoVerificacion"] != null)
                {
                    lblCorreoUsuario.Text = Session["CorreoVerificacion"].ToString();
                }
                else
                {
                    lblCorreoUsuario.Text = "No se ha proporcionado un correo electrónico para verificar.";
                }
            }
        }
        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            //if (!Page.IsValid) return;
            //string correo = lblCorreoUsuario.Text.Trim();
            //string codigo = txtCodigo.Text.Trim();
            //try
            //{
            //    var servicioRegistro = new RegistroBO();
            //    bool resultado = servicioRegistro.verificarCodigo(correo, codigo);

            //}
            //if (string.IsNullOrEmpty(correo) || !correo.Contains("@"))
            //{
            //    lblMensaje.Text = "Por favor, ingrese un correo electrónico válido.";
            //    return;
            //}
            //Aquí se puede agregar la lógica para enviar un correo de verificación
            // o realizar cualquier otra acción necesaria.
            //lblMensaje.Text = "Se ha enviado un correo de verificación a " + correo + ".";
        }
    }
}