using AutoMapper;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Models
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<MessageBase, Message>()
                .ForMember(d => d.Token, i => i.MapFrom(s => s.Token))
                .ForMember(d => d.Topic, i => i.MapFrom(s => s.Topic))
                .ForMember(d => d.Condition, i => i.MapFrom(s => s.Condition));

            CreateMap<MessageData, Message>()
                .IncludeBase<MessageBase, Message>()
                .ForMember(d => d.Data, i => i.MapFrom(s => s.Data));

            CreateMap<MessageNotification, Message>()
                .IncludeBase<MessageData, Message>()
                .ForMember(d => d.Notification, i => i.MapFrom(s =>
                    new Notification
                    {
                        Title = s.Title,
                        Body = s.Body,
                        ImageUrl = s.ImageUrl
                    }));
        }
    }
}
