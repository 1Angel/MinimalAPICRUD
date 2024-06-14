using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace minimalapiCRUD
{
    public class Appdbcontext: IdentityDbContext<IdentityUser>
    {
        public Appdbcontext(DbContextOptions<Appdbcontext> options) : base(options) { } 

        public DbSet<Post> Posts { get; set; }
    }
}
