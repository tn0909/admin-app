namespace AdminApp.Dtos
{
    public class SearchCompanyDto
    {
        public string SearchTerm { get; set; }
        public int Limit { get; set; } = 10;

        public bool IncludeUsers { get; set; }

        public int UsersLimit {get; set; } = 10;
    }
}