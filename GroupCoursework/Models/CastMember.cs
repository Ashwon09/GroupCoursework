using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupCoursework.Models
{
    public class CastMember
    {
        [Key]
        public int DVDNumber { get; set; }
        [ForeignKey("DVDNumber")]
        [Key]
        public DVDTitle DVDTitle { get; set; }
        public int ActorNumber { get; set; }
        [ForeignKey("ActorNumber")]
        public Actor Actor { get; set; }
    }
}
