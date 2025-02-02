namespace PortalJob.Models
{
    public class JobCandidate
    {
        public int Id { get; set; }
        public string Resume { get; set; }
        public string Messsage { get; set; }
        public bool IsHired { get; set; }
        public int CompanyJobId { get; set; }
        public string CandidateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CompanyJob Job { get; set; }
        public User Profile { get; set; }
    }
}
