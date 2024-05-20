using RecruitingSystem.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitingSystem.DTOs.ApplicationDtos
{
    public class ApplicationAddDto
    {
        public double Expected_Salary { get; set; }


        public string Number_Year_Experience { get; set; }


        public string Notice_Period { get; set; }


        public int VacancyId { get; set; } // Corrected casing for consistency
  

        public string ApplicantId { get; set; }
    }
}
