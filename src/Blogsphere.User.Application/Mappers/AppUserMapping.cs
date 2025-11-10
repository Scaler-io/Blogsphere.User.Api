using AutoMapper;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Mappers;

public class AppUserMapping : Profile
{
    public AppUserMapping()
    {
        CreateMap<ApplicationUser, AppUserResponse>()
        .ForMember(d => d.IsEmailConfirmed, o => o.MapFrom(s => s.EmailConfirmed))
        .ForMember(d => d.IsPhoneNumberConfirmed, o => o.MapFrom(s => s.PhoneNumberConfirmed))
        .ForMember(d => d.IsTwoFactorEnabled, o => o.MapFrom(s => s.TwoFactorEnabled))
        .ForMember(d => d.Metadata, o => o.MapFrom(s => new Metadata {
            CreatedAt = s.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = s.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            LastLoginAt = s.LastLogin.ToString("yyyy-MM-dd HH:mm:ss"),
            CreatedBy = s.CreatedBy,
            UpdatedBy = s.UpdateBy,
        }))
        .ForMember(d => d.Roles, o => o.MapFrom(s => s.GetDetailedUserRoleMappings()))
        .ForMember(d => d.Permissions, o => o.MapFrom(s => s.GetUserPermissionMappings()))
        .ForMember(d => d.Profile, o => o.MapFrom(s => s.Profile))
        .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Image.Url))
        .ReverseMap();

        CreateMap<ProfileDetails, ProfileResponse>()
        .ForMember(d => d.Bio, o => o.MapFrom(s => s.Bio))
        .ForMember(d => d.WebsiteUrl, o => o.MapFrom(s => s.WebsiteUrl))
        .ForMember(d => d.LinkedIn, o => o.MapFrom(s => s.LinkedIn))
        .ForMember(d => d.Twitter, o => o.MapFrom(s => s.Twitter))
        .ForMember(d => d.Instagram, o => o.MapFrom(s => s.Instagram))
        .ReverseMap();
    }
}
