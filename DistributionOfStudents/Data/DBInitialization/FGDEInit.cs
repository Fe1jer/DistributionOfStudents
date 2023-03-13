using DistributionOfStudents.Data.DBInitialization.Interface;
using DistributionOfStudents.Data.Models;
using System.ComponentModel;
using System.Drawing;

namespace DistributionOfStudents.Data.DBInitialization
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
                Code = "1-53 01 01",
            };
            Speciality rmpi = new()
            {
                FullName = "Разработка месторождений полезных ископаемых",
                DirectionName = "Разработка месторождений полезных ископаемых (по направлениям)",
                Code = "1-53 01 01",
            };
            Speciality emiavp = new()
            {
                FullName = "Экологический менеджмент и аудит в промышленности",
                Code = "1-53 01 01",
            };

            return new List<Speciality>() { gmio, rmpi, emiavp };
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
