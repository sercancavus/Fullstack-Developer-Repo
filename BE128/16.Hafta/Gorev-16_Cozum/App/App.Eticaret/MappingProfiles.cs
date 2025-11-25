using App.Eticaret.Models.ViewModels;
using App.Models.DTO.Auth;
using AutoMapper;

namespace App.Eticaret
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LoginViewModel, AuthLoginRequest>()
                .ReverseMap();
            CreateMap<RegisterUserViewModel, AuthRegisterRequest>()
                .ReverseMap();
            CreateMap<ForgotPasswordViewModel, AuthForgotPasswordRequest>()
                .ReverseMap();

            CreateMap<RenewPasswordViewModel, AuthResetPasswordRequest>()
                .AfterMap((src, dest) => dest.PasswordRepeat = src.ConfirmPassword)
                .ReverseMap()
                .ForMember(x => x.ConfirmPassword, opt => opt.MapFrom(x => x.PasswordRepeat));
        }
    }
}