using Carter;
using Carter.ModelBinding;
using CarterWebApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarterWebApi.Modules
{
    public class ProductsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products");
            group.MapGet("/", () =>
            {
                return Results.Ok(new[]
                {
                    new { Id = 1, Name = "Laptop", Price = 999.99m },
                    new { Id = 2, Name = "Phone", Price = 699.99m }
                });
            });

            group.MapPost("/", (HttpContext ctx, ProductRequest request) =>
            {
                // Carter's built-in validation
                var result = ctx.Request.Validate(request);

                if (!result.IsValid)
                {
                    // Return validation problem details
                    return Results.ValidationProblem(result.ToDictionary());
                }

                return Results.Created($"/api/products/{request.Id}", request);
            });

            //group.MapPost("/", async (IValidator<ProductRequest> validator, ProductRequest request) =>
            //{
            //    var validationResult = await validator.ValidateAsync(request);
            //    if (!validationResult.IsValid)
            //    {
            //        return Results.ValidationProblem(validationResult.ToDictionary());
            //    }

            //    return Results.Created($"/api/products/{request.Id}", request);
            //});

        }
    }
}
