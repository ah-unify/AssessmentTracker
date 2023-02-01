using AssessmentTracker.Api.Controllers;

namespace AssessmentTracker.Api.Models;

public class StudentDto 
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public List<CourseDto> Courses { get; init; }
}