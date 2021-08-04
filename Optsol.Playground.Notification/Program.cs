using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Optsol.Playground.Notification
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(GetJsonToken())
            });

            Console.WriteLine("ctrl+c para sair.");

            do
            {
                var registrationTokens = new List<string>()
                {
                    "BI88CGP9Tza2ptduvY57aG9otvnVAJI6Mr5PRYPaEgugWTjC94sc3fvAVSZYoIr8wrSYu3t9BO2I4qDgpsEZGLc",
                };

                var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
                    registrationTokens, "/topics/broadcast");

                Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");
                
                Thread.Sleep((int)TimeSpan.FromSeconds(10).TotalMilliseconds);

            } while (true);
        }

        private static string GetJsonToken()
        {
            return @"{
  ""type"": ""service_account"",
  ""project_id"": ""optsol-notifications"",
  ""private_key_id"": ""377e45abfbd198a2c8275edfb78f2f1e84ebe6aa"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCOLyGesAlHSYMT\nhYW2lYOWOWx5S0l5QrkwZFIclP3KKkJbX+5RY9qsGOCl9rp+CI+9HwEgFiiiWNRO\ndpyHI+2MMrbLRhSIfryz+KoPrGqoHC0LoKhZrp5FR9ayAd2aYFxgqeAZp+dniQN9\nIRMs+rQT5TzpMX0hhY8vdBH7ehFztAHl/5sPcT8YptdPzwOZ9RcRJNvfGbp6wtHH\nVPQABrVtpGmrKAuPtyd++1lUVfEIxj7leXwjdTn+HKAd+ItiSvtWFrmJC583KgaJ\no6iXCz5h6iVDB6oUr3PdE9pOSQKy7N+hqH0ATWJT+XUxzGCE+iOPK9vYn0b7HmhI\nGYNUV/4RAgMBAAECggEAASg+1GkzA9joyLt/XOyZPa2nd8BBpnAcjIcZo9eOKakr\nLLu15Sl42uVdFLS3MMgxswJh+7ZDmZ07ZWiEFWfrM8cEUTQlRv5npI5W37Vs/zqg\n5/DbQ6BzmD6Kr67YtEVXkyH5GX1/vdNx+O5ZhLYXgME8fBGIJt2DqhJi8l3X3QTg\n0x+nWusyfRk7DLUbZ6pde9/2syRiFLYVnsznWmeJkho242nAxEExt45grAAev7FQ\n5xZS1WaF+d8ZP50vHketIlIhpbFtoz6cAe6Km2Ew/6J9Q5i/0qLP2nOdwjxtzsBq\n4wV0jTzWOKIGoFGfoU4sbdWCMLysIOpwH8xsCoC5PQKBgQDDOs0nCRpy2dBC9Tt1\nqdPEGj1aI00MWF14U9PKyS7oobNP3awLbEGZNxSyQFNdxF+AdxPVnWH+0f7Ktd1C\nyVCof9AJnJoMwfwhkL8vgYNm8tMu4d795/r+KT6wNrcg/F81j6hUkX1DZNN3EVwf\nydZOld+N7kHk0SOmu/3veHXebQKBgQC6cU4pxQqMRUfg6vcOG2+I+vmJmhhNRumW\nQQNOmuDuTwTVn5Tn4WHtBMh1+fhO5WjOc/qx2WW4oD6vyHiXGmwGxFJ2Ekn0dIvk\n89Rb6e9lognUYc3HS5Rk6jxJRhYEdPMkDrd/KL4tR05AmvHlrZtuiTEkMlHcVivU\nGatzMKfHtQKBgQCb1h8WTrzu+pC0Mf5xcMtaGLvqbI0/EnYh63/+udKY3SI92StQ\nvuwp+f6qPpCNI4g8ClEpzYQAnO1uL/dbLUkWB1gvo/KPxnSU8m654/7YuXH7VU7j\njD4cjR2+GR7a1LjHD1IFl1DO7/egbpoDweAwQI//QjUVCiAUIrGTaqtjjQKBgH/9\nVIk8KTeOP//ZjWxzzSeeEzwxOsmiCq0JSHnnvM4cFNeJy7E7efw6Mls7FQkkV8SS\nveDluvz5lM8bsh0ZGFu03l8LwxU8BOVRtdC7UYrVqCXSVm+gJOj6HBS5Nlgs6NUx\n/SogEB4JO2ECfVkMyw0gxUlx+dxmk4Pxc0+KcUOZAoGAVjAo1Pjw/ml+zEnHNieB\nrKfyyqrWtfH+VChG6Y5qiLyQRzbHUa7hEnIhc953sCJ/1/YvDqsm9J8slooQCNu9\n4PiIIzeUH/jpa1nNii8sLxr8E9hIbg5mDjf1jX2W1mACnSIT+fKYvsF2ab/j3ubq\nX+noRyLGM+eDc6gGDNuRv0o=\n-----END PRIVATE KEY-----\n"",
  ""client_email"": ""firebase-adminsdk-a8suz@optsol-notifications.iam.gserviceaccount.com"",
  ""client_id"": ""117738386793366647032"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-a8suz%40optsol-notifications.iam.gserviceaccount.com""
}
";
        }
    }
}
