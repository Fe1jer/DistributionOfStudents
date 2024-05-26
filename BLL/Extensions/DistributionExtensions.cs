using AutoMapper;
using BLL.DTO;
using BLL.DTO.RecruitmentPlans;
using BLL.DTO.Students;
using BLL.Mappers;
using BLL.Services.Interfaces;

namespace BLL.Extensions
{
    public static class DistributionExtensions
    {
        private static IMapper Mapper { get; }

        static DistributionExtensions()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DTOToDTOMappingProfile));
                cfg.AllowNullCollections = true;
            });
            Mapper = config.CreateMapper();
        }

        public static Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> Distribution(this List<RecruitmentPlanDTO> recruitmentPlans, List<AdmissionDTO> admissions)
        {
            Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents = new();

            recruitmentPlans.ForEach(plan => distributedStudents.Add(plan, new List<AdmissionDTO>()));
            List<AdmissionDTO> freeAdmissions = Mapper.Map<List<AdmissionDTO>>(admissions);
            if (recruitmentPlans.All(i => i.EnrolledStudents is null))
            {
                DistributionPlans(distributedStudents, recruitmentPlans, freeAdmissions);
            }
            else
            {
                DistributionControversialPlans(distributedStudents, recruitmentPlans, freeAdmissions);
            }

            return distributedStudents;
        }

        private static void DistributionControversialPlans(Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents, List<RecruitmentPlanDTO> plans, List<AdmissionDTO> freeAdmissions)
        {
            List<RecruitmentPlanDTO> controversialPlans = GetControversialPlans(plans, freeAdmissions);
            DistributionPlans(distributedStudents, controversialPlans, freeAdmissions);
        }

        private static List<RecruitmentPlanDTO> GetControversialPlans(List<RecruitmentPlanDTO> plans, List<AdmissionDTO> freeAdmissions)
        {
            foreach (RecruitmentPlanDTO plan in plans.OrderByDescending(i => i.PassingScore).ToList())
            {
                if (plan.EnrolledStudents is not null && plan.EnrolledStudents.Any() && plan.Count <= plan.EnrolledStudents.Count)
                {
                    freeAdmissions.RemoveAll(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id));
                    freeAdmissions.ForEach(admission => admission.SpecialityPriorities.RemoveAll(i => i.RecruitmentPlanId == plan.Id));
                    plans.Remove(plan);
                }
            }

            return plans;
        }

        public static float Competition(this Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents, List<AdmissionDTO> admissions)
        {
            float allPlans = distributedStudents.Keys.Sum(i => i.Count);
            if (allPlans == 0)
            {
                return 0;
            }
            return admissions.Count / allPlans;
        }

        public static bool AreControversialStudents(this Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents)
        {
            return distributedStudents.Keys.Any(i => (i.EnrolledStudents ?? new()).Count > i.Count);
        }

        public static Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> GetPlansWithEnrolledStudents(this Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents)
        {
            distributedStudents.GetPlansWithPassingScores();
            foreach (KeyValuePair<RecruitmentPlanDTO, List<AdmissionDTO>> keyValuePair in distributedStudents)
            {
                if (keyValuePair.Key.EnrolledStudents is null || !keyValuePair.Key.EnrolledStudents.Any())
                {
                    keyValuePair.Key.EnrolledStudents = keyValuePair.Value.Select(i => new EnrolledStudentDTO { Student = i.Student }).ToList();
                }
            }
            return distributedStudents;
        }

        public static Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> GetPlansWithPassingScores(this Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents)
        {
            foreach (KeyValuePair<RecruitmentPlanDTO, List<AdmissionDTO>> keyValuePair in distributedStudents)
            {
                if (keyValuePair.Key.EnrolledStudents is null || keyValuePair.Key.Count == 0)
                {
                    keyValuePair.Key.TargetPassingScore = keyValuePair.Key.Target == 0 || keyValuePair.Key.Target > keyValuePair.Value.Count(s => s.IsTargeted) ? 0 :
                        keyValuePair.Value.Where(s => s.IsTargeted).OrderByDescending(i => i.Score).Take(keyValuePair.Key.Target).Min(a => a.Score);

                    keyValuePair.Key.PassingScore = keyValuePair.Key.Count > keyValuePair.Value.Count ? 0 :
                        keyValuePair.Value.Where(s => !(s.IsTargeted && s.Score >= keyValuePair.Key.TargetPassingScore)
                                                      && s is { IsWithoutEntranceExams: false, IsOutOfCompetition: false }).Min(a => a.Score);
                }
            }

            return distributedStudents;
        }

        private static void DistributionPlans(Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents, List<RecruitmentPlanDTO> plans, List<AdmissionDTO> freeAdmissions)
        {
            for (int i = 0; i < plans.Count; i++)
            {
                distributedStudents.GetPlansWithPassingScores();
                foreach (RecruitmentPlanDTO plan in plans.OrderByDescending(i => i.PassingScore))
                {
                    List<AdmissionDTO> tempDistributedAdmissions = distributedStudents[plan].ToList();
                    AddPriorityAdmissionsToPlan(distributedStudents, plan, freeAdmissions);
                    DistributionToPlan(distributedStudents, plan, freeAdmissions);
                    freeAdmissions.AddRange(tempDistributedAdmissions.Except(distributedStudents[plan]));
                }
            }
        }

        private static void DistributionToPlan(Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents, RecruitmentPlanDTO plan, List<AdmissionDTO> freeAdmissions)
        {
            if (distributedStudents[plan].Count > plan.Count)
            {
                var targetPassingScore = 0;
                List<AdmissionDTO> targetAdmissions = new();

                if (plan.Target != 0)
                {
                    targetAdmissions = distributedStudents[plan].Where(s => s.IsTargeted).OrderByDescending(i => i.Score).Take(plan.Target).ToList();
                    targetPassingScore = targetAdmissions.Any() ? targetAdmissions.Min(i => i.Score) : 0;
                    targetAdmissions = distributedStudents[plan].Where(s => s.IsTargeted && s.Score >= targetPassingScore).ToList();
                }
                List<AdmissionDTO> generalAdmissions = distributedStudents[plan].Where(s => !(s.IsTargeted && s.Score >= targetPassingScore))
                    .OrderByDescending(i => i.IsWithoutEntranceExams).ThenByDescending(i => i.IsOutOfCompetition).ThenByDescending(i => i.Score)
                    .Take(plan.Count - targetAdmissions.Count).ToList();
                var generalPassingScore = generalAdmissions.Any() ? generalAdmissions.Last().Score : 0;
                generalAdmissions = distributedStudents[plan].Where(s => !(s.IsTargeted && s.Score >= targetPassingScore))
                    .Where(s => s.IsWithoutEntranceExams || s.IsOutOfCompetition || s.Score >= generalPassingScore).ToList();

                freeAdmissions.AddRange(distributedStudents[plan].Except(targetAdmissions).Except(generalAdmissions));
                distributedStudents[plan].RemoveAll(freeAdmissions.Contains);
            }
        }

        private static void AddPriorityAdmissionsToPlan(Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents, RecruitmentPlanDTO plan, List<AdmissionDTO> freeAdmissions)
        {
            foreach (AdmissionDTO admission in freeAdmissions.Where(i => i.SpecialityPriorities.Any()).Where(i => i.SpecialityPriorities[0].RecruitmentPlanId == plan.Id))
            {
                distributedStudents[plan].Add(admission);
                admission.SpecialityPriorities.RemoveAll(i => i.RecruitmentPlanId == plan.Id);
            }
            freeAdmissions.RemoveAll(i => distributedStudents[plan].Contains(i));
        }

        public static void NotifyEnrolledStudents(this Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>> distributedStudents, List<AdmissionDTO> allAdmissions)
        {
            foreach (RecruitmentPlanDTO plan in distributedStudents.Keys)
            {
                if (plan.EnrolledStudents != null)
                {
                    foreach (AdmissionDTO admission in allAdmissions.Where(i => plan.EnrolledStudents.Select(s => s.Student.Id).Contains(i.Student.Id)))
                    {
                        if (admission.Email != null)
                        {
                            string studentFullName = admission.Student.Name + " " + admission.Student.Patronymic;
                            string specialityName = plan.Speciality.DirectionName ?? plan.Speciality.FullName;
                            string message = studentFullName + ", вы зачислены на \"" + plan.Speciality.Faculty!.FullName + "\""
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
