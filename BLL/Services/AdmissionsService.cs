using BLL.DTO;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.Specialities;
using BLL.DTO.Subjects;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Specifications;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Shared.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AdmissionsService : BaseService, IAdmissionsService
    {
        public AdmissionsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<AdmissionDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.Admissions.GetAllAsync();
            return Mapper.Map<List<AdmissionDTO>>(result);
        }

        public async Task<List<AdmissionDTO>> GetByGroupAsync(Guid groupId)
        {
            var result = await _unitOfWork.Admissions.GetAllAsync(new AdmissionsSpecification(i => i.GroupOfSpecialties.Id == groupId).SortByStudent().IncludeGroupWithSpecialities());
            return Mapper.Map<List<AdmissionDTO>>(result);
        }

        public async Task<(List<AdmissionDTO> rows, int count)> GetByLastYearAsync(DefaultFilter filter)
        {
            var (rows, count) = await _unitOfWork.Admissions.GetByFilterAsync(filter, new AdmissionsSpecification().IncludeSpecialtyPriorities().SortByStudent());
            return (Mapper.Map<List<AdmissionDTO>>(rows), count);
        }

        public async Task<(List<AdmissionDTO> rows, int count)> GetByGroupAsync(Guid groupId, DefaultFilter filter)
        {
            var (rows, count) = await _unitOfWork.Admissions.GetByFilterAsync(filter, new AdmissionsSpecification(i => i.GroupOfSpecialties.Id == groupId).IncludeSpecialtyPriorities().SortByStudent());
            return (Mapper.Map<List<AdmissionDTO>>(rows), count);
        }

        public async Task<AdmissionDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.Admissions.GetByIdAsync(id, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScores());

            if (entity != null)
            {
                entity.SpecialityPriorities = entity.SpecialityPriorities.OrderBy(i => i.Priority).ToList();
            }

            return Mapper.Map<AdmissionDTO>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await _unitOfWork.Admissions.GetByIdAsync(id, new AdmissionsSpecification());
            await _unitOfWork.Admissions.DeleteAsync(toDelete);
            _unitOfWork.Commit();
        }

        public async Task<AdmissionDTO> SaveAsync(AdmissionDTO model)
        {
            Admission? entity;
            List<SpecialityPriority> specialityPriorities = new();

            if (model.IsNew)
            {
                entity = Mapper.Map<Admission>(model);

                foreach (StudentScoreDTO studentScore in model.StudentScores)
                {
                    entity.StudentScores.Add(new() { Subject = await _unitOfWork.Subjects.GetByIdAsync(studentScore.Subject.Id) ?? new() });
                }
                foreach (SpecialityPriorityDTO specialityPriority in model.SpecialityPriorities.Where(p => p.Priority > 0))
                {
                    RecruitmentPlan? plan = await _unitOfWork.RecruitmentPlans.GetByIdAsync(specialityPriority.RecruitmentPlan.Id);
                    specialityPriorities.Add(new() { Priority = specialityPriority.Priority, RecruitmentPlan = plan ?? new() });
                }
            }
            else
            {
                entity = await _unitOfWork.Admissions.GetByIdAsync(model.Id, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScores());
                Mapper.Map(model, entity);

                //entity.StudentScores.ForEach(s => s.Score = model.StudentScores.First(i => i.Id == s.Id).Score);

                foreach (SpecialityPriorityDTO specialityPriority in model.SpecialityPriorities.Where(p => p.Priority > 0))
                {
                    SpecialityPriority priority = entity.SpecialityPriorities.FirstOrDefault(i => i.RecruitmentPlan.Id == specialityPriority.RecruitmentPlan.Id) ??
                        new() { RecruitmentPlan = await _unitOfWork.RecruitmentPlans.GetByIdAsync(specialityPriority.RecruitmentPlan.Id) ?? new() };
                    priority.Priority = specialityPriority.Priority;
                    specialityPriorities.Add(priority);
                }
            }
            entity.SpecialityPriorities = specialityPriorities;

            await _unitOfWork.Admissions.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<AdmissionDTO>(entity);
        }
    }
}
