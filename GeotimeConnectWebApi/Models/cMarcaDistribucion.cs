using GeotimeConnectWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoTimeConnectWebApi.Models
{
    public class cMarcaDistribucion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IDREGISTRO { get; set; }
        public string IDPLANILLA { get; set; }
        public string IDNUMERO { get; set; }
        public DateTime FECHA { get; set; }
        public int IDCONCEPTO { get; set; }
        public string NOMINAEQ { get; set; }
        public decimal CANTIDAD { get; set; }
        public string? IDCCOSTO { get; set; }
        public string? PROYECTO { get; set; }
        public string? FASE { get; set; }
        public string? CONCEPTO { get; set; }
        public char? TIPO { get; set; }
        public char? ESTADO { get; set; }
        public string ENTRADA { get; set; }

        public cConcepto? cConcepto { get; set; }
    }
}
