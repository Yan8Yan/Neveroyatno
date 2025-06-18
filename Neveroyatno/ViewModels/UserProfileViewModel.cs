using Neveroyatno.Models;

namespace Neveroyatno.ViewModels
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public int CompletedTests { get; set; }
        public int TotalTests { get; set; }

        public List<TestResult> Results { get; set; }
    }


}
