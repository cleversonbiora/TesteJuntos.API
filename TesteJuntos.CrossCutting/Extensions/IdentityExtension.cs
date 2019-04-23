using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace TesteJuntos.CrossCutting.Extensions
{
    public static class IdentityExtension
    {
        public static Claim GetClaim(this IIdentity identity, string type)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                var identityClaims = (ClaimsIdentity)identity;
                IEnumerable<Claim> claims = identityClaims.Claims;
                return claims.FirstOrDefault(c => c.Type == type);
            }
            else
            {
                return null;
            }
        }
        
        public static string GetClaimValue(this IIdentity identity, string type)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                var identityClaims = (ClaimsIdentity)identity;
                IEnumerable<Claim> claims = identityClaims.Claims;
                var claim = claims.FirstOrDefault(c => c.Type == type);

                if (claim != null)
                {
                    return claim.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return null;
            }
        }
        
        public static bool VerifyClaimValue(this IIdentity identity, string type, string value)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                var identityClaims = (ClaimsIdentity)identity;
                IEnumerable<Claim> claims = identityClaims.Claims;
                var claim = claims.FirstOrDefault(c => c.Type == type);

                if (claim != null)
                {
                    return claim.Value.Contains(value);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        public static Guid GetUserId(this IIdentity identity)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                var identityClaims = (ClaimsIdentity)identity;
                IEnumerable<Claim> claims = identityClaims.Claims;
                var claim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);

                if (claim != null)
                {
                    return new Guid(claim.Value);
                }
                else
                {
                    return new Guid();
                }
            }
            else
            {
                return new Guid();
            }
        }
        public static long GetId(this IIdentity identity)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                var identityClaims = (ClaimsIdentity)identity;
                IEnumerable<Claim> claims = identityClaims.Claims;
                var claim = claims.FirstOrDefault(c => c.Type == "Id");

                if (claim != null)
                {
                    return long.Parse(claim.Value);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
