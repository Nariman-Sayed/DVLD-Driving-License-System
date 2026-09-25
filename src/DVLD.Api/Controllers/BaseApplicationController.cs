using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DVLD.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApplicationController : ControllerBase
    {
        private readonly IBaseApplicationService _service;

        public BaseApplicationController(IBaseApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BaseApplicationDto>>> GetAllApplications()
        {
            var applications = await _service.GetAllApplicationsAsync();
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseApplicationDto>> GetApplicationById(int id)
        {
            var application = await _service.GetApplicationByIdAsync(id);
            return application is null ? NotFound($"Application with ID {id} not found.") : Ok(application);
        }

        [HttpPost]
        public async Task<ActionResult<BaseApplicationDto>> AddNewApplication(CreateBaseApplicationRequest request)
        {
            // The acting user is taken from the authenticated token, never from the body.
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Authenticated user id claim is missing or invalid.");
            }

            var created = await _service.AddNewApplicationAsync(request, userId);
            return CreatedAtAction(nameof(GetApplicationById), new { id = created.ApplicationID }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateApplication(int id, BaseApplicationDto application)
        {
            var updated = await _service.UpdateApplicationAsync(id, application);
            return updated ? NoContent() : NotFound($"Application with ID {id} not found.");
        }
    }
}