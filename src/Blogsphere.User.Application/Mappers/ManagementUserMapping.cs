using AutoMapper;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Application.Helpers;
using Blogsphere.User.Domain.Entities.Management;
using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Mappers;

public class ManagementUserMapping : Profile
{
    public ManagementUserMapping()
    {
        CreateMap<ManagementUser, ManagementUserResponse>()
         .ForMember(d => d.IsEmailConfirmed, o => o.MapFrom(s => s.EmailConfirmed))
         .ForMember(d => d.IsPhoneNumberConfirmed, o => o.MapFrom(s => s.PhoneNumberConfirmed))
         .ForMember(d => d.IsTwoFactorEnabled, o => o.MapFrom(s => s.TwoFactorEnabled))
         .ForMember(d => d.Metadata, o => o.MapFrom(s => new Metadata {
            CreatedAt = s.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = s.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            LastLoginAt = s.LastLogin.ToString("yyyy-MM-dd HH:mm:ss"),
            CreatedBy = s.CreatedBy,
            UpdatedBy = s.UpdatedBy,
         }))
         .ForMember(d => d.Roles, o => o.MapFrom(s => s.GetDetailedUserRoleMappings()))
         .ForMember(d => d.Permissions, o => o.MapFrom(s => s.GetUserPermissionMappings()))
         .ReverseMap();

        CreateMap<CreateManagementUserRequest, ManagementUser>()
        .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.Roles.Count > 0 
            ? IdGenerator.NewManagementUserId(s.Department, s.Roles.First()) 
            : IdGenerator.NewId()))
        .ReverseMap();
    }
}
