﻿using AutoMapper;
using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.DTO.Students;
using BLL.DTO.Subjects;
using BLL.DTO.User;
using DAL.Postgres.Entities;
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
            CreateMap<SpecialityPlansDTO, SpecialityPlansViewModel>();

            CreateMap<SpecialityDTO, SpecialityViewModel>();
            CreateMap<SpecialityPriorityDTO, SpecialityPriorityViewModel>();
            CreateMap<StudentDTO, StudentViewModel>();
            CreateMap<StudentScoreDTO, StudentScoreViewModel>();
            CreateMap<SubjectDTO, SubjectViewModel>();
        }
        public override string ProfileName => "DomainToDTOMappings";
    }
}
