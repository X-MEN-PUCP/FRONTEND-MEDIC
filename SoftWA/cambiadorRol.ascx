<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cambiadorRol.ascx.cs" Inherits="SoftWA.cambiadorRol" %>

<asp:PlaceHolder ID="phCambiador" runat="server" Visible="false">
    <div class="dropdown">
        <a href="#" class="btn btn-outline-light dropdown-toggle" role="button" id="dropdownMenuLink" data-bs-toggle="dropdown" aria-expanded="false">
            <i class="fa-solid fa-user-shield me-2"></i>
            <asp:Literal ID="ltlRolActual" runat="server"></asp:Literal>
        </a>

        <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="dropdownMenuLink">
            <asp:Repeater ID="rptRoles" runat="server" OnItemCommand="rptRoles_ItemCommand">
                <ItemTemplate> 
                    <li>
                        <asp:LinkButton ID="btnCambiarRol" runat="server" 
                            CssClass="dropdown-item"
                            CommandName="CambiarRol"
                            CommandArgument='<%# Eval("rol.idRol") %>'>
                            <i class="fa-solid fa-sync-alt me-2"></i> <%# ObtenerNombreRol((int)Eval("rol.idRol")) %>
                        </asp:LinkButton>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:PlaceHolder>

