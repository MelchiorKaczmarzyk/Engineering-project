using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Any;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;
using System.Reflection.Metadata;

namespace MyBackend.Controllers
{
    [Route("api/vtts")]
    [ApiController]
    public class VttsController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        public VttsController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }
        [HttpGet("vttService")]
        public async Task<ActionResult<IEnumerable<VttModel>>> GetVttsForService()
        {
            try
            {
                var vtts = await _repos.GetVttsAsync();
                var result = _mapper.Map<IEnumerable<VttModel>>(vtts);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("addTrigger")]
        public ActionResult AddVtt(VttModel vtt)
        {
            try
            {
                if (vtt == null)
                {
                    return BadRequest();
                }
                var vttToAdd = new Vtt
                {
                    Id = new Guid().ToString(),
                    Name = vtt.Name,
                };
                _repos.AddVtt(vttToAdd);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteVtt")]
        public ActionResult DeleteVtt(string vttName)
        {
            try
            {
                if (vttName == null || vttName == string.Empty)
                {
                    return BadRequest();
                }
                _repos.DeleteVtt(vttName);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
