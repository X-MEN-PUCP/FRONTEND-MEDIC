using SoftBO.loginWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class LoginBO
    {
        private LoginWSClient loginCliente;
        public LoginBO()
        {
            this.loginCliente = new LoginWSClient();
        }
        public usuarioDTO IniciarSesion(string numeroDoc, string tipoDoc, string contrasenha)
        {
            return this.loginCliente.iniciarSesion(numeroDoc,tipoDoc,contrasenha);
        }
        public bool CerrarSesion(usuarioDTO usuario)
        {
            return this.loginCliente.cerrarSesion(usuario);
        }
    }
}
