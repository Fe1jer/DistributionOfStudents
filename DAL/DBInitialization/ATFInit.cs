using DAL.DBInitialization.Interface;
using DAL.Entities;

namespace DAL.DBInitialization
{
    public class ATFInit : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty atf = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Автотракторный факультет",
                ShortName = "АТФ",
                Specialities = specialties
            };

            return atf;
        }

        public List<Speciality> GetSpecialties()
        {
            Speciality tlat = new()
            {
                FullName = "Транспортная логистика",
                Code = "1-27 02 01",
                DirectionName = "Транспортная логистика (автомобильный транспорт)",
                DirectionCode = "1-27 02 01-01"
            };
            Speciality gmitm = new()
            {
                FullName = "Гидропневмосистемы мобильных и технологических машин",
                Code = "1-36 01 07",
            };
            Speciality dvs = new()
            {
                FullName = "Двигатели внутреннего сгорания",
                Code = "1-37 01 01",
            };
            Speciality am = new()
            {
                FullName = "Автомобилестроение",
                Code = "1-37 01 02",
                DirectionName = "Автомобилестроение (механика)",
                DirectionCode = "1-37 01 02-01"
            };
            Speciality ae = new()
            {
                FullName = "Автомобилестроение",
                Code = "1-37 01 02",
                DirectionName = "Автомобилестроение (электроника)",
                DirectionCode = "1-37 01 02-02"
            };
            Speciality ts = new()
            {
                FullName = "Тракторостроение",
                Code = "1-37 01 03",
            };
            Speciality eiat = new()
            {
                FullName = "Электрический и автономный транспорт",
                Code = "1-37 01 05",
            };
            Speciality tea = new()
            {
                FullName = "Техническая эксплуатация автомобилей",
                Code = "1-37 01 06",
                DirectionName = "Техническая эксплуатация автомобилей (автотранспорт общего и личного пользованиям)",
                DirectionCode = "1-37 01 06-01"
            };
            Speciality a = new()
            {
                FullName = "Автосервис",
                Code = "1-37 01 07",
            };
            Speciality opiunaigt = new()
            {
                FullName = "Организация перевозок и управление на автомобильном и городском транспорте",
                Code = "1-44 01 01",
            };
            Speciality odd = new()
            {
                FullName = "Организация дорожного движения",
                Code = "1-44 01 02",
            };
            Speciality eitsnaigt = new()
            {
                FullName = "Эксплуатация интеллектуальных транспортных систем на автомобильном и городском транспорте",
                Code = "1-44 01 06",
            };
            Speciality pd = new()
            {
                FullName = "Промышленный дизайн",
                Code = "1-61 01 01",
                DirectionName = "Промышленный дизайн (транспортных средств)",
                DirectionCode = "1-61 01 01-01"
            };

            return new List<Speciality>() { tlat, gmitm, dvs, am, ae, ts, eiat, tea, a, opiunaigt, odd, eitsnaigt, pd };
        }

        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form)
        {
            return new();
        }

        public List<GroupOfSpecialities> GetGroupsOfSpecialties(List<Speciality> specialities, List<Subject> subjects, FormOfEducation form)
        {
            return new();
        }
    }
}
