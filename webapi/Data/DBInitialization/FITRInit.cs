﻿using webapi.Data.DBInitialization.Interface;
using webapi.Data.Models;

namespace webapi.Data.DBInitialization
{
    public class FITRInit : IFacultyInit
    {
        public Faculty GetFaculty()
        {
            List<Speciality> specialties = GetSpecialties();

            Faculty fitr = new()
            {
                Img = "\\img\\Faculties\\Default.jpg",
                FullName = "Факультет информационных технологий и робототехники",
                ShortName = "ФИТР",
                Specialities = specialties
            };

            return fitr;
        }

        public List<Speciality> GetSpecialties()
        {
            Speciality poit = new()
            {
                FullName = "Программное обеспечение информационных технологий",
                ShortName = "ПОИТ",
                Code = "1-40 01 01",
                ShortCode = "01",
                SpecializationName = "Управление качеством и тестирование программного обеспечения",
                SpecializationCode = "1-40 01 01 05"
            };
            Speciality isitOPI = new()
            {
                FullName = "Информационные системы и технологии",
                ShortName = "ИСИТ",
                Code = "1-40 05 01",
                ShortCode = "02",
                SpecializationName = "Математическое обеспечение и системное программирование",
                SpecializationCode = "1-40 05 01 04 01",
                DirectionName = "Информационные системы и технологии (в обработке и представлении информации)",
                DirectionCode = "1-40 05 01-04"
            };
            Speciality isitPP = new()
            {
                FullName = "Информационные системы и технологии",
                ShortName = "ИСИТ",
                Code = "1-40 05 01",
                ShortCode = "03",
                DirectionName = "Информационные системы и технологии (в проектировании и производстве)",
                DirectionCode = "1-40 05 01-01"
            };
            Speciality atpipvpir = new()
            {
                FullName = "Автоматизация технологических процессов и производств",
                DirectionName = "Автоматизация технологических процессов и производств (по направлениям)",
                Code = "1-53 01 01",
            };
            Speciality ae = new()
            {
                FullName = "Автоматизированные электроприводы",
                Code = "1-53 01 05",
            };
            Speciality prirk = new()
            {
                FullName = "Промышленные роботы и робототехнические комплексы",
                Code = "1-53 01 06",
                SpecializationName = "Промышленные роботы и робототехнические комплексы (в приборостроении)",
                SpecializationCode = "1-53 01 06-02",
            };

            return new List<Speciality>() { poit, isitOPI, isitPP, atpipvpir, ae, prirk };
        }

        public List<GroupOfSpecialties> GetGroupsOfSpecialties(List<Speciality> specialities, FormOfEducation form)
        {
            GroupOfSpecialties group = new()
            {
                Name = "Дневная форма получения образования за счет средств республиканского бюджета",
                StartDate = DateTime.Today,
                EnrollmentDate = DateTime.Today.AddDays(10),
                IsCompleted = false,
                Specialities = specialities,
                FormOfEducation = form
            };

            return new() { group };
        }

        public List<RecruitmentPlan> GetRecruitmentPlans(List<Speciality> specialities, FormOfEducation form)
        {
            List<RecruitmentPlan> plans = new();

            foreach (Speciality specialty in specialities)
            {
                RecruitmentPlan plan = new()
                {
                    Count = 20,
                    PassingScore = 0,
                    Speciality = specialty,
                    FormOfEducation = form
                };

                plans.Add(plan);
            };

            return plans;
        }
    }
}
