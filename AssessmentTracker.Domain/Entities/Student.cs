namespace AssessmentTracker.Domain.Entities;

public class Student : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime RegisterDate { get; set; }
    
    public virtual List<Course> RegisteredCourses { get; protected set; }
    public virtual List<AssessmentRecord> AssessmentRecords { get; protected set; }
}