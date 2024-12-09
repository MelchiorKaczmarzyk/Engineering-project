using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        public TagsController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }
        [HttpGet("tagService")]
        public async Task<ActionResult<IEnumerable<TagModel>>> GetTagsForService()
        {
            try
            {
                var triggers = await _repos.GetTagsAsync();
                var result = _mapper.Map<IEnumerable<TagModel>>(triggers);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("addTag")]
        public ActionResult AddTag(TagModel tag)
        {
            try
            {
                if (tag == null)
                {
                    return BadRequest();
                }
                var tagToAdd = new Tag
                {
                    Id = new Guid().ToString(),
                    Name = tag.Name,
                };
                _repos.AddTag(tagToAdd);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteTag")]
        public ActionResult DeleteTag(string tagName)
        {
            try
            {
                if (tagName == null || tagName == string.Empty)
                {
                    return BadRequest();
                }
                _repos.DeleteTag(tagName);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
