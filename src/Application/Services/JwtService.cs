using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dtos;
using Application.Interfaces.Business;
using Application.Interfaces.Data;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
    private readonly UserManager<BaseUser> _userManager;
    private readonly TokenValidationParameters _tokenValidationParameters;
        public JwtService(IGenericRepository<RefreshToken> refreshTokenRepository, UserManager<BaseUser> userManager,
            IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
        _configuration = configuration;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<object> CreateToken(BaseUser user)
    {
        var secret = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var accessTokenLifeTime = TimeSpan.Parse(_configuration["JWT:AccessTokenLifeTime"]);
        var refreshTokenLifetime = TimeSpan.Parse(_configuration["JWT:RefreshTokenLifeTime"]);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
                new Claim("role", user.Role.ToString())
            }),

            Expires = DateTime.UtcNow.Add(accessTokenLifeTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            BaseUserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpriryDate = DateTime.UtcNow.Add(refreshTokenLifetime)
        };

        await _refreshTokenRepository.InsertAsync(refreshToken);

        await _refreshTokenRepository.SaveChanges();

        var userBaseResponse = new AuthenticationResultDto
            {AccessToken = accessToken, RefreshToken = refreshToken.Token};
        return userBaseResponse;
    }

    public async Task<object> RefreshToken(RefreshRequest refreshRequest)
    {
        var accessToken = refreshRequest.AccessToken;
        var refreshToken = refreshRequest.RefreshToken;

        var validatedTokenClaims = GetPrincipalFromToken(accessToken);
        if (validatedTokenClaims == null) throw new Exception("invalid token");

        var expiryDateTimeUnix = long.Parse(validatedTokenClaims.Claims
            .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expiryDateTimeUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow) return new Exception("token not expired yet");

        var jti = validatedTokenClaims.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await _refreshTokenRepository.GetOneByQueryAsync(x => x.Token == refreshToken);

        if (storedRefreshToken == null) return new Exception("this refresh token does not exist");

        if (DateTime.UtcNow > storedRefreshToken.ExpriryDate)
            return new Exception("refresh token has been expired");

        if (storedRefreshToken.Invalidated)
            return new Exception("this refresh token has been invalidated");

        if (storedRefreshToken.Used)
            return new Exception("this refresh token has been used");

        if (storedRefreshToken.JwtId != jti)
            return new Exception("this refresh token does not match the jwt");

        storedRefreshToken.Used = true;
        _refreshTokenRepository.Update(storedRefreshToken);
        await _refreshTokenRepository.SaveChanges();

        var user = await _userManager.FindByIdAsync(validatedTokenClaims.Claims.Single(x => x.Type == "id").Value);
        return await CreateToken(user);
    }


    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        _tokenValidationParameters.ValidateLifetime = false;
        var expiredTokenValidationParameters = _tokenValidationParameters;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, expiredTokenValidationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;

            return principal;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase));
    }
}