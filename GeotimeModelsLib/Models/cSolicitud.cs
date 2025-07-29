using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.gsitcr.geotime.Models
{
    public class cSolicitud
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string IdPlanilla { get; set; }
        public string IdNumero { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TipoSolicitudId { get; set; }
        public string? IdDepart { get; set; }
        public int? IdGrupo { get; set; }
        public string? IdCCosto { get; set; }
        public string? proyecto { get; set; }
        public string? fase { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string TotalHoras { get; set; }
        public decimal Cantidad { get; set; }
        public string? Comentario { get; set; }
        public int EstadoId { get; set; }
        public string IdUsuarioRegistra { get; set; }
        public DateTime FechaRegistro { get; set; }

        public IEnumerable<cSolicitudAutorizacion>? cSolicitudAutorizacion { get; set; }
        public cEstado? cEstado { get; set; }
        public cTipoSolicitud? cTipoSolicitud { get; set; }

    }
}
