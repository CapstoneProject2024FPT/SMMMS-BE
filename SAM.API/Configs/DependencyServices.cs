using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using SAM.DataTier.Repository.Implement;
using Microsoft.EntityFrameworkCore;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Utils;
namespace SAM.API.Configs
{

    public static class DependencyServices
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<SamContext>, UnitOfWork<SamContext>>();
            return services;
        }       

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SamContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SQLServerDatabase"));
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMachineryService, MachineryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IWarrantyService, WarrantyService>();
            services.AddScoped<IWarrantyDetailService, WarrantyDetailService>();
            services.AddScoped<IRankService, RankService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IMachineryComponentService, MachineryComponentService>();
            services.AddScoped<IOriginService, OriginService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<INewsCategoryService, NewsCategoryService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IWardService, WardService>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = JsonUtil.GetFromAppSettings("Jwt:Issuer"),
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonUtil.GetFromAppSettings("Jwt:SecretKey")))
                    };
                });
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "SMMMS", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                    });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
            });
            return services;
        }
    }
}