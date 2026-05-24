using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email is a Required Field")]
        [EmailAddress(ErrorMessage ="Email must be in proper format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is a Required Field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password must match the ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
