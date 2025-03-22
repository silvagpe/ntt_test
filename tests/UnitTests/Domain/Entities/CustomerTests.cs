using DeveloperStore.Domain.Entities;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class CustomerTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var customer = new Customer("FirstName", "LastName", "email@example.com", "123456789");

        Assert.Equal("FirstName", customer.FirstName);
        Assert.Equal("LastName", customer.LastName);
        Assert.Equal("email@example.com", customer.Email);
        Assert.Equal("123456789", customer.Phone);
        Assert.Equal("FirstName LastName", customer.FullName);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var customer = new Customer("FirstName", "LastName", "email@example.com", "123456789");

        customer.UpdateDetails("NewFirstName", "NewLastName", "newemail@example.com", "987654321");

        Assert.Equal("NewFirstName", customer.FirstName);
        Assert.Equal("NewLastName", customer.LastName);
        Assert.Equal("newemail@example.com", customer.Email);
        Assert.Equal("987654321", customer.Phone);
        Assert.Equal("NewFirstName NewLastName", customer.FullName);
    }
}
