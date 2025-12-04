using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace com.gsitcr.geotime.Models
{
    public class cPh_RolTurno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDREGISTRO { get; set; }
        [Key]
        public int IDROL { get; set; }
        public int IDTURNO { get; set; }

        public cPh_Rol? cPh_Rol { get; set; }
        public cTurno? cTurno { get; set; }
    }
}
