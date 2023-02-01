namespace AssessmentTracker.Domain.ValueObjects;

public record Grade
{
    public decimal Value { get; protected init; }

    protected Grade()
    {
    }

    public Grade(decimal value)
    {
        if (!InnerIsValid(value))
        {
            throw new ArgumentOutOfRangeException($"{nameof(Grade)} value should be within range 0-100. " +
                                                  $"Provided value: {value}");
        }

        Value = value;
    }

    public static bool IsValid(decimal value)
    {
        return InnerIsValid(value);
    }

    private static bool InnerIsValid(decimal value)
    {
        return value >= 0 || value <= 100;
    }
}