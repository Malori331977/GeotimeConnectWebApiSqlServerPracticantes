using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cPh_Distribucion_CCosto
    {

        public int idregistro { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string idccosto { get; set; }
        public string? proyecto { get; set; }
        public string? fase { get; set; }
    }
}
