namespace ECommerce.Application.Features.Products;

public static class ProductConsts
{
    public const string NameExists = "Product.Name.Exists";
    public const string NotFound = "Product.NotFound";
    public const string NameIsRequired = "Product.Name.IsRequired";
    public const string NameMustBeAtLeastCharacters = "Product.Name.MustBeAtLeast3Characters";
    public const string NameMustBeLessThanCharacters = "Product.Name.MustBeLessThan100Characters";
    public const string PriceMustBeGreaterThanZero = "Product.Price.MustBeGreaterThanZero";
    public const string CategoryNotFound = "Product.Category.NotFound";

    public const int NameMinLength = 3;
    public const int NameMaxLength = 100;
}