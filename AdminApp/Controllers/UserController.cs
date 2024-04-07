using Microsoft.AspNetCore.Mvc;
using AdminApp.Services;
using AdminApp.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public UserController(IUserService userService, ICompanyService companyService)
        {
            _userService = userService;
            _companyService = companyService;
        }

        [HttpPost("search")]
        public async Task<IEnumerable<UserDto>> Search(SearchUserDto searchParams)
        {
            var users = await _userService.Search(searchParams);
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(string id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost()]
        public async Task<IActionResult> Add(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) 
                || string.IsNullOrWhiteSpace(userDto.FirstName)
                || string.IsNullOrWhiteSpace(userDto.LastName))
            {
                return BadRequest();
            }

            var company = await _companyService.GetById(userDto.CompanyId);

            if (company == null)
            {
                return BadRequest();
            }

            var isIndexed = await _userService.Add(userDto);
            return Ok(new { IsIndexed = isIndexed });
        }
    }
}
