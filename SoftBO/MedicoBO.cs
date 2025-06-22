using SoftBO.medicoWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class MedicoBO
    {
        private MedicoWSClient medicoCliente;
        public MedicoBO()
        {
            this.medicoCliente = new MedicoWSClient();
        }
        public BindingList<citaDTO> ListarCitasMedicos(int codMedico,estadoCita estadoCita)
        {
            citaDTO[] citas = this.medicoCliente.listarCitasMedico(codMedico, estadoCita);
            return new BindingList<citaDTO>(citas);
        }
        public int LlenarEpicrisisMedico(historiaClinicaPorCitaDTO epicrisis)
        {
            return this.medicoCliente.llenarEpicrisisMedico(epicrisis);
        }
    }
}
