<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registro_cuenta_nueva.aspx.cs" Inherits="SoftWA.MA_General.resgistro_cuenta_nueva" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registrarse - Medical App</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.2.0/css/all.min.css" />
    <style>
        body {
            background-color: #f8f9fa;
        }
        .register-container {
            max-width: 450px;
            margin: 8vh auto;
            padding: 2rem;
            background-color: #fff;
            border-radius: 0.5rem;
            box-shadow: 0 0.5rem 1rem rgba(0,0,0,.15);
        }
        .register-header {
            text-align: center;
            margin-bottom: 1.5rem;
        }
         .register-header i {
             font-size: 3rem;
             color: #0d6efd;
             margin-bottom: 0.5rem;
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
    </style>
</head>
<body>
    <form id="form1" runat="server" class="needs-validation" novalidate>
        <div class="container">
            <div class="register-container">
                <div class="register-header d-flex flex-column align-items-center text-center">
                <a class="navbar-brand d-flex align-items-center" href="registro_cuenta_nueva.aspx">
                    <img src="Images/op3.png" alt="Logo" width="200">
                </a>
                <h2>Medical App</h2>
                <p class="text-muted">Cree su cuenta nueva</p>
            </div>
                <asp:Literal ID="ltlMensajeError" runat="server" EnableViewState="false"></asp:Literal>
                <div class="mb-3 document-type-toggle-container">
                    <label class="form-label">Tipo de Documento:</label>
                    <div class="toggle-switch-custom">
                        <button type="button" id="btnToggleDNI" class="toggle-option active" onclick="selectDocumentType('DNI')">DNI</button>
                        <button type="button" id="btnToggleCE" class="toggle-option" onclick="selectDocumentType('CE')">CE</button>
                    </div>
                    <asp:HiddenField ID="hdnSelectedDocumentType" runat="server" Value="DNI" />
                </div>
                <div id="dniFieldContainer" class="mb-3">
                    <label for="<%=txtDNI.ClientID%>" class="form-label">Número de DNI:</label>
                    <div class="input-group">
                         <span class="input-group-text"><i class="fa-solid fa-id-card"></i></span>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ingrese su DNI"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvDNI" runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="El DNI es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revDNI" runat="server"
                        ControlToValidate="txtDNI"
                        ValidationExpression="^\d{8}$"
                        ErrorMessage="El DNI debe tener 8 dígitos."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation">
                    </asp:RegularExpressionValidator>
                </div>
                <div id="ceFieldContainer" class="mb-3" style="display:none;">
                    <label for="<%=txtCE.ClientID%>" class="form-label">Número de Carnet de Extranjería:</label>
                    <div class="input-group">
                         <span class="input-group-text"><i class="fa-solid fa-passport"></i></span>
                        <asp:TextBox ID="txtCE" runat="server" CssClass="form-control" MaxLength="12" placeholder="Ingrese su Carnet de Extranjería" Enabled="false"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvCE" runat="server"
                        ControlToValidate="txtCE"
                        ErrorMessage="El Carnet de Extranjería es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation" Enabled="false">
                    </asp:RequiredFieldValidator>
                     <asp:RegularExpressionValidator ID="revCE" runat="server"
                        ControlToValidate="txtCE"
                        ValidationExpression="^[a-zA-Z0-9]{9,12}$"
                        ErrorMessage="Formato de CE inválido (9-12 alfanuméricos)."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation" Enabled="false">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="mb-3">
                    <label for="<%=txtPassword.ClientID%>" class="form-label">Contraseña:</label>
                     <div class="input-group">
                        <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation">
                     </asp:RequiredFieldValidator>
                </div>
                <div class="mb-3">
                    <label for="<%=txtConfirmPassword.ClientID%>" class="form-label">Confirmar Contraseña:</label>
                     <div class="input-group">
                        <span class="input-group-text"><i class="fa-solid fa-lock-open"></i></span>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirme su contraseña"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Confirmar la contraseña es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation">
                     </asp:RequiredFieldValidator>
                     <asp:CompareValidator ID="cvPassword" runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ControlToCompare="txtPassword"
                        Operator="Equal"
                        ErrorMessage="Las contraseñas no coinciden."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="RegisterValidation">
                    </asp:CompareValidator>
                </div>
                <div class="d-grid">
                    <asp:Button ID="btnRegister" runat="server" Text="Registrar Cuenta"
                        CssClass="btn btn-primary btn-block" OnClick="btnRegister_Click"
                        ValidationGroup="RegisterValidation" />
                </div>
                <div class="mt-3 text-center">
                    ¿Ya tienes una cuenta? <a href="indexLogin.aspx" class="btn btn-link">Inicia Sesión aquí</a>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src='<%= ResolveUrl("~/Scripts/registro.js") %>' type="text/javascript"></script>
    <script type="text/javascript">
        window.addEventListener('DOMContentLoaded', function () {
            const elementIds = {
                hdnFieldId: '<%= hdnSelectedDocumentType.ClientID %>',
                btnDniId: 'btnToggleDNI',
                btnCeId: 'btnToggleCE',
                dniContainerId: 'dniFieldContainer',
                ceContainerId: 'ceFieldContainer',
                txtDniId: '<%= txtDNI.ClientID %>',
                txtCeId: '<%= txtCE.ClientID %>',
                rfvDniId: '<%= rfvDNI.ClientID %>',
                revDniId: '<%= revDNI.ClientID %>',
                rfvCeId: '<%= rfvCE.ClientID %>',
                revCeId: '<%= revCE.ClientID %>'
            };
            initializeRegistrationForm(elementIds);
        });
    </script>
</body>
</html>