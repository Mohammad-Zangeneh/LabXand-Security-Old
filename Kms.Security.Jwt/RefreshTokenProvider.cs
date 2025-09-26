using Kms.Security.Common.DataContract;
using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Util;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Jwt
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly Func<ISecurityService> _securityService;
        private readonly IAppJwtConfiguration _configuration;
        private readonly Func<ITokenStoreService> _tokenStoreService;
        private readonly ISecurityConfigurationContext _securityConfigurationContext;

        public RefreshTokenProvider(
            IAppJwtConfiguration configuration,
            Func<ITokenStoreService> tokenStoreService,
            Func<ISecurityService> securityService,
            ISecurityConfigurationContext securityConfigurationContext)
        {
            _configuration = configuration;
            _configuration.CheckArgumentNull(nameof(_configuration));

            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentNull(nameof(_tokenStoreService));

            _securityService = securityService;
            _securityService.CheckArgumentNull(nameof(_securityService));

            _securityConfigurationContext = securityConfigurationContext;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).RunSynchronously();
        }
        
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");

            var now = DateTime.UtcNow;
            var ownerUserId = context.Ticket.Identity.FindFirst(LabxandClaimTypes.UserId).Value;
            var sn = context.Ticket.Identity.FindFirst(LabxandClaimTypes.SerialNumber).Value;
            var token = new UserToken
            {
                OwnerUserId = new Guid(ownerUserId),
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _securityService().GetSha256Hash(refreshTokenId),
                Subject = context.Ticket.Identity.Name,
                RefreshTokenExpiresUtc = now.AddMinutes(Convert.ToDouble(_securityConfigurationContext.Instance.RefreshTokenExpirationMinutes)),
                AccessTokenExpirationDateTime = now.AddMinutes(Convert.ToDouble(_securityConfigurationContext.Instance.ExpirationMinutes)),
                SerialNumber = sn
            };

            context.Ticket.Properties.IssuedUtc = now;
            context.Ticket.Properties.ExpiresUtc = token.RefreshTokenExpiresUtc;

            token.RefreshToken = context.SerializeTicket();

            _tokenStoreService().CreateUserToken(token);
            _tokenStoreService().DeleteExpiredTokens();

            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).RunSynchronously();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var hashedTokenId = _securityService().GetSha256Hash(context.Token);
            var refreshToken = _tokenStoreService().FindToken(hashedTokenId);
            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.RefreshToken);
                _tokenStoreService().DeleteToken(hashedTokenId);
            }
        }
    }
}