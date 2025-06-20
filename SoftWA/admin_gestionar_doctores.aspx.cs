using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace SoftWA
{
    public class DoctorInfo
    {
        public int IdDoctor { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    public class EspecialidadSimple
    {
        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; }
    }
    public partial class admin_gestionar_doctores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateEspecialidadesDropdowns();
                BindDoctorList();
                pnlAddEditDoctor.Visible = false;
                btnShowAddPanel.Visible = true;
            }
        }

        private void BindDoctorList()
        {
            Debug.WriteLine("Llamando a BindDoctorList...");

            int filtroEspecialidadId = 0;
            int.TryParse(ddlFiltroEspecialidad.SelectedValue, out filtroEspecialidadId);

            string filtroNombreApellido = txtFiltroNombreApellido.Text.Trim().ToLower();
            string ordenSeleccionado = ddlOrdenarDoctores.SelectedValue;

            Debug.WriteLine($"Filtrando por Especialidad ID: {filtroEspecialidadId}, Nombre/Apellido: '{filtroNombreApellido}', Orden: {ordenSeleccionado}");

            List<DoctorInfo> doctores = ObtenerDoctores_Simulado();

            if (filtroEspecialidadId > 0)
            {
                doctores = doctores.Where(d => d.IdEspecialidad == filtroEspecialidadId).ToList();
            }

            if (!string.IsNullOrEmpty(filtroNombreApellido))
            {
                doctores = doctores.Where(d =>
                    d.Nombre.ToLower().Contains(filtroNombreApellido) ||
                    d.Apellido.ToLower().Contains(filtroNombreApellido) ||
                    d.NombreCompleto.ToLower().Contains(filtroNombreApellido)
                ).ToList();
            }

            switch (ordenSeleccionado)
            {
                case "IdDesc":
                    doctores = doctores.OrderByDescending(d => d.IdDoctor).ToList();
                    break;
                case "NombreAsc":
                    doctores = doctores.OrderBy(d => d.NombreCompleto).ToList();
                    break;
                case "NombreDesc":
                    doctores = doctores.OrderByDescending(d => d.NombreCompleto).ToList();
                    break;
                case "EspecialidadAsc":
                    doctores = doctores.OrderBy(d => d.NombreEspecialidad).ThenBy(d => d.NombreCompleto).ToList();
                    break;
                case "EspecialidadDesc":
                    doctores = doctores.OrderByDescending(d => d.NombreEspecialidad).ThenBy(d => d.NombreCompleto).ToList();
                    break;
                case "IdAsc":
                default:
                    doctores = doctores.OrderBy(d => d.IdDoctor).ToList();
                    break;
            }

            lvDoctores.DataSource = doctores;
            lvDoctores.DataBind();
            PlaceHolder phNoDoctores = lvDoctores.FindControl("phNoDoctores") as PlaceHolder;
            if (phNoDoctores != null)
            {
                bool hayFiltrosAplicados = filtroEspecialidadId > 0 || !string.IsNullOrEmpty(filtroNombreApellido);
                phNoDoctores.Visible = hayFiltrosAplicados && !doctores.Any();
            }
        }

        private List<DoctorInfo> ObtenerDoctores_Simulado()
        {
            Debug.WriteLine($"Simulando: ObtenerDoctores_Simulado() - Todos");
            var todosLosDoctores = new List<DoctorInfo>
            {
                new DoctorInfo { IdDoctor = 101, Nombre = "Juan", Apellido = "Pérez", IdEspecialidad = 1, NombreEspecialidad = "Cardiología", Email = "jperez@hospital.com", Telefono = "123456789" },
                new DoctorInfo { IdDoctor = 102, Nombre = "Ana", Apellido = "López", IdEspecialidad = 1, NombreEspecialidad = "Cardiología", Email = "alopez@hospital.com", Telefono = "987654321" },
                new DoctorInfo { IdDoctor = 201, Nombre = "Carlos", Apellido = "García", IdEspecialidad = 2, NombreEspecialidad = "Dermatología", Email = "cgarcia@hospital.com", Telefono = "965647326" },
                new DoctorInfo { IdDoctor = 301, Nombre = "Sofía", Apellido = "Gómez", IdEspecialidad = 3, NombreEspecialidad = "Pediatría", Email = "sgomez@hospital.com", Telefono = "112233445" },
                new DoctorInfo { IdDoctor = 401, Nombre = "Luis", Apellido = "Martínez", IdEspecialidad = 2, NombreEspecialidad = "Dermatología", Email = "lmartinez@hospital.com", Telefono = "223344556"},
                new DoctorInfo { IdDoctor = 103, Nombre = "Laura", Apellido = "Vargas", IdEspecialidad = 1, NombreEspecialidad = "Cardiología", Email = "lvargas@hospital.com", Telefono = "333222111"},
                new DoctorInfo { IdDoctor = 501, Nombre = "Pedro", Apellido = "Sánchez", IdEspecialidad = 4, NombreEspecialidad = "Traumatología", Email = "psanchez@hospital.com", Telefono = "444555666"},
                new DoctorInfo { IdDoctor = 302, Nombre = "Martín", Apellido = "Gutiérrez", IdEspecialidad = 3, NombreEspecialidad = "Pediatría", Email = "mgutierrez@hospital.com", Telefono = "777888999"}
            };
            return todosLosDoctores;
        }

        private void PopulateEspecialidadesDropdowns()
        {
            Debug.WriteLine("Llamando a PopulateEspecialidadesDropdowns...");
            List<EspecialidadSimple> especialidades = ObtenerEspecialidades_Simulado();

            ddlEspecialidadAddEdit.DataSource = especialidades;
            ddlEspecialidadAddEdit.DataTextField = "NombreEspecialidad";
            ddlEspecialidadAddEdit.DataValueField = "IdEspecialidad";
            ddlEspecialidadAddEdit.DataBind();

            ddlFiltroEspecialidad.DataSource = especialidades;
            ddlFiltroEspecialidad.DataTextField = "NombreEspecialidad";
            ddlFiltroEspecialidad.DataValueField = "IdEspecialidad";
            ddlFiltroEspecialidad.DataBind();
        }

        private List<EspecialidadSimple> ObtenerEspecialidades_Simulado()
        {
            Debug.WriteLine("Simulando: ObtenerEspecialidades_Simulado()");
            return new List<EspecialidadSimple> {
                new EspecialidadSimple { IdEspecialidad = 1, NombreEspecialidad = "Cardiología" },
                new EspecialidadSimple { IdEspecialidad = 2, NombreEspecialidad = "Dermatología" },
                new EspecialidadSimple { IdEspecialidad = 3, NombreEspecialidad = "Pediatría" },
                new EspecialidadSimple { IdEspecialidad = 4, NombreEspecialidad = "Traumatología" },
                new EspecialidadSimple { IdEspecialidad = 5, NombreEspecialidad = "Odontología General" }
             };
        }
        protected void btnShowAddPanel_Click(object sender, EventArgs e)
        {
            ResetForm();
            lblFormTitle.Text = "Agregar Nuevo Doctor";
            hfDoctorId.Value = "0";
            pnlAddEditDoctor.Visible = true;
            btnShowAddPanel.Visible = false;
            updGestionDoctores.Update();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAddEditDoctor.Visible = false;
            btnShowAddPanel.Visible = true;
            ResetForm();
            updGestionDoctores.Update();
        }

        private void ResetForm()
        {
            hfDoctorId.Value = "0";
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            ddlEspecialidadAddEdit.ClearSelection();
            ListItem selectItem = ddlEspecialidadAddEdit.Items.FindByValue("");
            if (selectItem != null) selectItem.Selected = true;
            lblFormTitle.Text = "Agregar Nuevo Doctor";
        }

        protected void btnGuardarDoctor_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEditDoctor");
            if (!Page.IsValid)
            {
                updGestionDoctores.Update();
                return;
            }

            int doctorId = Convert.ToInt32(hfDoctorId.Value);
            string nombreEspecialidadSeleccionada = ddlEspecialidadAddEdit.SelectedItem != null && ddlEspecialidadAddEdit.SelectedValue != "" ?
                                                    ddlEspecialidadAddEdit.SelectedItem.Text : "N/A";
            var doctorInfo = new DoctorInfo
            {
                IdDoctor = doctorId,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                IdEspecialidad = Convert.ToInt32(ddlEspecialidadAddEdit.SelectedValue),
                NombreEspecialidad = nombreEspecialidadSeleccionada
            };

            bool success = false; string accion = "";
            if (doctorId == 0)
            {
                accion = "agregado"; success = AgregarDoctor_Simulado(doctorInfo);
            }
            else
            {
                accion = "actualizado"; success = ActualizarDoctor_Simulado(doctorInfo);
            }

            if (success)
            {
                pnlAddEditDoctor.Visible = false; btnShowAddPanel.Visible = true;
                ResetForm(); BindDoctorList();
            }
            else { Debug.WriteLine($"Error al {accion} doctor."); }
            updGestionDoctores.Update();
        }

        protected void lvDoctores_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                int doctorId = Convert.ToInt32(e.CommandArgument);
                DoctorInfo doctor = ObtenerDoctorPorId_Simulado(doctorId);
                if (doctor != null)
                {
                    hfDoctorId.Value = doctor.IdDoctor.ToString();
                    txtNombre.Text = doctor.Nombre; txtApellido.Text = doctor.Apellido;
                    txtEmail.Text = doctor.Email; txtTelefono.Text = doctor.Telefono;
                    ddlEspecialidadAddEdit.ClearSelection();
                    ListItem especialidadItem = ddlEspecialidadAddEdit.Items.FindByValue(doctor.IdEspecialidad.ToString());
                    if (especialidadItem != null) especialidadItem.Selected = true;
                    lblFormTitle.Text = "Editar Doctor";
                    pnlAddEditDoctor.Visible = true; btnShowAddPanel.Visible = false;
                    updGestionDoctores.Update();
                }
                else { Debug.WriteLine($"Error: No se encontró el doctor con ID {doctorId} para editar."); }
            }
        }

        protected void lvDoctores_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            int doctorId = Convert.ToInt32(lvDoctores.DataKeys[e.ItemIndex].Value);
            bool success = EliminarDoctor_Simulado(doctorId);
            if (success) { BindDoctorList(); }
            else { Debug.WriteLine($"Error al eliminar doctor ID: {doctorId}."); }

            if (pnlAddEditDoctor.Visible && hfDoctorId.Value == doctorId.ToString())
            {
                pnlAddEditDoctor.Visible = false; btnShowAddPanel.Visible = true; ResetForm();
            }
            updGestionDoctores.Update();
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnAplicarFiltros_Click disparado.");
            BindDoctorList();
            updGestionDoctores.Update();
        }

        protected void lnkLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroNombreApellido.Text = string.Empty;
            ddlFiltroEspecialidad.SelectedValue = "0";
            ddlOrdenarDoctores.SelectedValue = "IdAsc";
            BindDoctorList();
            updGestionDoctores.Update();
        }

        private DoctorInfo ObtenerDoctorPorId_Simulado(int id)
        {
            return ObtenerDoctores_Simulado().FirstOrDefault(d => d.IdDoctor == id);
        }
        private bool AgregarDoctor_Simulado(DoctorInfo doctor) { Debug.WriteLine($"Simulado: Agregando Doctor {doctor.NombreCompleto}"); return true; }
        private bool ActualizarDoctor_Simulado(DoctorInfo doctor) { Debug.WriteLine($"Simulado: Actualizando Doctor ID {doctor.IdDoctor}"); return true; }
        private bool EliminarDoctor_Simulado(int id) { Debug.WriteLine($"Simulado: Eliminando Doctor ID {id}"); return true; }
    }
}