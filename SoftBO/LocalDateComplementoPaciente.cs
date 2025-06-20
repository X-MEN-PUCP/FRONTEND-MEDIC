using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBO.pacienteWS
{
    public partial class localDate
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }

        public localDate() { }

        public localDate(System.DateTime fecha)
        {
            this.year = fecha.Year;
            this.month = fecha.Month;
            this.day = fecha.Day;
        }
    }
}
