namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaDistribucion
    {
        //public long IDREGISTRO { get; set; }
        public string IDPLANILLA { get; set; }
        public string IDNUMERO { get; set; }
        public DateTime FECHA { get; set; }
        //public int idconcepto { get; set; }
        public string NOMINAEQ { get; set; }
        public decimal CANTIDAD { get; set; }
        public string? IDCCOSTO { get; set; }
        /*public string? concepto { get; set; }
        public string? proyecto { get; set; }
        public string? fase { get; set; }
        public char? tipo { get; set; }
        public char? estado { get; set; }
        public string entrada { get; set; }*/
    }
}
