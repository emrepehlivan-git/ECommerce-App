using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public sealed record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId) : IRequest<Result<Guid>>, IValidateRequest, ITransactionalRequest;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        LocalizationHelper localizer)
    {
        RuleFor(x => x.Name)
            .MinimumLength(ProductConsts.NameMinLength)
            .WithMessage(localizer[ProductConsts.NameMustBeAtLeastCharacters])
            .MaximumLength(ProductConsts.NameMaxLength)
            .WithMessage(localizer[ProductConsts.NameMustBeLessThanCharacters])
            .MustAsync(async (name, ct) =>
                !await productRepository.AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken: ct))
            .WithMessage(localizer[ProductConsts.NameExists]);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage(localizer[ProductConsts.PriceMustBeGreaterThanZero]);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (id, ct) =>
                await categoryRepository.AnyAsync(x => x.Id == id, cancellationToken: ct))
            .WithMessage(localizer[ProductConsts.CategoryNotFound]);
    }
}

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<CreateProductCommand, Result<Guid>>(lazyServiceProvider)
{
    public override Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            command.Name,
            command.Description,
            command.Price,
            command.CategoryId);

        productRepository.Add(product);

        return Task.FromResult(Result.Success(product.Id));
    }
}