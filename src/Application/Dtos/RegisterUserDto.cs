using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class RegisterUserDto
{
    public string Name { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}