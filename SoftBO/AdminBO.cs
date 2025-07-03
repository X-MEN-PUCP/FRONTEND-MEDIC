using SoftBO.SoftCitWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class AdminBO
    {
        private AdminWSClient adminCliente;

        public AdminBO()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://52.70.76.31:8080/SoftCitWS/AdminWS");
            this.adminCliente = new AdminWSClient(binding, endpoint);
        }

        public int AsignarNuevoRolParaUsuario(usuarioPorRolDTO usuario)
        {
            return this.adminCliente.asignarNuevoRolParaUsuario(usuario);
        }
        public int EliminarRolDeUsuario(usuarioPorRolDTO usuario)
        {
            return this.adminCliente.eliminarRolDeUsuario(usuario);
        }
        public int InsertarNuevaEspecialidad(especialidadDTO especialidad)
        {
            return this.adminCliente.insertarNuevaEspecialidad(especialidad);
        }
        public BindingList<usuarioDTO> ListarMedicos()
        {
            usuarioDTO[] medicos = this.adminCliente.listarMedicos();
            return new BindingList<usuarioDTO>(medicos);
        }
        public BindingList<usuarioDTO> ListarTodosUsuarios()
        {
            usuarioDTO[] usuarios = this.adminCliente.listarTodosUsuarios();
            return new BindingList<usuarioDTO>(usuarios);
        }

        public BindingList<especialidadDTO> ListarEspecialidades()
        {
            especialidadDTO[] especialidades = this.adminCliente.listarEspecialidades();
            return new BindingList<especialidadDTO>(especialidades);
        }

        public especialidadDTO ObtenerEspecialidadPorId(int idEspecialidad)
        {
            return this.adminCliente.obtenerEspecialidadPorId(idEspecialidad);
        }

        public int ModificarEspecialidad(especialidadDTO especialidad)
        {
            return this.adminCliente.modificarEspecialidads(especialidad);
        }

        public BindingList<usuarioPorEspecialidadDTO> ListarUsuariosPorEspecialidad(int idEspecialidad)
        {
            usuarioPorEspecialidadDTO[] usuarios = this.adminCliente.listarUsuariosPorEspecialidad(idEspecialidad);
            return new BindingList<usuarioPorEspecialidadDTO>(usuarios ?? new usuarioPorEspecialidadDTO[0]);
        }

        public BindingList<usuarioPorRolDTO> ListarRolesDeUsuario(int idUsuario)
        {
            var lista = this.adminCliente.listarRolesDeUsuario(idUsuario);
            return new BindingList<usuarioPorRolDTO>(lista);
        }

        public usuarioDTO ObtenerUsuarioPorId(int idUsuario)
        {
            return this.adminCliente.obtenerUsuarioPorId(idUsuario);
        }

        public int ModificarUsuario(usuarioDTO usuario)
        {
            return this.adminCliente.modificarUsuarios(usuario);
        }

        public int AgregarEspecialidadAMedico(usuarioPorEspecialidadDTO usuarioPorEspecialidad)
        {
            return this.adminCliente.agregarEspecialidadAMedico(usuarioPorEspecialidad);
        }

        public int EliminarEspecialidadMedico(usuarioPorEspecialidadDTO usuarioPorEspecialidad)
        {
            return this.adminCliente.eliminarEspecialidadMedico(usuarioPorEspecialidad);
        }

        public int InsertarNuevoAdministrador(usuarioDTO administrador)
        {
            return this.adminCliente.insertarNuevoAdministrador(administrador);
        }

        public int InsertarNuevoPaciente(usuarioDTO paciente)
        {
            return this.adminCliente.insertarNuevoPaciente(paciente);
        }

        public int InsertarNuevoMedico(usuarioDTO medico, BindingList<especialidadDTO> especialidades)
        {
            return this.adminCliente.insertarNuevoMedico(medico, especialidades.ToArray());
        }
        public BindingList<reporteCitaDTO> ReporteDeCitasGeneralAdministrador(int idEspecialidad, int idMedico, string fechaInicio, string fechaFin)
        {
            reporteCitaDTO[] lista = this.adminCliente.ReporteDeCitasGeneralAdministrador(idEspecialidad, idMedico, fechaInicio, fechaFin);
            return new BindingList<reporteCitaDTO>(lista ?? new reporteCitaDTO[0]);
        }

        public int modificarUsuarioPoneContraDefault(usuarioDTO usuario)
        {
            return this.adminCliente.modificarUsuarioPoneContraDefault(usuario);
        }
    }
}
