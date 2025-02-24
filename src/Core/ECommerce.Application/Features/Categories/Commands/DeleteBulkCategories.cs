using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Categories.Commands;

public sealed record DeleteBulkCategoriesCommand(List<Guid> Ids) : IRequest<Result>, ITransactionalRequest;

public class DeleteBulkCategoriesCommandHandler(
    ICategoryRepository categoryRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<DeleteBulkCategoriesCommand, Result>(lazyServiceProvider)
{
    public override async Task<Result> Handle(DeleteBulkCategoriesCommand command, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.Query().Where(x => command.Ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (categories.Count == 0)
            return Result.NotFound();

        categoryRepository.DeleteRange(categories);
        return Result.Success();
    }
}
