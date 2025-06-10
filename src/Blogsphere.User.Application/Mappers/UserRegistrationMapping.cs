using AutoMapper;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Models.Requests.Registration;

namespace Blogsphere.User.Application.Mappers;

public class UserRegistrationMapping : Profile
{
    public UserRegistrationMapping()
    {
        CreateMap<RegistrationRequest, ApplicationUser>()
        .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email))
        .ForMember(d => d.Profile, o => o.MapFrom(s => new ProfileDetails
        {
            Bio = s.Bio,
            WebsiteUrl = s.WebsiteUrl,
            LinkedIn = s.LinkedIn,
            Twitter = s.Twitter,
            Instagram = s.Instagram,
        }))
        .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set manually later
        .ForMember(dest => dest.UpdateBy, opt => opt.Ignore()) // Set manually later
        .ForMember(dest => dest.Image, opt => opt.Ignore())     // Set via method if needed
        .ForMember(dest => dest.UserRoles, opt => opt.Ignore()) // Set manually based on Role
        .ForMember(dest => dest.LastLogin, opt => opt.Ignore()) // Set during login
        .ReverseMap();
    }
}
