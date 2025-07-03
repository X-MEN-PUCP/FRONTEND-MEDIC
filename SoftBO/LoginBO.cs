using SoftBO.SoftCitWS;
using System.ServiceModel;

namespace SoftBO
{
    public class LoginBO
    {
        private SoftCitWS.LoginWSClient loginCliente;
        public LoginBO()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://52.70.76.31:8080/SoftCitWS/LoginWS");
            loginCliente = new LoginWSClient(binding, endpoint);
        }
        public usuarioDTO IniciarSesion(string numeroDoc, string tipoDoc, string contrasenha)
        {
            return this.loginCliente.iniciarSesion(numeroDoc, tipoDoc, contrasenha);
        }
        public bool CerrarSesion(usuarioDTO usuario)
        {
            return this.loginCliente.cerrarSesion(usuario);
        }
    }
}
