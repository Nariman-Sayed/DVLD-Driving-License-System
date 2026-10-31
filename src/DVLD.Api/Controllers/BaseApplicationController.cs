using DVLD.Api.Constants;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DVLD.Api.Controllers
{
    /// <summary>
    /// Endpoints for reading and managing base applications. All endpoints require a valid JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApplicationController : ControllerBase
    {
        private readonly IBaseApplicationService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApplicationController"/> class.
        /// </summary>
        /// <param name="service">The base application service.</param>
        public BaseApplicationController(IBaseApplicationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all base applications.
        /// </summary>
        /// <returns>All base applications; an empty array when none exist.</returns>
        /// <response code="200">The list of base applications was returned successfully.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<BaseApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BaseApplicationDto>>> GetAllApplications()
        {
            var applications = await _service.GetAllApplicationsAsync();
            return Ok(applications);
        }

        /// <summary>
        /// Returns a single base application by identifier.
        /// </summary>
        /// <param name="id">The base application identifier.</param>
        /// <returns>The matching base application.</returns>
        /// <response code="200">The base application was found.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No base application exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseApplicationDto>> GetApplicationById(int id)
        {
            var application = await _service.GetApplicationByIdAsync(id);
            return application is null ? NotFound($"Application with ID {id} not found.") : Ok(application);
        }

        /// <summary>
        /// Creates a new base application.
        /// </summary>
        /// <param name="request">The base application to create.</param>
        /// <returns>The created base application.</returns>
        /// <response code="201">The base application was created; the Location header points to the new resource.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApplicationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseApplicationDto>> AddNewApplication(CreateBaseApplicationRequest request)
        {
            // The acting user is taken from the authenticated token, never from the body.
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(ApiErrorMessages.MissingAuthenticatedUserId);
            }

            var created = await _service.AddNewApplicationAsync(request, userId);
            return CreatedAtAction(nameof(GetApplicationById), new { id = created.ApplicationID }, created);
        }

        /// <summary>
        /// Updates an existing base application.
        /// </summary>
        /// <param name="id">The identifier of the base application to update.</param>
        /// <param name="application">The new values for the base application.</param>
        /// <response code="204">The base application was updated.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No base application exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // Status transitions are intentionally unconstrained for now; a full state
        // machine (New -> Cancelled/Completed) is a deferred concern.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateApplication(int id, BaseApplicationDto application)
        {
            var updated = await _service.UpdateApplicationAsync(id, application);
            return updated ? NoContent() : NotFound($"Application with ID {id} not found.");
        }
    }
}
