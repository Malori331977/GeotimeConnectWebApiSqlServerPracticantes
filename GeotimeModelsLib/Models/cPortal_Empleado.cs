using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{
    public class cPortal_Empleado
    {
        public string IDNUMERO { get; set; }
        public string PORTALROLID { get; set; }
        public bool HABILITADO { get; set; }

        [NotMapped]
        public int USUARIOS_HABILITADOS { get; set; }
    }
}
