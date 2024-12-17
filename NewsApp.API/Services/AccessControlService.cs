using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.GuardClauses;
using Chatty.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Constants;

namespace NewsApp.API.Services;

[Service]
public class AccessControlService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccessControlService(
        UserManager<User> userManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateJWTToken(List<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:SigningKey"));

        // Проверяем и оставляем только одну роль
        var roleClaims = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        if (roleClaims.Any())
        {
            // Удаляем все claims ролей
            claims.RemoveAll(c => c.Type == ClaimTypes.Role);
            // Добавляем только одну роль (самую высокую по иерархии)
            var highestRole = GetHighestRole(roleClaims.Select(c => c.Value));
            claims.Add(new Claim(ClaimTypes.Role, highestRole));
        }

        var expiration = claims.Any(c => c.Type == "IsTemporary") 
            ? DateTime.UtcNow.AddHours(1)  
            : DateTime.UtcNow.AddHours(24);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration.GetValue<string>("JWT:Issuer"),
            Audience = _configuration.GetValue<string>("JWT:Audience"),
            Subject = new ClaimsIdentity(claims, "Bearer"), // Указываем схему аутентификации
            Expires = expiration,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private string GetHighestRole(IEnumerable<string> roles)
    {
        if (roles.Contains(UserRoles.Admin))
            return UserRoles.Admin;
        if (roles.Contains(UserRoles.Premium))
            return UserRoles.Premium;
        return UserRoles.Default;
    }
    

    public async Task<User?> GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedException("User is not authenticated");
        }

        var id = (user.Identity as ClaimsIdentity)?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);

        if (id is null || string.IsNullOrEmpty(id.Value))
        {
            return null;
        }

        var userEntity = await _userManager.FindByIdAsync(id.Value);
        Guard.Against.Null(userEntity, nameof(userEntity));
        return userEntity;
    }

    public async Task<List<Claim>> GetUserClaimsByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var userRoles = await _userManager.GetRolesAsync(user);
    
        // Создаем базовые claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name ?? ""),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Sub, user.Id),
        };

        // Добавляем только одну роль
        if (userRoles.Any())
        {
            var highestRole = GetHighestRole(userRoles);
            claims.Add(new Claim(ClaimTypes.Role, highestRole));
        }

        return claims;
    }

   

    private async Task<string?> GetHighestRoleAsync(IEnumerable<string> roles)
    {
        if (roles.Contains(UserRoles.Admin))
            return UserRoles.Admin;
        if (roles.Contains(UserRoles.Premium))
            return UserRoles.Premium;
        if (roles.Contains(UserRoles.Default))
            return UserRoles.Default;
        
        return null;
    }

   
}