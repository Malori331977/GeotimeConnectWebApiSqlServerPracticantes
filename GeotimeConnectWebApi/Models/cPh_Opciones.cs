using Microsoft.EntityFrameworkCore;

namespace GeoTimeConnectWebApi.Models
{
    public class cPh_Opciones
    {
        public int IDOPCION { get; set; }
        public char POST_EMP { get; set; }
        public char POST_SINC { get; set; }
        public int NUM_ALM { get; set; }
        public char UTILIZA_DESC { get; set; }
        public char DESC_ABIERT { get; set; }
        public string VER_DB { get; set; }
        public string CORTE_DIURNO { get; set; }
        public string CORTE_NOCTURNO { get; set; }
        public char? USA_ALERTA_EXD { get; set; }
        public string? ALERTA_EXTRAS_DIARIAS { get; set; }
        public string? COLOR_FONDO_ALERTD { get; set; }
        public string? COLOR_FUENTE_ALERTD { get; set; }
        public char? DIST_TADIC { get; set; }
        public string? DIST_LIC_USR { get; set; }
        public string? DIST_LIC_EMP { get; set; }
        public char? TIPO_DIST { get; set; }
        public char? ACC_BLOC_PT { get; set; }

    }
}
