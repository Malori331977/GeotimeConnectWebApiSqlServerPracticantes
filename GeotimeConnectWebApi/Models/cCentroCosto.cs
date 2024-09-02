namespace GeoTimeConnectWebApi.Models
{
    public class cCentroCosto
    {
        public String? IdCCosto { get; set; }
        public String? Descripcion { get; set; }
        public char? Distribuye { get; set; }
        public string? Alias_CCosto { get; set; }

        public IEnumerable<cEmpleado>? Empleado { get; set; }
        public IEnumerable<cMarcaTiempoAdicional>? cMarcaTiempoAdicionals { get; set; }

    }
}
