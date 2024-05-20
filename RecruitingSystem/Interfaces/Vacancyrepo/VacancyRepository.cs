using Microsoft.EntityFrameworkCore;
using RecruitingSystem.DTOs;
using RecruitingSystem.DTOs.VacancyDtos;
using RecruitingSystem.Model;

namespace RecruitingSystem.Interfaces.Vacancyrepo
{
    public class VacancyRepository 
    {
        private readonly DBcontext _dbcontext;

        public VacancyRepository(DBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public bool Add(VacancyAddDto vacancy)
        {
            var vacancydata = new Vacancy();
            vacancydata.Experience_needed = vacancy.Experience_needed;
            vacancydata.Career_Level = vacancy.Career_Level;
            vacancydata.Job_Description = vacancy.Job_Description;
            vacancydata.Job_Type = vacancy.Job_Type;
            vacancydata.Job_Title = vacancy.Job_Title;
            vacancydata.Job_Time = vacancy.Job_Time;
            vacancydata.NumberOfAplications = vacancy.NumberOfAplications;
            vacancydata.employer_Id=vacancy.employer_Id;
            vacancydata.Actual_NumberOfAplications = vacancy.Actual_NumberOfAplications;
            try
            {
                _dbcontext.Vacancies.Add(vacancydata);
                save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" We have error "+ex.ToString());
                return false;
            }
            return true;
        }

        public List<VacancyGetAllDto> GetAll()
        {
            List<Vacancy> vacancies = _dbcontext.Vacancies.Where(v=>v.IsDeleted== false).Include(e=>e.employer).ToList();
            List<VacancyGetAllDto> vacancyGetAlls = new List<VacancyGetAllDto>();
            foreach(Vacancy vacancy in vacancies)
            {
                var vacancylist = new VacancyGetAllDto()
                {
                    Id_Vacancy = vacancy.Id_Vacancy,
                    Job_Time = vacancy.Job_Time,
                    Job_Title = vacancy.Job_Title,
                    Job_Description = vacancy.Job_Description,
                    Career_Level = vacancy.Career_Level,
                    Experience_needed = vacancy.Experience_needed,
                    Job_Type = vacancy.Job_Type,
                    NumberOfAplications = vacancy.NumberOfAplications,
                    Actual_NumberOfAplications = vacancy.Actual_NumberOfAplications,
                    expiry_time = vacancy.expiry_time,
                    created_time = vacancy.created_time,
                    employer_Id = vacancy.employer_Id
                    
                };
                vacancyGetAlls.Add(vacancylist);
            }
            return vacancyGetAlls;
        }

        public VacancyGetAllDto GetById(int id)
        {
            Vacancy vacancy = _dbcontext.Vacancies.Where(v => v.IsDeleted == false && v.Id_Vacancy == id).Include(v=>v.employer).FirstOrDefault();
            var  vacancyDto = new VacancyGetAllDto()
            {
                Id_Vacancy = vacancy.Id_Vacancy,
                Job_Description = vacancy.Job_Description,
                Job_Time = vacancy.Job_Time,
                Job_Type = vacancy.Job_Type,
                Career_Level = vacancy.Career_Level,
                Experience_needed = vacancy.Experience_needed,
                Job_Title = vacancy.Job_Title,
                Actual_NumberOfAplications = vacancy.Actual_NumberOfAplications,
                NumberOfAplications = vacancy.NumberOfAplications,
                expiry_time = vacancy.expiry_time,
                created_time= vacancy.created_time      
            };
            return vacancyDto;
        }

        public void save()
        {
            _dbcontext.SaveChanges();
        }
        public bool Delete(int id)
        {
            var vacancy = _dbcontext.Vacancies.FirstOrDefault(v => v.Id_Vacancy == id && v.IsDeleted == false);
            if (vacancy != null)
            {
                vacancy.IsDeleted = true;
                save();
                return true;
            }
            return false;
        }
        public bool Deactivate(int id)
        {
            var vacancy = _dbcontext.Vacancies.FirstOrDefault(v => v.Id_Vacancy == id && v.IsDeleted == false);
            if (vacancy != null)
            {
                vacancy.Is_Deactivated = !vacancy.Is_Deactivated;
                save();
                return true;
            }
            return false;
        }
        public bool Update(int id, VacancyGetAllDto vacancyDto)
        {
            if(vacancyDto != null)
            {
                var vacancy = _dbcontext.Vacancies.FirstOrDefault(v=>v.Id_Vacancy==id && v.IsDeleted==false);
                if(vacancy != null)
                {
                    vacancy.Id_Vacancy = id;
                    vacancy.Job_Description= vacancyDto.Job_Description;
                    vacancy.Career_Level= vacancyDto.Career_Level;
                    vacancy.Job_Time= vacancyDto.Job_Time;
                    vacancy.Job_Title= vacancyDto.Job_Title;
                    vacancy.Job_Type= vacancyDto.Job_Type;
                    vacancy.NumberOfAplications = vacancyDto.NumberOfAplications;
                    vacancy.NumberOfAplications = vacancyDto.NumberOfAplications;

                    vacancy.Experience_needed= vacancyDto.Experience_needed;
                    vacancy.created_time = DateTime.Now;
                    vacancy.expiry_time = DateTime.Now.AddDays(30);

                    //_dbcontext.Vacancies.Update(vacancy);
                    save();
                }
            }
            return false;
        }


    }
}
