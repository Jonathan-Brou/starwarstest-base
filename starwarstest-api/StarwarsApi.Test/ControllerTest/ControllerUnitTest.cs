using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Domain.Models;
using Domain.Interfaces;
using Services;
using starwarstest_api.controllers;

namespace StarwarsApi.Test;

public class CharactersControllerTests
    {
       [Fact]
public async Task GetCharacters_ReturnsOkResult()
{
    // Arrange
    var mockStarWarsService = new Mock<IStarWarsService>();
    mockStarWarsService.Setup(service => service.GetAllAndSortCharactersAsync())
        .ReturnsAsync(GetSampleCharacters()); // Replace with your sample characters data

    var controller = new StarWarsController(mockStarWarsService.Object);

    // Act
    var result = await controller.GetCharacters();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.NotNull(okResult);
    var model = Assert.IsAssignableFrom<IEnumerable<Character>>(okResult.Value);
    Assert.Equal(2, model.Count());
}

[Fact]
public async Task GetCharacters_ReturnsInternalServerError_WhenServiceThrowsException()
{
    // Arrange
    var mockStarWarsService = new Mock<IStarWarsService>();
    mockStarWarsService.Setup(service => service.GetAllAndSortCharactersAsync())
        .ThrowsAsync(new Exception("Test exception"));

    var controller = new StarWarsController(mockStarWarsService.Object);

    // Act
    var result = await controller.GetCharacters();

    // Assert
    var objectResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(500, objectResult.StatusCode);
}
        // Helper method to generate sample characters data
        private List<Character> GetSampleCharacters()
        {
            return new List<Character>
            {
                new Character { Name = "Luke Skywalker" },
                new Character { Name = "Darth Vader" }
            };
        }
    }
