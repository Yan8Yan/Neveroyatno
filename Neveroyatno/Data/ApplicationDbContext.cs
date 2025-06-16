using Microsoft.EntityFrameworkCore;
using Neveroyatno.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Neveroyatno.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}





