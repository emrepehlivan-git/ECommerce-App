using ECommerce.Application.Features.Products;
using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.UnitTests.Features.Products.Commands;

public sealed class CreateProductCommandTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ILazyServiceProvider> _lazyServiceProviderMock;
    private readonly Mock<ILocalizationService> _localizationServiceMock;
    private readonly CreateProductCommandHandler _handler;
    private readonly CreateProductCommandValidator _validator;
    private readonly LocalizationHelper _localizer;

    public CreateProductCommandTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _lazyServiceProviderMock = new Mock<ILazyServiceProvider>();
        _localizationServiceMock = new Mock<ILocalizationService>();

        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.NameMustBeAtLeastCharacters))
            .Returns("Product name must be at least 3 characters long");
        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.NameMustBeLessThanCharacters))
            .Returns("Product name must be less than 100 characters long");
        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.PriceMustBeGreaterThanZero))
            .Returns("Product price must be greater than zero");
        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.CategoryNotFound))
            .Returns("Product category not found");

        _localizer = new LocalizationHelper(_localizationServiceMock.Object);

        _handler = new CreateProductCommandHandler(
            _productRepositoryMock.Object,
            _lazyServiceProviderMock.Object);

        _validator = new CreateProductCommandValidator(
            _categoryRepositoryMock.Object,
            _localizer);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProduct()
    {
        // Arrange
        var command = new CreateProductCommand(
            Name: "Test Product",
            Description: "Test Description",
            Price: 100m,
            CategoryId: Guid.NewGuid(),
            StockQuantity: 10);

        Product? capturedProduct = null;
        _productRepositoryMock
            .Setup(x => x.Add(It.IsAny<Product>()))
            .Callback<Product>(product => capturedProduct = product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(capturedProduct!.Id);

        _productRepositoryMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);

        capturedProduct.Should().NotBeNull();
        capturedProduct!.Name.Should().Be(command.Name);
        capturedProduct.Description.Should().Be(command.Description);
        capturedProduct.Price.Should().Be(command.Price);
        capturedProduct.CategoryId.Should().Be(command.CategoryId);
        capturedProduct.StockQuantity.Should().Be(command.StockQuantity);
    }

    [Theory]
    [InlineData("", "Product name must be at least 3 characters long")]
    [InlineData("AB", "Product name must be at least 3 characters long")]
    public async Task Validate_WithInvalidName_ShouldReturnValidationError(string name, string expectedError)
    {
        // Arrange
        var command = new CreateProductCommand(
            Name: name,
            Description: "Test Description",
            Price: 100m,
            CategoryId: Guid.NewGuid(),
            StockQuantity: 10);

        // Setup category to exist to isolate the name validation
        _categoryRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Verify localization service is called with correct constant
        _localizationServiceMock.Verify(x => x.GetLocalizedString(ProductConsts.NameMustBeAtLeastCharacters), Times.Once);
        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.NameMustBeAtLeastCharacters))
            .Returns(expectedError)
            .Verifiable();

        // Act
        var validationResult = await _validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(expectedError);

    }

    [Fact]
    public async Task Validate_WithInvalidPrice_ShouldReturnValidationError()
    {
        // Arrange
        var command = new CreateProductCommand(
            Name: "Test Product",
            Description: "Test Description",
            Price: 0m,
            CategoryId: Guid.NewGuid(),
            StockQuantity: 10);

        var expectedError = "Product price must be greater than zero";
        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.PriceMustBeGreaterThanZero))
            .Returns(expectedError);

        // Act
        var validationResult = await _validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == expectedError);
    }

    [Fact]
    public async Task Validate_WithNonExistentCategory_ShouldReturnValidationError()
    {
        // Arrange
        var command = new CreateProductCommand(
            Name: "Test Product",
            Description: "Test Description",
            Price: 100m,
            CategoryId: Guid.NewGuid(),
            StockQuantity: 10);

        var expectedError = "Product category not found";
        _localizationServiceMock
            .Setup(x => x.GetLocalizedString(ProductConsts.CategoryNotFound))
            .Returns(expectedError);

        _ = _categoryRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var validationResult = await _validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == expectedError);
    }
}