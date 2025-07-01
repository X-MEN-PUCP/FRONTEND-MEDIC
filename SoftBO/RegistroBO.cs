using SoftBO.registroWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class RegistroBO
    {
        private RegistroWSClient registroCliente;
        public RegistroBO()
        {
            this.registroCliente = new RegistroWSClient();
        }
        public int Registrarse(usuarioDTO usuario)
        {
            return this.registroCliente.registrarse(usuario);
        }
        public usuarioDTO VerificarCodigo(string correo, string codigo)
        {
            return this.registroCliente.verificarCodigo(correo, codigo);
        }
        public bool ReenviarCodigo(string correo)
        {
            return this.registroCliente.reenviarCodigo(correo);
        }
    }
}
