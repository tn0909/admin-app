
namespace AdminApp.Dtos
{
    public class CompanyResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public List<UserDto> Users { get; set; }
    }
}