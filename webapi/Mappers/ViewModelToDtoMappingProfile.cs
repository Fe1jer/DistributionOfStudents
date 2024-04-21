using AutoMapper;
using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Students;
using BLL.DTO.Subjects;
using BLL.DTO.User;
using webapi.ViewModels;
using webapi.ViewModels.Admissions;
using webapi.ViewModels.Faculties;
using webapi.ViewModels.General;
using webapi.ViewModels.GroupsOfSpecialities;
using webapi.ViewModels.RecruitmentPlans;
using webapi.ViewModels.Specialities;
using webapi.ViewModels.Students;
using webapi.ViewModels.Users;

namespace webapi.Mappers
{
    public class ViewModelToDtoMappingProfile : Profile
    {
        public ViewModelToDtoMappingProfile()
        {
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<LoginViewModel, LoginDTO>();
            CreateMap<AdmissionViewModel, AdmissionDTO>();
            CreateMap<FacultyViewModel, FacultyDTO>();
            CreateMap<FormOfEducationViewModel, FormOfEducationDTO>();
            CreateMap<GroupOfSpecialitiesViewModel, GroupOfSpecialitiesDTO>();
            CreateMap<RecruitmentPlanViewModel, RecruitmentPlanDTO>();
            CreateMap<SpecialityViewModel, SpecialityDTO>();
            CreateMap<SpecialityPlansViewModel, SpecialityPlansDTO>();
            CreateMap<SpecialityPriorityViewModel, SpecialityPriorityDTO>();
            CreateMap<StudentViewModel, StudentDTO>();
            CreateMap<StudentScoreViewModel, StudentScoreDTO>();
            CreateMap<SubjectViewModel, SubjectDTO>();
        }
        public override string ProfileName => "DTOToDomainMappings";
    }
}
