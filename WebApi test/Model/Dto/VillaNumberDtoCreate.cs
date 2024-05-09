using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Model.Dto
{
    public class VillaNumberDtoCreate
    {

        [Required]
        public int VillaNo { get; set; }

        public string SpecialDetails { get; set; }
        [Required]
        public int VillaId { get; set; }
    }
}
