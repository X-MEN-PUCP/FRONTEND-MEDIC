using SoftBO.SoftCitWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class UsuarioBO
    {
        private UsuarioWSClient usuarioCliente;
        public UsuarioBO()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://52.70.76.31:8080/SoftCitWS/UsuarioWS");
            this.usuarioCliente = new UsuarioWSClient(binding, endpoint);
        }
        public usuarioDTO ObtenerPorIdUsuario(int id)
        {
            return this.usuarioCliente.obtenerPorIdUsuario(id);
        }
        public usuarioDTO BuscarCuentaUsuario(string documento, string tipoDoc, string contrasenha)
        {
            return this.usuarioCliente.buscarCuentaUsuario(documento, tipoDoc, contrasenha);
        }
        public int InsertarUsuario(usuarioDTO usuario)
        {
            return this.usuarioCliente.insertarUsuario(usuario);
        }
        public int ModificarUsuario(usuarioDTO usuario)
        {
            return this.usuarioCliente.modificarUsuario(usuario);
        }
        public int CambiarEstadoGeneralUsuario(usuarioDTO usuario, int estadoGeneral)
        {
            return this.usuarioCliente.cambiarEstadoGeneralUsuario(usuario, estadoGeneral);
        }
        public int CambiarEstadoLogicoUsuario(usuarioDTO usuario, int estadoLogico)
        {
            return this.usuarioCliente.cambiarEstadoLogicoUsuario(usuario, estadoLogico);
        }
        public usuarioDTO CompletarRolesUsuario(usuarioDTO usuario)
        {
            return this.usuarioCliente.completarRolesUsuario(usuario);
        }
        public int CambiarContraseñaUsuario(usuarioDTO usuario, string nuevaContraseña)
        {
            return this.usuarioCliente.CambiarContrasenhaUsuario(usuario, nuevaContraseña);
        }

        public BindingList<usuarioPorEspecialidadDTO> listarEspecialidadesPorUsuarioParaUsaurio(int id)
        {
            usuarioPorEspecialidadDTO[] arreglo = this.usuarioCliente.listarEspecialidadesPorUsuarioParaUsaurio(id);
            return new BindingList<usuarioPorEspecialidadDTO>(arreglo);
        }
    }
}
