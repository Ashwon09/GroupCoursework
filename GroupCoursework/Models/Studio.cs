using System.ComponentModel.DataAnnotations;

namespace GroupCoursework.Models
{
    public class Studio
    {
        [Key]
        public int StudioNumber { get; set; }
        [Required(ErrorMessage = "Studio name is required")]
        public string StudioName { get; set; }
    }
}
