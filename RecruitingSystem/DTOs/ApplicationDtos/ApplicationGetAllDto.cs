using RecruitingSystem.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RecruitingSystem.DTOs.ApplicationDtos
{
    public class ApplicationGetAllDto
    {
        public int Id_Application { get; set; } // Auto-increment assumed

      
        public double Expected_Salary { get; set; }

        
        public string Number_Year_Experience { get; set; }

       
        public string Notice_Period { get; set; }

       
        public int VacancyId { get; set; } 
       

        
        public string ApplicantId { get; set; }
      
    }
}
