using AutoMapper;
using BLL.Mappers;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Base
{
    public class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected string Lang = "be";
        protected IMapper Mapper { get; }

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DomainToDTOMappingProfile));
                cfg.AddProfile(typeof(DTOToDomainMappingProfile));
                cfg.AllowNullCollections = true;
            });
            Mapper = config.CreateMapper();
        }
    }
}
