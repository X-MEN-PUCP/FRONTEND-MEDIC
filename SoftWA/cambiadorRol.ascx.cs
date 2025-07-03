using SoftBO.SoftCitWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class cambiadorRol : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRoles();
            }
        }
        private void CargarRoles()
        {
            var listaRoles = Session["ListaRolesUsuario"] as List<usuarioPorRolDTO>;
            var rolActual = Session["RolActual"] as rolDTO;
            if (listaRoles != null && listaRoles.Count > 1 && rolActual != null)
            {
                phCambiador.Visible = true;
                int rolActualId = rolActual.idRol;
                ltlRolActual.Text = ObtenerNombreRol(rolActual.idRol);
                var rolesDisponibles = listaRoles.Where(r => r.rol.idRol != rolActual.idRol).ToList();
                rptRoles.DataSource = rolesDisponibles;
                rptRoles.DataBind();
            }
            else
            {
                phCambiador.Visible = false;
            }
        }

        protected string ObtenerNombreRol(int idRol)
        {
            switch (idRol)
            {
                case 1: return "Administrador";
                case 2: return "Médico";
                case 3: return "Paciente";
                default: return "Desconocido";
            }
        }

        protected void rptRoles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CambiarRol")
            {
                int nuevoRolId = int.Parse(e.CommandArgument.ToString());
                var listaRoles = Session["ListaRolesUsuario"] as List<usuarioPorRolDTO>;
                var nuevoRolSeleccionado = listaRoles.FirstOrDefault(r => r.rol.idRol == nuevoRolId)?.rol;
                if (nuevoRolSeleccionado != null)
                {
                    Session["RolActual"] = nuevoRolSeleccionado;
                    string targetUrl = ObtenerUrlRedireccion(nuevoRolSeleccionado);
                    Response.Redirect(targetUrl, false);
                }
            }
        }
        private string ObtenerUrlRedireccion(rolDTO rol)
        {
            if (rol == null) return "~/indexLogin.aspx";

            switch (rol.idRol)
            {
                case 1: return "~/indexAdmin.aspx";
                case 2: return "~/indexMedico.aspx";
                case 3: return "~/indexPaciente.aspx";
                default: return "~/indexLogin.aspx";
            }
        }
    }
}