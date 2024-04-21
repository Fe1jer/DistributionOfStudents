using AutoMapper;
using BLL.DTO;

namespace BLL.Mappers
{
    public class DTOToDTOMappingProfile : Profile
    {
        public DTOToDTOMappingProfile()
        {
            CreateMap<AdmissionDTO, AdmissionDTO>();
        }
        public override string ProfileName => "DTOToDTOMapping";
    }
}
