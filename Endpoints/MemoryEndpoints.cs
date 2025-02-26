using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MnemosyneAPI.Context;
using MnemosyneAPI.Model;

namespace MnemosyneAPI.Endpoints
{
    public static class MemoryEndpoints
    {

        public static void MapMemoryEndpoints(this WebApplication app)
        {
            // Listar Todos
            app.MapGet("/memories", async (MemoryDbContext db) =>
            {
                return await db.Memories.ToListAsync();
            });

            app.MapGet("/memories/{id}", async (int id, MemoryDbContext db) =>
            {
                return await db.Memories.FindAsync(id)
                        is Memory memory
                            ? Results.Ok(memory)
                            : Results.NotFound();
            })
                .Produces<Memory>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);


            app.MapPost("/memories", async (Memory memory, IValidator<Memory> validator,  MemoryDbContext db) =>
            {
                if (memory == null) return Results.BadRequest("Requisição Inválida");

                var validation = await validator.ValidateAsync(memory);

                if(!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

                db.Memories.Add(memory);
                await db.SaveChangesAsync();

                return Results.Created($"memories/{memory.Id}", memory);
            })
                .Produces<Memory>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);



            app.MapPut("/memories/{id}", async (int id, Memory memory, IValidator<Memory> validator, MemoryDbContext db) =>
            {
                var foundMemory = await db.Memories.FindAsync(id);

                if (foundMemory is null) return Results.NotFound();

                var validation = await validator.ValidateAsync(memory);

                if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

                foundMemory.Title = memory.Title;
                foundMemory.Date = memory.Date;
                foundMemory.Description = memory.Description;
                foundMemory.Images = memory.Images;

                await db.SaveChangesAsync();

                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);


            app.MapDelete("/memories/{id}", async (int id, MemoryDbContext db) =>
            {
                var foundMemory = await db.Memories.FindAsync(id);

                if (foundMemory is null) return Results.NotFound();

                db.Memories.Remove(foundMemory);
                await db.SaveChangesAsync();
                return Results.Ok(foundMemory);
            })
                .Produces<Memory>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

        }

    }
}
