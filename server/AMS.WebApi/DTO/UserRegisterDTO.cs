using System.ComponentModel.DataAnnotations;

namespace AMS.WebApi.DTO
{
  public class UserRegisterDTO
  {
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
  }
}