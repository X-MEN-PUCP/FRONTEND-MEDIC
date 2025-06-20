using SoftBO.tipoexamenWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class TipoExamenBO
    {
        private TipoExamenWSClient tipoExamenCliente;
        public TipoExamenBO()
        {
            this.tipoExamenCliente = new TipoExamenWSClient();
        }
        public int InsertarTipoExamen(tipoExamenDTO tipoExamen)
        {
            return this.tipoExamenCliente.insertarTipoExamen(tipoExamen);
        }
        public int ModificarTipoExamen(tipoExamenDTO tipoExamen)
        {
            return this.tipoExamenCliente.modificarTipoExamen(tipoExamen);
        }
        public BindingList<tipoExamenDTO> ListarTodosTipoExamen()
        {
            tipoExamenDTO[] tipos = this.tipoExamenCliente.listarTodosTipoExamen();
            return new BindingList<tipoExamenDTO>(tipos);
        }
        public tipoExamenDTO ObtenerPorIdTipoExamen(int id)
        {
            return this.tipoExamenCliente.obtenerPorIdTipoExamen(id);
        }
    }
}
