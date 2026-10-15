using ProductsApi.Domain.Entities;

namespace ProductsApi.Application.Products;

public record ProductDto(Guid Id, string Name, decimal Price)
{
    public static ProductDto From(Product product) => new(product.Id, product.Name, product.Price);
}
