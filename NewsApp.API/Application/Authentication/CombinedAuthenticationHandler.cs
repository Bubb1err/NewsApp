using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Primitives;

namespace Analytics.API.Application.Authentication;
public class CombinedAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ILogger<CombinedAuthenticationHandler> _logger;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _signingKey;


    public CombinedAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _logger = logger.CreateLogger<CombinedAuthenticationHandler>();

        _issuer = configuration.GetValue<string>("Jwt:Issuer");
        _audience = configuration.GetValue<string>("Jwt:Audience");
        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")));
        
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        StringValues tokenHeaderValues;

        StringValues apiKeyHeaderValues = new StringValues();
        
        if ((Request.Headers.TryGetValue("Authorization", out tokenHeaderValues) 
            && (tokenHeaderValues.First().Contains("X-API-KEY")))
            || Request.Headers.TryGetValue("X-API-KEY", out apiKeyHeaderValues))
        {
            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            
            if (tokenHeaderValues.Any())
            {
                providedApiKey = tokenHeaderValues.FirstOrDefault().Replace("X-API-KEY", "").Trim();
            }
            
          
            Request.Headers.TryGetValue("UserId", out var userId);
            
        }
        if (Request.Headers.TryGetValue("Authorization", out tokenHeaderValues))
        {
            var sas = tokenHeaderValues.FirstOrDefault();
            var headerValueTrimmed = sas.Replace("Bearer", "").Trim();
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = _signingKey,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = handler.ValidateToken(headerValueTrimmed, tokenValidationParameters, out SecurityToken validatedToken);
                var jwtSecurityToken = validatedToken as JwtSecurityToken;
             const string EmailClaims = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";


                if (jwtSecurityToken == null)
                    return AuthenticateResult.Fail("Invalid token");
                
                var email = principal.Claims.FirstOrDefault(x => x.Type == EmailClaims)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return AuthenticateResult.NoResult();
                }

                var claimsIdentity = new ClaimsIdentity(principal.Claims, "Bearer", "email", "roles");
                var authProperties = new AuthenticationProperties();
                var authenticationTicket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), authProperties, "Bearer");

                return AuthenticateResult.Success(authenticationTicket);
            }
            catch (SecurityTokenExpiredException)
            {
                Context.Response.Headers["Token-Expired"] = "true";
                return AuthenticateResult.Fail("Token expired");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                return AuthenticateResult.Fail("Token validation failed");
            }
        }
        return null;
    }
}
