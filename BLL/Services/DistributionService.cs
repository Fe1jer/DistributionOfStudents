using BLL.DTO;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Students;
using BLL.Extensions;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.Specifications;


namespace BLL.Services
{
    public class DistributionService : BaseService, IDistributionService
    {
        public DistributionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>>> GetAsync(string facultyUrl, Guid groupId)
        {
            var distribution = await GetDistribution(facultyUrl, groupId);

            return distribution.GetPlansWithEnrolledStudents();
        }

        public async Task<float> GetCompetitionAsync(string facultyUrl, Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions().IncludeSpecialties());
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereGroup(group));
            List<RecruitmentPlanDTO> planDtos = Mapper.Map<List<RecruitmentPlanDTO>>(plans);
            List<AdmissionDTO> admissionDtos = Mapper.Map<List<AdmissionDTO>>(group.Admissions);

            return (float)Math.Round(planDtos.Distribution(admissionDtos).Competition(admissionDtos), 2);
        }

        public async Task<bool> ExistsEnrolledStudentsAsync(string facultyUrl, Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties());
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyUrl).WhereGroup(group));

            return plans.Exists(i => i.EnrolledStudents?.Count > 0);
        }

        public async Task SaveAsync(string facultyUrl, Guid groupId, List<PlanForDistributionDTO> models, bool notify)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions().IncludeSpecialties());
            group.IsCompleted = true;
            await _unitOfWork.GroupsOfSpecialities.InsertOrUpdateAsync(group);

            List<AdmissionDTO> admissionDtos = Mapper.Map<List<AdmissionDTO>>(group.Admissions);
            List<RecruitmentPlanDTO> plans = await GetPlansFromModelsAsync(models, facultyUrl, group);
            var distribution = plans.Distribution(admissionDtos);

            foreach (var plan in distribution.GetPlansWithPassingScores().Keys)
            {
                RecruitmentPlan? entity = await _unitOfWork.RecruitmentPlans.GetByIdAsync(plan.Id, new RecruitmentPlansSpecification().IncludeEnrolledStudents());
                Mapper.Map(plan, entity);
                foreach (var item in entity.EnrolledStudents)
                {
                    item.Student = await _unitOfWork.Students.GetByIdAsync(item.Student.Id);
                }
                await _unitOfWork.RecruitmentPlans.InsertOrUpdateAsync(entity);
            }
            _unitOfWork.Commit();

            if (notify) distribution.NotifyEnrolledStudents(admissionDtos);
        }

        public async Task<Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>>> CreateAsync(string facultyUrl, Guid groupId, List<PlanForDistributionDTO> models)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions().IncludeSpecialties());
            List<AdmissionDTO> admissionDtos = Mapper.Map<List<AdmissionDTO>>(group?.Admissions);
            List<RecruitmentPlanDTO> plans = await GetPlansFromModelsAsync(models, facultyUrl, group);

            return plans.Distribution(admissionDtos).GetPlansWithEnrolledStudents();
        }

        public async Task DeleteAsync(string facultyUrl, Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyUrl).IncludeAdmissions().IncludeSpecialties());
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyUrl).WhereGroup(group));

            group.IsCompleted = false;
            await _unitOfWork.GroupsOfSpecialities.InsertOrUpdateAsync(group);
            foreach (RecruitmentPlan plan in plans)
            {
                plan.PassingScore = 0;
                plan.EnrolledStudents = null;
                await _unitOfWork.RecruitmentPlans.InsertOrUpdateAsync(plan);
            }
            _unitOfWork.Commit();
        }

        private async Task<Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>>> GetDistribution(string facultyUrl, Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions().IncludeSpecialties());
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereGroup(group));
            List<RecruitmentPlanDTO> planDtos = Mapper.Map<List<RecruitmentPlanDTO>>(plans);
            List<AdmissionDTO> admissionDtos = Mapper.Map<List<AdmissionDTO>>(group.Admissions);

            return planDtos.Distribution(admissionDtos);
        }

        private async Task<List<RecruitmentPlanDTO>> GetPlansFromModelsAsync(IEnumerable<PlanForDistributionDTO> models, string facultyUrl, GroupOfSpecialities group)
        {
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereGroup(group));
            foreach (PlanForDistributionDTO distributedPlan in models)
            {
                RecruitmentPlan? plan = await _unitOfWork.RecruitmentPlans.GetByIdAsync(distributedPlan.Id, new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereGroup(group));
                if (plan is not null)
                {
                    plan.EnrolledStudents = new();
                    plan.PassingScore = distributedPlan.PassingScore;
                    foreach (IsDistributedStudentDTO distributedStudent in distributedPlan.DistributedStudents.Where(i => i.IsDistributed))
                    {
                        Student? student = await _unitOfWork.Students.GetByIdAsync(distributedStudent.StudentId);
                        if (student is not null)
                        {
                            plan.EnrolledStudents.Add(new EnrolledStudent() { Student = student });
                        }
                    }
                    plans = plans.Select(i => i.Id != plan.Id ? i : plan).ToList();
                }
            }

            return Mapper.Map<List<RecruitmentPlanDTO>>(plans);
        }
    }
}
