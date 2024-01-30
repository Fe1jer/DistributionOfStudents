using DAL.Postgres.DBInitialization.Interface;
using DAL.Postgres.Entities;

namespace DAL.Postgres.DBInitialization
{
    public class MTFInit : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty mtf = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Механико-технологический факультет",
                ShortName = "МТФ",
                Specialities = specialties
            };

            return mtf;
        }

        public List<Speciality> GetSpecialties()
        {

            Speciality mvm = new()
            {
                FullName = "Материаловедение в машиностроении",
                Code = "1-36 01 02",
            };
            Speciality mitomd = new()
            {
                FullName = "Машины и технология обработки материалов давлением",
                Code = "1-36 01 05",
            };
            Speciality oitsp = new()
            {
                FullName = "Оборудование и технология сварочного производства",
                Code = "1-36 01 06",
            };
            Speciality mitlp = new()
            {
                FullName = "Машины и технология литейного производства",
                Code = "1-36 02 01",
            };
            Speciality mpimm = new()
            {
                FullName = "Металлургическое производство и материалообработка",
                Code = "1-42 01 01",
                DirectionName = "Металлургическое производство и материалообработка (металлургия)",
                DirectionCode = "1-42 01 01-01"
            };
            Speciality mpimpb = new()
            {
                FullName = "Металлургическое производство и материалообработка",
                Code = "1-42 01 01",
                DirectionName = "Металлургическое производство и материалообработка (промышленная безопасность)",
                DirectionCode = "1-42 01 01-03"
            };

            return new List<Speciality>() { mvm, mitomd, oitsp, mitlp, mpimm, mpimpb };
        }

        public List<GroupOfSpecialities> GetGroupsOfSpecialties(List<Speciality> specialities, FormOfEducation form)
        {
            return new();
        }

        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form)
        {
            return new();
        }
    }
}
