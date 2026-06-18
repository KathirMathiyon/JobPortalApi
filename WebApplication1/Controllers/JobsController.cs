using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobContext _context;

        public JobsController(JobContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobsById(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound($"Job with ID {id} not found.");
            }
            return Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobDTO jobDto)
        {
            var job = new Job
            {
                Title = jobDto.Title,
                Company = jobDto.Company,
                Location = jobDto.Location,
                PostedDate = jobDto.PostedDate
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJobsById), new { id = job.Id }, job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound($"The Job {id} is not found");
            }

            job.Title = updatedJob.Title;
            job.Company = updatedJob.Company;
            job.Location = updatedJob.Location;
            job.PostedDate = updatedJob.PostedDate;

            await _context.SaveChangesAsync();

            return Ok(job);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound($"The Job {id} is not found");
            }
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return Ok($"The job {id} has been deleted.");
        }
    }
}
