namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_RolDet
    {
        public string PORTALROLID { get; set; }
        public string PORTALMENUID { get; set; }
        public string PORTALOPCIONID { get; set; }
        public bool HABILITADO { get; set; }

        public cPortal_Rol? cPortal_Rol { get; set; }


    }
}
