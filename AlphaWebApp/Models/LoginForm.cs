using System.ComponentModel.DataAnnotations;

namespace ProjectFlowWebApp.Models;

public class LoginForm
{
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [RegularExpression(@"^[\w\.-]+@([\w-]+\.)+[\w-]{2,}$", ErrorMessage = "Invalid email")]

    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; } = false;
  
}
