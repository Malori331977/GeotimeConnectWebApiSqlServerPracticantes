namespace GeotimeConnectWebApi.Models
{
    public class cDashboardStatusDiario
    {
        public int TotalEmpleados { get; set; }
        public int EmpleadosPresentes { get; set; }
        public int AusenciasJustificadas { get; set; }
        public int AusenciasInjustificadas { get; set; }
        public int Tardias { get; set; }
    }
}
