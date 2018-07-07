using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refactored.Logic;
using Refactored.Models;
using Refactored.Utilities;

namespace Refactored
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
	    public void ConfigureServices(IServiceCollection services)
	    {
		    services.AddMvc();

	        var dbPath = Path.Combine(_hostingEnvironment.ContentRootPath, "app_data", "Database.mdf");

	        var connection =
	            $@"Server=(localdb)\mssqllocaldb;AttachDbFilename={dbPath};Trusted_Connection=True;ConnectRetryCount=0";

	        services.AddDbContext<ProductsContext>(options => options.UseSqlServer(connection));

			services.AddTransient<IProductRepository, ProductRepository>();

			//handle model validation failures
			//this sends a custom error result when the model validation fails
			//this is applied to all the controllers and is a new feature that comes with ApiController in core 2.1 
			services.Configure<ApiBehaviorOptions>(options =>
		    {
			    options.InvalidModelStateResponseFactory = actionContext =>
			    {
				    var errors = actionContext.ModelState
					    .Where(e => e.Value.Errors.Count > 0)
					    .Select(e => new InvalidField
					    {
						    FieldName = e.Key,
						    Errors = e.Value.Errors.Select(error => error.ErrorMessage).ToList()
					    }).ToList();


				    return new BadRequestObjectResult(new ErrorResult
				    {
					    ErrorCode = 4000,
					    Message = "One or more fields have invalid values. See details for more information",
					    Details = errors
				    });
			    };
		    });
	    }

	    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
