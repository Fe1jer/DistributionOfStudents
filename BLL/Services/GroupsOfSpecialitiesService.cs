using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.Specialities;
using BLL.DTO.Subjects;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using DAL.Postgres.Specifications;

namespace BLL.Services
{
    public class GroupsOfSpecialitiesService : BaseService, IGroupsOfSpecialitiesService
    {
        public GroupsOfSpecialitiesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<GroupOfSpecialitiesDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.GroupsOfSpecialities.GetAllAsync();
            return Mapper.Map<List<GroupOfSpecialitiesDTO>>(result);
        }

        public async Task<GroupOfSpecialitiesDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(id, new GroupsOfSpecialitiesSpecification());
            return Mapper.Map<GroupOfSpecialitiesDTO>(entity);
        }

        public async Task<List<GroupOfSpecialitiesDTO>> GetByFacultyAsync(string facultyUrl, int year)
        {
            var result = await _unitOfWork.GroupsOfSpecialities.GetByFacultyAsync(facultyUrl, year);
            return Mapper.Map<List<GroupOfSpecialitiesDTO>>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(id);
            await _unitOfWork.GroupsOfSpecialities.DeleteAsync(toDelete);
            _unitOfWork.Commit();
        }

        public async Task<GroupOfSpecialitiesDTO> SaveAsync(UpdateGroupOfSpecialitiesDTO model)
        {
            GroupOfSpecialities? entity;
            if (model.IsNew)
            {
                entity = Mapper.Map<GroupOfSpecialities>(model);
            }
            else
            {
                entity = await _unitOfWork.GroupsOfSpecialities.GetByIdAsync(model.Id);
                Mapper.Map(model, entity);
            }

            model.FormOfEducation.Year = model.StartDate.Year;
            FormOfEducation form = Mapper.Map<FormOfEducation>(model.FormOfEducation);
            form = await _unitOfWork.FormsOfEducation.GetByFormAsync(form) ?? form;
            entity.FormOfEducation = form;
            entity.Specialities = await GetSelectedSpecialitiesAsync(model.Specialities);
            entity.Subjects = await GetSelectedSubjectsAsync(model.Subjects);

            await _unitOfWork.GroupsOfSpecialities.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<GroupOfSpecialitiesDTO>(entity);
        }

        private async Task<List<Speciality>> GetSelectedSpecialitiesAsync(IEnumerable<SelectedSpecialityDTO>? selectedSpecialities)
        {
            List<Speciality> specialities = new();

            if (selectedSpecialities != null)
            {
                foreach (SelectedSpecialityDTO speciality in selectedSpecialities.Where(p => p.IsSelected))
                {
                    Speciality? entity = await _unitOfWork.Specialities.GetByIdAsync(speciality.Id);
                    if (entity != null)
                    {
                        specialities.Add(entity);
                    }
                }
            }

            return specialities;
        }

        private async Task<List<Subject>> GetSelectedSubjectsAsync(IEnumerable<SelectedSubjectDTO>? selectedSubjects)
        {
            List<Subject> subjects = new();

            if (selectedSubjects != null)
            {
                foreach (SelectedSubjectDTO subject in selectedSubjects.Where(p => p.IsSelected))
                {
                    Subject? entity = await _unitOfWork.Subjects.GetByIdAsync(subject.Id);
                    if (entity != null)
                    {
                        subjects.Add(entity);
                    }
                }
            }

            return subjects;
        }
    }
}
