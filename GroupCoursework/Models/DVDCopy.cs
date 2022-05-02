using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupCoursework.Models
{
    public class DVDCopy
    {

        [Key]
        public int CopyNumber { get; set; }
        public int DVDNumber { get; set; }
        [ForeignKey("DVDNumber")]
        public DVDTitle DVDTitle { get; set; }
        [Required(ErrorMessage = "Purchased Date is required")]
        public int Stock { get; set; }
        public DateTime DatePurchased { get; set; }
    }
}
