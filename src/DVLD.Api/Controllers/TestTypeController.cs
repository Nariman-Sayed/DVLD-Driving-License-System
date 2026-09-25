using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestTypeController : ControllerBase
    {
        private readonly ITestTypeService _service;

        public TestTypeController(ITestTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestTypeDto>>> GetAllTestTypes()
        {
            var types = await _service.GetAllTestTypesAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestTypeDto>> GetTestTypeById(int id)
        {
            var type = await _service.GetTestTypeByIdAsync(id);
            return type is null ? NotFound($"TestType with ID {id} not found.") : Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult<TestTypeDto>> AddNewTestType(TestTypeDto type)
        {
            var created = await _service.AddNewTestTypeAsync(type);
            return CreatedAtAction(nameof(GetTestTypeById), new { id = created.TestTypeID }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTestType(int id, TestTypeDto type)
        {
            var updated = await _service.UpdateTestTypeAsync(id, type);
            return updated ? NoContent() : NotFound($"TestType with ID {id} not found.");
        }
    }
}