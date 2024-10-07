using BLL.DTO;
using BLL.DTO.Students;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Specifications;
using Shared.Filters.Base;

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

        public async Task<(List<StudentItemDTO> rows, int count)> GetByLastYearAsync(DefaultFilter filter)
        {
            var (rows, count) = await _unitOfWork.Admissions.GetByFilterAsync(filter, new AdmissionsSpecification().IncludeGroupWithSpecialities().SortByStudent());
            return (Mapper.Map<List<StudentItemDTO>>(rows), count);
        }

        public async Task<(List<AdmissionDTO> rows, int count)> GetByGroupAsync(Guid groupId, DefaultFilter filter)
        {
            var (rows, count) = await _unitOfWork.Admissions.GetByFilterAsync(filter, new AdmissionsSpecification(i => i.GroupOfSpecialties.Id == groupId).IncludeSpecialtyPriorities().IncludeStudentScoresWithSubject().SortByStudent());
            return (Mapper.Map<List<AdmissionDTO>>(rows), count);
        }

        public async Task<AdmissionDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.Admissions.GetByIdAsync(id, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScoresWithSubject());

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
            }
            else
            {
                entity = await _unitOfWork.Admissions.GetByIdAsync(model.Id, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScores());
                Mapper.Map(model, entity);
            }

            await _unitOfWork.Admissions.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<AdmissionDTO>(entity);
        }
    }
}
