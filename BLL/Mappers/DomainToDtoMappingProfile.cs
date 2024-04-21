using AutoMapper;
using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Students;
using BLL.DTO.Subjects;
using BLL.DTO.User;
using DAL.Postgres.Entities;

namespace BLL.Mappers
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Admission, AdmissionDTO>();
            CreateMap<EnrolledStudent, EnrolledStudentDTO>();
            CreateMap<Faculty, FacultyDTO>();
            CreateMap<Faculty, FacultyRecruitmentPlanDTO>();
            CreateMap<FormOfEducation, FormOfEducationDTO>();
            CreateMap<GroupOfSpecialities, GroupOfSpecialitiesDTO>();
            CreateMap<RecruitmentPlan, RecruitmentPlanDTO>();
            CreateMap<Speciality, SpecialityDTO>();
            CreateMap<SpecialityPriority, SpecialityPriorityDTO>();
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentScore, StudentScoreDTO>();
            CreateMap<Subject, SubjectDTO>();
        }
        public override string ProfileName => "DomainToDTOMappings";
    }
}
