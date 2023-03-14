using DistributionOfStudents.Data.DBInitialization.Interface;
using DistributionOfStudents.Data.Models;

namespace DistributionOfStudents.Data.DBInitialization
{
    public class FMMP : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty fmmp = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Факультет маркетинга менеджмента и предпринимательства",
                ShortName = "ФММП",
                Specialities = specialties
            };

            return fmmp;
        }

        public List<Speciality> GetSpecialties()
        {
            Speciality eiunp = new()
            {
                FullName = "Экономика и управление на предприятии",
                Code = "1-25 01 07",
            };
            Speciality ba = new()
            {
                FullName = "Бизнес-администрирование",
                Code = "1-26 02 01",
            };
            Speciality m = new()
            {
                FullName = "Маркетинг",
                Code = "1-26 02 03",
            };
            Speciality uippp = new()
            {
                FullName = "Управление инновационными проектами промышленных предприятий",
                Code = "1-27 03 01",
            };
            Speciality udpnpp = new()
            {
                FullName = "Управление дизайн-проектами на промышленном предприятии",
                Code = "1-27 03 02",
            };
            Speciality toit = new()
            {
                FullName = "Торговое оборудование и технологии",
                Code = "1-36 20 03",
            };

            return new List<Speciality>() { eiunp, ba, m, uippp, udpnpp, toit };
        }

        public List<GroupOfSpecialties> GetGroupsOfSpecialties(List<Speciality> specialities, FormOfEducation form)
        {
            return new();
        }

        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form)
        {
            return new();
        }
    }
}
