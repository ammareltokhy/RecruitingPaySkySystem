using RecruitingSystem.DTOs;
using RecruitingSystem.DTOs.VacancyDtos;
using RecruitingSystem.Model;

namespace RecruitingSystem.Interfaces.Vacancyrepo
{
    public interface IVacancyRepository
    {
        List<VacancyGetAllDto> GetAll();
        VacancyGetAllDto GetById(int id);
        bool Add(VacancyAddDto vacancy);
        bool Update( int id , VacancyGetAllDto vacancy);
        bool Delete(int id);
        void  save();

    }
}
