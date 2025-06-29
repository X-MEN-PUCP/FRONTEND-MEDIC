<%@ Page Title="" Language="C#" MasterPageFile="~/SoftMA_Doctor.Master" AutoEventWireup="true" CodeBehind="doctor_cambiar_contraseña.aspx.cs" Inherits="SoftWA.doctor_cambiar_contraseña" %>
<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Cambiar Contraseña
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container">
        <div class="pswrd-container" style="max-width: 500px; margin: auto;">
            <div class="pswrd-header d-flex flex-column align-items-center text-center">
                <a class="navbar-brand d-flex align-items-center" href="doctor_cambiar_contraseña.aspx">
                    <img src="Images/op3.png" alt="Logo" width="200">
                </a>
                <h2>Medical App</h2>
                <p class="text-muted">Actualice su contraseña</p>
            </div>
            <asp:Literal ID="ltlMensajeError" runat="server" EnableViewState="false"></asp:Literal>
            <div class="mb-3">
                <label for="<%=txtCurrentPassword.ClientID%>" class="form-label">Contraseña Actual:</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="fa-solid fa-key"></i></span>
                    <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña actual"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server"
                    ControlToValidate="txtCurrentPassword"
                    ErrorMessage="La contraseña actual es obligatoria."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="ChangePasswordValidation">
                </asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <label for="<%=txtNewPassword.ClientID%>" class="form-label">Nueva Contraseña:</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su nueva contraseña"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                    ControlToValidate="txtNewPassword"
                    ErrorMessage="La nueva contraseña es obligatoria."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="ChangePasswordValidation">
                </asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <label for="<%=txtConfirmNewPassword.ClientID%>" class="form-label">Confirmar Nueva Contraseña:</label>
                 <div class="input-group">
                    <span class="input-group-text"><i class="fa-solid fa-lock-open"></i></span>
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirme su nueva contraseña"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvConfirmNewPassword" runat="server"
                    ControlToValidate="txtConfirmNewPassword"
                    ErrorMessage="Confirmar la nueva contraseña es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="ChangePasswordValidation">
                 </asp:RequiredFieldValidator>
                 <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmNewPassword"
                    ControlToCompare="txtNewPassword"
                    Operator="Equal"
                    ErrorMessage="Las nuevas contraseñas no coinciden."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="ChangePasswordValidation">
                </asp:CompareValidator>
            </div>

            <div class="d-grid">
                <asp:Button ID="btnChangePassword" runat="server" Text="Cambiar Contraseña"
                    CssClass="btn btn-primary btn-block" OnClick="btnChangePassword_Click"
                    ValidationGroup="ChangePasswordValidation" />
            </div>
            
            <div class="mt-3 text-center">
                <a href="indexMedico.aspx" class="btn btn-link">Cancelar</a>
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
