namespace AssessmentTracker.Domain.Entities;

public class Course : Entity
{
    public string Name { get; set; }
    
    public void WithAssessments(List<Assessment> assessments)
    {
        var percentileTotal = assessments.Sum(x => x.CoursePercentage.Value);

        if (percentileTotal != 100)
        {
            throw new InvalidOperationException("The encountered assessments did not match the expected total");
        }

        Assessments = assessments;
    }

    public virtual List<Student> RegisteredStudents { get; protected set; }
    public virtual List<Assessment> Assessments { get; protected set; }
}