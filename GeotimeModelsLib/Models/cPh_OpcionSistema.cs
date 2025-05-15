namespace com.gsitcr.geotime.Models
{
    public class cPh_OpcionSistema
    {
        public string ID { get; set; }
        public bool PRINCIPAL { get; set; }
        public string? HREF { get; set; }
        public int ICONID { get; set; }
        public string MENUTEXT { get; set; }
        public string? PARENTID { get; set; }
        public cPh_MenuSistema? cPh_MenuSistema { get; set; }
    }
}
