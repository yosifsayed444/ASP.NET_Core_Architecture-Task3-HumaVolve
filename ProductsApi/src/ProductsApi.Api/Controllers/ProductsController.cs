using Microsoft.AspNetCore.Mvc;
using ProductsApi.Api.Contracts;
using ProductsApi.Application.Abstractions;
using ProductsApi.Application.Products;
using ProductsApi.Application.Products.Commands;
using ProductsApi.Application.Products.Queries;

namespace ProductsApi.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll(CancellationToken ct)
        => Ok(await dispatcher.Query(new GetProductsQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
    {
        var product = await dispatcher.Query(new GetProductByIdQuery(id), ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request, CancellationToken ct)
    {
        var product = await dispatcher.Send(new CreateProductCommand(request.Name, request.Price), ct);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
