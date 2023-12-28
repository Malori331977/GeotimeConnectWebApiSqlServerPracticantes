using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeoTimeConnectWebApi.Models
{
    public class cPh_RolTurno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDREGISTRO { get; set; }
        [Key]
        public int IDROL { get; set; }
        public int IDTURNO { get; set; }

        public cTurno? Turno { get; set; }
    }
}
