namespace RecruitingSystem.Model
{
    public class Applicant:ApplicationUser
    {
        public bool Is_Enable { get; set; } = true;
        public DateTime? Limit_Date { get; set; }=DateTime.Now;
    }
}
