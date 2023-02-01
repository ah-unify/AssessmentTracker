namespace AssessmentTracker.Api.Models.Requests;

public class RecordAssessmentRequest
{
    public Guid StudentId { get; set; }
    public Guid AssessmentId { get; set; }
    public decimal Grade { get; set; }
}