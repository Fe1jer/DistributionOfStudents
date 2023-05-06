using System.Collections.Generic;
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
            return _admissions.Select(i => new Admission { SpecialityPriorities = i.SpecialityPriorities.OrderBy(p => p.Priority).ToList(), StudentScores = i.StudentScores, Student = i.Student, Email = i.Email }).ToList();
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
                    keyValuePair.Key.PassingScore = keyValuePair.Key.Count > keyValuePair.Value.Count ? 0 : keyValuePair.Value.Min(a => a.Score);
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
                int passingScore = _distributedStudents[plan].OrderByDescending(i => i.Score).ToList()[plan.Count - 1].Score;
                freeAdmissions.AddRange(_distributedStudents[plan].Where(i => i.Score < passingScore));
                _distributedStudents[plan].RemoveAll(i => i.Score < passingScore);
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
