namespace E_commerceElect.Models
{
    public class LignePanier
    {
        public int Id { get; set; }
        public int PanierId { get; set; }
        public Panier Panier { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int Quantity { get; set; }
    }
}
