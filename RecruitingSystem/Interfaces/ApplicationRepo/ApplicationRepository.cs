using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitingSystem.DTOs.ApplicationDtos;
using RecruitingSystem.DTOs.VacancyDtos;
using RecruitingSystem.Interfaces.Vacancyrepo;
using RecruitingSystem.Model;

namespace RecruitingSystem.Interfaces.ApplicationRepo
{
    public class ApplicationRepository
    {
        private readonly DBcontext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
       


        public ApplicationRepository(DBcontext dbcontext, UserManager<ApplicationUser> userManager, VacancyRepository vacancyRepository)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            
        }

        public async Task<bool> Add(ApplicationAddDto Applicationdto)
        {


            var applicant = new Applicant();
            applicant = await _userManager.Users.OfType<Applicant>()
                                    .FirstOrDefaultAsync(u => u.Id == Applicationdto.ApplicantId);

            if (applicant.Limit_Date < DateTime.Now)
            {
                applicant.Is_Enable = true;
            }

            if (applicant.Is_Enable)
            {

                // will handle based on number of vac 
                var vacancy = _dbcontext.Vacancies.Where(n => n.Id_Vacancy == Applicationdto.VacancyId && n.Is_Deactivated == false && n.IsDeleted == false).FirstOrDefault();
                if (vacancy == null)
                {
                    return false;
                }
                if (vacancy.NumberOfAplications == vacancy.Actual_NumberOfAplications)
                {
                    return false;
                }
                //*** we will ensure from that update ***
                vacancy.Actual_NumberOfAplications += 1;



                var applicationData = new Application();

                applicationData.Expected_Salary = Applicationdto.Expected_Salary;
                applicationData.Notice_Period = Applicationdto.Notice_Period;
                applicationData.Number_Year_Experience = Applicationdto.Number_Year_Experience;
                applicationData.VacancyId = Applicationdto.VacancyId;
                applicationData.ApplicantId = Applicationdto.ApplicantId;

                try
                {
                    _dbcontext.applications.Add(applicationData);
                    save();
                    // get the id of applicant 

                    if (applicant != null)
                    {
                        // will make the applicant disable 
                        applicant.Is_Enable = false;

                        // will make date expired after 1 Day 

                        applicant.Limit_Date = DateTime.Now.AddDays(1);
                        save();


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("we have error" + ex.ToString());
                }
                return true;
            }
            else { return false; }

        }
        public List<ApplicationGetAllDto> GetAll()
        {
            List<Application> applications = _dbcontext.applications.Where(a => a.IsDeleted == false).Include(a => a.applicant).Include(a => a.vacancy).ToList();
            List<ApplicationGetAllDto> applicationsDto = new List<ApplicationGetAllDto>();
            foreach (Application application in applications)
            {
                var applicationlist = new ApplicationGetAllDto()
                {
                    Id_Application = application.Id_Application,
                    Expected_Salary = application.Expected_Salary,
                    Notice_Period = application.Notice_Period,
                    Number_Year_Experience = application.Number_Year_Experience,
                    VacancyId = application.VacancyId,
                    ApplicantId = application.ApplicantId,


                };
                applicationsDto.Add(applicationlist);
            }
            return applicationsDto;
        }
       public ApplicationGetAllDto GetById(int id)
        {
            Application application = _dbcontext.applications.Where(a => a.Id_Application == id && a.IsDeleted == false).Include(a => a.vacancy).Include(a => a.applicant).FirstOrDefault();
            var applicationDto = new ApplicationGetAllDto()
            {
                Id_Application = application.Id_Application,
                Expected_Salary = application.Expected_Salary,
                Notice_Period = application.Notice_Period,
                Number_Year_Experience = application.Number_Year_Experience,
                VacancyId = application.VacancyId,
                ApplicantId = application.ApplicantId,
            };
            return applicationDto;
        }



        public void save()
        {
            _dbcontext.SaveChanges();
        }
        public bool Delete(int id)
        {
            var application = _dbcontext.applications.FirstOrDefault(a => a.Id_Application == id && a.IsDeleted == false);
            if (application != null)
            {
                application.IsDeleted = true;
                save();
                return true;
            }
            return false;
        }

        public bool Update(int id, ApplicationGetAllDto applicationUpdateDto)
        {
            if (applicationUpdateDto != null)
            {
                var application = _dbcontext.applications.FirstOrDefault(a => a.IsDeleted == false && a.Id_Application == id);
                if (application != null)
                {
                    application.Id_Application = id;
                    application.Expected_Salary = applicationUpdateDto.Expected_Salary;
                    application.Notice_Period = applicationUpdateDto.Notice_Period;
                    application.Number_Year_Experience = applicationUpdateDto.Number_Year_Experience;
                    application.VacancyId = applicationUpdateDto.VacancyId;
                    application.ApplicantId = applicationUpdateDto.ApplicantId;
                    save();
                }
            }
            return false;
        }


    }

}
