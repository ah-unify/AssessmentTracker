namespace AssessmentTracker.Api.Models.Requests;

public class RegisterCourseRequest
{
    public string Name { get; set; }
    public List<AssessmentDto> Assessments { get; set; }

    public class AssessmentDto
    {
        public string Name { get; set; }
        public decimal CoursePercentage { get; set; }
    }
}