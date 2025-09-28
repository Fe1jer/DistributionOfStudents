using AutoMapper;
using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Students;
using BLL.DTO.Subjects;
using webapi.ViewModels.Admissions;
using webapi.ViewModels.Distribution;
using webapi.ViewModels.Faculties;
using webapi.ViewModels.General;
using webapi.ViewModels.GroupsOfSpecialities;
using webapi.ViewModels.RecruitmentPlans;
using webapi.ViewModels.Specialities;
using webapi.ViewModels.Students;

namespace webapi.Mappers
{
    public class ViewModelToDtoMappingProfile : Profile
    {
        public ViewModelToDtoMappingProfile()
        {
            CreateMap<AdmissionViewModel, AdmissionDTO>();
            CreateMap<FacultyViewModel, FacultyDTO>();
            CreateMap<FormOfEducationViewModel, FormOfEducationDTO>();

            CreateMap<GroupOfSpecialitiesViewModel, GroupOfSpecialitiesDTO>();
            CreateMap<UpdateGroupOfSpecialitiesViewModel, UpdateGroupOfSpecialitiesDTO>();
            CreateMap<SelectedSubjectViewModel, SelectedSubjectDTO>();
            CreateMap<SelectedSpecialityViewModel, SelectedSpecialityDTO>();

            CreateMap<RecruitmentPlanViewModel, RecruitmentPlanDTO>();
            CreateMap<SpecialityPlansViewModel, SpecialityPlansDTO>();
            CreateMap<PlanForDistributionViewModel, PlanForDistributionDTO>();

            CreateMap<SpecialityViewModel, SpecialityDTO>();
            CreateMap<SpecialityPriorityViewModel, SpecialityPriorityDTO>();
            CreateMap<StudentScoreViewModel, StudentScoreDTO>();
            CreateMap<SubjectViewModel, SubjectDTO>();

            CreateMap<IsDistributedStudentVM, IsDistributedStudentDTO>();
            CreateMap<StudentViewModel, StudentDTO>();
        }
        public override string ProfileName => "DTOToDomainMappings";
    }
}
