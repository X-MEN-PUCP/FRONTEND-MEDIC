using SoftBO.reporteCitasWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class ReporteCitasBO
    {
        private ReporteCitasWSClient reporteCitasCliente;
        public ReporteCitasBO()
        {
            this.reporteCitasCliente = new ReporteCitasWSClient();
        }
        public BindingList<reporteCitaDTO> ReporteCitasGeneral(int idEspecialidad, int codMedico, string fechaInicio, string fechaFin)
        {
            reporteCitaDTO[] reportes = this.reporteCitasCliente.ReporteDeCitasGeneral(idEspecialidad, codMedico, fechaInicio, fechaFin);
            return new BindingList<reporteCitaDTO>(reportes);
        }
    }
}
