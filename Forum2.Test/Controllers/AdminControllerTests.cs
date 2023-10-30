using Forum2.Controllers;
using Forum2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Forum2.Test.Controllers;

public class AdminControllerTests
{
    [Fact]
    public async Task TestCreateRoleOk()
    {
        // Arrange
        var mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
            Mock.Of<IRoleStore<ApplicationRole>>(), 
            null, null, null, null);
        
        var newRole = new ApplicationRole
        {
            Name = "TestRole", 
            Color = "#000000"
        };
        
        mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
            .ReturnsAsync(IdentityResult.Success);
        var controller = new AdminController(mockRoleManager.Object, null, null,null,null);
        
        
        // Act
        var result = await controller.NewRole(newRole);
        
        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Roles", redirectToActionResult.ActionName);
    }

    [Fact]
    public async Task TestCreateRoleNotOk()
    {
        // Arrange
        var mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
            Mock.Of<IRoleStore<ApplicationRole>>(), 
            null, null, null, null);

        var newRole = new ApplicationRole();
        
        mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
            .ReturnsAsync(IdentityResult.Failed());
        var controller = new AdminController(mockRoleManager.Object, null, null, null, null);
        
        // Act
        var result = await controller.NewRole(newRole);
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(newRole, viewResult.Model);
    }
}