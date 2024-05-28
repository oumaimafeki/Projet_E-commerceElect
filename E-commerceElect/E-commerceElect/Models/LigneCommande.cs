namespace E_commerceElect.Models
{
    public class LigneCommande
    {
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public Commande Commande { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
