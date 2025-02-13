using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands;

public sealed record UpdateCategoryCommand(Guid Id, string Name) : IRequest<Result>;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository, L localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .MustAsync(async (command, name, ct) =>
                !await categoryRepository.AnyAsync(x => x.Name == name, cancellationToken: ct) ||
                (await categoryRepository.GetByIdAsync(command.Id, cancellationToken: ct))?.Name == name)
            .WithMessage(localizer[CategoryConsts.NameExists]);
    }
}

internal sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<UpdateCategoryCommand, Result>(lazyServiceProvider)
{
    public override async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken: cancellationToken);
        category!.UpdateName(command.Name);

        categoryRepository.Update(category);

        return Result.Success();
    }
}