using Nest;

namespace AdminApp.Models
{
    public class User : Document
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
    }
}
