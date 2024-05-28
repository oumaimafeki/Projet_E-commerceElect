namespace E_commerceElect.Models
{
    public class ApplicationUser
    {
        public int UtilisateurId { get; set; } 
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }

        public string MotDePasse { get; set; }
    }
}
