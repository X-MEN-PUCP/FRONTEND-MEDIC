<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="verificar_correo.aspx.cs" Inherits="SoftWA.verificar_correo" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Verificar Correo - Medical App</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background-color: #f8f9fa; }
        .verify-container { max-width: 450px; margin: 10vh auto; padding: 2rem; background-color: #fff; border-radius: 0.5rem; box-shadow: 0 0.5rem 1rem rgba(0,0,0,.15); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="verify-container text-center">
                <h2>Verificar Correo Electrónico</h2>
                <p class="text-muted">Hemos enviado un código de 6 dígitos a: 
                    <strong><asp:Literal ID="ltlCorreoUsuario" runat="server"></asp:Literal></strong>
                </p>
                <hr />
                <asp:Literal ID="ltlMensaje" runat="server" EnableViewState="false"></asp:Literal>
                <div class="mb-3">
                    <label for="<%=txtCodigo.ClientID %>" class="form-label visually-hidden">Código de Verificación:</label>
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control text-center fs-4" MaxLength="6" placeholder="------" autocomplete="one-time-code"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El código es obligatorio." CssClass="text-danger small" ValidationGroup="Verificacion" />
                </div>
                <div class="d-grid">
                    <asp:Button ID="btnVerificar" runat="server" Text="Verificar y Activar Cuenta" OnClick="btnVerificar_Click" CssClass="btn btn-primary" ValidationGroup="Verificacion" />
                </div>
                 <div class="mt-3">
                    <asp:LinkButton ID="lbtnReenviar" runat="server" OnClick="lbtnReenviar_Click">¿No recibiste el código? Reenviar</asp:LinkButton>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
