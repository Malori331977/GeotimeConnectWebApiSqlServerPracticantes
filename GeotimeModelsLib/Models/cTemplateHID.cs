using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace com.gsitcr.geotime.Models
{
    public class cTemplateHID
    {
        [Key]
        public string IDNUMERO { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int INDEXID { get; set; }
        public byte[] HID_TEMPLATE { get; set; }
    }
}
