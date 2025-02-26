using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MnemosyneAPI.Context;
using MnemosyneAPI.Endpoints;
using MnemosyneAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<MemoryValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mnemosyne API",
        Version = "v1",
        Description = "API desenvolvida no curso de Programação com C#, para atender ao Frontend do site Mnemosyne"
    });
});

builder.Services.AddDbContext<MemoryDbContext>(options => options.UseSqlite("Data Source=memories.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mnemosyne API v1");
    });
}

app.MapMemoryEndpoints();

app.Run();
