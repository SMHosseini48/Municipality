using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class RefreshToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Token { get; set; }

    public string JwtId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpriryDate { get; set; }
    public bool Invalidated { get; set; }
    public bool Used { get; set; }
    public string BaseUserId { get; set; }

    public BaseUser BaseUser { get; set; }
}