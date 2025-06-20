using SoftBO.consultorioWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SoftBO
{
    public class ConsultorioBO
    {
        private ConsultorioWSClient consultorioCliente;
        public ConsultorioBO()
        {
            this.consultorioCliente = new ConsultorioWSClient();
        }
        public int InsertarConsultorio(consultorioDTO consultorio)
        {
            return this.consultorioCliente.insertarConsultorio(consultorio);
        }
        public int ModificarConsultorio(consultorioDTO consultorio)
        {
            return this.consultorioCliente.modificarConsultorio(consultorio);
        }
        public consultorioDTO obtenerPorIdConsultorio(int consultorioId)
        {
            return this.consultorioCliente.obtenerPorIdConsultorio(consultorioId);
        }
        public BindingList<consultorioDTO> ListarTodosConsultorio()
        {
            consultorioDTO[] consultorios = this.consultorioCliente.listarTodosConsultorio();
            return new BindingList<consultorioDTO>(consultorios);
        }
    }
}
