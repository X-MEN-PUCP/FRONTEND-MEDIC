using SoftBO.interconsultaWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class InterconsultaBO
    {
        private InterconsultaWSClient interconsultaCliente;
        public InterconsultaBO()
        {
            this.interconsultaCliente = new InterconsultaWSClient();
        }
        public int InsertarInterconuslta(interconsultaDTO turno)
        {
            return this.interconsultaCliente.insertarInterconuslta(turno);
        }
        public interconsultaDTO ObtenerPorIdInterconuslta(int idEspecialidad, int idCita)
        {
            return this.interconsultaCliente.obtenerPorIdInterconuslta(idEspecialidad, idCita);
        }
        public BindingList<interconsultaDTO> ListarTodosInterconuslta()
        {
            interconsultaDTO[] interconsultas = this.interconsultaCliente.listarTodosInterconuslta();
            return new BindingList<interconsultaDTO>(interconsultas);
        }
    }
}
