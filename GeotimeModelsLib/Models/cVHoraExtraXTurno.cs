namespace com.gsitcr.geotime.Models
{
    using System.ComponentModel.DataAnnotations;

    public class cVHoraExtraXTurno
    {
        [Display(Name = "ID Período")]
        public string idperiodo { get; set; }

        [Display(Name = "ID Turno")]
        public int? idturno { get; set; }

        [Display(Name = "Descripción del Turno")]
        public string DescripcionTurno { get; set; }

        [Display(Name = "Hora de Entrada")]
        public string HoraEntradaTurno { get; set; }

        [Display(Name = "Hora de Salida")]
        public string HoraSalidaTurno { get; set; }

        [Display(Name = "Horas del Turno")]
        public string? HorasTurno { get; set; }

        [Display(Name = "Horas Extra Diurnas Anticipadas")]
        public int? HorasExtraDiurnasAnticipadas { get; set; }

        [Display(Name = "Horas Extra Nocturnas Anticipadas")]
        public int? HorasExtraNocturasAnticipadas { get; set; }

        [Display(Name = "Horas Extra Mixtas Anticipadas")]
        public int? HorasExtraMixtasAnticipadas { get; set; }

        [Display(Name = "Horas Extra Diurnas Posteriores")]
        public decimal? HorasExtraDiurnasPosterior { get; set; }

        [Display(Name = "Horas Extra Nocturnas Posteriores")]
        public decimal? HorasExtraNocturasPosterior { get; set; }

        [Display(Name = "Horas Extra Mixtas Posteriores")]
        public decimal? HorasExtraMixtasPosterior { get; set; }

    }
}
