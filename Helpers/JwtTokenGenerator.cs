using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AucaJobPortalDotNet.Helpers;
public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration=configuration;
    }

    public string GenerateToken(int userId,string email,int roleId,string roleName)
    {
        var jwtSettings=_configuration.GetSection("Jwt");
        var secret=jwtSettings["Secret"]?? throw new InvalidOperationException("JWT Secret not configured");
        var issuer=jwtSettings["Issuer"];
        var audience=jwtSettings["Audience"];
        var expiryMinutes=double.Parse(jwtSettings["ExpiryMinutes"] ?? "1440");

        var claims=new List<Claim>
        {
            new Claim("userId" , userId.ToString()),
            new Claim("email", email) ,
            new Claim("roleId", roleId.ToString()),
            new Claim("roleName",roleName)
        };

        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials=new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token=new JwtSecurityToken(
            issuer:issuer,
            audience:audience,
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}