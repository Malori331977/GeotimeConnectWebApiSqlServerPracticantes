using com.gsitcr.geotime.Models;
using System.ComponentModel.DataAnnotations;

namespace com.gsitcr.geotime.Models
{

    public class cMarcaMovTurnoBitacora
    {
        [Display(Name = "ID Bitácora")]
        public long idbitacora { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? fecha_modificacion { get; set; }

        [Display(Name = "Usuario que Modifica")]
        public string UsuarioModifica { get; set; }

        [Display(Name = "ID Registro")]
        public long idregistro { get; set; }

        [Display(Name = "ID Planilla")]
        public string idplanilla { get; set; }

        [Display(Name = "ID Número")]
        public string idnumero { get; set; }

        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }

        [Display(Name = "Hora")]
        public string hora { get; set; }

        [Display(Name = "Turno")]
        public int turno { get; set; }

        [Display(Name = "Estado")]
        public char estado { get; set; }

        [Display(Name = "Usuario")]
        public string? usuario { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime? fecha_reg { get; set; }

        [Display(Name = "Línea")]
        public int? linea { get; set; }

        [Display(Name = "Hora de Entrada 2")]
        public string? hentra2 { get; set; }

        [Display(Name = "Estado de Envío")]
        public bool estado_envio { get; set; }

        [Display(Name = "Acción")]
        public char Accion { get; set; }

        public cEmpleado? cEmpleado { get; set; }
        public cTurno? cTurno { get; set; }
        public cPh_Planilla? cPh_Planilla { get; set; }
    }
}
