using Optsol.Components.Shared.Extensions;
using System;

namespace Optsol.Components.Shared.Settings
{
    public class FirebaseSettings : BaseSettings
    {
        public string FileKeyJson { get; set; }

        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }

        public override void Validate()
        {

            if (FileKeyJson.IsEmpty())
            {
                ShowingException(nameof(FileKeyJson));
                if (string.IsNullOrEmpty(type))
                {
                    ShowingException($"{nameof(type)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(project_id))
                {
                    ShowingException($"{nameof(project_id)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(private_key_id))
                {
                    ShowingException($"{nameof(private_key_id)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(private_key))
                {
                    ShowingException($"{nameof(private_key)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(client_email))
                {
                    ShowingException($"{nameof(client_email)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(client_id))
                {
                    ShowingException($"{nameof(client_id)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(auth_uri))
                {
                    ShowingException($"{nameof(auth_uri)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(token_uri))
                {
                    ShowingException($"{nameof(token_uri)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(auth_provider_x509_cert_url))
                {
                    ShowingException($"{nameof(auth_provider_x509_cert_url)} ou {nameof(FileKeyJson)}");
                }

                if (string.IsNullOrEmpty(client_x509_cert_url))
                {
                    ShowingException($"{nameof(client_x509_cert_url)} ou {nameof(FileKeyJson)}");
                }
            }
        }
    }
}
