using SoftBO.usuarioporespecialidadWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class UsuarioPorEspecialidadBO
    {
        private UsuarioPorEspecialidadWSClient usuarioPorEspecialidadCliente;
        public UsuarioPorEspecialidadBO()
        {
            this.usuarioPorEspecialidadCliente = new UsuarioPorEspecialidadWSClient();
        }
        public BindingList<usuarioPorEspecialidadDTO> ListarPorUsuarioUsuarioPorEspecialidad(int id)
        {
            usuarioPorEspecialidadDTO[] usuarios = this.usuarioPorEspecialidadCliente.listarPorUsuarioUsuarioPorEspecialidad(id);
            return new BindingList<usuarioPorEspecialidadDTO>(usuarios);
        }
        public BindingList<usuarioPorEspecialidadDTO> ListarPorEspecialidadUsuarioPorEspecialidad(int id)
        {
            usuarioPorEspecialidadDTO[] usuarios = this.usuarioPorEspecialidadCliente.listarPorEspecialidadUsuarioPorEspecialidad(id);
            return new BindingList<usuarioPorEspecialidadDTO>(usuarios);
        }
        public int InsertarUsuarioPorEspecialidad(usuarioPorEspecialidadDTO usuarioPorEspecialidad)
        {
            return this.usuarioPorEspecialidadCliente.insertarUsuarioPorEspecialidad(usuarioPorEspecialidad);
        }
    }
}
