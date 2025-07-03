using SoftBO.SoftCitWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class RegistroBO
    {
        private RegistroWSClient registroCliente;
        public RegistroBO()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://52.70.76.31:8080/SoftCitWS/RegistroWS");
            this.registroCliente = new RegistroWSClient(binding, endpoint);
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
