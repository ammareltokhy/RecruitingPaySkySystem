﻿using RecruitingSystem.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitingSystem.DTOs.VacancyDtos
{
    public class VacancyAddDto
    {
      
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
     

        public int NumberOfAplications { get; set; }
        public int Actual_NumberOfAplications { get; set; } = 0; // we will increase it when any applicant apply to job 
       
        public string employer_Id { get; set; }
       
       






    }
}