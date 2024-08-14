using SAM.API.Configs;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Converter;
using SAM.BusinessTier.Middlewares;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();


// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsConstant.PolicyName,
        policy =>
        {
            policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddUnitOfWork();
builder.Services.AddOptions();
builder.Services.AddServices();
builder.Services.AddJwtValidation();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfigSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseHttpsRedirection();
app.UseCors(CorsConstant.PolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();