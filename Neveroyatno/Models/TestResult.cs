using Neveroyatno.Models;


namespace Neveroyatno.Models
{
    public class TestResult
    {
        public int Id { get; set; }

        public int TestId { get; set; }
        public virtual Test Test { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int Score { get; set; }
        public int Total { get; set; }
        public int Percentage { get; set; }

        public DateTime PassedAt { get; set; } = DateTime.UtcNow;

        public string? CertificateFileName { get; set; }
    }
}