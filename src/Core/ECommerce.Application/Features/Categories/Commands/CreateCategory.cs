using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<Guid>>, IValidateRequest, ITransactionalRequest;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly CategoryBusinessRules _categoryBusinessRules;
    private readonly LocalizationHelper _localizer;
    public CreateCategoryCommandValidator(CategoryBusinessRules categoryBusinessRules, LocalizationHelper localizer)
    {
        _categoryBusinessRules = categoryBusinessRules;
        _localizer = localizer;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_localizer[CategoryConsts.NameIsRequired])
            .MinimumLength(CategoryConsts.NameMinLength)
            .WithMessage(_localizer[CategoryConsts.NameMustBeAtLeastCharacters])
            .MaximumLength(CategoryConsts.NameMaxLength)
            .WithMessage(_localizer[CategoryConsts.NameMustBeLessThanCharacters])
            .MustAsync(async (name, ct) =>
                !await _categoryBusinessRules.CheckIfCategoryExistsAsync(name, cancellationToken: ct))
            .WithMessage(_localizer[CategoryConsts.NameExists]);
    }
}

public sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ILazyServiceProvider lazyServiceProvider) :
    BaseHandler<CreateCategoryCommand, Result<Guid>>(lazyServiceProvider)
{
    public override Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.Create(command.Name);

        categoryRepository.Add(category);

        return Task.FromResult(Result.Success(category.Id));
    }
}