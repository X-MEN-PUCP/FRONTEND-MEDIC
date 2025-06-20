using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA.MA_General
{
    public partial class resgistro_cuenta_nueva : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCE.Enabled = false;
            rfvCE.Enabled = false;
            revCE.Enabled = false;

            txtDNI.Enabled = true;
            rfvDNI.Enabled = true;
            revDNI.Enabled = true;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string selectedDocType = hdnSelectedDocumentType.Value;
            string documentNumber = "";

            if (selectedDocType == "DNI")
            {
                rfvDNI.Enabled = true;
                revDNI.Enabled = true;
                txtDNI.Enabled = true;

                rfvCE.Enabled = false;
                revCE.Enabled = false;
                txtCE.Enabled = false;
                txtCE.Text = string.Empty;

                documentNumber = txtDNI.Text;
            }
            else if (selectedDocType == "CE")
            {
                rfvDNI.Enabled = false;
                revDNI.Enabled = false;
                txtDNI.Enabled = false;
                txtDNI.Text = string.Empty;

                rfvCE.Enabled = true;
                revCE.Enabled = true;
                txtCE.Enabled = true;

                documentNumber = txtCE.Text;
            }

            Page.Validate("RegisterValidation");

            if (Page.IsValid)
            {
                string password = txtPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                ltlMensajeError.Text = $"<div class='alert alert-success'>Registro exitoso! Tipo Doc: {selectedDocType}, Número: {documentNumber}, Pass: {password}</div>";
            }
            else
            {
                ltlMensajeError.Text = "<div class='alert alert-danger'>Por favor corrija los errores e intente de nuevo.</div>";
            }
        }
    }
}