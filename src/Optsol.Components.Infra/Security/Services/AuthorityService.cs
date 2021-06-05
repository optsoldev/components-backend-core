using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.Models;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Security.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly AuthorityClient _authorityClient;
        private readonly ILogger<AuthorityService> _logger;

        public AuthorityService(AuthorityClient authorityClient, ILogger<AuthorityService> logger)
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
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }

            return clientOauth;
        }
    }
}
