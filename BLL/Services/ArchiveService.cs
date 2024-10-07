using BLL.DTO;
using BLL.DTO.Faculties;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Specialities;
using BLL.Extensions;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Specifications;

namespace BLL.Services
{
    public class ArchiveService : BaseService, IArchiveService
    {
        public ArchiveService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<int>> GetYears()
        {
            int maxYear = DateTime.Now.Year - 1;
            List<GroupOfSpecialities> groups = await _unitOfWork.GroupsOfSpecialities.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.FormOfEducation.Year <= maxYear).WhereCompleted());
            List<int> allYears = groups.Select(i => i.FormOfEducation.Year).Distinct().OrderBy(i => i).ToList();

            return allYears;
        }

        public async Task<List<string>> GetForms(int year)
        {
            List<GroupOfSpecialities> groups = await _unitOfWork.GroupsOfSpecialities.GetAllAsync(new GroupsOfSpecialitiesSpecification().WhereCompleted().WhereYear(year));
            return groups.Select(i => i.Name).Distinct().ToList();
        }

        public async Task<List<ArchiveFacultyDTO>> GetFacultiesArchive(int year, string form)
        {
            List<ArchiveFacultyDTO> models = Mapper.Map<List<ArchiveFacultyDTO>>(await _unitOfWork.Faculties.GetAllAsync());

            foreach (ArchiveFacultyDTO faculty in models)
            {
                List<ArchiveGroupOfSpecialitiesDTO> archiveGroups = new();
                List<GroupOfSpecialities> groups = await _unitOfWork.GroupsOfSpecialities.GetAllAsync(new GroupsOfSpecialitiesSpecification(i => i.Name == form)
                    .WhereFaculty(faculty.ShortName).WhereYear(year).WhereCompleted().IncludeAdmissions());

                foreach (GroupOfSpecialities group in groups)
                {
                    List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification()
                        .IncludeSpecialty().IncludeEnrolledStudents().WhereGroup(group));

                    List<RecruitmentPlanDTO> planDtos = Mapper.Map<List<RecruitmentPlanDTO>>(plans);
                    List<AdmissionDTO> addmissionDtos = Mapper.Map<List<AdmissionDTO>>(group.Admissions);

                    ArchiveGroupOfSpecialitiesDTO archiveGroup = Mapper.Map<ArchiveGroupOfSpecialitiesDTO>(group);
                    archiveGroup.Competition = planDtos.Distribution(addmissionDtos).Competition(addmissionDtos);
                    archiveGroup.SpetialityPlans = Mapper.Map<List<ArchiveSpecialityPlanDTO>>(plans).OrderBy(i => i.Code).ToList();

                    archiveGroups.Add(archiveGroup);
                }
                faculty.GroupsOfSpecialities = archiveGroups;
            }

            return models;
        }
    }
}
