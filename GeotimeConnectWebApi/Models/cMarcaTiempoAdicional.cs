using GeotimeConnectWebApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaTiempoAdicional
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IDREGISTRO { get; set; }
        public string IDPLANILLA { get; set; }
        public string IDNUMERO { get; set; }
        public string PERIODO { get; set; }
        public int IDCONCEPTO { get; set; }
        public string CANTIDAD { get; set; }
        public DateTime FECHA_REFERENCIA { get; set; }
        public string USUARIO { get; set; }
        
        public DateOnly? FECHA_REGISTRO { get; set; }
        public DateOnly? FECHA_ACTUALIZA { get; set; }
        public string? CENTRO_COSTO { get; set; }
        public string? COMENTARIO { get; set; }
        public string? USUARIO_ACTUALIZA { get; set; }
        public char ESTADO { get; set; }
        public decimal? TCANTIDAD { get; set; }
        public string? PROYECTO { get; set; }
        public string? FASE { get; set; }

        public cEmpleado? cEmpleado { get; set; }
        public cCentroCosto? cCentroCosto { get; set; }
        public cConcepto? cConcepto { get; set; }
    }
}
