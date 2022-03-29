using AutoMapper;
using CompleteMinimalAPI.Data;
using CompleteMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

namespace CompleteMinimalAPI.Endpoints;

public class ProviderEndpoints
{
    const string ErrorMessage = "There was an error saving the record";
    const string ProviderTag = "Provider";

    public static void AddEndpoints(WebApplication app)
    {
        app.MapGet("/providers", async (MinimalContextDb context, IMapper mapper) =>
        {
            var mappedProviders = new List<ProviderDto>();

            foreach (var provider in context.Providers)
            {
                var mappedProvider = new ProviderDto()
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    Email = provider.Email,
                    PhoneNumber = provider.PhoneNumber,
                    Active = provider.Active,
                };
                mappedProviders.Add(mappedProvider);
            }
            return mappedProviders;

        }).WithName("FindProviders").WithTags(ProviderTag);

        app.MapGet("/providers/{id}", async (Guid id, MinimalContextDb context) =>
        {
            var provider = context.Providers.FindAsync(id).Result;

            if (provider is Provider validProvider)
            {
                var mappedProvider = new ProviderDto()
                {
                    Id = id,
                    Name = provider.Name,
                    Email = provider.Email,
                    PhoneNumber = provider.PhoneNumber,
                    Active = provider.Active,
                };
                return Results.Ok(mappedProvider);
            }
            else
                return Results.NotFound();
        })
        .Produces(StatusCodes.Status200OK)
        .WithName("FindProviderById")
        .WithTags(ProviderTag);

        app.MapPost("/providers", async (MinimalContextDb context, ProviderDto providerDto, IMapper mapper) =>
        {
            var mappedProvider = mapper.Map<Provider>(providerDto);
            mappedProvider.Id = Guid.NewGuid();
            mappedProvider.CreatedDate = DateTime.UtcNow;
            mappedProvider.LastUpdateDate = DateTime.UtcNow;

            if (!MiniValidator.TryValidate(mappedProvider, out var errors))
                return Results.ValidationProblem(errors);

            context.Providers.Add(mappedProvider);
            var result = await context.SaveChangesAsync();

            return result > 0
            ? Results.CreatedAtRoute("FindProviderById", new { id = mappedProvider.Id }, mappedProvider)
            : Results.BadRequest(ErrorMessage);
        })
        .Produces<Provider>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("CreateProvider")
        .WithTags(ProviderTag);

        app.MapPut("/providers/{id}", async (Guid id, MinimalContextDb context, ProviderDto providerDto, IMapper mapper) =>
        {
            var dbProvider = await context.Providers.FindAsync(id);

            if (dbProvider is null)
                return Results.NotFound();

            var mappedProvider = mapper.Map<Provider>(providerDto);

            mappedProvider.Id = dbProvider.Id;
            mappedProvider.CreatedDate = dbProvider.CreatedDate;
            mappedProvider.LastUpdateDate = DateTime.UtcNow;

            if (!MiniValidator.TryValidate(mappedProvider, out var errors))
                return Results.ValidationProblem(errors);

            context.Entry(dbProvider).CurrentValues.SetValues(mappedProvider);

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
    }
}

