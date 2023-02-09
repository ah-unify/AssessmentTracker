using System.Net;
using System.Net.Http.Json;
using AssessmentTracker.Api.Models.Requests;
using AssessmentTracker.Api.Models.Responses;
using AssessmentTracker.Domain.Entities;
using AssessmentTracker.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AssessmentTracker.Integration.Testing.StudentControllerTests;

public class WhiteBoxTesting : IClassFixture<AssessmentTrackerApplicationFactory>
{
    private readonly AssessmentTrackerApplicationFactory _factory;

    /// <summary>
    /// This will be the running API
    /// </summary>
    private readonly HttpClient _client;

    private const string Endpoint = "/Students/RegisterStudent";

    public WhiteBoxTesting(AssessmentTrackerApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_CreateStudent_Successfully()
    {
        // Given
        var request = new RegisterStudentRequest
        {
            FirstName = "Jerry",
            LastName = "Seinfeld",
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        // When
        var response = await _client.PostAsJsonAsync(Endpoint, request);

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadFromJsonAsync<StudentRegistered>();

        var student = await FindStudent(responseContent.StudentId);
        Assert.Equal(request.FirstName, student.FirstName);
        Assert.Equal(request.LastName, student.LastName);
        Assert.Equal(request.DateOfBirth.ToUniversalTime(), student.DateOfBirth);
    }

    [Fact]
    public async Task Should_Fail_IfFirstNameIsEmpty()
    {
        // Given
        var request = new RegisterStudentRequest
        {
            FirstName = "  ",
            LastName = "Seinfeld",
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        // When
        var response = await _client.PostAsJsonAsync(Endpoint, request);

        // Then
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Assert.Equal($"{nameof(request.FirstName)} must be provided.", responseContent);
    }

    private async Task<Student?> FindStudent(Guid studentId)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var student = await context!.Students.FirstOrDefaultAsync(x => x.Id == studentId);
        return student;
    }
}