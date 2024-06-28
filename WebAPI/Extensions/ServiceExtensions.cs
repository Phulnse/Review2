using Application.IServices;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;
using WebAPI.Services;
using WebAPI.Services.JobServices;
using WebAPI.Validations;

namespace WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMemberReviewService, MemberReviewService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IContractTypeService, ContractTypeService>();
            services.AddScoped<IFileTypeService, FileTypeService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IRemunerationService, RemunerationService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<INationService, NationService>();
            services.AddScoped<INotifyService, NotifyService>();
            services.AddValidatorsFromAssemblyContaining<IAssemblyMaker>();

            return services;

        }

        public static IServiceCollection OpenCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("open", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            //string mySqlConnectionString = configuration.GetConnectionString("MySqlConnectionString")!;
            //services.AddDbContext<SRMSContext>(options =>
            //    options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));

            string sqlServerConnectionString = configuration.GetConnectionString("SqlServerConnectionString")!;
            services.AddDbContext<SRMSContext>(options =>
                options.UseSqlServer(sqlServerConnectionString));
            return services;
        }

        public static IApplicationBuilder DatabaseMigrate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SRMSContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }

        public static IServiceCollection MakeDefaultDecisionOverdueJobConfigure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var jobKey = new JobKey("MakeDefaultDecisionOverdue");
                options.AddJob<MakeDefaultDecisionOverdueJob>(opts => opts.WithIdentity(jobKey));

                options.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("MakeDefaultDecisionOverdue-trigger")
                    .WithCronSchedule("0/10 * * ? * *")
                );
            });

            services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });
            return services;
        }

        public static IServiceCollection SumarizeResultJobConfigure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var jobKey = new JobKey("SumarizeResultJob");
                options.AddJob<SumarizeResultJob>(opts => opts.WithIdentity(jobKey));

                options.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("SumarizeResultJob-trigger")
                    .WithCronSchedule("0/20 * * ? * *")
                );
            });

            services.AddQuartzHostedService(opt =>
            {
                opt .WaitForJobsToComplete = true;
            });
            return services;
        }

        public static IServiceCollection SendNotifyEmailJobConfigure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var jobKey = new JobKey("SendNotifyEmailJob");
                options.AddJob<SendNotifyEmailJob>(opts => opts.WithIdentity(jobKey));

                options.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("SendNotifyEmailJob-trigger")
                    .WithCronSchedule("0/5 * * ? * *")
                );
            });

            services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });
            return services;
        }

        public static IServiceCollection AddAuthenticationConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration.GetValue<string>("JWTSettings:Issuer"),
                        ValidAudience = configuration.GetValue<string>("JWTSettings:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTSettings:SecretKey")!)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization();
            return services;
        }
    }
}
