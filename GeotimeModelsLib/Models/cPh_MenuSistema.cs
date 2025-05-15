namespace com.gsitcr.geotime.Models
{
    public class cPh_MenuSistema
    {
        public string ID { get; set; }
        public string MENUTEXT { get; set; }
        public int ICONID { get; set; }

        public IEnumerable<cPh_OpcionSistema>? cPh_OpcionSistema {  get; set; }


    }
}
