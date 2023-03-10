using DistributionOfStudents.Data.Models;
using System.Drawing;
using System.Numerics;

namespace DistributionOfStudents.Data.Services
{
    public class DistributionService
    {
        private readonly List<RecruitmentPlan> _recruitmentPlans;
        private readonly List<Admission> _admissions;
        private readonly Dictionary<RecruitmentPlan, List<Admission>> _distributedStudents;

        public DistributionService(List<RecruitmentPlan> recruitmentPlans, List<Admission> admissions)
        {
            _recruitmentPlans = recruitmentPlans;
            _admissions = admissions;
            _distributedStudents = new();
            foreach (RecruitmentPlan plan in _recruitmentPlans)
            {
                _distributedStudents.Add(plan, new List<Admission>());
            }
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
            return _admissions.Select(i => new Admission { SpecialityPriorities = i.SpecialityPriorities.OrderBy(p => p.Priority).ToList(), StudentScores = i.StudentScores, Student = i.Student }).ToList();
        }

        private void DistridutionControversialPlans()
        {
            List<RecruitmentPlan> controversialPlans = GetControversialPlans(_recruitmentPlans, out List<Admission> freeAdmissions);
            DistridutionPlans(controversialPlans, freeAdmissions);
        }

        private List<RecruitmentPlan> GetControversialPlans(List<RecruitmentPlan> plans, out List<Admission> freeAdmissions)
        {
            freeAdmissions = GetCloneOfAdmissions();
            foreach (RecruitmentPlan plan in plans.OrderByDescending(i => i.PassingScore).ToList())
            {
                if (plan.EnrolledStudents.Count != 0 && plan.Count <= plan.EnrolledStudents.Count)
                {
                    freeAdmissions.RemoveAll(i => plan.EnrolledStudents.Select(s => s.Student).Contains(i.Student));
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
            return _recruitmentPlans.Any(i => i.EnrolledStudents.Count > i.Count);
        }

        public List<RecruitmentPlan> GetPlansWithEnrolledStudents()
        {
            GetPlansWithPassingScores();
            foreach (KeyValuePair<RecruitmentPlan, List<Admission>> keyValuePair in _distributedStudents)
            {
                keyValuePair.Key.EnrolledStudents = keyValuePair.Value.Select(i => new EnrolledStudent { Student = i.Student }).ToList();
            }
            return _recruitmentPlans;
        }

        public List<RecruitmentPlan> GetPlansWithPassingScores()
        {
            foreach (KeyValuePair<RecruitmentPlan, List<Admission>> keyValuePair in _distributedStudents)
            {
                if (keyValuePair.Key.Count > keyValuePair.Value.Count)
                {
                    keyValuePair.Key.PassingScore = 0;
                }
                else
                {
                    int? passingScore = keyValuePair.Value.Min(a => a.Score);
                    keyValuePair.Key.PassingScore = passingScore ?? 0;
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
            List<Admission> tempPlanAdmissions = new();

            foreach (Admission admission in freeAdmissions.Where(i => i.SpecialityPriorities.Any()).Where(i => i.SpecialityPriorities[0].RecruitmentPlan == plan))
            {
                tempPlanAdmissions.Add(admission);
                admission.SpecialityPriorities.RemoveAll(i => i.RecruitmentPlan == plan);
            }
            freeAdmissions.RemoveAll(i => tempPlanAdmissions.Contains(i));
            _distributedStudents[plan].AddRange(tempPlanAdmissions);
        }
    }
}
