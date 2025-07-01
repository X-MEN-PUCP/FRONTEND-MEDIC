<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="indexLogin.aspx.cs" Inherits="SoftWA.indexLogin"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Iniciar Sesión - Medical App</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.2.0/css/all.min.css" />
    
    <style>
        body {
            background-color: #f8f9fa;
        }
        .login-container {
            max-width: 400px;
            margin: 10vh auto;
            padding: 2rem;
            background-color: #fff;
            border-radius: 0.5rem;
            box-shadow: 0 0.5rem 1rem rgba(0,0,0,.15);
        }
        .login-header {
            text-align: center;
            margin-bottom: 1.5rem;
        }
        .document-type-toggle-container .form-label {
            display: block;
            margin-bottom: .5rem;
        }
        .toggle-switch-custom {
            display: flex;
            width: 100%;
            border: 1px solid #ced4da;
            border-radius: .375rem;
            overflow: hidden;
            background-color: #fff;
        }
        .toggle-switch-custom .toggle-option {
            flex-grow: 1;
            padding: .65rem 1rem;
            text-align: center;
            background-color: #f8f9fa;
            color: #495057;
            border: none;
            cursor: pointer;
            transition: background-color 0.2s ease-in-out, color 0.2s ease-in-out;
            font-size: 0.9rem;
            line-height: 1.5;
        }
        .toggle-switch-custom .toggle-option:first-child {
            border-right: 1px solid #ced4da;
        }
        .toggle-switch-custom .toggle-option.active {
            background-color: #5bd3c5;
            color: #ffffff;
            font-weight: 600;
        }
        .toggle-switch-custom .toggle-option:not(.active):hover {
            background-color: #e9ecef;
        }
        #togglePassword {
        cursor: pointer;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="needs-validation" novalidate>
        <div class="container">
            <div class="login-container">
                <div class="login-header d-flex flex-column align-items-center text-center">
                    <a class="navbar-brand d-flex align-items-center" href="indexLogin.aspx">
                        <img src='<%= ResolveUrl("~/Images/op3.png") %>' alt="Logo" width="200">
                    </a>
                    <h2>Medical App</h2>
                    <p class="text-muted">Ingrese sus datos</p>
                </div>
                <asp:Literal ID="ltlMensajeError" runat="server" EnableViewState="false"></asp:Literal>
                <div class="mb-3 document-type-toggle-container">
                    <label class="form-label">Tipo de Documento:</label>
                    <div class="toggle-switch-custom">
                        <button type="button" id="btnToggleDNI" class="toggle-option active">DNI</button>
                        <button type="button" id="btnToggleCE" class="toggle-option">Carnet Ext.</button>
                    </div>
                    <asp:HiddenField ID="hdnLoginDocType" runat="server" Value="DNI" />
                </div>
                
                <div class="mb-3">
                    <label for="<%=txtDNI.ClientID%>" class="form-label">Número de Documento:</label>
                    <div class="input-group">
                         <span class="input-group-text"><i class="fa-solid fa-id-card"></i></span>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ingrese su DNI"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvDNI" runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="El documento es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="LoginValidation">
                    </asp:RequiredFieldValidator>
                    
                    <asp:RegularExpressionValidator ID="revDNI" runat="server"
                        ControlToValidate="txtDNI" ValidationExpression="^\d{8}$"
                        ErrorMessage="El DNI debe tener 8 dígitos."
                        CssClass="text-danger small" Display="Dynamic" ValidationGroup="LoginValidation">
                    </asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revCE" runat="server"
                        ControlToValidate="txtDNI" ValidationExpression="^[a-zA-Z0-9]{9,12}$"
                        ErrorMessage="El Carnet debe tener de 9 a 12 caracteres."
                        CssClass="text-danger small" Display="Dynamic" ValidationGroup="LoginValidation" Enabled="false">
                    </asp:RegularExpressionValidator>
                </div>

                <div class="mb-3">
                    <label for="<%=txtPassword.ClientID%>" class="form-label">Contraseña:</label>
                     <div class="input-group">
                        <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña"></asp:TextBox>
                        <button class="btn btn-outline-secondary" type="button" id="togglePassword">
                            <i class="fa-solid fa-eye" id="togglePasswordIcon"></i>
                        </button>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria." CssClass="text-danger small" Display="Dynamic" ValidationGroup="LoginValidation"></asp:RequiredFieldValidator>
                </div>
                <div class="d-grid">
                    <asp:Button ID="btnLogin" runat="server" Text="Ingresar" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" ValidationGroup="LoginValidation" />
                </div>
                <div class="mt-3 text-center">
                    ¿No tienes cuenta? <a href="registro_cuenta_nueva.aspx" class="btn btn-link">Regístrate aquí</a>
                </div>
                <hr class="my-3" />
                <div class="text-center form-text px-3">
                    <i class="fa-solid fa-circle-info me-1"></i>
                    <strong>¿Olvidó su contraseña?</strong>
                    <p class="mb-1">
                        Para recuperarla, por favor comuníquese con nosotros indicando su número de documento y el correo con el que se registró.
                    </p>
                    <div>
                        <a href="mailto:medicalapp.pro@gmail.com" class="btn btn-sm btn-link text-decoration-none">
                            <i class="fa-solid fa-envelope me-1"></i> Correo
                        </a>
                        <span class="mx-1">|</span>
                        <a href="https://wa.me/51951043269" target="_blank" class="btn btn-sm btn-link text-decoration-none">
                           <i class="fa-brands fa-whatsapp me-1"></i> WhatsApp
                        </a>
                    </div>
                </div>

            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src='<%= ResolveUrl("~/Scripts/login.js") %>' type="text/javascript"></script>
    <script type="text/javascript">
        window.addEventListener('DOMContentLoaded', function () {
            const elementIds = {
                hdnFieldId: '<%= hdnLoginDocType.ClientID %>',
                btnDniId: 'btnToggleDNI',
                btnCeId: 'btnToggleCE',
                txtDocumentoId: '<%= txtDNI.ClientID %>',
                revDniId: '<%= revDNI.ClientID %>',
                revCeId: '<%= revCE.ClientID %>',
                txtPasswordId: '<%= txtPassword.ClientID %>'
            };
            initializeLoginForm(elementIds);
        });
    </script>
</body>
</html>