using AutoMapper;
using Blogsphere.User.Domain.Entities;
using Contracts.Events;

namespace Blogsphere.User.Application.Mappers;

public class UserEventMapping : Profile
{
   public UserEventMapping()
   {
        CreateMap<ApplicationUser, UserInvitationSent>()
        .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));
   }
}
