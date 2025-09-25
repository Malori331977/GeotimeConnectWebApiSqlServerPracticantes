using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cVHistoricoMarca
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string? IdNumero { get; set; }
        public string? Nombre { get; set; }       
        public string? DescGrupo { get; set; }
        public string? DescDepartamento { get; set; }
        public string? DescPlanilla { get; set; }
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
        public string descanso1 { get; set; }
        public string descanso2 { get; set; }
        public string descanso3 { get; set; }
    }
}
