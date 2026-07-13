using Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infraestructure.Security;

public class JwtTokenBuilder : IJwtTokenBuilder
{
    private readonly string _key = "TuClaveSecretaSuperLargaYSeguraQueDebesCambiarEnProduccion"; 
    private readonly List<Claim> _claims = new();

    public IJwtTokenBuilder WithUserId(Guid userId)
    {
        _claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        return this;
    }

    public IJwtTokenBuilder WithTenantId(Guid tenantId)
    {
        _claims.Add(new Claim("TenantId", tenantId.ToString()));
        return this;
    }

    public IJwtTokenBuilder WithUsername(string username)
    {
        _claims.Add(new Claim(ClaimTypes.Name, username));
        return this;
    }

    public IJwtTokenBuilder WithEmail(string email)
    {
        _claims.Add(new Claim(ClaimTypes.Email, email));
        return this;
    }

    public string Build()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "SaaSStockAPI",
            audience: "SaaSStockReactClient",
            claims: _claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}