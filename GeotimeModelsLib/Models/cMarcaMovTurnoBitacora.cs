using com.gsitcr.geotime.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{

    public class cMarcaMovTurnoBitacora
    {
        [Display(Name = "ID Bitácora")]
        public long idbitacora { get; set; }       

        [Display(Name = "ID Registro")]
        public long idregistro { get; set; }

        [Display(Name = "ID Planilla")]
        public string idplanilla { get; set; }

        [NotMapped]
        [Display(Name = "Planilla")]
        public string? DescripcionPlanilla { get; set; }

        [NotMapped]
        [Display(Name = "Nombre de Departamento")]
        public string? DescripcionDepartamento { get; set; }

        [Display(Name = "ID Colaborador")]
        public string idnumero { get; set; }

        [NotMapped]
        [Display(Name = "Nombre de Colaborador")]
        public string? NombreEmpleado { get; set; }

        [Display(Name = "Fecha Programada")]
        public DateTime fecha { get; set; }

        [Display(Name = "Hora Programada")]
        public string hora { get; set; }

        [Display(Name = "Turno Programado")]
        public int turno { get; set; }

        [NotMapped]
        [Display(Name = "Descripción turno")]
        public string? DescipcionTurno { get; set; }


        [Display(Name = "Estado")]
        public char estado { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime? fecha_reg { get; set; }

        [Display(Name = "Línea")]
        public int? linea { get; set; }

        [Display(Name = "Hora de Entrada 2")]
        public string? hentra2 { get; set; }

        [Display(Name = "Estado de Envío")]
        public bool estado_envio { get; set; }


        [Display(Name = "Id Acción")]
        public char Accion { get; set; }

        [NotMapped]
        [Display(Name = "Acción")]
        public string? DescipcionAccion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? fecha_modificacion { get; set; }

        [Display(Name = "Usuario que Registra")]
        public string? usuario { get; set; }

        [Display(Name = "Usuario que Modifica")]
        public string UsuarioModifica { get; set; }

        [Display(Name = "Nombre Modifica")]
        public string? NombreUsuarioModifica { get; set; }

        public cEmpleado? cEmpleado { get; set; }
        public cTurno? cTurno { get; set; }
        public cPh_Planilla? cPh_Planilla { get; set; }

       

        
       

       
    }
}
