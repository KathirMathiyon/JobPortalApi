using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        public IActionResult GetAllJobs()
        {
            var jobs = new[]
            {
                new { Id = 1, Title = "Software Engineer", Company = "Google", Location = "Bangalore"},
                new { Id = 1, Title = "Backend Engineer", Company = "ZOHO", Location = "Chennai"},
                new { Id = 1, Title = "Full Stack Engineer", Company = "Freshworks", Location = "Chennai"},
            };

            return Ok(jobs);
        }
    }
}
