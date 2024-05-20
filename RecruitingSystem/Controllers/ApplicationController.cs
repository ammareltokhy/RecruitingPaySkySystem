using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecruitingSystem.DTOs.ApplicationDtos;
using RecruitingSystem.Interfaces.ApplicationRepo;

namespace RecruitingSystem.Controllers
{
    [Route("api/[controller]")]
  //  [Authorize]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        ApplicationRepository applicationRepository;

        public ApplicationController(ApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List <ApplicationGetAllDto> result = applicationRepository.GetAll();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            ApplicationGetAllDto result = applicationRepository.GetById(id);
            if(result!=null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task <IActionResult> Add(ApplicationAddDto Appdto) 
        {
            if (Appdto!=null)
            {
                bool result = await applicationRepository.Add(Appdto);
                if (result)
                {
                    return Ok("application added successfully");
                }              
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public IActionResult Update (int id , ApplicationGetAllDto applicationUpdateDto)
        {
            if (applicationUpdateDto != null)
            {
                var result = applicationRepository.Update(id, applicationUpdateDto);
                if(result != null)
                {
                    return Ok("updated successfully");
                }
            }
            return BadRequest();
        }
        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            if(id > 0)
            {
                var result = applicationRepository.Delete(id);
                return Ok("deleted");
            }
            return BadRequest();
        }

    }
}
