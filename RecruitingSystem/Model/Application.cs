using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitingSystem.Model
{
    public class Application
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Application { get; set; } // Auto-increment assumed

        [Required(ErrorMessage = "Expected Salary is required")]
        public double Expected_Salary { get; set; }

        [Required(ErrorMessage = "Number of Years of Experience is required")]
        public string Number_Year_Experience { get; set; }

        [Required(ErrorMessage = "Notice Period is required")]
        public string Notice_Period { get; set; }

        [ForeignKey(nameof(vacancy))]
        public int VacancyId { get; set; } // Corrected casing for consistency
        public Vacancy? vacancy { get; set; }

        [ForeignKey(nameof(applicant))]
        public string ApplicantId { get; set; }
        public Applicant? applicant { get; set; }
        public bool IsDeleted { get; set; } = false;





    }
}
