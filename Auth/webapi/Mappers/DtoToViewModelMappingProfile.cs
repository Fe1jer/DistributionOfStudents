using AutoMapper;
using BLL.DTO.User;
using webapi.ViewModels.Users;

namespace webapi.Mappers
{
    public class DtoToViewModelMappingProfile : Profile
    {
        public DtoToViewModelMappingProfile()
        {
            CreateMap<UserDTO, UserViewModel>();
        }
        public override string ProfileName => "DomainToDTOMappings";
    }
}
