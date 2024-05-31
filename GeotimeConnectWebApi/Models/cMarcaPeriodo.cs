namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaPeriodo
    {
        public string idnumero { get; set; }
        public string nombre { get; set; }
        public DateTime fecha_entra { get; set; }
        public string hora_entra { get; set; }
        public string hora_sale { get; set; }
        public int idturno { get; set; }
        public string turno { get; set; }
        public decimal ordinario { get; set; }
        public decimal extras { get; set; }
        public decimal suma_extras { get; set; }
        public decimal suma_dobles { get; set; }
        public decimal suma_otros { get; set; }        
        public string estado { get; set; }
        public string mtardia { get; set; }
        public string manticipo { get; set; }
    }
}
