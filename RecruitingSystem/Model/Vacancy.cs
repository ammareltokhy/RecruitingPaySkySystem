using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitingSystem.Model
{
    public class Vacancy
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Vacancy { get; set; }
        public string Job_Title { get; set; }
        public string Job_Description { get; set; }
        // remotly onsite hybird
        public string Job_Type { get; set; }
        //Full Time Part time 
        public string Job_Time { get; set; }
        //Entry Level (Junior Level / Fresh Grad)
        public string Career_Level { get; set; }
        public int Experience_needed { get; set; }
       // public double? Salary { get; set; }
        public DateTime created_time { get; set; } = DateTime.Now;
        public DateTime expiry_time { get; set; } = DateTime.Now.AddDays(30);
        public bool Is_Deactivated { get; set; }=false;
        public int NumberOfAplications { get; set; }
        public int Actual_NumberOfAplications { get; set; } = 0; // we will increase it when any applicant apply to job 
        [ForeignKey(nameof(employer))]
        public string employer_Id { get; set; }
        public Employer employer { get; set; }
        public bool IsDeleted { get; set; } = false;




    }
}
