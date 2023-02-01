using System.Net;
using System.Net.Http.Json;
using AssessmentTracker.Api.Models;
using AssessmentTracker.Api.Models.Requests;
using AssessmentTracker.Api.Models.Responses;
using AssessmentTracker.Domain.Entities;
using AssessmentTracker.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AssessmentTracker.Integration.Testing.FlowExample;

public class StudentAndCourseRegisterFlow : IClassFixture<AssessmentTrackerApplicationFactory>
{
    private readonly AssessmentTrackerApplicationFactory _factory;

    /// <summary>
    /// This will be the running API
    /// </summary>
    private readonly HttpClient _client;

    private const string RegisterStudent = "/Students/RegisterStudent";
    private const string RegisterCourse = "/Courses/RegisterCourse";
    private const string RegisterStudentForCourse = "/Students/RegisterStudentForCourse";
    private const string GetStudent = "/Students/GetStudent";

    public StudentAndCourseRegisterFlow(AssessmentTrackerApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Covers the flow of registering a student, course, and then linking the two.
    /// </summary>
    [Fact]
    public async Task Should_CreateStudent_AndCreateCourse_AndRegisterStudentForCourse()
    {
        // When
        var firstName = "Jerry";
        var lastName = "Seinfeld";  
        var registerStudentResponse = await _client.PostAsJsonAsync(RegisterStudent, new RegisterStudentRequest
        {
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = new DateTime(2000, 1, 1)
        });

        // Then
        Assert.Equal(HttpStatusCode.OK, registerStudentResponse.StatusCode);
        var registerStudentResponseContent =
            await registerStudentResponse.Content.ReadFromJsonAsync<StudentRegistered>();
        var studentId = registerStudentResponseContent.StudentId;

        // When
        var registerCourseResponse = await _client.PostAsJsonAsync(RegisterCourse, new RegisterCourseRequest()
        {
            Name = "Course 01",
            Assessments = new List<RegisterCourseRequest.AssessmentDto>
            {
                new()
                {
                    Name = "Practical",
                    CoursePercentage = 40
                },
                new()
                {
                    Name = "Exam",
                    CoursePercentage = 60
                }
            }
        });

        // Then
        Assert.Equal(HttpStatusCode.OK, registerCourseResponse.StatusCode);
        var registerCourseResponseContent = await registerCourseResponse.Content.ReadFromJsonAsync<CourseRegistered>();
        var courseId = registerCourseResponseContent.CourseId;

        // When
        var registerStudentForCourseResponse = await _client.PostAsJsonAsync(RegisterStudentForCourse,
            new RegisterStudentForCourseRequest()
            {
                StudentId = studentId,
                CourseId = courseId
            });

        // Then
        Assert.Equal(HttpStatusCode.OK, registerStudentForCourseResponse.StatusCode);
        
        // When
        var getStudentResponse = await _client.GetAsync($"{GetStudent}?id={studentId}");
        var student = await getStudentResponse.Content.ReadFromJsonAsync<StudentDto>();
        
        // Then
        Assert.Equal(HttpStatusCode.OK, getStudentResponse.StatusCode);
        Assert.NotNull(student);
        Assert.Contains(student.Courses, x => x.Id == courseId);
        Assert.Equal(firstName, student.FirstName);
        Assert.Equal(lastName, student.LastName);
    }
}