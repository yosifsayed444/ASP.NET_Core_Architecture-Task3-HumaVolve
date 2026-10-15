using ProductsApi.Application.Abstractions;

namespace ProductsApi.Application.Products.Queries;

public record GetProductsQuery : IQuery<List<ProductDto>>;

public class GetProductsQueryHandler(IProductRepository repository)
    : IQueryHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(ProductDto.From).ToList();
    }
}
