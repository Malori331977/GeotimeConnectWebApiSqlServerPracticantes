using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeoTimeConnectWebApi.Models
{
    public class cTransformacionTipoMarcaDet
    {
        [Key]
        public int TRANSFORMACIONID { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDREGISTRO { get; set; }
        public bool VERIFICA_MARCA { get; set; }
        public string? HORA_INICIO { get; set; }
        public string? HORA_FIN { get; set; }
        public int TIPO_MARCA { get; set; }
        public int TIPO_MARCA_TRANS { get; set; }
        public bool TIPO_DEFAULT { get; set; }
    }
}
