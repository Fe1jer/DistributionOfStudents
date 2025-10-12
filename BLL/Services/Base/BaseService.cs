using AutoMapper;
using BLL.Mappers;
using DAL.Postgres.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Base
{
    public class BaseService(IUnitOfWork unitOfWork)
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected string Lang = "be";

        protected IMapper Mapper { get; } = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToDTOMappingProfile>();
                cfg.AddProfile<DTOToDomainMappingProfile>();
                cfg.AllowNullCollections = true;
            }, new LoggerFactory()).CreateMapper();
    }
}
