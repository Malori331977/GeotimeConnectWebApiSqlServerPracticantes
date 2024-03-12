namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_DocMarca
    {
        public string IDNUMERO { get; set; }
        public DateTime FECHA { get; set; }
        public byte[]? DOCUMENTO { get; set; }
        public string? OBSERVACIONES { get; set; }
        public char TIPO { get; set; }
    }
}
