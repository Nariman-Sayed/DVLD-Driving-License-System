using DVLD.Api.Extensions;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DVLD.Api.Controllers
{
    /// <summary>
    /// Endpoints for reading countries. All endpoints require a valid JWT.
    /// </summary>
    // Must stay public: ASP.NET Core MVC only discovers public controller types.
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitingServiceExtensions.FixedWindowPolicy)]
    [Authorize]
    public sealed class CountryController(ICountryService countryService) : ControllerBase
    {
        /// <summary>
        /// Returns all countries ordered by name.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>All countries; an empty array when none exist.</returns>
        /// <response code="200">The list of countries was returned successfully.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CountryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CountryDto>>> GetAllCountries(CancellationToken cancellationToken)
        {
            var countries = await countryService.GetAllCountriesAsync(cancellationToken);
            return Ok(countries);
        }

        /// <summary>
        /// Returns a single country by identifier.
        /// </summary>
        /// <param name="id">The country identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The matching country.</returns>
        /// <response code="200">The country was found.</response>
        /// <response code="401">Unauthorized. A valid JWT is required.</response>
        /// <response code="404">No country exists with the given identifier.</response>
        /// <response code="429">Too many requests. Try again later.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CountryDto>> GetCountryById(int id, CancellationToken cancellationToken)
        {
            var country = await countryService.GetCountryByIdAsync(id, cancellationToken);
            return Ok(country);
        }
    }
}
