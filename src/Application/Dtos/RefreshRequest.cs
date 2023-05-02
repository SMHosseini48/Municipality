namespace Application.Dtos;

public class RefreshRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken{ get; set; }

}