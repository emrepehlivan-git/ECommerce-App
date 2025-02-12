using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Features.Categories.Constants;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<Guid>>;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository, L localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(CategoryConsts.NameMinLength)
            .WithMessage(localizer[CategoryConsts.NameMustBeAtLeast3Characters])
            .MaximumLength(CategoryConsts.NameMaxLength)
            .WithMessage(localizer[CategoryConsts.NameMustBeLessThan100Characters])
            .MustAsync(async (name, ct) =>
                !await categoryRepository.AnyAsync(x => x.Name == name, cancellationToken: ct))
            .WithMessage(localizer[CategoryConsts.NameExists]);
    }
}

internal sealed class CreateCategoryCommandHandler : BaseHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        _categoryRepository = categoryRepository;
    }

    public override Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.Create(command.Name);

        _categoryRepository.Add(category);

        return Task.FromResult(Result.Success(category.Id));
    }
}