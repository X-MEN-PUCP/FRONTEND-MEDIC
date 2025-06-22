using SoftBO.adminWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class AdminBO
    {
        private AdminWSClient adminCliente;

        public AdminBO()
        {
            this.adminCliente = new AdminWSClient();
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
        public int InsertarNuevoMedico(usuarioDTO medico, especialidadDTO especialidad)
        {
            return this.adminCliente.insertarNuevoMedico(medico, especialidad);
        }
    }
}
