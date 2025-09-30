using AutoMapper;
using BLL.DTO.User;
using DAL.Entities;

namespace BLL.Mappers
{
    public class DTOToDomainMappingProfile : Profile
    {
        public DTOToDomainMappingProfile()
        {
            CreateMap<DateTime, DateTime>().ConvertUsing((src, dest) =>
                src.ToUniversalTime());

            CreateMap<UserDTO, User>()
                .ForMember(p => p.PasswordHash, opts => opts.Ignore());
            CreateMap<RegisterDTO, User>();
        }
        public override string ProfileName => "DTOToDomainMappings";
    }
}
