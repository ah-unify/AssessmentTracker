using AssessmentTracker.Domain.ValueObjects;

namespace AssessmentTracker.Domain.Extensions;

public static class AssessmentGradeExtensions
{
    public static string GetLetterGrade(this Grade grade)
    {
        return grade.Value switch
        {
            > 90 => "A+",
            > 85 => "A",
            > 80 => "A-",
            > 75 => "B+",
            > 70 => "B",
            > 65 => "B-",
            > 60 => "C+",
            > 55 => "C",
            > 50 => "C-",
            > 40 => "D",
            _ => "E"
        };
    }
}