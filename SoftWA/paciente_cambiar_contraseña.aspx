<%@ Page Title="Cambiar Contraseña" Language="C#" MasterPageFile="~/SoftMA_Paciente.Master" AutoEventWireup="true" 
    CodeBehind="paciente_cambiar_contraseña.aspx.cs" Inherits="SoftWA.paciente_cambiar_contraseña" %>
<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Cambiar Contraseña
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <style>
        .pswrd-container { max-width: 450px; margin: 5vh auto; padding: 2.5rem; background-color: #fff; border-radius: 0.5rem; box-shadow: 0 0.5rem 1rem rgba(0,0,0,.15); }
        .pswrd-header { margin-bottom: 2rem; }
        .password-toggle-icon { cursor: pointer; position: absolute; right: 10px; top: 50%; transform: translateY(-50%); color: #6c757d; }
        .password-toggle-icon-container {cursor: pointer; }
        .password-requirements { font-size: 0.9rem; margin-top: 0.5rem; }
        .password-requirements li { transition: color 0.3s ease;}
        .password-requirements li.neutral { color: #6c757d; }
        .password-requirements li.neutral i { display: none; }
        .password-requirements li.neutral::before {
            content: "\2022";
            color: #6c757d;
            font-weight: bold;
            display: inline-block;
            width: 1.25em;
            margin-left: -0.2em;
        }
        .password-requirements li.invalid { color: #dc3545; }
        .password-requirements li.valid { color: #198754; text-decoration: line-through; }
        .password-requirements li i { width: 20px; text-align: center; }
    </style>
    <div class="container">
        <div class="pswrd-container">
            <div class="pswrd-header d-flex flex-column align-items-center text-center">
                <h2><i class="fa-solid fa-shield-halved me-2"></i>Actualice su Contraseña</h2>
                <p class="text-muted">Para su seguridad, elija una contraseña fuerte.</p>
            </div>
            <asp:UpdatePanel ID="updChangePassword" runat="server">
                <ContentTemplate>
                    <asp:Literal ID="ltlMensajeError" runat="server" EnableViewState="false"></asp:Literal>
                    
                    <div class="mb-3">
                        <label for="<%=txtCurrentPassword.ClientID%>" class="form-label">Contraseña Actual:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-key"></i></span>
                            <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <span class="input-group-text password-toggle-icon-container" data-target="<%=txtCurrentPassword.ClientID%>"><i class="fas fa-eye"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" 
                            ErrorMessage="La contraseña actual es obligatoria." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ChangePasswordValidation" />
                    </div>

                    <div class="mb-3">
                        <label for="<%=txtNewPassword.ClientID%>" class="form-label">Nueva Contraseña:</label>
                         <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <span class="input-group-text password-toggle-icon-container" data-target="<%=txtNewPassword.ClientID%>"><i class="fas fa-eye"></i></span>
                        </div>
                        <div id="passwordRequirementsContainer"></div>
                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" 
                            ErrorMessage="La nueva contraseña es obligatoria." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ChangePasswordValidation" />
                    </div>

                    <div class="mb-3">
                        <label for="<%=txtConfirmNewPassword.ClientID%>" class="form-label">Confirmar Nueva Contraseña:</label>
                         <div class="input-group">
                            <span class="input-group-text"><i class="fa-solid fa-lock-open"></i></span>
                            <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                             <span class="input-group-text password-toggle-icon-container" data-target="<%=txtConfirmNewPassword.ClientID%>"><i class="fas fa-eye"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword" 
                            ErrorMessage="Confirmar la contraseña es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ChangePasswordValidation" />
                         <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword"
                             Operator="Equal" ErrorMessage="Las nuevas contraseñas no coinciden." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ChangePasswordValidation" />
                    </div>

                    <div class="d-grid mt-4">
                        <asp:Button ID="btnChangePassword" runat="server" Text="Cambiar Contraseña" CssClass="btn btn-primary btn-block" OnClick="btnChangePassword_Click"
                            ValidationGroup="ChangePasswordValidation" />
                    </div>
                    
                    <div class="mt-3 text-center">
                        <a href="indexPaciente.aspx" class="btn btn-link">Cancelar y Volver</a>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script src='<%= ResolveUrl("~/Scripts/contraseñaValida.js") %>' type="text/javascript"></script>
    <%--<script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            initializePasswordValidator(
                '<%= txtNewPassword.ClientID %>',
                'passwordRequirementsContainer'
            );
        });
    </script>--%>
    <script type="text/javascript">
        function pageLoad() {
            initializePasswordValidator(
                '<%= txtNewPassword.ClientID %>',
                'passwordRequirementsContainer'
            );
            const passwordToggles = document.querySelectorAll('.password-toggle-icon-container');
            passwordToggles.forEach(toggle => {
                const inputId = toggle.dataset.target;
                const icon = toggle.querySelector('i');
                toggle.addEventListener('click', function () {
                    togglePasswordVisibility(inputId, icon);
                });
            });
        }
        if (typeof (Sys) === 'undefined' || !Sys.WebForms || !Sys.WebForms.PageRequestManager) {
            window.addEventListener('load', pageLoad);
        }

    </script>>
</asp:Content>
