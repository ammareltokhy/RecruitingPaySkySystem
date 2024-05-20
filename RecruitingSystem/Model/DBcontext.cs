using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace RecruitingSystem.Model
{
    public class DBcontext : IdentityDbContext<ApplicationUser>
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options)
        {
        }
           public virtual DbSet<Vacancy> Vacancies { get; set; }
           public virtual DbSet<Application> applications { get; set; }

           public virtual DbSet<Employer> employers { get; set; }
         
           public virtual DbSet<Applicant> applicants { get; set; }
        

    }
}
