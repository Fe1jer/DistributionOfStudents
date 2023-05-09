using System.Collections.Generic;
using System.Linq;
using webapi.Data.Interfaces.Services;
using webapi.Data.Models;

namespace webapi.Data.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly List<RecruitmentPlan> _recruitmentPlans;
        private readonly List<Admission> _admissions;
        private readonly Dictionary<RecruitmentPlan, List<Admission>> _distributedStudents;

        public DistributionService(List<RecruitmentPlan> recruitmentPlans, List<Admission>? admissions)
        {
            _recruitmentPlans = recruitmentPlans;
            _admissions = admissions ?? new();
            _distributedStudents = new();

            _recruitmentPlans.ForEach(plan => _distributedStudents.Add(plan, new List<Admission>()));
            if (_recruitmentPlans.All(i => i.EnrolledStudents == null))
            {
                DistridutionPlans(_recruitmentPlans, GetCloneOfAdmissions());
            }
            else
            {
                DistridutionControversialPlans();
            }
        }

        private List<Admission> GetCloneOfAdmissions()
        {
            return _admissions.Select(i => new Admission
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
            List<RecruitmentPlan> controversialPlans = GetControversialPlans(_recruitmentPlans.ToList(), out List<Admission> freeAdmissions);
            DistridutionPlans(controversialPlans, freeAdmissions);
        }

        private List<RecruitmentPlan> GetControversialPlans(List<RecruitmentPlan> plans, out List<Admission> freeAdmissions)
        {
            freeAdmissions = GetCloneOfAdmissions();
            foreach (RecruitmentPlan plan in plans.OrderByDescending(i => i.PassingScore).ToList())
            {
                if (plan.EnrolledStudents != null && plan.EnrolledStudents.Count != 0 && plan.Count <= plan.EnrolledStudents.Count)
                {
                    plan.PassingScore = freeAdmissions.Where(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id)).Min(i => i.Score);
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

        public List<RecruitmentPlan> GetPlansWithEnrolledStudents()
        {
            GetPlansWithPassingScores();
            foreach (KeyValuePair<RecruitmentPlan, List<Admission>> keyValuePair in _distributedStudents)
            {
                if (keyValuePair.Key.EnrolledStudents == null || keyValuePair.Key.EnrolledStudents.Count == 0)
                {
                    keyValuePair.Key.EnrolledStudents = keyValuePair.Value.Select(i => new EnrolledStudent { Student = i.Student }).ToList();
                }
            }
            return _recruitmentPlans;
        }

        public List<RecruitmentPlan> GetPlansWithPassingScores()
        {
            foreach (KeyValuePair<RecruitmentPlan, List<Admission>> keyValuePair in _distributedStudents)
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

        private void DistridutionPlans(List<RecruitmentPlan> plans, List<Admission> freeAdmissions)
        {
            List<Admission> tempDistributedAdmissions = new();

            for (int i = 0; i < plans.Count; i++)
            {
                GetPlansWithPassingScores();
                foreach (RecruitmentPlan plan in plans.OrderByDescending(i => i.PassingScore))
                {
                    tempDistributedAdmissions = _distributedStudents[plan].ToList();
                    AddPriorityAdmissonsToPlan(plan, freeAdmissions);
                    DistridutionToPlan(plan, freeAdmissions);
                    freeAdmissions.AddRange(tempDistributedAdmissions.Except(_distributedStudents[plan]));
                }
            }
        }

        private void DistridutionToPlan(RecruitmentPlan plan, List<Admission> freeAdmissions)
        {
            if (_distributedStudents[plan].Count > plan.Count)
            {
                int targetPassingScore = 0, generalPassingScore = 0;
                List<Admission> targetAdmissions = new(), generalAdmissions = new();

                if (plan.Target != 0)
                {
                    targetAdmissions = _distributedStudents[plan].Where(s => s.IsTargeted).OrderByDescending(i => i.Score).Take(plan.Target).ToList();
                    targetPassingScore = targetAdmissions.Count == 0 ? 0 : targetAdmissions.Min(i => i.Score);
                    targetAdmissions = _distributedStudents[plan].Where(s => s.IsTargeted && s.Score >= targetPassingScore).ToList();
                }
                generalAdmissions = _distributedStudents[plan].Where(s => !(s.IsTargeted && s.Score >= targetPassingScore))
                    .OrderByDescending(i => i.IsWithoutEntranceExams).ThenByDescending(i => i.IsOutOfCompetition).ThenByDescending(i => i.Score)
                    .Take(plan.Count - plan.Target).ToList();
                generalPassingScore = generalAdmissions.Count == 0 ? 0 : generalAdmissions.Last().Score;
                generalAdmissions = _distributedStudents[plan].Where(s => !(s.IsTargeted && s.Score >= targetPassingScore))
                    .Where(s => s.IsWithoutEntranceExams || s.IsOutOfCompetition || s.Score >= generalPassingScore).ToList();

                freeAdmissions.AddRange(_distributedStudents[plan].Except(targetAdmissions).Except(generalAdmissions));
                _distributedStudents[plan].RemoveAll(i => freeAdmissions.Contains(i));
            }
        }

        private void AddPriorityAdmissonsToPlan(RecruitmentPlan plan, List<Admission> freeAdmissions)
        {
            foreach (Admission admission in freeAdmissions.Where(i => i.SpecialityPriorities.Any()).Where(i => i.SpecialityPriorities[0].RecruitmentPlan == plan))
            {
                _distributedStudents[plan].Add(admission);
                admission.SpecialityPriorities.RemoveAll(i => i.RecruitmentPlan == plan);
            }
            freeAdmissions.RemoveAll(i => _distributedStudents[plan].Contains(i));
        }

        public void NotifyEnrolledStudents()
        {
            List<Admission> allAdmissions = GetCloneOfAdmissions();
            foreach (RecruitmentPlan plan in _recruitmentPlans)
            {
                if (plan.EnrolledStudents != null)
                {
                    foreach (Admission admission in allAdmissions.Where(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id)))
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
            foreach (Admission admission in allAdmissions)
            {
                if (admission.Email != null)
                {
                    string studentFullName = admission.Student.Name + " " + admission.Student.Patronymic;
                    string message = studentFullName + ", вы не были зачислены на следующие специальности:\n";

                    foreach (SpecialityPriority specialityPriority in admission.SpecialityPriorities)
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
