namespace AssessmentTracker.Domain.ValueObjects;

public record Grade
{
    public decimal Value { get; }

    protected Grade(decimal value)
    {
        Value = value;
    }

    public static Grade From(decimal value)
    {
        if (value is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException($"{nameof(Grade)} value should be within range 0-100. " +
                                                  $"Provided value: {value}");
        }

        return new Grade(value);
    }
}