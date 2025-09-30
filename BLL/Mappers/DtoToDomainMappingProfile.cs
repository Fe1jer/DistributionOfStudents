using AutoMapper;
using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Students;
using BLL.DTO.Subjects;
using DAL.Postgres.Entities;

namespace BLL.Mappers
{
    public class DTOToDomainMappingProfile : Profile
    {
        public DTOToDomainMappingProfile()
        {
            CreateMap<DateTime, DateTime>().ConvertUsing((src, dest) =>
                src.ToUniversalTime());

            CreateMap<AdmissionDTO, Admission>()
                .ForMember(p => p.SpecialityPriorities, opts => opts.MapFrom(p => p.SpecialityPriorities.Where(p => p.Priority > 0)));
            CreateMap<Admission, StudentItemDTO>()
                .ForMember(p => p.FullName, opts => opts.MapFrom(p => string.Join(' ', p.Student.Surname, p.Student.Name, p.Student.Patronymic)))
                .ForMember(p => p.FacultyName, opts => opts.MapFrom(p => p.GroupOfSpecialties.Specialities!.First().Faculty.ShortName))
                .ForMember(p => p.GroupName, opts => opts.MapFrom(p => p.GroupOfSpecialties.Name));

            CreateMap<EnrolledStudentDTO, EnrolledStudent>();
            CreateMap<FacultyDTO, Faculty>();
            CreateMap<FormOfEducationDTO, FormOfEducation>();

            CreateMap<GroupOfSpecialitiesDTO, GroupOfSpecialities>();
            CreateMap<UpdateGroupOfSpecialitiesDTO, GroupOfSpecialities>()
                .ForMember(p => p.Specialities, opts => opts.Ignore())
                .ForMember(p => p.Subjects, opts => opts.Ignore());

            CreateMap<RecruitmentPlanDTO, RecruitmentPlan>();
            CreateMap<SpecialityDTO, Speciality>();
            CreateMap<SpecialityPriorityDTO, SpecialityPriority>()
                .ForMember(p => p.RecruitmentPlan, opts => opts.Ignore())
                .ForMember(p => p.Id, opts => opts.Ignore());
            CreateMap<StudentDTO, Student>();
            CreateMap<StudentScoreDTO, StudentScore>()
                .ForMember(p => p.Subject, opts => opts.Ignore())
                .ForMember(p => p.Id, opts => opts.Ignore());
            CreateMap<SubjectDTO, Subject>();
        }
        public override string ProfileName => "DTOToDomainMappings";
    }
}
