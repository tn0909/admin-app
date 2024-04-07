using Nest;

namespace AdminApp.Models
{
    public abstract class Document
    {
        public string Id { get; set; }
        public JoinField JoinField { get; set; }
    }
}