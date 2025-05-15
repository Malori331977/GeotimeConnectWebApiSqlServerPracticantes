namespace com.gsitcr.geotime.Models
{
    public class cPh_RolSistema
    {
        public string ID { get; set; }
        public string DESCRIPCION { get; set; }
        public bool HABILITADO { get; set; }

        public IEnumerable<cPh_RolSistemaDet>? cPh_RolSistemaDet { get; set; }
    }
}
