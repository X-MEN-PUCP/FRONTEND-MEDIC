using SoftBO.rolesporusuarioWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class RolesPorUsuarioBO
    {
        private RolesPorUsuarioWSClient rolesPorUsuarioCliente;
        public RolesPorUsuarioBO()
        {
            this.rolesPorUsuarioCliente = new RolesPorUsuarioWSClient();
        }
        public BindingList<usuarioPorRolDTO> ListarPorUsuarioRolesPorUsuario(int id)
        {
            return new BindingList<usuarioPorRolDTO>(this.rolesPorUsuarioCliente.listarPorUsuarioRolesPorUsuario(id));
        }
        public int InsertarRolesPorUsuario(usuarioPorRolDTO usuarioPorRol)
        {
            return this.rolesPorUsuarioCliente.insertarRolesPorUsuario(usuarioPorRol);
        }
    }
}
