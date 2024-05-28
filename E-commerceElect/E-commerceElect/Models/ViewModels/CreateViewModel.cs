using E_commerceElect.Models;
using System.ComponentModel.DataAnnotations;

namespace E_commerceElect.Models.ViewModels
{
    public class CreateViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        [Required]
        [Display(Name = "Image :")]
        public IFormFile ImageUrl { get; set; }

        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }
        public string Marque { get; set; }
    }
}
