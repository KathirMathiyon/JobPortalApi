using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        public static List<Job> jobs = new List<Job>
        {
            new Job { Id = 1, Title = "Software Engineer", Company = "Google", Location = "Bangalore", PostedDate = DateTime.Now },
            new Job { Id = 2, Title = "Backend Engineer", Company = "ZOHO", Location = "Chennai", PostedDate = DateTime.Now },
            new Job { Id = 3, Title = "Full Stack Engineer", Company = "Freshworks", Location = "Chennai", PostedDate = DateTime.Now }
        };

        [HttpGet]
        public IActionResult GetAllJobs()
        {
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public IActionResult GetJobsById(int id)
        {
            var job = jobs.FirstOrDefault(j => j.Id == id);
                        if (job == null)
            {
                return NotFound($"Job with ID {id} not found.");
            }
            return Ok(job);
        }
}}
