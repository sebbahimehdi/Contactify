using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManagerPro.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Le contenu est requis")]
        [StringLength(2000)]
        public string Content { get; set; }

        [Display(Name = "Date de création")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key
        [ForeignKey("Contact")]
        public int ContactId { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}
