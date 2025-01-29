using System.ComponentModel.DataAnnotations.Schema;

namespace GeoTimeConnectWebApi.Models
{
    public class cPh_UsuarioRol
    {
        public int IDUSUARIO { get; set; }
        public int IDREGISTRO { get; set; }
        public string ROL { get; set; }
        public int IDUSUARIOREGISTRA { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public int IDUSUARIOMODIFICA { get; set; }
        public DateTime FECHAMODIFICA { get; set; }

        [NotMapped]
        public string? ROLID { get; set; }

        [NotMapped]
        public bool? HABILITADO { get; set; }
    }
}
