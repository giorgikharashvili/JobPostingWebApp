using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.ViewModels;
using DevSpot.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{

    [Authorize]
    public class JobPostingsController : Controller
    {
        private readonly IRepsoitory<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(
            IRepsoitory<JobPosting> repsoitory,
            UserManager<IdentityUser> userManager)
        {
            _repository = repsoitory;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var JobPostings = await _repository.GetAllAsync();

            if(User.IsInRole(Roles.Employer))
            {
                var allJobPostings = await _repository.GetAllAsync();

                var userId = _userManager.GetUserId(User);

                var filteredJobPostings = allJobPostings.Where(jp => jp.UserId == userId);

                return View(filteredJobPostings);

                
            }
                

            return View(JobPostings);
        }

        [Authorize(Roles ="Admin,Employer")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {
            if(ModelState.IsValid)
            {
                var jobPosting = new JobPosting
                {
                    Title = jobPostingVm.Title,
                    Description = jobPostingVm.Description,
                    Company = jobPostingVm.Company,
                    Location = jobPostingVm.Location,
                    UserId = _userManager.GetUserId(User),
                    
                };

                await _repository.AddAsync(jobPosting);
                return RedirectToAction(nameof(Index)); // es nameof(Index) igivea rac "index" chawero ubralod hard coded arari kargi amitomac viyeneb nameof's
            }
            return View(jobPostingVm);
        }

        // JobPosting/Delete/7
        [HttpDelete]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _repository.GetByIdAsync(id);

            if(jobPosting == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if(User.IsInRole(Roles.Admin) == false && jobPosting.UserId != userId)
            {
                return Forbid();
            }

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
