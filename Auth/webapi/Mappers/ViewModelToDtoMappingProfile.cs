using AutoMapper;
using BLL.DTO.User;
using webapi.ViewModels;
using webapi.ViewModels.Users;

namespace webapi.Mappers
{
    public class ViewModelToDtoMappingProfile : Profile
    {
        public ViewModelToDtoMappingProfile()
        {
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<LoginViewModel, LoginDTO>();
        }
        public override string ProfileName => "DTOToDomainMappings";
    }
}
