using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Model.Dto
{
    public class VillaNumberDtoCreate
    {

        [Required]
        public int VillaNo { get; set; }

        public string SpecialDetails { get; set; }
        [Required]
        public int VillaID { get; set; }
    }
}
