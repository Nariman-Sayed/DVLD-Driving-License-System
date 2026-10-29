using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    /// <summary>
    /// Endpoints for reading and managing application types. All endpoints require a valid JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationTypeController : ControllerBase
    {
        private readonly IApplicationTypeService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTypeController"/> class.
        /// </summary>
        /// <param name="service">The application type service.</param>
        public ApplicationTypeController(IApplicationTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all application types.
        /// </summary>
        /// <returns>All application types; an empty array when none exist.</returns>
        /// <response code="200">The list of application types was returned successfully.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ApplicationTypeDto>>> GetAllTypes()
        {
            var types = await _service.GetAllTypesAsync();
            return Ok(types);
        }

        /// <summary>
        /// Returns a single application type by identifier.
        /// </summary>
        /// <param name="id">The application type identifier.</param>
        /// <returns>The matching application type.</returns>
        /// <response code="200">The application type was found.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No application type exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApplicationTypeDto>> GetTypeById(int id)
        {
            var type = await _service.GetTypeByIdAsync(id);
            return type is null ? NotFound($"ApplicationType with ID {id} not found.") : Ok(type);
        }

        /// <summary>
        /// Creates a new application type.
        /// </summary>
        /// <param name="type">The application type to create.</param>
        /// <returns>The created application type.</returns>
        /// <response code="201">The application type was created; the Location header points to the new resource.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApplicationTypeDto>> AddNewType(ApplicationTypeDto type)
        {
            var created = await _service.AddNewTypeAsync(type);
            return CreatedAtAction(nameof(GetTypeById), new { id = created.ApplicationTypeID }, created);
        }

        /// <summary>
        /// Updates an existing application type.
        /// </summary>
        /// <param name="id">The identifier of the application type to update.</param>
        /// <param name="type">The new values for the application type.</param>
        /// <response code="204">The application type was updated.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No application type exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateType(int id, ApplicationTypeDto type)
        {
            var updated = await _service.UpdateTypeAsync(id, type);
            return updated ? NoContent() : NotFound($"ApplicationType with ID {id} not found.");
        }
    }
}
