using SoftBO.examenWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class ExamenBO
    {
        private ExamenWSClient examenCliente;
        public ExamenBO()
        {
            this.examenCliente = new ExamenWSClient();
        }
        public int InsertarExamen(examenDTO examen)
        {
            return this.examenCliente.insertarExamen(examen);
        }
        public examenDTO ObtenerPorIdTablaExamen(int examenId)
        {
            return this.examenCliente.obtenerPorIdTablaExamen(examenId);
        }
        public BindingList<examenDTO> ListarTodosTablaExamen()
        {
            examenDTO[] examenes = this.examenCliente.listarTodosTablaExamen();
            return new BindingList<examenDTO>(examenes);
        }
    }
}
