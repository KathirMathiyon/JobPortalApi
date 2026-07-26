using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;
using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobPortal.Tests
{
    public class JobsControllerTests
    {
        private JobContext JobContext()
        {
            var options = new DbContextOptionsBuilder<JobContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new JobContext(options);

            //seed test data
            context.Jobs.AddRange(
                new Job { Id = 1, Title = "Senior .NET developer", Company = "ZOHO", Location = "Chennai", PostedDate = DateTime.Now },
                new Job { Id = 2, Title = "React developer", Company = "FreshWorks", Location = "Bangalore", PostedDate = DateTime.Now }
                );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetJobById_ValidId_ReturnsOk()
        {
            var context = JobContext();

            var controller = new JobsController(context);

            var result = await controller.GetJobsById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetJobById_InValidId_ReturnsNFO()
        {
            var context = JobContext();

            var controller = new JobsController(context);

            var result = await controller.GetJobsById(999);

            var nFOResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(nFOResult.Value);
        }

        [Fact]
        public async Task GetAllJobs_NoFilters_OkResult()
        {
            var context = JobContext();

            var controller = new JobsController(context);

            var result = await controller.GetAllJobs(null,null);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }
    }
}
