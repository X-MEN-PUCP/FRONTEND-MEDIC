using SoftBO.historiaWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class HistoriaBO
    {
        private HistoriaWSClient historiaCliente;
        public HistoriaBO()
        {
            this.historiaCliente = new HistoriaWSClient();
        }
        public BindingList<historiaClinicaDTO> ListarHistoria()
        {
            historiaClinicaDTO[] historias = this.historiaCliente.listarHistoria();
            return new BindingList<historiaClinicaDTO>(historias);
        }
        public int InsertarHistoria(historiaClinicaDTO historia)
        {
            return this.historiaCliente.insertarHistoria(historia);
        }
        public historiaClinicaDTO ObtenerHistoriaPorIdPaciente(int id)
        {
            return this.historiaCliente.obtenerHistoriaPorIdPaciente(id);
        }
        public historiaClinicaDTO ObtenerHistoriaPorId(int id)
        {
            return this.historiaCliente.obtenerHistoriaPorId(id);
        }
    }
}
