using System.ComponentModel.DataAnnotations;

namespace com.gsitcr.geotime.Models
{
    
    public class cVHoraLaboradaEmpleado
    {
        [Display(Name = "ID Período")]
        public string idperiodo { get; set; }

        [Display(Name = "ID Planilla")]
        public string idplanilla { get; set; }

        [Display(Name = "Planilla")]
        public string planilla { get; set; }

        [Display(Name = "ID Número")]
        public string idnumero { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "ID Departamento")]
        public string IdDepartamento { get; set; }

        [Display(Name = "Departamento")]
        public string Departamento { get; set; }

        [Display(Name = "Tipo de Jornada")]
        public string TipoJornada { get; set; }

        [Display(Name = "Horas Ordinarias")]
        public decimal? Horas_Ordinarias { get; set; }

        [Display(Name = "Horas Preaprobadas")]
        public int? Horas_Preaprobadas { get; set; }

        [Display(Name = "Extras Aprobadas")]
        public decimal? Extras_Aprobadas { get; set; }

        [Display(Name = "Total Horas Laboradas")]
        public decimal? Total_Horas_Laboradas { get; set; }
    }
}
