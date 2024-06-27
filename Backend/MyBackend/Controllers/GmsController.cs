using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/gms")]
    [ApiController]
    public class GmsController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        GmsController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }

        
    }
}
