using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Domain.ValueObjects;
using Optsol.Components.Infra.Bus.Events;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjects;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Test.Utils.Entity.Entities
{
    public class TestNotificationEntity : PushMessageAggregateRoot, IEvent
    {
        public NomeValueObject Nome { get; private set; }

        public EmailValueObject Email { get; private set; }

        public PushMessageValueObject PushMessage { get; private set; }

        public bool Ativo { get; private set; }

        public TestNotificationEntity()
        {
        }

        public TestNotificationEntity(Guid id, NomeValueObject nome, EmailValueObject email)
            : this(nome, email)
        {
            Id = id;
        }

        public TestNotificationEntity(NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;
            Ativo = false;

            Validate();
        }

        public void InserirNome(NomeValueObject nomeValueObject)
        {
            Nome = nomeValueObject;
        }

        public override void Validate()
        {
            base.Validate();
        }

        public override IEnumerable<ValueObject> GetPushMessages()
        {
            yield return new PushMessageValueObject("Enviando um titulo", "Enviando a mensagem").SetTopic("mobem");
            yield return new PushMessageDataValueObject("Enviando um titulo 2", "Enviando a mensagem 2 @s", new Dictionary<string, string>
            {
                {"Lutiano","Brabo" }
            }).SetTopic("mobem");
        }
    }
}
