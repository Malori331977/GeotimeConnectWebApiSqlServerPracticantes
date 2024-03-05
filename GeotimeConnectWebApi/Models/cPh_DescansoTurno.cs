namespace GeoTimeConnectWebApi.Models
{
    public class cPh_DescansoTurno
    {
        public int IDTURNO { get; set; }
        public int IDTIEMPO { get; set; }
        public string INICIO { get; set; }
        public string FIN { get; set; }
        public string TIEMPO { get; set; }
        public char? DESCUENTA { get; set; }
        public char? TIEMP_EXT { get; set; }
        public char? DESC_EXC { get; set; }
    }
}
