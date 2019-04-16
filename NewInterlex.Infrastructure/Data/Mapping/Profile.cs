using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using NewInterlex.Core.Entities;
using NewInterlex.Infrastructure.Identity;

namespace NewInterlex.Infrastructure.Data.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<User, ApplicationUser>().ConstructUsing(u => new ApplicationUser { UserName = u.UserName, Email = u.Email }).ForMember(u => u.Id, opt => opt.Ignore());
            this.CreateMap<ApplicationUser, User>().ForMember(dest => dest.Email, opt => opt.MapFrom(u => u.Email))
                .ForAllOtherMembers(expr => expr.Ignore());
        }
    }
}
