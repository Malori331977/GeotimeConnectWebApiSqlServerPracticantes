namespace GeoTimeConnectWebApi.Models
{
    public class cCierroPeriodo
    {
        public string IdPlanilla { get; set; }
        public string Grupo { get; set; }
        public string Periodo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int Usuario { get; set; }
        public char Est_M { get; set; }

    }
}
