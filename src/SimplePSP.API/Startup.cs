using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SimplePSP.API.Models.Request;
using SimplePSP.API.Validators;
using SimplePSP.Application.Services;
using SimplePSP.Domain.Repositories;
using SimplePSP.Infrastructure.Persistence;
using SimplePSP.Infrastructure.Persistence.Repositories;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimplePSP.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SimplePSPContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQLServer")));

            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, allowIntegerValues: false));
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Simple PSP",
                    Description = "Um Payment Service Provider simples",
                    Contact = new OpenApiContact
                    {
                        Name = "André Corrêa",
                    },
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddScoped<IValidator<CreateTransactionRequest>, CreateTransactionRequestValidator>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IPayableRepository, PayableRepository>();

            services.AddScoped<IPaymentsService, PaymentsService>();
            services.AddScoped<IStoreBalanceService, StoreBalanceService>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            RunMigrations(app);
        }

        private static void RunMigrations(WebApplication app)
        {
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SimplePSPContext>();
                context.Database.Migrate();
            }
        }
    }
}