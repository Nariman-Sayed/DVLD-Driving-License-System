using DVLD.Api.Extensions;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DVLD.Api.Controllers
{
    /// <summary>
    /// Endpoints for reading and managing people. All endpoints require a valid JWT.
    /// </summary>
    // Must stay public: ASP.NET Core MVC only discovers public controller types.
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitingServiceExtensions.FixedWindowPolicy)]
    [Authorize]
    public sealed class PersonController(IPersonService personService) : ControllerBase
    {
        /// <summary>
        /// Returns a page of people ordered by person identifier.
        /// </summary>
        /// <param name="pageNumber">The one-based page number. Defaults to 1.</param>
        /// <param name="pageSize">The page size between 1 and 100. Defaults to 20.</param>
        /// <param name="filterOn">Optional filter field: nationalNo, firstName, lastName or countryId.</param>
        /// <param name="filterQuery">Optional value to filter by.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>One page of people together with paging metadata.</returns>
        /// <response code="200">The page was returned successfully.</response>
        /// <response code="400">Paging or filter values are invalid.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // GET: api/person?pageNumber=1&amp;pageSize=20
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<PersonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<PersonDto>>> GetAllPeople(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            CancellationToken cancellationToken = default)
        {
            var query = new PersonQuery(pageNumber, pageSize, filterOn, filterQuery);
            var people = await personService.GetPeopleAsync(query, cancellationToken);
            return Ok(people);
        }

        /// <summary>
        /// Returns a single person by identifier.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <returns>The matching person.</returns>
        /// <response code="200">The person was found.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No person exists with the given identifier.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // GET: api/person/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDto>> GetPersonById(int id)
        {
            var person = await personService.GetPersonByIdAsync(id);
            return person is null ? NotFound($"Person with ID {id} not found.") : Ok(person);
        }

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to create.</param>
        /// <returns>The created person.</returns>
        /// <response code="201">The person was created; the Location header points to the new resource.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="409">A person with the same national number already exists.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // POST: api/person
        [HttpPost]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDto>> AddNewPerson(PersonDto person)
        {
            var created = await personService.AddNewPersonAsync(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = created.PersonID }, created);
        }

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="id">The identifier of the person to update.</param>
        /// <param name="person">The new values for the person.</param>
        /// <response code="204">The person was updated.</response>
        /// <response code="400">The request body failed validation.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No person exists with the given identifier.</response>
        /// <response code="409">A person with the same national number already exists.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // PUT: api/person/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePerson(int id, PersonDto person)
        {
            var updated = await personService.UpdatePersonAsync(id, person);
            return updated ? NoContent() : NotFound($"Person with ID {id} not found.");
        }

        /// <summary>
        /// Deletes an existing person.
        /// </summary>
        /// <param name="id">The identifier of the person to delete.</param>
        /// <response code="204">The person was deleted.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No person exists with the given identifier.</response>
        /// <response code="409">Other records still reference this person.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // DELETE: api/person/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var deleted = await personService.DeletePersonAsync(id);
            return deleted ? NoContent() : NotFound($"Person with ID {id} not found.");
        }
    }
}
