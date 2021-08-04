using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Domain.ValueObjects;
using Optsol.Components.Infra.Firebase.Clients;
using System;

namespace Optsol.Components.Infra.Firebase.Models
{
    public abstract class PushMessageBase : ValueObject, IClient
    {
        public PushMessageBase()
        {
            CreatedDate = DateTime.Now;
        }

        public string Token { get; private set; }

        public string Topic { get; private set; }

        public string Condition { get; private set; }

        public bool IsBroadcast
        {
            get
            {
                return string.IsNullOrEmpty(Token);
            }
        }

        public PushMessageBase SetToken(string token)
        {
            var topicIsNull = string.IsNullOrEmpty(token);
            if (topicIsNull)
            {
                throw new ArgumentNullException(nameof(token));

            }

            Token = token;

            return this;
        }

        public PushMessageBase SetTopic(string topic)
        {
            var topicIsNull = string.IsNullOrEmpty(topic);
            if (topicIsNull)
            {
                throw new ArgumentNullException(nameof(topic));

            }

            Topic = topic;

            return this;
        }

        public PushMessageBase SetCondition(string condition)
        {
            var conditionIsNull = string.IsNullOrEmpty(condition);
            if (conditionIsNull)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            Condition = condition;

            return this;
        }
    }
}
