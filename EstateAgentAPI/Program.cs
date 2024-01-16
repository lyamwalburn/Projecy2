using EstateAgentAPI.Business.Services;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication;

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();  
builder.Services.AddScoped<IBookingRepository, BookingRepository>(); 
builder.Services.AddScoped<ISellerService,  SellerService>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<EstateAgentContext>(options =>
                                options.UseSqlServer(
                                    builder.Configuration.GetConnectionString("Estates")));

//CORS
builder.Services.AddCors(options =>
        options.AddPolicy(name: myAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("https://localhost:3000", "http://localhost:3000")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                          })
);

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
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
