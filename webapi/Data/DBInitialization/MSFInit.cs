using webapi.Data.DBInitialization.Interface;
using webapi.Data.Models;

namespace webapi.Data.DBInitialization
{
    public class MSFInit : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty msf = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Машиностроительный факультет",
                ShortName = "МСФ",
                Specialities = specialties
            };

            return msf;
        }

        public List<Speciality> GetSpecialties()
        {
            Speciality eiopm = new()
            {
                FullName = "Экономика и организация производства",
                Code = "1-27 01 01",
                DirectionName = "Экономика и организация производства (машиностроение)",
                DirectionCode = "1-27 01 01-01"
            };
            Speciality eiopp = new()
            {
                FullName = "Экономика и организация производства",
                Code = "1-27 01 01",
                DirectionName = "Экономика и организация производства (приборостроение)",
                DirectionCode = "1-27 01 01-08"
            };
            Speciality tm = new()
            {
                FullName = "Технология машиностроения",
                Code = "1-36 01 01",
            };
            Speciality tpomp = new()
            {
                FullName = "Технологическое оборудование машиностроительного производства",
                Code = "1-36 01 03",
            };
            Speciality iss = new()
            {
                FullName = "Интегральные сенсорные системы",
                Code = "1-55 01 02",
            };
            Speciality km = new()
            {
                FullName = "Компьютерная мехатроника",
                Code = "1-55 01 03",
            };

            return new List<Speciality>() { eiopm, eiopp, tm , tpomp, iss, km };
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
