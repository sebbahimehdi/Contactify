using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManagerPro.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100)]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
        [StringLength(20)]
        public string Telephone { get; set; }

        [StringLength(255)]
        public string Adresse { get; set; }

        [StringLength(100)]
        public string Profession { get; set; }

        [StringLength(100)]
        public string Ville { get; set; }

        [StringLength(500)]
        public string PhotoUrl { get; set; }

        public bool Favori { get; set; }

        [Display(Name = "Date de création")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Foreign keys
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }

        // Navigation properties
        public Company? Company { get; set; }

        public Category? Category { get; set; }
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}