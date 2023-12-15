namespace GeoTimeConnectWebApi.Models
{
    public class cDepartamento
    {
        public String? IDDEPART { get; set; }
        public String? DESCRIPCION { get; set; }

        public IEnumerable<cEmpleado>? Empleado { get; set; }

    }
}
