using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeotimeConnectWebApi.Models
{
    public class cIncidencia_Conf_Pago
    {
        public int ID { get; set; }
        public string DESCRIPCION { get; set; }
        public int ID_APL { get; set; }
        public int ID_HRS { get; set; }
        public int ID_CON { get; set; }
        public int ID_ADICIONAL { get; set; }
        public int? APL_TURNO { get; set; }
        public int? TRAN_TURNO { get; set; }

    }
}
