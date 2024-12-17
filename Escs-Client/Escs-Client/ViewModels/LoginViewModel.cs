using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Escs_Client.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [AllowNull]
        public string ReturnUrl { get; set; } = String.Empty;
    }
}
