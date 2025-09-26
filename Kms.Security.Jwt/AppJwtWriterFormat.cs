﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Kms.Security.Jwt
{

    public class AppJwtWriterFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string DigestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";
        private const string SignatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

        private readonly OAuthAuthorizationServerOptions _options;
        private readonly IAppJwtConfiguration _configuration;

        public AppJwtWriterFormat(OAuthAuthorizationServerOptions options, IAppJwtConfiguration configuration)
        {
            _options = options;
            _options.CheckArgumentNull(nameof(_options));

            _configuration = configuration;
            _configuration.CheckArgumentNull(nameof(_configuration));
        }
        //generation token
        
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_options.AccessTokenExpireTimeSpan.TotalMinutes);

            var symmetricKey = Encoding.UTF8.GetBytes(_configuration.JwtKey);
            SigningCredentials signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(symmetricKey),
                                        SignatureAlgorithm, DigestAlgorithm);
            var token = new JwtSecurityToken(_configuration.JwtIssuer, _configuration.JwtAudience, data.Identity.Claims,
                                             now, expires, signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}
