using BLL.DTO.Subjects;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.Specifications;

namespace BLL.Services
{
    public class SubjectsService : BaseService, ISubjectsService
    {
        public SubjectsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<SubjectDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.Subjects.GetAllAsync();
            return Mapper.Map<List<SubjectDTO>>(result);
        }
        public async Task<List<SubjectDTO>> GetByGroupAsync(Guid groupId)
        {
            var result = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSubjects());
            return Mapper.Map<List<SubjectDTO>>(result?.Subjects);
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await _unitOfWork.Subjects.GetByIdAsync(id);
            await _unitOfWork.Subjects.DeleteAsync(toDelete);
            _unitOfWork.Commit();
        }

        public async Task<SubjectDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.Subjects.GetByIdAsync(id);
            return Mapper.Map<SubjectDTO>(entity);
        }

        public async Task<SubjectDTO> SaveAsync(SubjectDTO model)
        {
            Subject? entity;
            if (model.IsNew)
            {
                entity = Mapper.Map<Subject>(model);
            }
            else
            {
                entity = await _unitOfWork.Subjects.GetByIdAsync(model.Id);
                Mapper.Map(model, entity);
            }

            await _unitOfWork.Subjects.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<SubjectDTO>(entity);
        }
    }
}
