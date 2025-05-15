using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{
    public class cIncidencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public char? requiere_accper { get; set; }
        public string? nom_conector { get; set; }

        // Añadidos Por Allan -> Son campos necesarios para el mantenimiento <Incidencias>
        public int? id_pago { get; set; }
        public int? tipo { get; set; }
        public char? ed_tiempo { get; set; }
        public char? marca_web { get; set; }

        public IEnumerable<cMarcaIncidencia>? cMarcaIncidencias { get; set; }
        public IEnumerable<cMarcaIncidencia>? cMarcaIncidenciasJust { get; set; }
        public IEnumerable<cAccionPersonal>? cAccionPersonal { get; set; }


    }
}
