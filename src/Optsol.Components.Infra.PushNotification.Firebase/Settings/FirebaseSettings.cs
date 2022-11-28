using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Optsol.Components.Infra.PushNotification.Firebase.Exceptions;

namespace Optsol.Components.Infra.PushNotification.Firebase.Settings
{
	public class FirebaseSettings
	{
        [ConfigurationKeyName("type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [ConfigurationKeyName("project_id")]
        [JsonProperty(PropertyName = "project_id")]
        public string ProjectId { get; set; }

        [ConfigurationKeyName("private_key_id")]
        [JsonProperty(PropertyName = "private_key_id")]
        public string PrivateKeyId { get; set; }

        [ConfigurationKeyName("private_key")]
        [JsonProperty(PropertyName = "private_key")]
        public string PrivateKey { get; set; }

        [ConfigurationKeyName("client_email")]
        [JsonProperty(PropertyName = "client_email")]
        public string ClientEmail { get; set; }

        [ConfigurationKeyName("client_id")]
        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }

        [ConfigurationKeyName("auth_uri")]
        [JsonProperty(PropertyName = "auth_uri")]
        public string AuthUri { get; set; }

        [ConfigurationKeyName("token_uri")]
        [JsonProperty(PropertyName = "token_uri")]
        public string TokenUri { get; set; }

        [ConfigurationKeyName("auth_provider_x509_cert_url")]
        [JsonProperty(PropertyName = "auth_provider_x509_cert_url")]
        public string AuthProviderX509CertUrl { get; set; }

        [ConfigurationKeyName("client_x509_cert_url")]
        [JsonProperty(PropertyName = "client_x509_cert_url")]
        public string ClientX509CertUrl { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Type))
            {
                ShowingException("type");
            }

            if (string.IsNullOrEmpty(ProjectId))
            {
                ShowingException("project_id");
            }

            if (string.IsNullOrEmpty(PrivateKeyId))
            {
                ShowingException("private_key_id");
            }

            if (string.IsNullOrEmpty(PrivateKey))
            {
                ShowingException("private_key");
            }

            if (string.IsNullOrEmpty(ClientEmail))
            {
                ShowingException("client_email");
            }

            if (string.IsNullOrEmpty(ClientId))
            {
                ShowingException("client_id");
            }

            if (string.IsNullOrEmpty(AuthUri))
            {
                ShowingException("auth_uri");
            }

            if (string.IsNullOrEmpty(TokenUri))
            {
                ShowingException("token_uri");
            }

            if (string.IsNullOrEmpty(AuthProviderX509CertUrl))
            {
                ShowingException("auth_provider_x509_cert_url");
            }

            if (string.IsNullOrEmpty(ClientX509CertUrl))
            {
                ShowingException("client_x509_cert_url");
            }
        }

        public void ShowingException(string objectName)
        {
            throw new SettingsNullException(objectName);
        }
    }
}

