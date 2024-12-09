using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using Claim = System.Security.Claims.Claim;

namespace NewsApp.API.Services;

[Service]
public class AccessControlService(
    UserManager<User> userManager,
    IConfiguration configuration)
{


    public async Task<bool> SetUserRefreshToken(string username, string refreshToken, TimeSpan expirationTime)
    {
        var user = await userManager.FindByEmailAsync(username);
        if(user != null)
        {
           /* RefreshToken refreshTokenItem = new RefreshToken()
            {
                Token = refreshToken,
                ExpireTime = DateTime.UtcNow.Add(expirationTime),
                UserId = user.Id
            };*/
            //await _unitOfWork.RefreshTokens.AddAsync(refreshTokenItem);
            //_unitOfWork.Save();
        }
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public string GenerateJWTToken(List<Claim> claims)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:SigningKey"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration.GetValue<string>("JWT:Issuer"),
            Audience = configuration.GetValue<string>("JWT:Audience"),
            Subject = new ClaimsIdentity(claims),
    
            Expires = DateTime.UtcNow.AddHours(1),
            
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<List<Claim>> GetUserClaimsByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var roles = await userManager.GetRolesAsync(user);
        if (roles == null) return null;

        var userRole = roles.Select(x => new Claim(ClaimTypes.Role, x));
        var userClaims = await userManager.GetClaimsAsync(user);

        return new List<Claim>()
        {   
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            new(ClaimTypes.Name, user.Name ?? ""),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            
        }.Union(userRole).Union(userClaims).ToList();
    }
}