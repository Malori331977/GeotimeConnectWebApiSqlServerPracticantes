namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_Rol
    {
        public string ID { get; set; }
        public string DESCRIPCION { get; set; }
        public bool ROLDEFAULT { get; set; }
        public bool HABILITADO { get; set; }

        public IEnumerable<cPortal_RolDet>? cPortal_RolDet { get; set; }
    }
}
