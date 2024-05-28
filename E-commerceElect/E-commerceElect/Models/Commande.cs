namespace E_commerceElect.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateCommande { get; set; }
        public decimal MontantTotal { get; set; }
        public int EstReglee { get; set; }
        public ICollection<LigneCommande> LignesCommande { get; set; }
    }
}
