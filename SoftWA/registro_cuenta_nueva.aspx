f<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registro_cuenta_nueva.aspx.cs" Inherits="SoftWA.registro_cuenta_nueva"  Async="true"%>

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
                    <img src="~Content/Images/op3.png" alt="Logo" width="200">
                </a>
                <h2>Medical App</h2>
                <p class="text-muted">Cree su cuenta nueva</p>
            </div>
                <asp:Literal ID="ltlMensaje" runat="server" EnableViewState="false"></asp:Literal>
                <asp:Panel ID="pnlBusquedaDocumento" runat="server">
                    <div class="mb-3 document-type-toggle-container">
                        <label class="form-label">Tipo de Documento:</label>
                        <div class="toggle-switch-custom">
                            <button type="button" id="btnToggleDNI" class="toggle-option active">DNI</button>
                            <button type="button" id="btnToggleCE" class="toggle-option">CE</button>
                        </div>
                        <asp:HiddenField ID="hdnSelectedDocumentType" runat="server" Value="DNI" />
                    </div>
                    <div id="dniFieldContainer" class="mb-3">
                        <label for="<%=txtDNI.ClientID%>" class="form-label">Número de DNI:</label>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ingrese su DNI"></asp:TextBox>
                    </div>
                    <div id="ceFieldContainer" class="mb-3" style="display:none;">
                        <label for="<%=txtCE.ClientID%>" class="form-label">Número de Carnet de Extranjería:</label>
                        <asp:TextBox ID="txtCE" runat="server" CssClass="form-control" MaxLength="12" placeholder="Ingrese su Carnet de Extranjería" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnValidarDocumento" runat="server" Text="Validar Documento" CssClass="btn btn-info" OnClick="btnValidarDocumento_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDatosRegistro" runat="server" Visible="false">
                    <hr class="my-3"/>
                    <h5 class="text-center text-muted mb-3">Datos Personales</h5>
                    <div class="mb-3">
                        <label class="form-label">Nombres:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-user"></i></span>
                            <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Apellido Paterno:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-user"></i></span>
                            <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Apellido Materno:</label>
                        <div class="input-group">
                             <span class="input-group-text"><i class="fa-solid fa-user"></i></span>
                            <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <hr class="my-3" />
                    <h5 class="text-center text-muted mb-3">Complete su Información</h5>
                    <div class="mb-3">
                        <label for="<%=txtCorreo.ClientID%>" class="form-label">Correo Electrónico:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-envelope"></i></span>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email" placeholder="ejemplo@correo.com"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="RegisterValidation" />
                    </div>
                    <div class="mb-3">
                        <label for="<%=txtCelular.ClientID%>" class="form-label">Número de Celular:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-mobile-screen-button"></i></span>
                            <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control" MaxLength="9" placeholder="987654321"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvCelular" runat="server" ControlToValidate="txtCelular" ErrorMessage="El celular es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="RegisterValidation" />
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="<%=txtFechaNacimiento.ClientID%>" class="form-label">Fecha de Nacimiento:</label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ControlToValidate="txtFechaNacimiento" ErrorMessage="Campo obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="RegisterValidation" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="<%=ddlGenero.ClientID%>" class="form-label">Género:</label>
                            <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Seleccionar..." Value=""></asp:ListItem>
                                <asp:ListItem Text="Masculino" Value="MASCULINO"></asp:ListItem>
                                <asp:ListItem Text="Femenino" Value="FEMENINO"></asp:ListItem>
                                <asp:ListItem Text="Otro" Value="OTRO"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvGenero" runat="server" ControlToValidate="ddlGenero" ErrorMessage="Campo obligatorio." CssClass="text-danger small" Display="Dynamic" InitialValue="" ValidationGroup="RegisterValidation" />
                        </div>
                    </div>
                    <hr class="my-3" />
                    <h5 class="text-center text-muted mb-3">Cree su Contraseña</h5>
                    <div class="mb-3">
                        <label for="<%=txtPassword.ClientID%>" class="form-label">Contraseña:</label>
                         <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria." CssClass="text-danger small" Display="Dynamic" ValidationGroup="RegisterValidation" />
                    </div>
                    <div class="mb-3">
                        <label for="<%=txtConfirmPassword.ClientID%>" class="form-label">Confirmar Contraseña:</label>
                         <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-lock-open"></i></span>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirme su nueva contraseña"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirmar la contraseña es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="RegisterValidation" />
                         <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Operator="Equal" ErrorMessage="Las contraseñas no coinciden." CssClass="text-danger small" Display="Dynamic" ValidationGroup="RegisterValidation" />
                    </div>
                    
                    <div class="d-grid">
                        <asp:Button ID="btnRegister" runat="server" Text="Crear Cuenta"
                            CssClass="btn btn-primary btn-block" OnClick="btnRegister_Click"
                            ValidationGroup="RegisterValidation" />
                    </div>
                </asp:Panel>
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
                txtCeId: '<%= txtCE.ClientID %>'
            };
            initializeRegistrationForm(elementIds);
        });
    </script>
</body>
</html>
