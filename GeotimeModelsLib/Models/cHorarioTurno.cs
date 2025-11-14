using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cHorarioTurno
    {
        public int Dia { get; set; }
        public int IdTurno { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
    }
}
