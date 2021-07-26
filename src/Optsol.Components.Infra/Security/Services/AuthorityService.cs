using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityClient _authorityClient;
        private readonly ILogger<AuthorityService> _logger;

        public AuthorityService(IAuthorityClient authorityClient, ILogger<AuthorityService> logger)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando AuthorityService");

            _authorityClient = authorityClient;
            _logger = logger;
        }

        public async Task<OauthClient> GetClient(string clientId)
        {
            _logger?.LogInformation($"Executanto GetClient(clientId: {clientId})");

            OauthClient clientOauth = null;

            try
            {
                clientOauth = await _authorityClient.GetClient(clientId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return clientOauth;
        }

        public async Task<UserInfo> GetUserInfo(string token)
        {
            _logger?.LogInformation($"Executanto GetUserInfo");

            UserInfo userInfo = null;

            try
            {
                userInfo = await _authorityClient.GetUserInfo($"Bearer {token}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return userInfo;
        }

        public async Task<UserInfo> GetValidateAccess(string token, IList<string> claims)
        {
            _logger?.LogInformation($"Executanto GetValidateAccess");

            UserInfo userInfo = null;

            try
            {
                userInfo = await _authorityClient.GetValidateAccess($"Bearer {token}", string.Join(",", claims));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return userInfo;
        }
    }
}
