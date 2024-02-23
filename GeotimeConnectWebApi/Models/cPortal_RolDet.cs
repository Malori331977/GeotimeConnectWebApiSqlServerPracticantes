namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_RolDet
    {
        public int PORTALROLID { get; set; }
        public int PORTALMENUID { get; set; }
        public int PORTALOPCIONID { get; set; }
        public bool HABILITADO { get; set; }

        public cPortal_Rol? cPortal_Rol { get; set; }


    }
}
