namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_Menu
    {
        public string ID { get; set; }
        public string MENUTEXT { get; set; }
        public int ICONID { get; set; }

        public IEnumerable<cPortal_Opcion>? cPortal_Opcion {  get; set; }


    }
}
