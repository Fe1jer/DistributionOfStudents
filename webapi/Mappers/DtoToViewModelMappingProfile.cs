using AutoMapper;
using BLL.DTO;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Subjects;
using BLL.DTO.User;
using webapi.ViewModels.Admissions;
using webapi.ViewModels.Faculties;
using webapi.ViewModels.General;
using webapi.ViewModels.GroupsOfSpecialities;
using webapi.ViewModels.RecruitmentPlans;
using webapi.ViewModels.Specialities;
using webapi.ViewModels.Users;

namespace webapi.Mappers
{
    public class DtoToViewModelMappingProfile : Profile
    {
        public DtoToViewModelMappingProfile()
        {
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<AdmissionDTO, AdmissionViewModel>();
            CreateMap<EnrolledStudentDTO, EnrolledStudentViewModel>();
            CreateMap<FacultyDTO, FacultyViewModel>();
            CreateMap<FormOfEducationDTO, FormOfEducationViewModel>();
            CreateMap<GroupOfSpecialitiesDTO, GroupOfSpecialitiesViewModel>();
            CreateMap<RecruitmentPlanDTO, RecruitmentPlanViewModel>();
            CreateMap<SpecialityDTO, SpecialityViewModel>();
            CreateMap<SpecialityPriorityDTO, SpecialityPriorityViewModel>();
            CreateMap<StudentDTO, StudentViewModel>();
            CreateMap<StudentScoreDTO, StudentScoreViewModel>();
            CreateMap<SubjectDTO, SubjectViewModel>();
        }
        public override string ProfileName => "DomainToDTOMappings";
    }
}
