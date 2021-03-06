﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Configuration;

namespace Trackwane.Framework.Infrastructure.Web.Security
{
    public class SecuredAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (UsesTokenAuthentication(context.Request))
            {
                SetClaimsPrincipalFromToken(context);
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }
        
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        /* Private */

        private static bool UsesTokenAuthentication(HttpRequestMessage req)
        {
            return req.Headers != null && req.Headers.Authorization != null && req.Headers.Authorization.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase);
        }

        private static void SetClaimsPrincipalFromToken(HttpAuthenticationContext context)
        {
            var req = context.Request;
            var userClaims = GetClaimsFromToken(req, new PlatformConfig().SecretKey);
            context.Principal = new ClaimsPrincipal(new[] {new ClaimsIdentity(userClaims.Claims, "JWT")});
        }

        private static UserClaims GetClaimsFromToken(HttpRequestMessage req, string secretKey)
        {
            var originalToken = GetTokenFromHeaders(req);

            try
            {
                return new UserClaims(secretKey, originalToken);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Unable to process the token {0}", originalToken), e);
            }
        }

        private static string GetTokenFromHeaders(HttpRequestMessage req)
        {
            var originalToken = req.Headers.Authorization.Parameter;

            if (string.IsNullOrWhiteSpace(originalToken))
            {
                throw new Exception("No token provided");
            }
            return originalToken;
        }
    }
}