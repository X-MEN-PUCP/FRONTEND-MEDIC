using SoftBO.diagnosticoporcitaWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SoftBO
{
    public class DiagnosticoPorCitaBO
    {
        private DiagnosticoPorCitaWSClient DiagnosticoPorCitaClient;
        public DiagnosticoPorCitaBO()
        {
            this.DiagnosticoPorCitaClient = new DiagnosticoPorCitaWSClient();
        }
        public int InsertarDiagnosticoPorCita(diagnosticoPorCita diagnostico)
        {
            return this.DiagnosticoPorCitaClient.InsertarDiagnosticoPorCita(diagnostico);
        }
        public BindingList<diagnosticoPorCita> ListarDiagnosticoPorIdCita(int idCita) 
        {
            diagnosticoPorCita[] diagnosticos = this.DiagnosticoPorCitaClient.ListarDiagnosticoPorIdCita(idCita);
            return new BindingList<diagnosticoPorCita>(diagnosticos);
        }
        public BindingList<diagnosticoPorCita> ListarTodosLosDiagnosticos()
        {
            diagnosticoPorCita[] diagnosticos = this.DiagnosticoPorCitaClient.ListarTodosLosDiagnosticos();
            return new BindingList<diagnosticoPorCita>(diagnosticos);
        }
    }
}
