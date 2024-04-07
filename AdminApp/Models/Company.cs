using Nest;

namespace AdminApp.Models
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class Company: Document
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
    }
}
