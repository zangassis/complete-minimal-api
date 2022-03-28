using CompleteMinimalAPI.Data;
using CompleteMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);
const string ErrorMessage = "There was an error saving the record";
const string ProviderTag = "Provider";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MinimalContextDb>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/providers", async (MinimalContextDb context) =>
    await context.Providers.ToListAsync())
    .WithName("FindProviders")
    .WithTags(ProviderTag);

app.MapGet("/providers/{id}", async (Guid id, MinimalContextDb context) =>
    await context.Providers.FindAsync(id)
         is Provider provider ? Results.Ok(provider) : Results.NotFound())
        .Produces(StatusCodes.Status200OK)
        .WithName("FindProviderById")
        .WithTags(ProviderTag);

app.MapPost("/providers", async (MinimalContextDb context, Provider provider) =>
{
    if (!MiniValidator.TryValidate(provider, out var errors))
        return Results.ValidationProblem(errors);

    context.Providers.Add(provider);
    var result = await context.SaveChangesAsync();

    return result > 0
    //? Results.Created($"/provider/{provider.Id}", provider)
    ? Results.CreatedAtRoute("FindProviderById", new { id = provider.Id }, provider)
    : Results.BadRequest(ErrorMessage);
})
.Produces<Provider>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.WithName("CreateProvider")
.WithTags(ProviderTag);

app.MapPut("/providers/{id}", async (Guid id, MinimalContextDb context, Provider provider) =>
{
    var dbProvider = await context.Providers.FindAsync(id);

    if (dbProvider is null)
        return Results.NotFound();

    if (!MiniValidator.TryValidate(provider, out var errors))
        return Results.ValidationProblem(errors);

    context.Entry(dbProvider).CurrentValues.SetValues(provider);

    var result = await context.SaveChangesAsync();

    return result >= 0 ? Results.NoContent() : Results.BadRequest(ErrorMessage);
})
.ProducesValidationProblem()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.WithName("PutProvider")
.WithTags(ProviderTag);

app.MapDelete("/providers/{id}", async (Guid id, MinimalContextDb context) =>
{
    var provider = await context.Providers.FindAsync(id);

    if (provider is null)
        return Results.NotFound();

    context.Providers.Remove(provider);
    var result = await context.SaveChangesAsync();

    return result > 0 ? Results.NoContent() : Results.BadRequest(ErrorMessage);

})
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.WithName("DeleteProvider")
.WithTags(ProviderTag);

app.Run();