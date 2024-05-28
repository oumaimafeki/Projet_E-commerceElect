using System.ComponentModel.DataAnnotations;

namespace E_commerceElect.Models
{
    public class Categorie
    {

        public int Id { get; set; }
        public string Nom { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
