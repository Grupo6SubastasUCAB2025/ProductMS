// ProductTests.cs

using FluentAssertions;
using ProductMS.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        product.State.Should().Be("available");
        product.Images.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetName_WithInvalidValue_ShouldThrowException(string invalidName)
    {
        // Arrange
        var product = new Product();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.Name = invalidName);
    }
}