namespace AssessmentTracker.Api.Models.Requests;

public class RegisterStudentForCourseRequest
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}