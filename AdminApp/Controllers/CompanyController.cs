using AdminApp.Services;
using AdminApp.Dtos;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminApp.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("search")]
        public async Task<IEnumerable<CompanyResponseDto>> Search(SearchCompanyDto searchParams)
        {
            var companies = await _companyService.Search(searchParams);
            return companies;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyResponseDto>> Get(string id)
        {
            var company = await _companyService.GetById(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company) ;
        }

        [HttpPost()]
        public async Task<IActionResult> Add(CompanyRequestDto companyDto)
        {
            if (string.IsNullOrWhiteSpace(companyDto.Name))
            {
                return BadRequest();
            }

            var isIndexed = await _companyService.Add(companyDto);
            return Ok(new { IsIndexed = isIndexed });
        }
    }
}
