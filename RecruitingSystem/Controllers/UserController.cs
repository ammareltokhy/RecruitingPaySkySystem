using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitingSystem.DTOs.UserDtos;
using RecruitingSystem.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecruitingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
        }

        #region Register

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(UserDto registerDto)
        {
            //when is employer
            if (registerDto.Is_Employer)
            {
                ApplicationUser employer = new Employer
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    CompanyDesc = registerDto.CompanyDesc,
                    CompanyName = registerDto.CompanyName
                };

                var result = await _userManager.CreateAsync(employer, registerDto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, employer.Id),
                    new Claim(ClaimTypes.Name, employer.UserName),
                    new Claim(ClaimTypes.Role, "Employer"),
                };
                await _userManager.AddClaimsAsync(employer, claims);

                return Ok();
            }
            else// in case it is applicant
            {
                ApplicationUser applicant = new Applicant
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,

                };

                var result = await _userManager.CreateAsync(applicant, registerDto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, applicant.Id),
                    new Claim(ClaimTypes.Name, applicant.UserName),
                    new Claim(ClaimTypes.Role, "Applicant"),
                };
                await _userManager.AddClaimsAsync(applicant, claims);

                return Ok();
            }
        }


        #endregion

        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
        {
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, credentials.Password);
            if (!isAuthenticated)
            {
                return Unauthorized();
            }

            var claimsList = await _userManager.GetClaimsAsync(user);

            #region Secret Key
            var secretKeyString = _config.GetValue<string>("SecretKey");
            var secretyKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString ?? string.Empty);
            var secretKey = new SymmetricSecurityKey(secretyKeyInBytes);
            #endregion

            #region Create a combination of secretKey, Algorithm
            var signingCredentials = new SigningCredentials(secretKey,
                SecurityAlgorithms.HmacSha256Signature);
            #endregion

            #region Putting all together

            var expiryDate = DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(
                claims: claimsList,
                expires: expiryDate,
                signingCredentials: signingCredentials);

            #endregion

            #region Convert Token Object To String

            var tokenHndler = new JwtSecurityTokenHandler();
            var tokenString = tokenHndler.WriteToken(token);

            #endregion

            return new TokenDto(tokenString, expiryDate);
        }

        #endregion
        #region Get All Employeer 
        [HttpGet]
        [Route("GetEmployers")]
        public ActionResult getAllEmployee()
        {
            string targetRole = "Employer";

            var employerUsers = GetUsersByRole(targetRole).Result;
            if (employerUsers.Count() > 0)
            {
                return Ok(employerUsers.Where(n => n.IsDeleted == false).ToList());
            }
                
            
            return NotFound();


        }
        [HttpGet("GetApplicants")]
        public ActionResult GetApplicants()
        {
            string targetRole = "Applicant";


            var applicantUsers = GetUsersByRole(targetRole).Result;

            if(applicantUsers.Count() > 0)
            {
                return Ok(applicantUsers.Where(a=>a.IsDeleted== false).ToList());
            }
            return BadRequest();




        }
        [HttpGet]
        [Route("getApplicant/{id}")]
        public ActionResult GetApplicantById (string id)
        {
            string targetRole = "Applicant";

            var user = GetUserByRole(targetRole, id).Result;


            if (user.IsDeleted == false)
            {
                Console.WriteLine($"User: {user.Email} (Is Deleted: {user.IsDeleted})");
                return Ok(user);

            }
            return NotFound();



        }
        [HttpGet]
        [Route("getEmployer/{id}")]
        public ActionResult GetEmployerById(string id)
        {
            string targetRole = "Employer";

            var user = GetUserByRole(targetRole, id).Result;


            if (user.IsDeleted == false)
            {
                Console.WriteLine($"User: {user.Email} (Is Deleted: {user.IsDeleted})");
                return Ok(user);

            }
            return NotFound();



        }
        private async Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role)
        {
            var users = await _userManager.Users.ToListAsync();

            // Filter users by role claim
            var filteredUsers = users.Where(user =>
            {
                var userClaims = _userManager.GetClaimsAsync(user).Result;
                return userClaims.Any(claim => claim.Type == ClaimTypes.Role && claim.Value == role);
            });

            return filteredUsers;
        }
        private async Task<ApplicationUser> GetUserByRole(string role, string id)
        {
            var users = await _userManager.Users.Where(n => n.Id == id).ToListAsync();

            // Filter users by role claim
            var filteredUser = users.FirstOrDefault(user =>
            {
                var userClaims = _userManager.GetClaimsAsync(user).Result;
                return userClaims.Any(claim => claim.Type == ClaimTypes.Role && claim.Value == role);
            });

            return filteredUser;
        }

        #endregion
        #region Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new { message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
