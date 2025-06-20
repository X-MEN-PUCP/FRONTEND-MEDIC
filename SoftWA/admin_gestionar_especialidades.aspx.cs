using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftWA
{
    public partial class admin_gestionar_especialidades : System.Web.UI.Page
    {
        private static List<Especialidades> listaGlobalEspecialidades;

        public class Especialidades
        {
            public int ID { get; set; }
            public string NombreEspecialidad { get; set; }
            public double PrecioConsulta { get; set; }
            public int CantMedicos { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosEspecialidadesSiEsNecesario();
                AplicarFiltrosYOrdenar();
            }
        }

        private void CargarDatosEspecialidadesSiEsNecesario()
        {
            if (listaGlobalEspecialidades == null || !listaGlobalEspecialidades.Any())
            {
                listaGlobalEspecialidades = new List<Especialidades>
                {
                    new Especialidades { ID = 1, NombreEspecialidad = "Traumatología", PrecioConsulta = 59.90, CantMedicos = 8},
                    new Especialidades { ID = 2, NombreEspecialidad = "Cardiología", PrecioConsulta = 69.90, CantMedicos = 11 }, // Changed price for sorting demo
                    new Especialidades { ID = 3, NombreEspecialidad = "Pediatría", PrecioConsulta = 49.90, CantMedicos = 5 },
                    new Especialidades { ID = 4, NombreEspecialidad = "Medicina General", PrecioConsulta = 39.90, CantMedicos = 13 }, // Changed price
                    new Especialidades { ID = 5, NombreEspecialidad = "Dermatología", PrecioConsulta = 79.90, CantMedicos = 3 },
                    new Especialidades { ID = 6, NombreEspecialidad = "Ginecología", PrecioConsulta = 59.90, CantMedicos = 7 }
                };
            }
        }

        private void AplicarFiltrosYOrdenar()
        {
            IEnumerable<Especialidades> especialidadesFiltradas = listaGlobalEspecialidades;

            string nombreFiltro = txtFiltrarNombre.Text.Trim();
            if (!string.IsNullOrEmpty(nombreFiltro))
            {
                especialidadesFiltradas = especialidadesFiltradas.Where(esp =>
                    esp.NombreEspecialidad.ToLower().Contains(nombreFiltro.ToLower()));
            }

            string ordenSeleccionado = ddlOrdenarPor.SelectedValue;
            switch (ordenSeleccionado)
            {
                case "NombreAsc":
                    especialidadesFiltradas = especialidadesFiltradas.OrderBy(esp => esp.NombreEspecialidad);
                    break;
                case "NombreDesc":
                    especialidadesFiltradas = especialidadesFiltradas.OrderByDescending(esp => esp.NombreEspecialidad);
                    break;
                case "PrecioAsc":
                    especialidadesFiltradas = especialidadesFiltradas.OrderBy(esp => esp.PrecioConsulta);
                    break;
                case "PrecioDesc":
                    especialidadesFiltradas = especialidadesFiltradas.OrderByDescending(esp => esp.PrecioConsulta);
                    break;
                case "MedicosAsc":
                    especialidadesFiltradas = especialidadesFiltradas.OrderBy(esp => esp.CantMedicos);
                    break;
                case "MedicosDesc":
                    especialidadesFiltradas = especialidadesFiltradas.OrderByDescending(esp => esp.CantMedicos);
                    break;
                default:
                    especialidadesFiltradas = especialidadesFiltradas.OrderBy(esp => esp.NombreEspecialidad);
                    break;
            }

            List<Especialidades> listaFinal = especialidadesFiltradas.ToList();
            rptEspecialidades.DataSource = listaFinal;
            rptEspecialidades.DataBind();

            phNoEspecialidad.Visible = !listaFinal.Any();
            updEspecialidadesRepeater.Update();
        }

        protected void rptEspecialidades_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int especialidadId = 0;
            if (e.CommandArgument != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                especialidadId = Convert.ToInt32(e.CommandArgument);
            }

            if (e.CommandName == "EditEspecialidad")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Acción: Editar Especialidad ID: {especialidadId}. Implementar lógica.');", true);
            }
            else if (e.CommandName == "EliminarEspecialidad")
            {
                bool eliminado = EliminarEspecialidadDeLista(especialidadId);
                if (eliminado)
                {
                    AplicarFiltrosYOrdenar();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Especialidad eliminada exitosamente.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Error: No se pudo encontrar la especialidad para eliminar.');", true);
                }
            }
        }

        private bool EliminarEspecialidadDeLista(int especialidadId)
        {
            CargarDatosEspecialidadesSiEsNecesario();

            var especialidadAEliminar = listaGlobalEspecialidades.FirstOrDefault(m => m.ID == especialidadId);
            if (especialidadAEliminar != null)
            {
                listaGlobalEspecialidades.Remove(especialidadAEliminar);
                return true;
            }
            return false;
        }

        protected void lnkAgregarNuevaEspecialidad_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Acción: Agregar nueva especialidad. Implementar lógica de formulario/modal.');", true);
        }

        protected void btnAplicarFiltrosEsp_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYOrdenar();
        }

        protected void btnLimpiarFiltrosEsp_Click(object sender, EventArgs e)
        {
            txtFiltrarNombre.Text = string.Empty;
            ddlOrdenarPor.SelectedValue = "MedicosAsc";
            AplicarFiltrosYOrdenar();
        }
    }
}