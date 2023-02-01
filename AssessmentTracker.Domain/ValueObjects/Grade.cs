namespace AssessmentTracker.Domain.ValueObjects;

public record Grade
{
    public decimal Value { get; }

    protected Grade()
    {
    }

    public Grade(decimal value)
    {
        if (value is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException($"{nameof(Grade)} value should be within range 0-100. " +
                                                  $"Provided value: {value}");
        }

        Value = value;
    }
}