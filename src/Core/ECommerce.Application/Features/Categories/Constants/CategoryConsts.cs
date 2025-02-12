namespace ECommerce.Application.Features.Categories.Constants;

public static class CategoryConsts
{
    public const string NameExists = "Category.Name.Exists";
    public const string NotFound = "Category.NotFound";
    public const string NameIsRequired = "Category.Name.IsRequired";
    public const string NameMustBeAtLeast3Characters = "Category.Name.MustBeAtLeast3Characters";
    public const string NameMustBeLessThan100Characters = "Category.Name.MustBeLessThan100Characters";

    public const int NameMinLength = 3;
    public const int NameMaxLength = 100;
}

