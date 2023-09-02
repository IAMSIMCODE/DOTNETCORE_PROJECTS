using DataSearch.Api.Models;
using DataSearch.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.OM;

namespace DataSearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSearchController : ControllerBase
    {
        private readonly IDataSearchService _dataSearchService;

        public DataSearchController(IDataSearchService dataSearchService)
        {
            _dataSearchService = dataSearchService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            if (person == null) {throw new ArgumentNullException(nameof(person));} 
            //Note : The above line of code can be handle properly and return an actual response

            var create = await _dataSearchService.AddPerson(person);
            return Ok(create);
        }

        [HttpGet("filterAge")]
        public async Task<IActionResult> FilterByAge([FromQuery] int minAge, [FromQuery] int maxAge)
        {
            var personAge = _dataSearchService.FilterByAge(minAge, maxAge);
            await Task.CompletedTask;
            return Ok(personAge);
        }

        [HttpGet("filterGeo")]
        public async Task<IActionResult> FilterByGeo([FromQuery] double lon, [FromQuery] double lat, [FromQuery] double radius, [FromQuery] string unit)
        {
            var getGeo = _dataSearchService.FilterByGeo(lon, lat, radius, unit);
            return Ok(getGeo);
        }

        [HttpGet("filterName")]
        public async Task<IActionResult> FilterByName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var getName = _dataSearchService.FilterByName(firstName, lastName);
            return Ok(getName);
        }

        [HttpGet("fullText")]
        public async Task<IActionResult> FilterByPersonalStatement([FromQuery] string text)
        {
            var getStatement = _dataSearchService.FilterByPersonalStatement(text);
            return Ok(getStatement);
        }

        [HttpGet("streetName")]
        public async Task<IActionResult> FilterByStreetName([FromQuery] string streetName)
        {
            var strtName = _dataSearchService.FilterByStreetName(streetName);
            await Task.CompletedTask;
            return Ok(strtName);
        }

        [HttpGet("skill")]
        public async Task<IActionResult> FilterBySkill([FromQuery] string skill)
        {
            var getSkil = _dataSearchService.FilterBySkill(skill);
            return Ok(getSkil);
        }

        [HttpPatch("updateAge/{id}")]
        public IActionResult UpdateAge([FromRoute] string id, [FromBody] int newAge)
        {
            _dataSearchService.UpdateAge(id, newAge);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson([FromRoute] string id)
        {
            _dataSearchService?.DeletePerson(id);
            return NoContent();
        }
    }
}
