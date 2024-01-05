using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeotimeConnectWebApi.Models
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


    }
}
