using BLL.DTO.RecruitmentPlans;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Specifications;

namespace BLL.Services
{
    public class RecruitmentPlansService : BaseService, IRecruitmentPlansService
    {
        public RecruitmentPlansService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<RecruitmentPlanDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification());
            return Mapper.Map<List<RecruitmentPlanDTO>>(result);
        }

        public async Task<RecruitmentPlanDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.RecruitmentPlans.GetByIdAsync(id, new RecruitmentPlansSpecification());
            return Mapper.Map<RecruitmentPlanDTO>(entity);
        }

        public async Task<List<FacultyRecruitmentPlanDTO>> GetLastFacultiesPlansAsync()
        {
            List<FacultyRecruitmentPlanDTO> result = new();
            List<Faculty> faculties = await _unitOfWork.Faculties.GetAllAsync();
            int lastYear = await _unitOfWork.RecruitmentPlans.GetLastYearAsync();

            foreach (var faculty in faculties)
            {
                FacultyRecruitmentPlanDTO? facultyPlan = Mapper.Map<FacultyRecruitmentPlanDTO>(faculty);

                facultyPlan.Year = await _unitOfWork.RecruitmentPlans.GetLastYearAsync();
                facultyPlan.PlansForSpecialities = await GetByFacultyAsync(faculty.ShortName, lastYear);

                result.Add(facultyPlan);
            }

            return result;
        }

        public async Task<FacultyRecruitmentPlanDTO> GetLastByFacultyAsync(string facultyUrl)
        {
            Faculty? faculty = await _unitOfWork.Faculties.GetByUrlAsync(facultyUrl, new FacultiesSpecification().IncludeSpecialties());
            FacultyRecruitmentPlanDTO? facultyPlan = Mapper.Map<FacultyRecruitmentPlanDTO>(faculty);

            facultyPlan.Year = await _unitOfWork.RecruitmentPlans.GetLastYearAsync();
            facultyPlan.PlansForSpecialities = await GetByFacultyAsync(facultyUrl, facultyPlan.Year);

            return facultyPlan;
        }

        public async Task<List<RecruitmentPlanDTO>> GetByFacultyAsync(string facultyUrl, int year)
        {
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereYear(year));

            return Mapper.Map<List<RecruitmentPlanDTO>>(plans);
        }

        public async Task<List<RecruitmentPlanDTO>> GetByGroupAsync(Guid groupId)
        {
            GroupOfSpecialities? group = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification());
            List<RecruitmentPlan> plans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereGroup(group));

            return Mapper.Map<List<RecruitmentPlanDTO>>(plans);
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await _unitOfWork.RecruitmentPlans.GetByIdAsync(id);
            await _unitOfWork.RecruitmentPlans.DeleteAsync(toDelete);
            _unitOfWork.Commit();
        }

        public async Task DeleteAsync(string facultyUrl, int year)
        {
            IEnumerable<RecruitmentPlan> toDelete = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereYear(year));

            foreach (RecruitmentPlan plan in toDelete)
            {
                await _unitOfWork.RecruitmentPlans.DeleteAsync(plan);
            }

            _unitOfWork.Commit();
        }

        public async Task<RecruitmentPlanDTO> SaveAsync(RecruitmentPlanDTO model)
        {
            RecruitmentPlan? entity;
            if (model.IsNew)
            {
                entity = Mapper.Map<RecruitmentPlan>(model);
            }
            else
            {
                entity = await _unitOfWork.RecruitmentPlans.GetByIdAsync(model.Id);
                Mapper.Map(model, entity);
            }

            await _unitOfWork.RecruitmentPlans.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<RecruitmentPlanDTO>(entity);
        }

        public async Task SaveAsync(List<SpecialityPlansDTO> plans, string facultyUrl, int year)
        {
            IEnumerable<RecruitmentPlan> allPlans = await _unitOfWork.RecruitmentPlans.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyUrl).WhereYear(year));
            IEnumerable<RecruitmentPlan> changedPlans = await CreateFacultyPlans(plans, year);
            IEnumerable<RecruitmentPlan> diff = allPlans.Except(changedPlans);

            foreach (RecruitmentPlan plan in diff)
            {
                await _unitOfWork.RecruitmentPlans.DeleteAsync(plan);
            }
            foreach (RecruitmentPlan plan in changedPlans)
            {
                await _unitOfWork.RecruitmentPlans.InsertOrUpdateAsync(plan);
            }

            _unitOfWork.Commit();
        }

        private async Task<IEnumerable<RecruitmentPlan>> CreateFacultyPlans(IEnumerable<SpecialityPlansDTO> facultyPlans, int year)
        {
            List<RecruitmentPlan> recruitmentPlans = new();

            foreach (SpecialityPlansDTO plans in facultyPlans)
            {
                if (plans.DailyFullBudget > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.DailyFullBudget, true, true, true, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.DailyFullPaid > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.DailyFullPaid, true, true, false, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.DailyAbbreviatedBudget > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.DailyAbbreviatedBudget, true, false, true, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.DailyAbbreviatedPaid > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.DailyAbbreviatedPaid, true, false, false, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.EveningFullBudget > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.EveningFullBudget, false, true, true, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.EveningFullPaid > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.EveningFullPaid, false, true, false, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.EveningAbbreviatedBudget > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.EveningAbbreviatedBudget, false, false, true, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
                if (plans.EveningAbbreviatedPaid > 0)
                {
                    RecruitmentPlan plan = await CreatePlan(plans.EveningAbbreviatedPaid, false, false, false, year, plans.SpecialityId);
                    recruitmentPlans.Add(plan);
                }
            }

            return recruitmentPlans;
        }

        public async Task<RecruitmentPlan> CreatePlan(int count, bool isDailyForm, bool isFullTime, bool isBudget, int year, Guid specialityId)
        {
            FormOfEducation form = new()
            {
                IsDailyForm = isDailyForm,
                IsBudget = isBudget,
                IsFullTime = isFullTime,
                Year = year,
            };
            RecruitmentPlan? plan = await _unitOfWork.RecruitmentPlans.GetBySpecialityAndFormAsync(specialityId, form) ?? new();

            plan.Count = count;
            plan.FormOfEducation = await _unitOfWork.FormsOfEducation.GetByFormAsync(form) ?? form;
            plan.SpecialityId = specialityId;

            return plan;
        }
    }
}
