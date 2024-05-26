using DAL.Postgres.DBInitialization.Interface;
using DAL.Postgres.Entities;

namespace DAL.Postgres.DBInitialization
{
    public class FGDEInit : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty fgde = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Факультет горного дела и инженерной экологии",
                ShortName = "ФГДЭ",
                Specialities = specialties
            };

            return fgde;
        }

        public List<Speciality> GetSpecialties()
        {
            Speciality gmio = new()
            {
                FullName = "Горные машины и оборудование",
                DirectionName = "Горные машины и оборудование (по направлениям)",
                Code = "1-36 10 01",
            };
            Speciality rmpi = new()
            {
                FullName = "Разработка месторождений полезных ископаемых",
                DirectionName = "Разработка месторождений полезных ископаемых (по направлениям)",
                Code = "1-51 02 01",
            };
            Speciality emiavp = new()
            {
                FullName = "Экологический менеджмент и аудит в промышленности",
                Code = "1-57 01 02",
            };

            return new List<Speciality>() { gmio, rmpi, emiavp };
        }

        public List<GroupOfSpecialities> GetGroupsOfSpecialties(List<Speciality> specialities, List<Subject> subjects, FormOfEducation form)
        {
            return new();
        }

        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form)
        {
            return new();
        }
    }
}
