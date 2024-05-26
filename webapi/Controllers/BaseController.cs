using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using webapi.Mappers;

namespace webapi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IMapper Mapper { get; }

        public BaseController(IHttpContextAccessor accessor, LinkGenerator generator)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DtoToViewModelMappingProfile));
                cfg.AddProfile(typeof(ViewModelToDtoMappingProfile));
                cfg.AllowNullCollections = true;
            });
            Mapper = config.CreateMapper();
            LinkGeneratorHelper.Current = new LinkGeneratorHelper(accessor, generator);
        }
    }
}
