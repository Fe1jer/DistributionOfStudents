using AutoMapper;
using BLL.DTO;
using BLL.DTO.User;
using DAL.Postgres.Entities;

namespace BLL.Mappers
{
    public class DTOToDomainMappingProfile : Profile
    {
        public DTOToDomainMappingProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(p => p.PasswordHash, opts => opts.Ignore());
            CreateMap<AdmissionDTO, Admission>();
            CreateMap<EnrolledStudentDTO, EnrolledStudent>();
            CreateMap<FacultyDTO, Faculty>();
            CreateMap<FormOfEducationDTO, FormOfEducation>();
            CreateMap<GroupOfSpecialitiesStatisticDTO, GroupOfSpecialitiesStatistic>();
            CreateMap<GroupOfSpecialitiesDTO, GroupOfSpecialities>();
            CreateMap<RecruitmentPlanDTO, RecruitmentPlan>();
            CreateMap<RecruitmentPlanStatisticDTO, RecruitmentPlanStatistic>();
            CreateMap<SpecialityDTO, Speciality>();
            CreateMap<SpecialityPriorityDTO, SpecialityPriority>();
            CreateMap<StudentDTO, Student>();
            CreateMap<StudentScoreDTO, StudentScore>();
            CreateMap<SubjectDTO, Subject>();
        }
        public override string ProfileName => "DTOToDomainMappings";
    }
}
