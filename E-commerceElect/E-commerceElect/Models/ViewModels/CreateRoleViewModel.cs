using System.ComponentModel.DataAnnotations;

namespace E_commerceElect.Models.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]

        public string RoleName { get; set; }
    }
}
