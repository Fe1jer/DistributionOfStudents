using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
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

        protected static HttpStatusCode GetErrorCode(Exception e)
        {
            return e switch
            {
                ValidationException _ => HttpStatusCode.BadRequest,
                FormatException _ => HttpStatusCode.BadRequest,
                AuthenticationException _ => HttpStatusCode.Forbidden,
                NotImplementedException _ => HttpStatusCode.NotImplemented,
                _ => HttpStatusCode.InternalServerError,
            };
        }
    }
}
