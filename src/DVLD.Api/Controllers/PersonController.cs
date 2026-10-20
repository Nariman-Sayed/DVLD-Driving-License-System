using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    /// <summary>
    /// Endpoints for reading and managing people.
    /// </summary>
    // Must stay public: ASP.NET Core MVC only discovers public controller types.
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PersonController(IPersonService personService) : ControllerBase
    {
        /// <summary>
        /// Returns all people.
        /// </summary>
        /// <returns>All people; an empty array when none exist.</returns>
        /// <response code="200">The list of people was returned successfully.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // GET: api/person
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonDto>>> GetAllPeople()
        {
            var people = await personService.GetAllPeopleAsync();
            return Ok(people);
        }

        /// <summary>
        /// Returns a single person by identifier.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <returns>The matching person.</returns>
        /// <response code="200">The person was found.</response>
        /// <response code="404">No person exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // GET: api/person/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <response code="500">An unexpected error occurred.</response>
        // POST: api/person
        [HttpPost]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="404">No person exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // PUT: api/person/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <response code="404">No person exists with the given identifier.</response>
        /// <response code="500">An unexpected error occurred.</response>
        // DELETE: api/person/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var deleted = await personService.DeletePersonAsync(id);
            return deleted ? NoContent() : NotFound($"Person with ID {id} not found.");
        }
    }
}
