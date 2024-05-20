using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecruitingSystem.DTOs.VacancyDtos;
using RecruitingSystem.Interfaces.Vacancyrepo;

namespace RecruitingSystem.Controllers
{
    [Route("api/[controller]")]
     [Authorize(Roles = "Employer")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        VacancyRepository vacancyRepository;

        public VacancyController (VacancyRepository _vacancyRepository)
        {
            this.vacancyRepository = _vacancyRepository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<VacancyGetAllDto> result = new List<VacancyGetAllDto>();
            result=vacancyRepository.GetAll();
            if(result.Count > 0)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            VacancyGetAllDto result = vacancyRepository.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpPost]
        []
        public IActionResult Add(VacancyAddDto vacancydto)
        {
            if(vacancydto != null)
            {
               bool result =  vacancyRepository.Add(vacancydto);
                if (result)
                {
                    return Ok("successfully added");
                }
            }
            return BadRequest();
        }
        [HttpPut("id")]
        public IActionResult update (int id , VacancyGetAllDto vacancyUpdatedto)
        {
            if (vacancyUpdatedto != null)
            {
                var result = vacancyRepository.Update(id, vacancyUpdatedto);
                if(result)
                {
                    return Ok(" successfully updated ");
                }
            }
            return BadRequest();
        }
        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            if(id>0)
            {
               var result =  vacancyRepository.Delete(id);
                return Ok("successfully deleted");

            }
            return BadRequest();
        }
    }
}
