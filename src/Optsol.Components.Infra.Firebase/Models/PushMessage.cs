using System;

namespace Optsol.Components.Infra.Firebase.Models
{
    public class PushMessage : PushMessageBase
    {
        public PushMessage(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public string Title { get; private set; }

        public string Body { get; private set; }

        public string ImageUrl { get; private set; }

        public PushMessage SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;

            return this;
        }

        public override void Validate()
        {
            var titleIsNull = string.IsNullOrEmpty(Title);
            if (titleIsNull)
            {
                throw new ArgumentNullException(nameof(Title));
            }

            var bodyIsNull = string.IsNullOrEmpty(Body);
            if (bodyIsNull)
            {
                throw new ArgumentNullException(nameof(Body));
            }
        }
    }
}
