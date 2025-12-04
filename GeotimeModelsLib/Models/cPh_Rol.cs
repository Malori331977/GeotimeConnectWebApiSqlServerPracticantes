namespace com.gsitcr.geotime.Models
{
    public class cPh_Rol
    {
        public int IDROL { get; set; }
        public string DESCRIPCION { get; set; }

        public List<cPh_RolTurno>? cPh_RolTurno { get; set; }
    }
}
