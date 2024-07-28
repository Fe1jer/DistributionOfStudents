using AutoMapper;
using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Students;
using BLL.DTO.Subjects;
using BLL.DTO.User;
using webapi.ViewModels.Admissions;
using webapi.ViewModels.Distribution;
using webapi.ViewModels.Faculties;
using webapi.ViewModels.General;
using webapi.ViewModels.GroupsOfSpecialities;
using webapi.ViewModels.RecruitmentPlans;
using webapi.ViewModels.Specialities;
using webapi.ViewModels.Students;
using webapi.ViewModels.Users;

namespace webapi.Mappers
{
    public class DtoToViewModelMappingProfile : Profile
    {
        public DtoToViewModelMappingProfile()
        {
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<AdmissionDTO, AdmissionViewModel>();

            CreateMap<FacultyDTO, FacultyViewModel>();
            CreateMap<FacultyDTO, DetailsFacultyViewModel>();
            CreateMap<FacultyRecruitmentPlanDTO, DetailsFacultyPlansViewModel>();

            CreateMap<FormOfEducationDTO, FormOfEducationViewModel>();
            CreateMap<GroupOfSpecialitiesDTO, GroupOfSpecialitiesViewModel>();

            CreateMap<RecruitmentPlanDTO, RecruitmentPlanViewModel>();
            CreateMap<RecruitmentPlanDTO, RecruitmentPlanItemViewModel>()
                .ForMember(p => p.SpecialityName, opts => opts.MapFrom(p => p.Speciality.DirectionName ?? p.Speciality.FullName));
            CreateMap<SpecialityPlansDTO, SpecialityPlansViewModel>();
            CreateMap<RecruitmentPlanDTO, DistributedPlanViewModel>();

            CreateMap<SpecialityDTO, SpecialityViewModel>();
            CreateMap<SpecialityPriorityDTO, SpecialityPriorityViewModel>();

            CreateMap<StudentItemDTO, StudentItemViewModel>();
            CreateMap<StudentDTO, StudentViewModel>();
            CreateMap<EnrolledStudentDTO, EnrolledStudentViewModel>();

            CreateMap<StudentScoreDTO, StudentScoreViewModel>();
            CreateMap<SubjectDTO, SubjectViewModel>();
        }
        public override string ProfileName => "DomainToDTOMappings";
    }
}
