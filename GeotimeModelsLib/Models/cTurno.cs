using GeotimeModelsLib.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.gsitcr.geotime.Models
{
    public class cTurno
    { 
        public int IdTurno { get; set; }
        public string? Descripcion { get; set; }
        public string? HEntra { get; set; }
        public string? HSale { get; set; }
        public string? Tipo { get; set; }
        public string? Tipo_Jor { get; set; }
        public char? tar_apl { get; set; }
        public char? ant_apl { get; set; }
        public string? des_1_in { get; set; }
        public string? des_1_out { get; set; }
        public string? des_2_in { get; set; }
        public string? des_2_out { get; set; }
        public string? des_3_in { get; set; }
        public string? des_3_out { get; set; }
        public char? apl_des_1 { get; set; }
        public char? apl_des_2 { get; set; }
        public char? apl_des_3 { get; set; }
        public string? des_1_tiem { get; set; }
        public string? des_2_tiem { get; set; }
        public string? des_3_tiem { get; set; }
        public char? marca_des_1 { get; set; }
        public char? marca_des_2 { get; set; }
        public char? marca_des_3 { get; set; }
        public string? tar_tiem { get; set; }
        public string? ant_tiem { get; set; }
        public int? con_1 { get; set; }
        public int? con_2 { get; set; }
        public int? con_3 { get; set; }
        public int? con_4 { get; set; }
        public int? con_5 { get; set; }
        public int? con_6 { get; set; }
        public string? cant_con_1 { get; set; }
        public string? cant_con_2 { get; set; }
        public string? cant_con_3 { get; set; }
        public string? cant_con_4 { get; set; }
        public string? cant_con_5 { get; set; }
        public string cant_con_6 { get; set; }
        public string? min_con_1 { get; set; }
        public string? min_con_2 { get; set; }
        public string? min_con_3 { get; set; }
        public string? min_con_4 { get; set; }
        public string? min_con_5 { get; set; }
        public string min_con_6 { get; set; }
        //public char? tipo { get; set; }
        //public char tipo_jor { get; set; }
        public char fuerza_calc { get; set; }
        public int idagrupamiento { get; set; }
        public int? apl_trans1 { get; set; }
        public int? id_trans1 { get; set; }
        public int? apl_trans2 { get; set; }
        public int? id_trans2 { get; set; }
        public int? apl_trans3 { get; set; }
        public int? id_trans3 { get; set; }
        public int? apl_trans4 { get; set; }
        public int? id_trans4 { get; set; }
        public int? apl_trans5 { get; set; }
        public int? id_trans5 { get; set; }
        public int? apl_trans6 { get; set; }
        public int? id_trans6 { get; set; }
        public int? apl_ben1 { get; set; }
        public int? id_ben1 { get; set; }
        public int? apl_ben2 { get; set; }
        public int? id_ben2 { get; set; }
        public int? apl_ben3 { get; set; }
        public int? id_ben3 { get; set; }
        public int? apl_ben4 { get; set; }
        public int? id_ben4 { get; set; }
        public int? apl_ben5 { get; set; }
        public int? id_ben5 { get; set; }
        public int? apl_ben6 { get; set; }
        public int? id_ben6 { get; set; }
        public int? conc_ben1 { get; set; }
        public int? conc_ben2 { get; set; }
        public int? conc_ben3 { get; set; }
        public int? conc_ben4 { get; set; }
        public int? conc_ben5 { get; set; }
        public int? conc_ben6 { get; set; }
        public int? apl_trans_post { get; set; }
        public int? id_trans_post { get; set; }
        public char? apl_redond_entrada { get; set; }
        public string? cant_redond_entrada { get; set; }
        public char? auto_pan { get; set; }
        public int? ColorId { get; set; }

        public cPaletaColor? PaletaColor { get; set; }
        public IEnumerable<cPh_RolTurno>? cPh_RolTurno { get; set; }        
        public IEnumerable<cMarcaProceso>? cMarcaProcesos { get; set; }
        public IEnumerable<cMarcaMovTurno>? cMarcaMovTurno { get; set; }
        public IEnumerable<cMarcaMovTurnoBitacora>? cMarcaMovTurnoBitacora { get; set; }

    }
}
