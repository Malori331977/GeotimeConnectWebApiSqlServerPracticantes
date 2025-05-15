using System;

namespace com.gsitcr.geotime.Models
{
    public class cMarcaEditParam
    {
        public long idregistro { get; set; }
        public DateTime fecha_entra { get; set; }
        public DateTime fecha_sale { get; set; }
        public string hora_entra { get; set; }
        public string hora_sale { get; set; }
        public int idturno { get; set; }
        public string usuario { get; set; }
        public string comentario { get; set; }
    }
}
