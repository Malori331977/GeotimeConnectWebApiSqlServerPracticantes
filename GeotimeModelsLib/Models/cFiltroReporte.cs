using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeotimeModelsLib.Models
{
    public class cFiltroReporte
    {
        public IEnumerable<string>? Planillas { get; set; }
        public IEnumerable<string>? Departamentos { get; set; }
        public IEnumerable<string>? CentrosCosto { get; set; }
        public IEnumerable<int>? Grupos { get; set; }
        public IEnumerable<string>? Empleados { get; set; }
        public IEnumerable<int>? Incidencias { get; set; }
        public DateTime inicio { get; set; } = DateTime.Now;
        public DateTime fin { get; set; } = DateTime.Now;

        public string RptName { get; set; } = string.Empty;
    }
}
