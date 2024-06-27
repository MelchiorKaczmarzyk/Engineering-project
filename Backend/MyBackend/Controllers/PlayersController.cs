using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        PlayersController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }


    }
}
