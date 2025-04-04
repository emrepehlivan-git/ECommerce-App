using System;

namespace ECommerce.Application.UnitTests.Features.Products.Queries;

public abstract class ProductQueriesTestsBase
{
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<ICategoryRepository> CategoryRepositoryMock;
    protected readonly Mock<ILazyServiceProvider> LazyServiceProviderMock;
    protected readonly Mock<ILocalizationService> LocalizationServiceMock;
    protected readonly Product DefaultProduct;
    protected readonly Category DefaultCategory;


    protected ProductQueriesTestsBase()
    {
        ProductRepositoryMock = new Mock<IProductRepository>();
        CategoryRepositoryMock = new Mock<ICategoryRepository>();
        LazyServiceProviderMock = new Mock<ILazyServiceProvider>();
        LocalizationServiceMock = new Mock<ILocalizationService>();

        DefaultProduct = Product.Create("Test Product", "Test Description", 100m, Guid.NewGuid());
        DefaultCategory = Category.Create("Test Category");
    }

    protected void SetupProductExists(bool exists = true)
    {
        ProductRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(),
            It.IsAny<Expression<Func<IQueryable<Product>, IQueryable<Product>>>?>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists ? DefaultProduct : null);
    }

    protected void SetupCategoryExists(bool exists = true)
    {
        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(),
            It.IsAny<Expression<Func<IQueryable<Category>, IQueryable<Category>>>?>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists ? DefaultCategory : null);
    }
}