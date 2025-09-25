using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{
    public class cSolicitudDetalle
    {
        [Key]
        public long SolicitudId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRegistro { get; set; }
        public DateTime Fecha { get; set; }
        public string? IdCCosto { get; set; }
        public string? Proyecto { get; set; }
        public string? Fase { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string TotalHoras { get; set; }
        public decimal Cantidad { get; set; }
        public string IdUsuarioRegistra { get; set; }
        public DateTime FechaRegistro { get; set; }

        public cSolicitud? cSolicitud { get; set; }
    }
}
