using SoftBO.especialidadWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO
{
    public class EspecialidadBO
    {
        private EspecialidadWSClient especialidadCliente;
        public EspecialidadBO()
        {
            this.especialidadCliente = new EspecialidadWSClient();
        }
        public BindingList<especialidadDTO> ListarEspecialidad()
        {
            especialidadDTO[] especialidades = this.especialidadCliente.listarEspecialidad();
            return new BindingList<especialidadDTO>(especialidades);
        }
        public int InsertarEspecialidad(especialidadDTO especialidad)
        {
            return this.especialidadCliente.insertarEspecialidad(especialidad);
        }
        //public int ModificarEspecialidad(especialidadDTO especialidad)
        //{
        //    return this.especialidadCliente.modificarEspecialidad(especialidad);
        //}
        public int CambiarEstadoEspecialidad(especialidadDTO especialidad, int estado)
        {
            return this.especialidadCliente.cambiarEstadoEspecialidad(especialidad, estado);
        }
        public especialidadDTO ObtenerPorIdTablaEspecialidad(int id)
        {
            return this.especialidadCliente.obtenerPorIdTablaEspecialidad(id);
        }
    }
}
