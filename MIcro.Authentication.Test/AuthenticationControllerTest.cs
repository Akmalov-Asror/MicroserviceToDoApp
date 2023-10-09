using System.Net;
using FluentAssertions;
using Micro.Authentication.Controllers;
using Micro.Authentication.Request;
using Micro.Authentication.Services;
using Micro.Domain.Data;
using Micro.Domain.Entities;
using Micro.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace MIcro.Authentication.Test;

public class AuthenticationControllerTest
{
    private readonly Mock<IAuthenticationRepository> _mockAuthRepo;
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly Mock<EmailProducer> _mockEmailProducer;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTest()
    {
        _mockAuthRepo = new Mock<IAuthenticationRepository>();
        _mockDbContext = new Mock<AppDbContext>();
        _mockEmailProducer = new Mock<EmailProducer>();
        _controller = new AuthenticationController(_mockAuthRepo.Object, _mockDbContext.Object, _mockEmailProducer.Object);
    }

    [Fact]
    public async Task Register_ValidUser_ReturnsOk()
    {
        // Arrange
        var validUserRequest = new UserRequest { /* Provide valid user data here */ };
        var expectedUser = new User
        {
            Id = 3, // Set the user ID
            Name = "john_doe", // Set the username
            Email = "john@example.com",
            Password = "q32332"
        };
        _mockAuthRepo
            .Setup(repo => repo.Login(It.IsAny<User>()))
            .ReturnsAsync("dad");

        using var httpClient = new HttpClient();

        var result = await _controller.Register(validUserRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

}