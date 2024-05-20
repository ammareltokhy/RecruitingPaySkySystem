using System.ComponentModel.DataAnnotations;

namespace RecruitingSystem.DTOs.UserDtos
{
    public class UserDto
    {


        public string UserName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        //[Compare("Password")]
        //public string ConfirmPassword { set; get; }
        public string? CompanyName { set; get; }
        public string? CompanyDesc { set; get; }
        public bool Is_Employer { set; get; }//if  true?  employer:applicant




    }

}

   

