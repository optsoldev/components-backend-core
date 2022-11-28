using AutoMapper;
using FirebaseAdmin.Messaging;
using Optsol.Components.Infra.PushNotification.Firebase.Models;

namespace Optsol.Components.Infra.PushNotification.Firebase.Mapper
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<PushMessageBase, Message>()
                .ForMember(d => d.Token, i => i.MapFrom(s => s.Token))
                .ForMember(d => d.Topic, i => i.MapFrom(s => s.Topic))
                .ForMember(d => d.Condition, i => i.MapFrom(s => s.Condition));

            CreateMap<PushMessage, Message>()
               .IncludeBase<PushMessageBase, Message>()
               .ForMember(d => d.Notification, i => i.MapFrom(s =>
                   new Notification
                   {
                       Title = s.Title,
                       Body = s.Body,
                       ImageUrl = s.ImageUrl
                   }));

            CreateMap<PushMessageData, Message>()
                .IncludeBase<PushMessage, Message>()
                .ForMember(d => d.Data, i => i.MapFrom(s => s.Data));
        }
    }
}
