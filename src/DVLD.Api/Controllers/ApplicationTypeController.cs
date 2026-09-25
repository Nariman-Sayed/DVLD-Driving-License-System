using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationTypeController : ControllerBase
    {
        private readonly IApplicationTypeService _service;

        public ApplicationTypeController(IApplicationTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationTypeDto>>> GetAllTypes()
        {
            var types = await _service.GetAllTypesAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationTypeDto>> GetTypeById(int id)
        {
            var type = await _service.GetTypeByIdAsync(id);
            return type is null ? NotFound($"ApplicationType with ID {id} not found.") : Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationTypeDto>> AddNewType(ApplicationTypeDto type)
        {
            var created = await _service.AddNewTypeAsync(type);
            return CreatedAtAction(nameof(GetTypeById), new { id = created.ApplicationTypeID }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateType(int id, ApplicationTypeDto type)
        {
            var updated = await _service.UpdateTypeAsync(id, type);
            return updated ? NoContent() : NotFound($"ApplicationType with ID {id} not found.");
        }
    }
}