using Microsoft.EntityFrameworkCore;
using ProductsApi.Application.Abstractions;
using ProductsApi.Domain.Entities;

namespace ProductsApi.Infrastructure.Persistence;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
        => db.Products.AsNoTracking().ToListAsync(cancellationToken);
}
