using SoftBO.diagnosticoWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class DiagnosticoBO
    {
        private DiagnosticoWSClient diagnosticoCliente;
        public DiagnosticoBO()
        {
            this.diagnosticoCliente = new DiagnosticoWSClient();
        }
        public int InsertarDiagnostico(diagnosticoDTO diagnostico)
        {
            return this.diagnosticoCliente.insertarDiagnostico(diagnostico);
        }
        public diagnosticoDTO ObtenerPorIdDiagnostico(int diagnosticoId)
        {
            return this.diagnosticoCliente.obtenerPorIdDiagnostico(diagnosticoId);
        }
        public BindingList<diagnosticoDTO> ListarTodosDiagnostico()
        {
            diagnosticoDTO[] diagnosticos = this.diagnosticoCliente.listarTodosDiagnostico();
            return new BindingList<diagnosticoDTO>(diagnosticos);
        }
    }
}
