using BLL.DTO;
using BLL.Services.Interfaces;

namespace BLL.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly List<RecruitmentPlanDTO> _recruitmentPlans;
        private readonly List<AdmissionDTO> _admissions;
        private readonly Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> _distributedStudents;

        public DistributionService(List<RecruitmentPlanDTO> recruitmentPlans, List<AdmissionDTO>? admissions)
        {
            _recruitmentPlans = recruitmentPlans;
            _admissions = admissions ?? new();
            _distributedStudents = new();

            _recruitmentPlans.ForEach(plan => _distributedStudents.Add(plan, new List<AdmissionDTO>()));
            if (_recruitmentPlans.All(i => i.EnrolledStudents == null))
            {
                DistridutionPlans(_recruitmentPlans, GetCloneOfAdmissions());
            }
            else
            {
                DistridutionControversialPlans();
            }
        }

        private List<AdmissionDTO> GetCloneOfAdmissions()
        {
            return _admissions.Select(i => new AdmissionDTO
            {
                SpecialityPriorities = i.SpecialityPriorities.OrderBy(p => p.Priority).ToList(),
                StudentScores = i.StudentScores,
                Student = i.Student,
                Email = i.Email,
                IsOutOfCompetition = i.IsOutOfCompetition,
                IsTargeted = i.IsTargeted,
                IsWithoutEntranceExams = i.IsWithoutEntranceExams
            }).ToList();
        }

        private void DistridutionControversialPlans()
        {
            List<RecruitmentPlanDTO> controversialPlans = GetControversialPlans(_recruitmentPlans.ToList(), out List<AdmissionDTO> freeAdmissions);
            DistridutionPlans(controversialPlans, freeAdmissions);
        }

        private List<RecruitmentPlanDTO> GetControversialPlans(List<RecruitmentPlanDTO> plans, out List<AdmissionDTO> freeAdmissions)
        {
            freeAdmissions = GetCloneOfAdmissions();
            foreach (RecruitmentPlanDTO plan in plans.OrderByDescending(i => i.PassingScore).ToList())
            {
                if (plan.EnrolledStudents != null && plan.EnrolledStudents.Count != 0 && plan.Count <= plan.EnrolledStudents.Count)
                {
                    freeAdmissions.RemoveAll(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id));
                    freeAdmissions.ForEach(admission => admission.SpecialityPriorities.RemoveAll(i => i.RecruitmentPlan.Id == plan.Id));
                    plans.Remove(plan);
                }
            }

            return plans;
        }

        public float Competition
        {
            get
            {
                float allPlans = _recruitmentPlans.Sum(i => i.Count);
                if (allPlans == 0)
                {
                    return 0;
                }
                return _admissions.Count / allPlans;
            }
        }

        public bool AreControversialStudents()
        {
            return _recruitmentPlans.Any(i => (i.EnrolledStudents ?? new()).Count > i.Count);
        }

        public List<RecruitmentPlanDTO> GetPlansWithEnrolledStudents()
        {
            GetPlansWithPassingScores();
            foreach (KeyValuePair<RecruitmentPlanDTO, List<AdmissionDTO>> keyValuePair in _distributedStudents)
            {
                if (keyValuePair.Key.EnrolledStudents == null || keyValuePair.Key.EnrolledStudents.Count == 0)
                {
                    keyValuePair.Key.EnrolledStudents = keyValuePair.Value.Select(i => new EnrolledStudentDTO { Student = i.Student }).ToList();
                }
            }
            return _recruitmentPlans;
        }

        public List<RecruitmentPlanDTO> GetPlansWithPassingScores()
        {
            foreach (KeyValuePair<RecruitmentPlanDTO, List<AdmissionDTO>> keyValuePair in _distributedStudents)
            {
                if (keyValuePair.Key.EnrolledStudents == null || keyValuePair.Key.Count == 0)
                {
                    keyValuePair.Key.TargetPassingScore = keyValuePair.Key.Target == 0 || keyValuePair.Key.Target > keyValuePair.Value.Where(s => s.IsTargeted).Count() ? 0 :
                        keyValuePair.Value.Where(s => s.IsTargeted).OrderByDescending(i => i.Score).Take(keyValuePair.Key.Target).Min(a => a.Score);

                    keyValuePair.Key.PassingScore = keyValuePair.Key.Count > keyValuePair.Value.Count ? 0 :
                        keyValuePair.Value.Where(s => !(s.IsTargeted && s.Score >= keyValuePair.Key.TargetPassingScore)
                        && !s.IsWithoutEntranceExams && !s.IsOutOfCompetition).Min(a => a.Score);
                }
            }

            return _recruitmentPlans;
        }

        private void DistridutionPlans(List<RecruitmentPlanDTO> plans, List<AdmissionDTO> freeAdmissions)
        {
            List<AdmissionDTO> tempDistributedAdmissions = new();

            for (int i = 0; i < plans.Count; i++)
            {
                GetPlansWithPassingScores();
                foreach (RecruitmentPlanDTO plan in plans.OrderByDescending(i => i.PassingScore))
                {
                    tempDistributedAdmissions = _distributedStudents[plan].ToList();
                    AddPriorityAdmissonsToPlan(plan, freeAdmissions);
                    DistridutionToPlan(plan, freeAdmissions);
                    freeAdmissions.AddRange(tempDistributedAdmissions.Except(_distributedStudents[plan]));
                }
            }
        }

        private void DistridutionToPlan(RecruitmentPlanDTO plan, List<AdmissionDTO> freeAdmissions)
        {
            if (_distributedStudents[plan].Count > plan.Count)
            {
                int targetPassingScore = 0, generalPassingScore = 0;
                List<AdmissionDTO> targetAdmissions = new(), generalAdmissions = new();

                if (plan.Target != 0)
                {
                    targetAdmissions = _distributedStudents[plan].Where(s => s.IsTargeted).OrderByDescending(i => i.Score).Take(plan.Target).ToList();
                    targetPassingScore = targetAdmissions.Count == 0 ? 0 : targetAdmissions.Min(i => i.Score);
                    targetAdmissions = _distributedStudents[plan].Where(s => s.IsTargeted && s.Score >= targetPassingScore).ToList();
                }
                generalAdmissions = _distributedStudents[plan].Where(s => !(s.IsTargeted && s.Score >= targetPassingScore))
                    .OrderByDescending(i => i.IsWithoutEntranceExams).ThenByDescending(i => i.IsOutOfCompetition).ThenByDescending(i => i.Score)
                    .Take(plan.Count - targetAdmissions.Count).ToList();
                generalPassingScore = generalAdmissions.Count == 0 ? 0 : generalAdmissions.Last().Score;
                generalAdmissions = _distributedStudents[plan].Where(s => !(s.IsTargeted && s.Score >= targetPassingScore))
                    .Where(s => s.IsWithoutEntranceExams || s.IsOutOfCompetition || s.Score >= generalPassingScore).ToList();

                freeAdmissions.AddRange(_distributedStudents[plan].Except(targetAdmissions).Except(generalAdmissions));
                _distributedStudents[plan].RemoveAll(i => freeAdmissions.Contains(i));
            }
        }

        private void AddPriorityAdmissonsToPlan(RecruitmentPlanDTO plan, List<AdmissionDTO> freeAdmissions)
        {
            foreach (AdmissionDTO admission in freeAdmissions.Where(i => i.SpecialityPriorities.Any()).Where(i => i.SpecialityPriorities[0].RecruitmentPlan == plan))
            {
                _distributedStudents[plan].Add(admission);
                admission.SpecialityPriorities.RemoveAll(i => i.RecruitmentPlan == plan);
            }
            freeAdmissions.RemoveAll(i => _distributedStudents[plan].Contains(i));
        }

        public void NotifyEnrolledStudents()
        {
            List<AdmissionDTO> allAdmissions = GetCloneOfAdmissions();
            foreach (RecruitmentPlanDTO plan in _recruitmentPlans)
            {
                if (plan.EnrolledStudents != null)
                {
                    foreach (AdmissionDTO admission in allAdmissions.Where(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id)))
                    {
                        if (admission.Email != null)
                        {
                            string studentFullName = admission.Student.Name + " " + admission.Student.Patronymic;
                            string specialityName = plan.Speciality.DirectionName ?? plan.Speciality.FullName;
                            string message = studentFullName + ", вы зачислены на \"" + plan.Speciality.Faculty.FullName + "\""
                                + " на специальность \"" + specialityName + "\"";

                            IEmailService.SendEmailAsync(admission.Email, "Вы зачислены", message);
                        }
                    }
                    allAdmissions.RemoveAll(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id));
                }
            }
            foreach (AdmissionDTO admission in allAdmissions)
            {
                if (admission.Email != null)
                {
                    string studentFullName = admission.Student.Name + " " + admission.Student.Patronymic;
                    string message = studentFullName + ", вы не были зачислены на следующие специальности:\n";

                    foreach (SpecialityPriorityDTO specialityPriority in admission.SpecialityPriorities)
                    {
                        string specialityName = specialityPriority.RecruitmentPlan.Speciality.DirectionName ?? specialityPriority.RecruitmentPlan.Speciality.FullName;
                        message += "- " + specialityName + "\n";
                    }
                    IEmailService.SendEmailAsync(admission.Email, "Вы не были зачислены", message);
                }
            }
        }
    }
}
