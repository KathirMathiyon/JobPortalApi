using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class JobDTO
    {
        [Required(ErrorMessage = "The Title cannot be Empty")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Company cannot be Empty")]
        [StringLength(100)]
        public string Company { get; set; }

        [Required(ErrorMessage = "The Location cannot be Empty")]
        [StringLength(100)]
        public string Location { get; set; }

        public DateTime PostedDate { get; set; }
    }
}
