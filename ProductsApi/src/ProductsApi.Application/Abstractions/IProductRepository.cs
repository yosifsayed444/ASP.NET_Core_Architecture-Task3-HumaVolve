using ProductsApi.Domain.Entities;

namespace ProductsApi.Application.Abstractions;

// الـ Interface هنا في Application، والتنفيذ في Infrastructure
public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
}
