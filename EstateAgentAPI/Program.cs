using EstateAgentAPI.Buisness.Services;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();  
builder.Services.AddScoped<IBookingRepository, BookingRepository>();   
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<EstateAgentContext>(options =>
                                options.UseSqlServer(
                                    builder.Configuration.GetConnectionString("Estates")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
