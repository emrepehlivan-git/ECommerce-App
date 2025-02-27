using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands;

public sealed record UpdateCategoryCommand(Guid Id, string Name) : IRequest<Result>, IValidateRequest, ITransactionalRequest;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly CategoryBusinessRules _categoryBusinessRules;
    private readonly ICategoryRepository _categoryRepository;
    public UpdateCategoryCommandValidator(CategoryBusinessRules categoryBusinessRules, ICategoryRepository categoryRepository, LocalizationHelper localizer)
    {
        _categoryBusinessRules = categoryBusinessRules;
        _categoryRepository = categoryRepository;
        RuleFor(x => x.Id)
            .MustAsync(async (id, ct) => await _categoryRepository.AnyAsync(x => x.Id == id, ct))
            .WithMessage(localizer[CategoryConsts.NotFound]);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[CategoryConsts.NameIsRequired])
            .MinimumLength(CategoryConsts.NameMinLength)
            .WithMessage(localizer[CategoryConsts.NameMustBeAtLeastCharacters])
            .MaximumLength(CategoryConsts.NameMaxLength)
            .WithMessage(localizer[CategoryConsts.NameMustBeLessThanCharacters])
            .MustAsync(async (command, name, ct) =>
                !await _categoryBusinessRules.CheckIfCategoryExistsAsync(name, command.Id, ct))
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