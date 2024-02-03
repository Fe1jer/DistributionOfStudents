using BLL.DTO.Specialities;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Specifications;

namespace BLL.Services
{
    public class SpecialitiesService : BaseService, ISpecialitiesService
    {
        public SpecialitiesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<SpecialityDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.Specialities.GetAllAsync();
            return Mapper.Map<List<SpecialityDTO>>(result);
        }

        public async Task<SpecialityDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.Specialities.GetByIdAsync(id);
            return Mapper.Map<SpecialityDTO>(entity);
        }

        public async Task<SpecialityDTO> GetAsync(string url)
        {
            var entity = await _unitOfWork.Specialities.GetByUrlAsync(url);
            return Mapper.Map<SpecialityDTO>(entity);
        }

        public async Task<List<SpecialityDTO>> GetByGroupAsync(Guid groupId)
        {
            var result = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties());
            return Mapper.Map<List<SpecialityDTO>>(result?.Specialities?.OrderBy(sp => sp.DirectionCode ?? sp.Code));
        }

        public async Task<List<SpecialityDTO>> GetByFacultyAsync(string facultyUrl, bool isDisable)
        {
            var result = await _unitOfWork.Specialities.GetByFacultyAsync(facultyUrl, isDisable);
            return Mapper.Map<List<SpecialityDTO>>(result);
        }

        public async Task<bool> CheckUrlIsUniqueAsync(string url, Guid id)
        {
            return (await _unitOfWork.Specialities.GetCountByUrlAsync(url, id)) == 0;
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await _unitOfWork.Specialities.GetByIdAsync(id);
            await _unitOfWork.Specialities.DeleteAsync(toDelete);
            _unitOfWork.Commit();
        }

        public async Task<SpecialityDTO> SaveAsync(SpecialityDTO model)
        {
            Speciality? entity;
            if (model.IsNew)
            {
                entity = Mapper.Map<Speciality>(model);
            }
            else
            {
                entity = await _unitOfWork.Specialities.GetByIdAsync(model.Id);
                Mapper.Map(model, entity);
            }

            await _unitOfWork.Specialities.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<SpecialityDTO>(entity);
        }
    }
}
