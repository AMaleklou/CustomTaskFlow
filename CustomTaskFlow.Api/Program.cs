using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.DTOs;
using CustomTaskFlow.Api.Mappings;
using CustomTaskFlow.Api.Middlewares;
using CustomTaskFlow.Api.Models;
using CustomTaskFlow.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CustomTaskFlow.Api.Repositories;

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


//Services Registration
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddAutoMapper(typeof(TaskProfile));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();

