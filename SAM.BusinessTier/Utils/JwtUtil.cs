using SAM.BusinessTier.Payload.Login;
using SAM.DataTier.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SAM.BusinessTier.Utils;

public class JwtUtil
{
    private JwtUtil()
    {
    }

    public static TokenModel GenerateJwtToken(User user)
    {
        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        SymmetricSecurityKey secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonUtil.GetFromAppSettings("Jwt:SecretKey")));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
        string issuer = JsonUtil.GetFromAppSettings("Jwt:Issuer");
        List<Claim> claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub,user.Username),
            new Claim(ClaimTypes.Role,user.Role),
        };
        var expires = DateTime.Now.AddDays(1);
        var token = new JwtSecurityToken(issuer, null, claims, notBefore: DateTime.Now, expires, credentials);

        var accessToken =  jwtHandler.WriteToken(token);
        var refreshToken = GenerateRefreshToken();
        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    private static string GenerateRefreshToken()
    {
        var random = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }
    }
}