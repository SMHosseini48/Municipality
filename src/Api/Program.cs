using Api.Extensions;
using Api.Filters;
using Application.MappingConfigurations;
using Infrastructure;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json", optional: false)
    .Build();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureJwt(configuration);
builder.Services.ServiceInjection(configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureIdentity();
builder.Services.SwaggerConfig();
builder.Services.AddDbContext<ResidueContext>();
builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionHandlingFilter))).AddNewtonsoftJson(op =>
    op.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(typeof(AutoMapperInitializer));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();