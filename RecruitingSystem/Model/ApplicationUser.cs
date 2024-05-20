using Microsoft.AspNetCore.Identity;

namespace RecruitingSystem.Model
{
    public class ApplicationUser: IdentityUser
    {
        public bool IsDeleted { get; set; }=false;
    }
}
