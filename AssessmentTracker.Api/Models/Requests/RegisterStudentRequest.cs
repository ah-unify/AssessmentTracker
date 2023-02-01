namespace AssessmentTracker.Api.Models.Requests;

public class RegisterStudentRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}