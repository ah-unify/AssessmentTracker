using AssessmentTracker.Domain.ValueObjects;

namespace AssessmentTracker.Domain.Entities;

public class Student : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime RegisterDate { get; set; }

    public void RegisterCourse(Course course)
    {
        Courses.Add(course);
    }

    public void RecordAssessment(AssessmentRecord assessmentRecord)
    {
        AssessmentRecords.Add(assessmentRecord);
    }

    public Grade GetTotalGradeForCourse(Guid courseId)
    {
        return new Grade(AssessmentRecords.Where(x => x.Assessment.CourseId == courseId)
            .Sum(x => x.Grade.Value * x.Assessment.CoursePercentage.Value));
    }

    public virtual List<Course> Courses { get; protected set; } = new();
    public virtual List<AssessmentRecord> AssessmentRecords { get; protected set; } = new();
}