using AutoMapper;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Entities.Management;
using Blogsphere.User.Domain.Events;
using Contracts.Events;

namespace Blogsphere.User.Application.Mappers;

public class ManagementUserEventMapping : Profile
{
    public ManagementUserEventMapping()
    {
        CreateMap<ManagementUser, ManagementUserWelcomeEmailSent>()
        .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
        .ForMember(d => d.Role, o => o.MapFrom(s => s.JobTitle));

        CreateMap<ManagementUser, ManagementUserPasswordEmailSent>()
        .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
        .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
        .ForMember(d => d.Password, o => o.MapFrom(s => "Welcome@123"));


        CreateMap<ManagementUser, ManagementUserCreated>()
        .ForMember(d => d.Roles, o => o.MapFrom(s => s.GetUserRoleMappings()))
        .ForMember(d => d.Status, o => o.MapFrom(s => s.IsActive ? "Active" : "Inactive"))
        .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt))
        .ForMember(d => d.LastUpdatedAt, o => o.MapFrom(s => s.UpdatedAt));

        CreateMap<ManagementUser, ManagementUserUpdated>()
        .ForMember(d => d.Roles, o => o.MapFrom(s => s.GetUserRoleMappings()))
        .ForMember(d => d.Status, o => o.MapFrom(s => s.IsActive ? "Active" : "Inactive"))
        .ForMember(d => d.LastUpdatedAt, o => o.MapFrom(s => s.UpdatedAt));
    }
}
