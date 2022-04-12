using System.ComponentModel.DataAnnotations;

namespace GroupCoursework.Models
{
    public class Actor
    {
        [Key]
        public int ActorNumber { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string ActorFirstName { get; set; }
        public string ActorSurname { get; set; }

    }
}
