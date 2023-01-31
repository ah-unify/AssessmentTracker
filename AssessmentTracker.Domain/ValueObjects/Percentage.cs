namespace AssessmentTracker.Domain.ValueObjects;

public record Percentage
{
    public decimal Value { get; }

    protected Percentage(decimal value)
    {
        Value = value;
    }

    public static Percentage From(decimal value)
    {
        if (value is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException($"{nameof(Percentage)} value should be within range 0-100. " +
                                                  $"Provided value: {value}");
        }

        return new Percentage(value);
    }
}