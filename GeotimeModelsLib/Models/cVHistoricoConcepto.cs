using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cVHistoricoConcepto
    {
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public string? IdNumero { get; set; }
        public string? Nombre { get; set; }
        public string? DescGrupo { get; set; }
        public string? DescDepartamento { get; set; }
        public string? DescPlanilla { get; set; }
        public int IdConcepto { get; set; }
        public string Codigo { get; set; }
        public string DescConcepto { get; set; }
        public decimal Cantidad { get; set; }
        public string? CCosto { get; set; }
        public string? Proyecto { get; set; }
        public string? Fase { get; set; }
    }
}
