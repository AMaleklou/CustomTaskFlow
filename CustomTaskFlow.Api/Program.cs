using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           options.InvalidModelStateResponseFactory = context =>
           {
               // جمع‌آوری تمام خطاها
               var errors = context.ModelState
                   .Where(x => x.Value?.Errors.Count > 0)
                   .SelectMany(x => x.Value!.Errors)
                   .Select(x => x.ErrorMessage)
                   .ToList();

               var response = ApiResponse<object>.ErrorResponse(
                   errors: errors,
                   message: "One or more validation errors occurred"
               );

               return new BadRequestObjectResult(response);
           };
       });

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

