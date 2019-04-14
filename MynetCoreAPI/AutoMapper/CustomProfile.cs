using AutoMapper;
using MynetCore.Model.DTO;
using MynetCore.Model.Entity;

namespace MynetCoreAPI.AutoMapper
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            
            CreateMap<UserInfo, UserInfoDTO>().ForMember(d => d.gender, s => s.MapFrom(a => a.usersex));
            

        }
    }
}
