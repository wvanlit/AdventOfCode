namespace AoC.Shared;

public record Answer(string Value)
{
    public static implicit operator Answer(string value) => new(value);
    public static implicit operator Answer(int value) => new(value.ToString());
    public static implicit operator Answer(long value) => new(value.ToString());
    public static implicit operator Answer(ulong value) => new(value.ToString());
    public static implicit operator Answer(uint value) => new(value.ToString());

    public static implicit operator string(Answer answer) => answer.Value;

    public override string ToString() => Value;

    public static Answer Failed => new("Failed");
}