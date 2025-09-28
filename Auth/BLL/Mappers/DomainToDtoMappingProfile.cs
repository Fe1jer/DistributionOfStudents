using AutoMapper;
using BLL.DTO.User;
using DAL.Entities;

namespace BLL.Mappers
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<User, UserDTO>();
        }
        public override string ProfileName => "DomainToDTOMappings";
    }
}
