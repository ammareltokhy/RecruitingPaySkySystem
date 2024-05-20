using RecruitingSystem.DTOs.ApplicationDtos;
using RecruitingSystem.DTOs.VacancyDtos;
using RecruitingSystem.Model;

namespace RecruitingSystem.Interfaces.ApplicationRepo
{
    public interface IApplicationRepository
    {
        List<ApplicationGetAllDto> GetAll();
        ApplicationGetAllDto GetById(int id);
        Task< bool> Add (ApplicationAddDto Applicationdto);
        bool Update (int id ,ApplicationGetAllDto applicationUpdateDto);
        bool Delete (int id);
        void save();

    }
}
