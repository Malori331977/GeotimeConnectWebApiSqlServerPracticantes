using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeoTimeConnectWebApi.Models
{
    public class cPortal_DocMarca
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IDREGISTRO { get; set; }
        public string IDNUMERO { get; set; }
        public DateTime FECHA { get; set; }
        public byte[]? DOCUMENTO { get; set; }
        public string? OBSERVACIONES { get; set; }
        public char TIPO { get; set; }
    }
}
