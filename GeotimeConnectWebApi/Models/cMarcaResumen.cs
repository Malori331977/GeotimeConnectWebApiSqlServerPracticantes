using GeotimeConnectWebApi.Models;

namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaResumen
    {
        public String? IdPlanilla { get; set; }
        public String? IdNumero { get; set; }
        public int IdConcepto { get; set; } 
        public String? NominaEq { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Monto { get; set; }
        public String? IdCCosto { get; set; }
        public String? Proyecto { get; set; }
        public String? Fase { get; set; }
        public String? IdPeriodo { get; set; }
    }
}

