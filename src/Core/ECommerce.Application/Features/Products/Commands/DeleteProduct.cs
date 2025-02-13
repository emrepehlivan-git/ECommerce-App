using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result>;

internal sealed class DeleteProductCommandHandler : BaseHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        _productRepository = productRepository;
    }

    public override async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken: cancellationToken);

        if (product is null)
            return Result.NotFound(Localizer[ProductConsts.NotFound]);

        _productRepository.Delete(product);

        return Result.Success();
    }
}