namespace AssessmentTracker.Domain.Entities;

public class AssessmentRecord : Entity
{
    public AssessmentGrade AssessmentGrade { get; protected set; }
    
    public virtual Student Student { get; protected set; }
    public virtual Assessment Assessment { get; protected set; }
}