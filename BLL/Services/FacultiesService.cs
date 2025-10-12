using BLL.DTO.Faculties;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces;
using FilesManager;
using System.Reflection.Metadata;

namespace BLL.Services
{
    public class FacultiesService(IUnitOfWork unitOfWork) : BaseService(unitOfWork), IFacultiesService
    {
        public async Task<List<FacultyDTO>> GetAllAsync()
        {
            var result = await _unitOfWork.Faculties.GetAllAsync();
            return Mapper.Map<List<FacultyDTO>>(result);
        }

        public async Task<bool> CheckUrlIsUniqueAsync(string newUrl, string oldUrl)
        {
            return (await _unitOfWork.Faculties.GetCountByUrlAsync(newUrl, oldUrl)) == 0;
        }

        public async Task DeleteAsync(string url)
        {
            var toDelete = await _unitOfWork.Faculties.GetByUrlAsync(url);
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
                string lastPath = model.Img != null ? model.ShortName.Replace(" ", "_") : "Default.jpg";
                var a = ManagerConstants.CombinePath(ManagerConstants.SUB_DIR, "Faculties", "Default.jpg");
                if (entity.Img != ManagerConstants.CombinePath(ManagerConstants.SUB_DIR, "Faculties", "Default.jpg"))
                {
                    FilesHelper.DeleteFile(entity.Img);
                }
                entity.Img = FilesHelper.UploadFile(model.FileImg, ManagerConstants.CombinePath("Faculties", lastPath, model.FileImg.FileName));
            }

            await _unitOfWork.Faculties.InsertOrUpdateAsync(entity);
            _unitOfWork.Commit();

            return Mapper.Map<FacultyDTO>(entity);
        }
    }
}
