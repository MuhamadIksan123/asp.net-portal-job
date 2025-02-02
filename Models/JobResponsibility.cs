using System.Text.Json.Serialization;

namespace PortalJob.Models
{
    public class JobResponsibility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyJobId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public CompanyJob CompanyJob { get; set; }
    }
}
