namespace AssessmentTracker.Domain;

public record AssessmentGrade
{
    public decimal Value { get; }

    private AssessmentGrade(decimal value)
    {
        Value = value;
    }

    public static AssessmentGrade From(decimal value)
    {
        if (value is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException($"{nameof(AssessmentGrade)} value should be within range 0-100. " +
                                                  $"Provided value: {value}");
        }

        return new AssessmentGrade(value);
    }
}