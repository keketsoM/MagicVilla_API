using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Model.Dto
{
    public class VillaNumberDtoUpdate
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [Required]
        public int VillaId { get; set; }
    }
}
