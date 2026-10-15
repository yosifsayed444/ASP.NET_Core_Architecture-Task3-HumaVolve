using FluentValidation;
using ProductsApi.Application.Abstractions;
using ProductsApi.Domain.Entities;

namespace ProductsApi.Application.Products.Commands;

public record CreateProductCommand(string Name, decimal Price) : ICommand<ProductDto>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

public class CreateProductCommandHandler(IProductRepository repository)
    : ICommandHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(command.Name.Trim(), command.Price);
        await repository.AddAsync(product, cancellationToken);
        return ProductDto.From(product);
    }
}
