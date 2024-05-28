namespace E_commerceElect.Models
{
    public class Panier
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<LignePanier> LignePaniers { get; set; }

    }
}