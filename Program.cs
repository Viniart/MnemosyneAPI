using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MnemosyneAPI.Context;
using MnemosyneAPI.Endpoints;
using MnemosyneAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddHealthChecks();

var MinhasOrigens = "_minhasOrigens";

builder.Services.AddCors(
    options => {
        options.AddPolicy(
            name: MinhasOrigens,
            policy => {
                policy.WithOrigins("http://localhost:5173");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            }
        );
    }
);

builder.Services.AddValidatorsFromAssemblyContaining<MemoryValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mnemosyne API",
        Version = "v1",
        Description = "API desenvolvida no curso de Programa��o com C#, para atender ao Frontend do site Mnemosyne"
    });
});

builder.Services.AddDbContext<MemoryDbContext>(options => options.UseSqlite("Data Source=memories.db"));

var app = builder.Build();

app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mnemosyne API v1");
    });
}

app.MapMemoryEndpoints();

app.UseCors(MinhasOrigens);

app.Run();
