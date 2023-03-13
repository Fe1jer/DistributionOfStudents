using DistributionOfStudents.Data.DBInitialization.Interface;
using DistributionOfStudents.Data.Models;

namespace DistributionOfStudents.Data.DBInitialization
{
    public class EFInit : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty fmmp = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Энергетический факультет",
                ShortName = "ЭФ",
                Specialities = specialties
            };

            return fmmp;
        }

        public List<Speciality> GetSpecialties()
        {
            Speciality eiope = new()
            {
                FullName = "Экономика и организация производства",
                Code = "1-27 01 01",
                DirectionName = "Экономика и организация производства (энергетика)",
                DirectionCode = "1-27 01 01-10"
            };
            Speciality es = new()
            {
                FullName = "Электрические станции",
                Code = "1-43 01 01",
            };
            Speciality esis = new()
            {
                FullName = "Электроэнергетические системы и сети",
                Code = "1-43 01 02",
            };
            Speciality e = new()
            {
                FullName = "Электроснабжение",
                Code = "1-43 01 03",
                DirectionName = "Электроснабжение (по отрослям)",
            };
            Speciality tes = new()
            {
                FullName = "Тепловые электрические станции",
                Code = "1-43 01 04",
            };
            Speciality pt = new()
            {
                FullName = "Промышленная теплоэнергетика",
                Code = "1-43 01 05",
            };
            Speciality pieaes = new()
            {
                FullName = "Проектирование и эксплуатация атомных электрических станций",
                Code = "1-43 01 08",
            };
            Speciality rzia = new()
            {
                FullName = "Релейная защита и автоматика",
                Code = "1-43 01 09",
            };
            Speciality aiutp = new()
            {
                FullName = "Автоматизация и управление теплоэнергетическими процессами",
                Code = "1-53 01 04",
            };

            return new List<Speciality>() { eiope, es, esis, e, tes, pt, pieaes, rzia, aiutp};
        }

        public List<GroupOfSpecialties> GetGroupsOfSpecialties(List<Speciality> specialities)
        {
            return new();
        }

        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities)
        {
            return new();
        }
    }
}
