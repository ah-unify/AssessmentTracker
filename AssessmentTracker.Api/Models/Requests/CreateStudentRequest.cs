namespace AssessmentTracker.Api.Models.Requests;

public class CreateStudentRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}