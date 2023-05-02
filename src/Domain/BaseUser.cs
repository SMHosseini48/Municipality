using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class BaseUser : IdentityUser
{
    public virtual Role Role { get; set; }
}