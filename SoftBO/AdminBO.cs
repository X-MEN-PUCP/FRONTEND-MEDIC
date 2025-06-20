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

        public int AsignarNuevoRolParaUsuario(usuarioDTO usuario, int idRol)
        {
            return this.adminCliente.asignarNuevoRolParaUsuario(usuario, idRol);
        }
    }
}
