using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cVMarcaComedor
    {
        public string idnumero { get; set; }
        public DateTime fecha_entra { get; set; }
        public DateTime fecha_sale { get; set; }
        public string hora_entra { get; set; }
        public string hora_sale { get; set; }
        public string desc1_ini { get; set; }
        public string desc1_fin { get; set; }
        public string desc2_ini { get; set; }
        public string desc2_fin { get; set; }
        public string desc3_ini { get; set; }
        public string desc3_fin { get; set; }
        public string idterminal { get; set; }
    }
}
