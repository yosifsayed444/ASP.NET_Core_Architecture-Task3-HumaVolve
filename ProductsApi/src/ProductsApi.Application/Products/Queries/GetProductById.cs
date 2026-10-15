using ProductsApi.Application.Abstractions;

namespace ProductsApi.Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto?>;

public class GetProductByIdQueryHandler(IProductRepository repository)
    : IQueryHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(query.Id, cancellationToken);
        return product is null ? null : ProductDto.From(product);
    }
}
