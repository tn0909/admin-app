namespace AdminApp.Dtos
{
    public class SearchUserDto
    {
        public string Company { get; set; }

        public string Email { get; set; }
        
        public int Limit { get; set; } = 10;
    }
}