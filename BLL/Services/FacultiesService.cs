using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;

namespace BLL.Services
{
    public class FacultiesService : BaseService, IFacultiesService
    {
        public FacultiesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<FacultyDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.Faculties.GetAllAsync();
            return Mapper.Map<List<FacultyDTO>>(result);
        }

        public async Task<bool> CheckUrlIsUniqueAsync(string url, Guid id)
        {
            return (await _unitOfWork.Faculties.GetCountByUrlAsync(url, id)) == 0;
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await _unitOfWork.Faculties.GetByIdAsync(id);
            await _unitOfWork.Faculties.DeleteAsync(toDelete);
            _unitOfWork.Commit();
        }

        public async Task<FacultyDTO> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.Faculties.GetByIdAsync(id);
            return Mapper.Map<FacultyDTO>(entity);
        }

        public async Task<FacultyDTO> GetAsync(string url)
        {
            var entity = await _unitOfWork.Faculties.GetByUrlAsync(url);
            return Mapper.Map<FacultyDTO>(entity);
        }

        public async Task<FacultyDTO> SaveAsync(FacultyDTO model)
        {
            Faculty? entity;
            if (model.IsNew)
            {
                entity = Mapper.Map<Faculty>(model);
            }
            else
            {
                entity = await _unitOfWork.Faculties.GetByIdAsync(model.Id);
                Mapper.Map(model, entity);
            }

            if (model.FileImg != null)
            {
                string path = "\\img\\Faculties\\";
                path += model.Img != null ? model.ShortName.Replace(" ", "_") + "\\" : "Default.jpg";
                if (entity.Img != "\\img\\Faculties\\Default.jpg")
                {
                    IFileService.DeleteWithPreviewsFile(entity.Img);
                }
                entity.Img = IFileService.UploadWithPreviewsFile(model.FileImg, path + model.FileImg.FileName);
            }

            await _unitOfWork.Faculties.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<FacultyDTO>(entity);
        }
    }
}
