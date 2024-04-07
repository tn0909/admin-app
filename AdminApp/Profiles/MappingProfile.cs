using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApp.Dtos;
using AdminApp.Models;
using AutoMapper;

namespace AdminApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.JoinField, opt => opt.MapFrom(src => Nest.JoinField.Link<User>(src.CompanyId)));

            CreateMap<Company, CompanyResponseDto>();

            CreateMap<CompanyRequestDto, Company>()
                .ForMember(dest => dest.JoinField, opt => opt.MapFrom(src => typeof(Company)));
        }
    }
}