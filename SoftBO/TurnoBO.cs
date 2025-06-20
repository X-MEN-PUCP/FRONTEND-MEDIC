using SoftBO.turnoWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class TurnoBO
    {
        private TurnoWSClient turnoCliente;
        public TurnoBO()
        {
            this.turnoCliente = new TurnoWSClient();
        }
        public int ModificarTurno(turnoDTO turno)
        {
            return this.turnoCliente.modificarTurno(turno);
        }
        public turnoDTO ObtenerPorIdTurno(int id)
        {
            return this.turnoCliente.obtenerPorIdTurno(id);
        }
        public BindingList<turnoDTO> ListarTodosTurno()
        {
            turnoDTO[] turnos = this.turnoCliente.listarTodosTurno();
            return new BindingList<turnoDTO>(turnos);
        }
    }
}
