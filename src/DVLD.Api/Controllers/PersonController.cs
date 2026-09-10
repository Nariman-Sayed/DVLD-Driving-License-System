using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        // GET: api/person
        [HttpGet]
        public async Task<ActionResult<List<PersonDto>>> GetAllPeople()
        {
            var people = await _personService.GetAllPeopleAsync();
            return Ok(people);
        }

        // GET: api/person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPersonById(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            return person is null ? NotFound($"Person with ID {id} not found.") : Ok(person);
        }

        // POST: api/person
        [HttpPost]
        public async Task<ActionResult<PersonDto>> AddNewPerson(PersonDto person)
        {
            var created = await _personService.AddNewPersonAsync(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = created.PersonID }, created);
        }

        // PUT: api/person/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePerson(int id, PersonDto person)
        {
            var updated = await _personService.UpdatePersonAsync(id, person);
            return updated ? NoContent() : NotFound($"Person with ID {id} not found.");
        }

        // DELETE: api/person/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var deleted = await _personService.DeletePersonAsync(id);
            return deleted ? NoContent() : NotFound($"Person with ID {id} not found.");
        }
    }
}