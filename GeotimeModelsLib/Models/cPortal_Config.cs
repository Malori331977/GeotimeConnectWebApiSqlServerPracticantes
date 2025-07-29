using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{
    public class cPortal_Config
    {
        public string IDAPLICACION { get; set; }
        public string IDVERSION { get; set; }
        public string COMPANIA { get; set; }
        public string BASEDATOS { get; set; }
        public string IDLICENCIA { get; set; }
        public bool ACTIVA { get; set; }
        public bool USORESTRINGIDO { get; set; }
        public string REGSITROLIC { get; set; }
        public bool PERMANENTE { get; set; }
        public bool USARECONOCIMIENTOFACIAL { get; set; }
        public bool USARGEOLOCALIZACION { get; set; }
        public string? MAPAPIKEY { get; set; }
        public decimal FACEDIST { get; set; }
        public decimal FACETEXT { get; set; }
        public DateTime FECHAULTMODIFICA { get; set; }
        public string IDUSUARIOMODIFICA { get; set; }
        public bool VERLOGMARCAS { get; set; }

        [NotMapped]
        public string ORGANIZACIONBASEID { get; set; }

    }
}
