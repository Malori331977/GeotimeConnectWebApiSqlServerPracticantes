namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_Rol
    {
        public int ID { get; set; }
        public string DESCRIPCION { get; set; }
        public bool ROLDEFAULT { get; set; }

        public IEnumerable<cPortal_RolDet>? cPortal_RolDet { get; set; }
    }
}
