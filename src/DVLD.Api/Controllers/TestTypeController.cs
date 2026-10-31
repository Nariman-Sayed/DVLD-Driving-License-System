using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    /// <summary>
    /// Endpoints for reading and managing test types. All endpoints require a valid JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestTypeController : ControllerBase
    {
        private readonly ITestTypeService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTypeController"/> class.
        /// </summary>
        /// <param name="service">The test type service.</param>
        public TestTypeController(ITestTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all test types.
        /// </summary>
        /// <returns>All test types; an empty array when none exist.</returns>
        /// <response code="200">The list of test types was returned successfully.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TestTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TestTypeDto>>> GetAllTestTypes()
        {
            var types = await _service.GetAllTestTypesAsync();
            return Ok(types);
        }

        /// <summary>
        /// Returns a single test type by identifier.
        /// </summary>
        /// <param name="id">The test type identifier.</param>
        /// <returns>The matching test type.</returns>
        /// <response code="200">The test type was found.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No test type exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TestTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TestTypeDto>> GetTestTypeById(int id)
        {
            var type = await _service.GetTestTypeByIdAsync(id);
            return type is null ? NotFound($"TestType with ID {id} not found.") : Ok(type);
        }

        /// <summary>
        /// Creates a new test type.
        /// </summary>
        /// <param name="type">The test type to create.</param>
        /// <returns>The created test type.</returns>
        /// <response code="201">The test type was created; the Location header points to the new resource.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TestTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TestTypeDto>> AddNewTestType(TestTypeDto type)
        {
            var created = await _service.AddNewTestTypeAsync(type);
            return CreatedAtAction(nameof(GetTestTypeById), new { id = created.TestTypeID }, created);
        }

        /// <summary>
        /// Updates an existing test type.
        /// </summary>
        /// <param name="id">The identifier of the test type to update.</param>
        /// <param name="type">The new values for the test type.</param>
        /// <response code="204">The test type was updated.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No test type exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTestType(int id, TestTypeDto type)
        {
            var updated = await _service.UpdateTestTypeAsync(id, type);
            return updated ? NoContent() : NotFound($"TestType with ID {id} not found.");
        }
    }
}
