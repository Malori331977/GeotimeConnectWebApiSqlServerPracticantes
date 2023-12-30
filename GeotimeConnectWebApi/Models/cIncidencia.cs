namespace GeotimeConnectWebApi.Models
{
    public class cIncidencia
    {
        public int Id { get; set; }
        public String? Codigo { get; set; }
        public String? Descripcion { get; set; }
        public string? requiere_accper { get; set; }
        public string? nom_conector { get; set; }

        // Añadidos Por Allan -> Son campos necesarios para el mantenimiento <Incidencias>
        public int? id_pago { get; set; }
        public int? tipo { get; set; }
        public char? ed_tiempo { get; set; }


    }
}
