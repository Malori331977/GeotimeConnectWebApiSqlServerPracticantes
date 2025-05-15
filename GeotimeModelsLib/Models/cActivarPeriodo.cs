namespace com.gsitcr.geotime.Models
{
    public class cActivarPeriodo
    {
        public string IdPlanilla { get; set; }
        public int Grupo { get; set; }
        public string Periodo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int Usuario { get; set; }

    }
}
